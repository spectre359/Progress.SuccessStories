using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progress.SuccessStories.Web.Services.Interfaces
{
    public interface INotificationService
    {
        void CreateMessageJobAndSendForExecution(string message);
    }
}
