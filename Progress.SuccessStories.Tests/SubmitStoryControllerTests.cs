using Moq;
using Progress.SuccessStories.Web.Mvc.Controllers;
using Progress.SuccessStories.Web.Mvc.Models;
using Progress.SuccessStories.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace Progress.SuccessStories.Tests
{
    public class SubmitStoryControllerTests
    {
        private Mock<ISuccessStoryService> _successStoryServiceMock;
        private Mock<INotificationService> _notificationServiceMock;
        public SubmitStoryControllerTests()
        {
            _successStoryServiceMock = new Mock<ISuccessStoryService>();
            _notificationServiceMock = new Mock<INotificationService>();
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithSuccessStoryViewModel()
        {
            // Arrange           
            var controller = new SubmitStoryController(_successStoryServiceMock.Object, _notificationServiceMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<SuccessStoryViewModel>(
                viewResult.ViewData.Model);
           
        }

        [Fact]
        public async Task Create_CallsService()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.Url).Returns(new Uri("http://www.google.com"));
            context
                .Setup(c => c.Request)
                .Returns(request.Object);
            _successStoryServiceMock.Setup(x => x.CreateSuccessStory(It.IsAny<SuccessStoryViewModel>(), It.IsAny<string>()));
            var story = new SuccessStoryViewModel();
            var cntlr = new SubmitStoryController(_successStoryServiceMock.Object, _notificationServiceMock.Object);
            cntlr.ControllerContext = new ControllerContext(context.Object, new RouteData(), cntlr);
            cntlr.Create(story);

            _successStoryServiceMock.Verify(x => x.CreateSuccessStory(It.IsAny<SuccessStoryViewModel>(), It.IsAny<string>()), Times.Once);
        }
       
    }
}
