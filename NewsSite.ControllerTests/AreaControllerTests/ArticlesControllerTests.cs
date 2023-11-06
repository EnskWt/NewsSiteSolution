using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NewsSite.Core.DataTransferObjects.ArticleObjects;
using NewsSite.Core.Domain.Models.IdentityModels;
using NewsSite.Core.Enums.Application;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using NewsSite.UI.Areas.Author.Controllers;
using System.Security.Claims;

namespace NewsSite.ControllerTests.AreaControllerTests
{
    public class ArticlesControllerTests
    {
        private readonly IFixture _fixture;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IArticlesValidatorService _articlesValidatorService;
        private readonly IArticleExpressionsProvider _articleExpressionsProvider;
        private readonly IArticlesAdderService _articlesAdderService;
        private readonly IArticlesDeleterService _articlesDeleterService;
        private readonly IArticlesCommentGetterService _articlesGetterService;
        private readonly IArticlesUpdaterService _articlesUpdaterService;

        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IUserStore<ApplicationUser>> _userStoreMock;
        private readonly Mock<IArticlesValidatorService> _articlesValidatorServiceMock;
        private readonly Mock<IArticleExpressionsProvider> _articleExpressionsProviderMock;
        private readonly Mock<IArticlesAdderService> _articlesAdderServiceMock;
        private readonly Mock<IArticlesDeleterService> _articlesDeleterServiceMock;
        private readonly Mock<IArticlesCommentGetterService> _articlesGetterServiceMock;
        private readonly Mock<IArticlesUpdaterService> _articlesUpdaterServiceMock;

        public ArticlesControllerTests()
        {
            _fixture = new Fixture();

            _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _articlesValidatorServiceMock = new Mock<IArticlesValidatorService>();
            _articleExpressionsProviderMock = new Mock<IArticleExpressionsProvider>();
            _articlesAdderServiceMock = new Mock<IArticlesAdderService>();
            _articlesDeleterServiceMock = new Mock<IArticlesDeleterService>();
            _articlesGetterServiceMock = new Mock<IArticlesCommentGetterService>();
            _articlesUpdaterServiceMock = new Mock<IArticlesUpdaterService>();

            _userManager = _userManagerMock.Object;
            _articlesValidatorService = _articlesValidatorServiceMock.Object;
            _articleExpressionsProvider = _articleExpressionsProviderMock.Object;
            _articlesAdderService = _articlesAdderServiceMock.Object;
            _articlesDeleterService = _articlesDeleterServiceMock.Object;
            _articlesGetterService = _articlesGetterServiceMock.Object;
            _articlesUpdaterService = _articlesUpdaterServiceMock.Object;
        }

        #region Index

        [Fact]
        public async Task Index_ReturnsRedirectionToActionResult_UserNull()
        {
            // Arrange
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as ApplicationUser);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.Index();           

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");

        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfArticles()
        {
            // Arrange
            var articles = _fixture.Build<ArticleResponse>()
                .Without(a => a.Author)
                .CreateMany(10)
                .ToList();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            _articlesGetterServiceMock.Setup(x => x.GetFilteredArticles(It.IsAny<SearchTerms>(), It.IsAny<string>(), It.IsAny<SortAttributes>(), It.IsAny<int>())).ReturnsAsync(articles);
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act

            var result = await controller.Index();

            // Assert

            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<ArticleResponse>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(articles);
        }

        #endregion

        #region CreateArticle_GET
        [Fact]
        public async Task CreateArticle_CommonGetRequest_ReturnsViewResult()
        {
            // Arrange
            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.CreateArticle();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }
        #endregion

        #region CreateArticle_POST
        [Fact]
        public async Task CreateArticle_UserNull_ShouldReturnRedirectToActionResult()
        {
            // Arrange
            var articleRequest = _fixture.Build<ArticleAddRequest>()
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as ApplicationUser);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.CreateArticle(articleRequest);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");      
        }

