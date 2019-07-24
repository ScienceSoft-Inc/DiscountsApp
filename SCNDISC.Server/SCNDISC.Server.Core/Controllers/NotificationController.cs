using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SCNDISC.Server.Application.Services.Notifications;
using SCNDISC.Server.Core.Mapper;
using SCNDISC.Server.Core.Models;
using SCNDISC.Server.Core.Models.Notifications;
using SCNDISC.Server.Domain.Queries.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SCNDISC.Server.Core.Controllers
{
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;

        public NotificationController(INotificationService notificationService, IConfiguration configuration)
        {
            _notificationService = notificationService;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminRole)]
        [Route("notification")]
        public async Task<ActionResult> PostAsync([FromBody]PushNotificationModel model, [FromQuery] string lang)
        {
            if (ModelState.IsValid)
            {
                var restrictions = await RestrictionsByNotification(lang);

                if (!restrictions.Restrictions.Any())
                {
                    const string langRus = "Рус";
                    var language = lang == langRus ? "ru" : "en";
                    var topicLanguage = model.IsSendToAllDevices ? "all" : $"lang_{language}";
                    var clickAction = $"message_{language}";

                    var androidNotifyResult = await _notificationService.PushNotificationAsync(
                        JsonConvert.SerializeObject(model.ToAndroidNotification(topicLanguage, clickAction)));

                    var iosNorifyResult = await _notificationService.PushNotificationAsync(
                        JsonConvert.SerializeObject(model.ToIosNotification(topicLanguage, clickAction),
                        Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        }));

                    if (!string.IsNullOrEmpty(androidNotifyResult) || !string.IsNullOrEmpty(iosNorifyResult))
                    {
                        var notification = await _notificationService.AddAsync(model.ToNotification(lang));
                        var isSuccess = !string.IsNullOrEmpty(notification.Id);

                        return isSuccess ? (ActionResult)Ok(notification) : BadRequest("Notification not saved");
                    }
                    return BadRequest("Sorry, but something went wrong");
                }
                return BadRequest(restrictions);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("restrictionsByNotification")]
        public async Task<RestrictionToSendNotification> RestrictionsByNotification([FromQuery] string lang)
        {
            var restrictionMessages = new List<string>();
            int countDown = 0;

            int maxQuontityPushNotificationsPerDay;
            int pushNotificationInterval;

            if (int.TryParse(_configuration.GetValue<string>("MaxCountPushNotificationsPerDay"), out maxQuontityPushNotificationsPerDay)
                | int.TryParse(_configuration.GetValue<string>("PushNotificationInterval"), out pushNotificationInterval))
            {
                var todayNotifications = await _notificationService.GetTodayNotificationAsync(lang);

                if (todayNotifications.Count() != 0)
                {
                    if (maxQuontityPushNotificationsPerDay != 0 && todayNotifications.Count() >= maxQuontityPushNotificationsPerDay)
                    {
                        restrictionMessages.Add("You can't create notifications today because a limit on notifications per day is reached");
                        var tomorrowDate = DateTime.UtcNow.AddDays(1);
                        countDown = (int)(new DateTime(tomorrowDate.Year, tomorrowDate.Month, tomorrowDate.Day) - DateTime.UtcNow).TotalSeconds;
                    }

                    if (pushNotificationInterval != 0)
                    {
                        // we should get a first element because of descending sorting
                        var timeLastNotification = todayNotifications.First().Created;
                        if ((DateTime.UtcNow - timeLastNotification).TotalSeconds < pushNotificationInterval)
                        {
                            restrictionMessages.Add("Sorry but you cannot create notifications so often");
                            var countDownByNotifyInterval = pushNotificationInterval - (int)(DateTime.UtcNow - timeLastNotification).TotalSeconds;
                            countDown = countDownByNotifyInterval > countDown ? countDownByNotifyInterval : countDown;
                        }
                    }
                }
            }

            return new RestrictionToSendNotification() { SecondsToCountDown = countDown, Restrictions = restrictionMessages.ToArray() };
        }

        [HttpGet]
        [Route("allnotifications")]
        public async Task<ActionResult<object>> GetAsync()
        {
            var notificationsCount = await _notificationService.GetCount(null);
            var notifications = await _notificationService.GetAll(new Domain.Queries.Notifications.FilterModel()
            {
                Skip = 0,
                Take = (int) notificationsCount
            });

            return Ok(notifications.Select(n => new PushNotificationModel
            {
                Title = n.Title,
                Text = n.Text,
                DocumentId = n.DocumentId,
                Created = n.Created,
            }).Reverse().ToList());
        }

        [HttpGet]
        [Route("notifications")]
        public async Task<ActionResult<object>> GetAsync([FromQuery(Name = "start")]int skip, [FromQuery(Name = "length")]int take, [FromQuery(Name = "lang")]string language)
        {
            var notifications = (await _notificationService.GetAll(new FilterModel { Skip = skip, Take = take, Language = language })).
                Select(x => new PushNotificationModel
                {
                    Title = x.Title,
                    Text = x.Text,
                    DocumentId = x.DocumentId,
                    Created = x.Created
                }).ToList();

            var count = await _notificationService.GetCount(language);

            var result = new DateTableResult<PushNotificationModel>
            {
                Total = count,
                Filtered = count,
                Data = notifications
            };

            return Ok(result);
        }
    }
}