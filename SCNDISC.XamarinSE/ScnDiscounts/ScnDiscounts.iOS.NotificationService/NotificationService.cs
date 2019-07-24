using Foundation;
using ScnDiscounts.Models.WebService;
using System;
using System.Threading.Tasks;
using UserNotifications;

namespace ScnDiscounts.iOS.NotificationService
{
    [Register(nameof(NotificationService))]
    public class NotificationService : UNNotificationServiceExtension
    {
        protected Action<UNNotificationContent> ContentHandler { get; set; }

        protected UNMutableNotificationContent BestAttemptContent { get; set; }

        protected NotificationService(IntPtr handle)
            : base(handle)
        {
        }

        public override void DidReceiveNotificationRequest(UNNotificationRequest request,
            Action<UNNotificationContent> contentHandler)
        {
            ContentHandler = contentHandler;
            BestAttemptContent = (UNMutableNotificationContent) request.Content.MutableCopy();

            var documentIdObj = request.Content.UserInfo.ObjectForKey(new NSString("documentId"));
            if (documentIdObj != null)
            {
                var documentId = documentIdObj.ToString();
                var url = ApiService.GetPartnerLogoUrl(documentId);

                var fileUrl = new NSUrl(url);
                var filePath = GetLocalFilePath();
                var isDownloaded = DownloadImageToPath(fileUrl, filePath);

                if (isDownloaded)
                {
                    const string attachmentId = "image";
                    var options = new UNNotificationAttachmentOptions();
                    var attachment = UNNotificationAttachment.FromIdentifier(attachmentId, filePath, options, out _);

                    BestAttemptContent.Attachments = new[]
                    {
                        attachment
                    };
                }
            }

            ContentHandler(BestAttemptContent);
        }

        public override void TimeWillExpire()
        {
            ContentHandler(BestAttemptContent);
        }

        private bool DownloadImageToPath(NSUrl fileUrl, NSUrl filePath)
        {
            var taskSource = new TaskCompletionSource<bool>();

            var task = NSUrlSession.SharedSession.CreateDownloadTask(fileUrl, (tempFile, response, error) =>
            {
                bool result;

                if (error == null && tempFile != null)
                    result = NSFileManager.DefaultManager.Move(tempFile, filePath, out _);
                else
                    result = false;

                taskSource.TrySetResult(result);
            });
            task.Resume();

            return taskSource.Task.Result;
        }

        private static NSUrl GetLocalFilePath()
        {
            var cache = NSSearchPath.GetDirectories(NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User);
            var cachedFolder = cache[0];
            var fileName = Guid.NewGuid() + ".png";
            var cachedFile = cachedFolder + fileName;
            return NSUrl.CreateFileUrl(cachedFile, false, null);
        }
    }
}
