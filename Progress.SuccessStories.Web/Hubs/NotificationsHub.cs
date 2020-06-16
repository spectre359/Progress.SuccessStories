using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Progress.SuccessStories.Web.Hubs
{
    [HubName("notificationsHub")]
    public class NotificationsHub : Hub
    {
        private static object _lockObj = new object();
        private static string Story { get; set; }        
        public void NewStorySubmitted(string story)
        {
            lock(_lockObj)
            {
                if (string.IsNullOrEmpty(Story))
                {
                    Story = story;
                    Clients.All.notifyUsers();
                }
                else if (Story != story)
                {
                    Story = story;
                    Clients.All.notifyUsers();
                }
            }            

        }
    }
}