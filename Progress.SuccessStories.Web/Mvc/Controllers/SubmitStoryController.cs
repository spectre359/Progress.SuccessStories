using DocumentFormat.OpenXml.EMMA;
using Progress.SuccessStories.Web.Filters;
using Progress.SuccessStories.Web.Mvc.Models;
using Progress.SuccessStories.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Lifecycle;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Services.Notifications;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Versioning;
using Telerik.Sitefinity.Workflow;

namespace Progress.SuccessStories.Web.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "SubmitStoryWidget", Title = "SubmitStoryWidget", SectionName = "MvcWidgets")]
    public class SubmitStoryController : Controller
    {
        private static string Message { get; set; }
        ISuccessStoryService _successStoryService;
        Services.Interfaces.INotificationService _notificationService;        

        public SubmitStoryController(ISuccessStoryService successStoryService, Services.Interfaces.INotificationService notificationService)
        {
            _successStoryService = successStoryService;
            _notificationService = notificationService;            
        }

        public ActionResult Index()
        {
            var story = new SuccessStoryViewModel();
            return View(story);
        }

        [HttpPost]
        [PreventDuplicateRequest]
        [ValidateAntiForgeryToken]
        public void Create(SuccessStoryViewModel submittedStory)
        {            
            if (ModelState.IsValid)
            {
                Message = _successStoryService.CreateSuccessStory(submittedStory, HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));
                _notificationService.CreateMessageJobAndSendForExecution(Message);
            }            
        }


        [Route("getMessage")]
        public string GetMessage()
        {
            return Message;
        }

        
    }
}