using Ninject.Modules;
using Progress.SuccessStories.Web.Services;
using Progress.SuccessStories.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Progress.SuccessStories.Web
{
    public class InterfaceMappings : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Bind<INotificationService>().To<NotificationService>();
            this.Bind<ISuccessStoryService>().To<SucessStoryService>();
        }
    }
}