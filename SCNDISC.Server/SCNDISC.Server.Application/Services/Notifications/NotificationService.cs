using SCNDISC.Server.Domain.Aggregates;
using SCNDISC.Server.Domain.Commands.Notifications;
using SCNDISC.Server.Domain.Queries.Notifications;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SCNDISC.Server.Infrastructure.Settings;

namespace SCNDISC.Server.Application.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationListQuery _notificationListQuery;
        private readonly INotificationListTodayQuery _notificationListTodayQuery;
        private readonly IInsertNotificationCommand _insertNotificationCommand;
        private readonly ISettingsInfService _settingsInfService;
        private readonly INotificationCountQuery _notificationCountQuery;

        public NotificationService(IInsertNotificationCommand insertNotificationCommand, INotificationListQuery notificationListQuery, 
            INotificationListTodayQuery notificationListTodayQuery, ISettingsInfService settingsInfService, INotificationCountQuery notificartionCountQuery)
        {
            _insertNotificationCommand = insertNotificationCommand;
            _notificationListQuery = notificationListQuery;
            _notificationListTodayQuery = notificationListTodayQuery;
            _settingsInfService = settingsInfService;
            _notificationCountQuery = notificartionCountQuery;
        }

        public async Task<string> PushNotificationAsync(string content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
            if (content != null)
            {
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");

                request.Headers.TryAddWithoutValidation("Authorization", "Key=" + _settingsInfService.FcmKey);

                using (var client = new HttpClient())
                {
                    var httpResponseMessage = await client.SendAsync(request);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        return await httpResponseMessage.Content.ReadAsStringAsync();
                    }
                }
            }

            return null;
        }

        public async Task<IEnumerable<Notification>> GetTodayNotificationAsync(string language)
        {
            return await _notificationListTodayQuery.RunAsync(language);
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            return await _insertNotificationCommand.ExecuteAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetAll(FilterModel filterModel)
        {
            return await _notificationListQuery.RunAsync(filterModel);
        }

        public async Task<long> GetCount(string language)
        {
            return await _notificationCountQuery.RunAsync(language);
        }
    }
}
