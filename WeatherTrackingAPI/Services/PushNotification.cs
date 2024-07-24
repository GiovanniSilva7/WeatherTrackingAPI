using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

public class PushNotificationService
{
    private readonly FirebaseMessaging _firebaseMessaging;

    public PushNotificationService(IOptions<AppSettings> appSettings)
    {
        var firebaseSettings = appSettings.Value.Firebase;
        var keyFilePath = Path.Combine(Directory.GetCurrentDirectory(), firebaseSettings.ServiceAccountKeyPath);
        var app = FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(keyFilePath)
        });
        _firebaseMessaging = FirebaseMessaging.DefaultInstance;
    }

    public async Task SendNotificationAsync(string token, string title, string body)
    {
        var message = new Message()
        {
            Token = token,
            Notification = new Notification()
            {
                Title = title,
                Body = body
            }
        };

        await _firebaseMessaging.SendAsync(message);
    }
}
