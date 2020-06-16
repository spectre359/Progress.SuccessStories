using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Owin;

namespace Progress.SuccessStories.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {            
            app.UseSitefinityMiddleware();

            app.MapSignalR();
        }
    }
}