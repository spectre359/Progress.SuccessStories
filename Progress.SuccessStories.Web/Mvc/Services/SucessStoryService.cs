using AngleSharp.Network.Default;
using Microsoft.Web.Administration;
using Progress.SuccessStories.Web.Mvc.Models;
using Progress.SuccessStories.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Lifecycle;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Versioning;
using Telerik.Sitefinity.Workflow;

namespace Progress.SuccessStories.Web.Services
{
    public class SucessStoryService : ISuccessStoryService
    {     

        public string CreateSuccessStory(SuccessStoryViewModel submittedStory, string domain)
        {
            var message = String.Empty;
            try
            {
                var providerName = String.Empty;                

                var transactionName = $"storyTransaction_{DateTime.Now}";
                var versionManager = VersionManager.GetManager(null, transactionName);

                DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName, transactionName);
                Type successStoryType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.SuccessStories.SuccessStory");
                DynamicContent successStoryItem = dynamicModuleManager.CreateDataItem(successStoryType);

                successStoryItem.SetValue("Title", submittedStory.Title);
                successStoryItem.SetValue("Description", submittedStory.Description);
                successStoryItem.SetValue("SummaryDescription", submittedStory.SummaryDescription);
                successStoryItem.SetValue("ProductsUsed", submittedStory.ProductsUsed);
                successStoryItem.SetValue("Company", submittedStory.Company);
                successStoryItem.SetValue("CompanyWebsite", submittedStory.CompanyWebsite);
                successStoryItem.SetValue("Industry", submittedStory.Industry);

                LibrariesManager thumbnailManager = LibrariesManager.GetManager();

                if (submittedStory.Thumbnail != null)
                {
                    var fileStream = submittedStory.Thumbnail.InputStream;
                    var imgId = Guid.NewGuid();
                    CreateImageWithNativeAPI(imgId, submittedStory.Title, fileStream, submittedStory.Thumbnail.FileName, Path.GetExtension(submittedStory.Thumbnail.FileName));
                    var thumbnailItem = thumbnailManager.GetImage(imgId);
                    if (thumbnailItem != null)
                    {
                        successStoryItem.CreateRelation(thumbnailItem, "Thumbnail");
                    }
                }

                successStoryItem.SetString("UrlName", $"{submittedStory.Title}-{submittedStory.Company}");
                successStoryItem.SetValue("Owner", SecurityManager.GetCurrentUserId());
                successStoryItem.SetValue("PublicationDate", DateTime.UtcNow);

                successStoryItem.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Draft");

                versionManager.CreateVersion(successStoryItem, false);

                ILifecycleDataItem publishedCarItem = dynamicModuleManager.Lifecycle.Publish(successStoryItem);

                successStoryItem.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Published");

                versionManager.CreateVersion(successStoryItem, true);

                TransactionManager.CommitTransaction(transactionName);
                message = $"A new Success Story has been submitted. Take a look <a style=\"font-weight:bold;color:blue;\" href=\"{domain}/success-story-details{successStoryItem.ItemDefaultUrl.Value}\">here</a>";
                                
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            
            return message;
        }

        public void CreateImageWithNativeAPI(Guid masterImageId, string imageTitle, Stream imageStream, string imageFileName, string imageExtension)
        {
            LibrariesManager librariesManager = LibrariesManager.GetManager();
            Image image = librariesManager.GetImages().Where(i => i.Id == masterImageId).FirstOrDefault();

            if (image == null)
            {                
                image = librariesManager.CreateImage(masterImageId);
                
                Album album = librariesManager.GetAlbums().FirstOrDefault();
                image.Parent = album;
                
                image.Title = imageTitle;
                image.DateCreated = DateTime.UtcNow;
                image.PublicationDate = DateTime.UtcNow;
                image.LastModified = DateTime.UtcNow;
                image.UrlName = Regex.Replace(imageTitle.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
                image.MediaFileUrlName = Regex.Replace(imageFileName.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
                                
                librariesManager.Upload(image, imageStream, imageExtension);
                                
                librariesManager.SaveChanges();
                
                var bag = new Dictionary<string, string>();
                bag.Add("ContentType", typeof(Image).FullName);
                WorkflowManager.MessageWorkflow(masterImageId, typeof(Image), null, "Publish", false, bag);
            }
        }
    }
}