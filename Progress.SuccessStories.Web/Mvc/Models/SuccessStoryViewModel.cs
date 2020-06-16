using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Progress.SuccessStories.Web.Mvc.Models
{
    public class SuccessStoryViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string SummaryDescription { get; set; }
        [Required]
        public string ProductsUsed { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string CompanyWebsite { get; set; }
        [Required]
        public string Industry { get; set; }
        public HttpPostedFileBase Thumbnail { get; set; }
    }
}