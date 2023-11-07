using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NewsSite.Core.DataTransferObjects.ArticleObjects;
using NewsSite.Core.Enums.Application;
using NewsSite.Core.ServiceContracts.ArticlesCommentsContracts;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using NewsSite.Core.ServiceContracts.ArticlesViewsContracts;
using NewsSite.UI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.ControllerTests
{
    public class ArticlesControllerTests
    {
        private readonly IFixture _fixture;

        private readonly IArticlesGetterService _articlesGetterService;
        private readonly IArticlesCommentsGetterService _articlesCommentGetterService;

        private readonly Mock<IArticlesGetterService> _articlesGetterServiceMock;
        private readonly Mock<IArticlesCommentsGetterService> _articlesCommentGetterServiceMock;

        public ArticlesControllerTests()
        {
            _fixture = new Fixture();

            _articlesGetterServiceMock = new Mock<IArticlesGetterService>();
            _articlesCommentGetterServiceMock = new Mock<IArticlesCommentsGetterService>();

            _articlesGetterService = _articlesGetterServiceMock.Object;
            _articlesCommentGetterService = _articlesCommentGetterServiceMock.Object;
        }

        #region Index

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfArticles()
        {
            // Arrange

            var articles = _fixture.Build<ArticleResponse>()
                .Without(a => a.Author)
                .CreateMany(10)
                .ToList();            

            _articlesGetterServiceMock.Setup(x => x.GetArticles(It.IsAny<SortAttributes>(), It.IsAny<int>())).ReturnsAsync(articles);

            var controller = new ArticlesController(_articlesGetterService, _articlesCommentGetterService);

            // Act

            var result = await controller.Index(_fixture.Create<SortAttributes>());

            // Assert

            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<ArticleResponse>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(articles);
        }

        #endregion

        #region ArticleDetails

        [Fact]
        public async Task ArticleDetails_ReturnsRedirectionToIndex_ArticleNull()
        {
            // Arrange

            _articlesGetterServiceMock.Setup(x => x.GetArticle(It.IsAny<Guid>())).ReturnsAsync(null as ArticleResponse);

            var controller = new ArticlesController(_articlesGetterService, _articlesCommentGetterService);

            // Act

            var result = await controller.ArticleDetails(_fixture.Create<Guid>());

            // Assert

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ControllerName.Should().Be("Articles");
            redirectResult.ActionName.Should().Be("Index");     
        }

        [Fact]
        public async Task ArticleDetails_ReturnsViewResult_WithArticle()
        {
            // Arrange

            var article = _fixture.Build<ArticleResponse>()
                .Without(a => a.Author)
                .Create();

            _articlesGetterServiceMock.Setup(x => x.GetArticle(It.IsAny<Guid>())).ReturnsAsync(article);

            var controller = new ArticlesController(_articlesGetterService, _articlesCommentGetterService);

            // Act

            var result = await controller.ArticleDetails(_fixture.Create<Guid>());

            // Assert

            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<ArticleResponse>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(article);
        }

        #endregion
    }
}
