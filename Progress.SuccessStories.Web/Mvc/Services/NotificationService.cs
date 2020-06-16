using Progress.SuccessStories.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Services.Notifications;

namespace Progress.SuccessStories.Web.Services
{
    public class NotificationService : Interfaces.INotificationService
    {
        public void CreateMessageJobAndSendForExecution(string message)
        {
            try
            {
                var ns = SystemManager.GetNotificationService();
                var context = new ServiceContext("philsw359@gmail.com", "SuccessStories");
                var contextDictionary = new Dictionary<string, string>();
                
                var profileName = "Default";

                var subjectTemplate = "New Success Story submitted";
                var bodyTemplate = message;

                var tmpl = new MessageTemplateRequestProxy()
                {
                    Subject = subjectTemplate,
                    BodyHtml = bodyTemplate
                };

                List<ISubscriberRequest> subscribers = new List<ISubscriberRequest>();
                string[] notificationRecipients = WebConfigurationManager.AppSettings["notificationRecipients"].Trim().Split(',');

                for (int i = 0; i < notificationRecipients.Length; i++)
                {
                    var subscriber = new SubscriberRequestProxy()
                    {
                        Email = notificationRecipients[i],
                        ResolveKey = $"unique-identifier-in-the-specified-context-{i}"
                    };
                    subscribers.Add(subscriber);
                }

                IMessageJobRequest job = new MessageJobRequestProxy()
                {
                    MessageTemplate = tmpl,
                    Subscribers = subscribers,
                    SenderProfileName = profileName
                };

                ns.SendMessage(context, job, contextDictionary);
            }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            
        }
    }
}