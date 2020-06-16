using Progress.SuccessStories.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progress.SuccessStories.Web.Services.Interfaces
{
   public interface ISuccessStoryService
    {
        string CreateSuccessStory(SuccessStoryViewModel submittedStory, string domain);
    }
}