        [Fact]
        public async Task CreateArticle_AddingError_ShouldReturnBadRequestResult()
        {
            // Arrange
            ArticleAddRequest? articleRequest = _fixture.Build<ArticleAddRequest>()
                .Create();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesAdderServiceMock.Setup(x => x.AddArticle(It.IsAny<ArticleAddRequest>(), It.IsAny<Guid>())).ReturnsAsync(null as ArticleResponse);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.CreateArticle(articleRequest);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeOfType<ArticleAddRequest>();
            viewResult.ViewData.Model.Should().Be(articleRequest);
        }

        [Fact]
        public async Task CreateArticle_AddingSuccess_ShouldReturnRedirectToActionResult()
        {
            // Arrange
            var articleRequest = _fixture.Build<ArticleAddRequest>()
                .Create();

            var articleResponse = _fixture.Build<ArticleResponse>()
                .Without(a => a.Author)
                .Create();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesAdderServiceMock.Setup(x => x.AddArticle(It.IsAny<ArticleAddRequest>(), It.IsAny<Guid>())).ReturnsAsync(articleResponse);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.CreateArticle(articleRequest);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("ArticleDetails");
            redirectResult.RouteValues.Should().ContainKey("id");
            redirectResult.RouteValues!["id"].Should().Be(articleResponse.Id);
        }


        #endregion

        #region UpdateArticle_GET
        [Fact]
        public async Task UpdateArticle_UserNull_ReturnsViewResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as ApplicationUser);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.UpdateArticle(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");           
        }

        [Fact]
        public async Task UpdateArticle_ArticleNotExistOrNotOwnedByUser_ShouldReturnRedirectToActionResult()
        {
            var id = Guid.NewGuid();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesValidatorServiceMock.Setup(x => x.IsArticleExist(It.IsAny<Guid>())).ReturnsAsync(false);
            _articlesValidatorServiceMock.Setup(x => x.IsArticleOwnedByAuthor(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.UpdateArticle(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task UpdateArticle_ArticleExistAndOwnedByUser_ShouldReturnViewResult()
        {
            var id = Guid.NewGuid();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            var article = _fixture.Build<ArticleResponse>()
                .Without(a => a.Author)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesValidatorServiceMock.Setup(x => x.IsArticleExist(It.IsAny<Guid>())).ReturnsAsync(true);
            _articlesValidatorServiceMock.Setup(x => x.IsArticleOwnedByAuthor(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _articlesGetterServiceMock.Setup(x => x.GetArticle(It.IsAny<Guid>())).ReturnsAsync(article);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.UpdateArticle(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<ArticleUpdateRequest>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(article.ToArticleUpdateRequest());
        }

        #endregion

        #region UpdateArticle_POST

        [Fact]
        public async Task UpdateArticle_UserNull_ReturnsRedirectToActionResult()
        {
            // Arrange
            var articleRequest = _fixture.Build<ArticleUpdateRequest>()
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as ApplicationUser);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);
            
            // Act
            var result = await controller.UpdateArticle(articleRequest);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task UpdateArticle_ArticleNotExistOrNotOwnedByUser_ReturnsRedirectToActionResult()
        {
            // Arrange
            var articleRequest = _fixture.Build<ArticleUpdateRequest>()
                .Create();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesValidatorServiceMock.Setup(x => x.IsArticleExist(It.IsAny<Guid>())).ReturnsAsync(false);
            _articlesValidatorServiceMock.Setup(x => x.IsArticleOwnedByAuthor(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);
            
            // Act
            var result = await controller.UpdateArticle(articleRequest);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task UpdateArticle_ArticleExistAndOwnedByUser_ReturnsRedirectToActionResult()
        {
            // Arrange
            var articleRequest = _fixture.Build<ArticleUpdateRequest>()
                .Create();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            var article = _fixture.Build<ArticleResponse>()
                .Without(a => a.Author)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesValidatorServiceMock.Setup(x => x.IsArticleExist(It.IsAny<Guid>())).ReturnsAsync(true);
            _articlesValidatorServiceMock.Setup(x => x.IsArticleOwnedByAuthor(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _articlesUpdaterServiceMock.Setup(x => x.UpdateArticle(It.IsAny<ArticleUpdateRequest>())).ReturnsAsync(article);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);
            
            // Act
            var result = await controller.UpdateArticle(articleRequest);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("ArticleDetails");
            redirectResult.RouteValues.Should().ContainKey("id");
            redirectResult.RouteValues!["id"].Should().Be(article.Id);
        }

        #endregion

        #region DeleteArticle_GET

        [Fact]
        public async Task DeleteArticle_UserNull_ReturnsRedirectToActionResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as ApplicationUser);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.DeleteArticle(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task DeleteArticle_ArticleNotExistOrNotOwnedByUser_ReturnsRedirectToActionResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesValidatorServiceMock.Setup(x => x.IsArticleExist(It.IsAny<Guid>())).ReturnsAsync(false);
            _articlesValidatorServiceMock.Setup(x => x.IsArticleOwnedByAuthor(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.DeleteArticle(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task DeleteArticle_ArticleExistAndOwnedByUser_ReturnsViewResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            var article = _fixture.Build<ArticleResponse>()
                .Without(a => a.Author)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesValidatorServiceMock.Setup(x => x.IsArticleExist(It.IsAny<Guid>())).ReturnsAsync(true);
            _articlesValidatorServiceMock.Setup(x => x.IsArticleOwnedByAuthor(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _articlesGetterServiceMock.Setup(x => x.GetArticle(It.IsAny<Guid>())).ReturnsAsync(article);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.DeleteArticle(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<ArticleResponse>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(article);
        }

        #endregion

        #region DeleteArticle_POST

        [Fact]
        public async Task DeleteArticlePost_UserNull_ReturnsRedirectToActionResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as ApplicationUser);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.DeleteArticle(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task DeleteArticlePost_ArticleNotExistOrNotOwnedByUser_ReturnsRedirectToActionResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesValidatorServiceMock.Setup(x => x.IsArticleExist(It.IsAny<Guid>())).ReturnsAsync(false);
            _articlesValidatorServiceMock.Setup(x => x.IsArticleOwnedByAuthor(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.DeleteArticle(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task DeleteArticlePost_DeletingError_ShouldReturnViewResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            var user = _fixture.Build<ApplicationUser>()
                .With(u => u.Id, Guid.NewGuid())
                .Without(u => u.Articles)
                .Create();

            var article = _fixture.Build<ArticleResponse>()
                .Without(a => a.Author)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesValidatorServiceMock.Setup(x => x.IsArticleExist(It.IsAny<Guid>())).ReturnsAsync(true);
            _articlesValidatorServiceMock.Setup(x => x.IsArticleOwnedByAuthor(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _articlesGetterServiceMock.Setup(x => x.GetArticle(It.IsAny<Guid>())).ReturnsAsync(article);
            _articlesDeleterServiceMock.Setup(x => x.DeleteArticle(It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.DeleteArticlePost(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<ArticleResponse>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(article);
        }

        [Fact]
        public async Task DeleteArticlePost_DeletingSuccess_ShouldReturnRedirectToActionResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            var user = _fixture.Build<ApplicationUser>()
                .Without(u => u.Articles)
                .Create();

            var article = _fixture.Build<ArticleResponse>()
                .Without(a => a.Author)
                .Create();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            _articlesValidatorServiceMock.Setup(x => x.IsArticleExist(It.IsAny<Guid>())).ReturnsAsync(true);
            _articlesValidatorServiceMock.Setup(x => x.IsArticleOwnedByAuthor(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _articlesGetterServiceMock.Setup(x => x.GetArticle(It.IsAny<Guid>())).ReturnsAsync(article);
            _articlesDeleterServiceMock.Setup(x => x.DeleteArticle(It.IsAny<Guid>())).ReturnsAsync(true);

            var controller = new ArticlesController(_userManager, _articlesValidatorService, _articlesAdderService, _articlesDeleterService, _articlesGetterService, _articlesUpdaterService);

            // Act
            var result = await controller.DeleteArticlePost(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");
        }


        #endregion
    }
}
