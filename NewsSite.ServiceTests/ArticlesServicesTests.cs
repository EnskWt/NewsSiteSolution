using AutoFixture;
using FluentAssertions;
using Moq;
using NewsSite.Core.DataTransferObjects.ArticleObjects;
using NewsSite.Core.Domain.Models.ArticleModels;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Core.Enums.Application;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using NewsSite.Core.Services.ArticlesServices;
using System.Linq.Expressions;

namespace NewsSite.ServiceTests
{
    public class ArticlesServicesTests
    {
        private readonly IArticlesAdderService _articlesAdderService;
        private readonly IArticlesDeleterService _articlesDeleterService;
        private readonly IArticlesCommentGetterService _articlesGetterService;
        private readonly IArticlesUpdaterService _articlesUpdaterService;

        private readonly Mock<IArticlesRepository> _articlesRepositoryMock;
        private readonly Mock<IArticleExpressionsProvider> _articleExpressionsProviderMock;

        private readonly IArticlesRepository _articlesRepository;
        private readonly IArticleExpressionsProvider _articleExpressionsProvider;

        private readonly IFixture _fixture;

        public ArticlesServicesTests()
        {
            _fixture = new Fixture();

            _articlesRepositoryMock = new Mock<IArticlesRepository>();
            _articleExpressionsProviderMock = new Mock<IArticleExpressionsProvider>();

            _articlesRepository = _articlesRepositoryMock.Object;
            _articleExpressionsProvider = _articleExpressionsProviderMock.Object;

            _articlesAdderService = new ArticlesAdderService(_articlesRepository);
            _articlesDeleterService = new ArticlesDeleterService(_articlesRepository);
            _articlesGetterService = new ArticlesGetterService(_articlesRepository, _articleExpressionsProvider);
            _articlesUpdaterService = new ArticlesUpdaterService(_articlesRepository);
        }

        #region AddArtticle

        [Fact]
        public async Task AddArticle_ArticleRequestNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ArticleAddRequest? articleRequest = null;
            Guid userId = _fixture.Create<Guid>();

            // Act
            Func<Task> action = async () =>
            {
                await _articlesAdderService.AddArticle(articleRequest, userId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddArticle_UserIdNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ArticleAddRequest articleAddRequest = _fixture.Create<ArticleAddRequest>();
            Guid? userId = null;

            // Act
            Func<Task> action = async () =>
            {
                await _articlesAdderService.AddArticle(articleAddRequest, userId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddArticle_UserIdEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            ArticleAddRequest articleAddRequest = _fixture.Create<ArticleAddRequest>();
            Guid? userId = Guid.Empty;

            // Act
            Func<Task> action = async () =>
            {
                await _articlesAdderService.AddArticle(articleAddRequest, userId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddArticle_ArticleRequestInvalid_ShouldThrowArgumentException()
        {
            // Arrange
            ArticleAddRequest articleAddRequest = _fixture.Build<ArticleAddRequest>()
                .Without(x => x.Title)
                .Create();
            Guid userId = _fixture.Create<Guid>();

            // Act
            Func<Task> action = async () =>
            {
                await _articlesAdderService.AddArticle(articleAddRequest, userId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddArticle_ArticleRequestValid_ShouldReturnArticleResponse()
        {
            // Arrange
            ArticleAddRequest articleAddRequest = _fixture.Create<ArticleAddRequest>();
            Guid userId = _fixture.Create<Guid>();

            Article article = articleAddRequest.ToArticle(userId);
            ArticleResponse expectedArticleResponse = article.ToArticleResponse();

            _articlesRepositoryMock.Setup(x => x.AddArticleAsync(It.IsAny<Article>())).ReturnsAsync(article);

            // Act
            ArticleResponse actualArticleResponse = await _articlesAdderService.AddArticle(articleAddRequest, userId);


            // Assert
            actualArticleResponse.Should().BeOfType<ArticleResponse>();
            actualArticleResponse.Should().BeEquivalentTo(expectedArticleResponse);
        }

        #endregion

        #region DeleteArticle

        [Fact]
        public async Task DeleteArticle_ArticleIdNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Guid? articleId = null;

            // Act
            Func<Task> action = async () =>
            {
                await _articlesDeleterService.DeleteArticle(articleId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteArticle_ArticleIdEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            Guid articleId = Guid.Empty;

            // Act
            Func<Task> action = async () =>
            {
                await _articlesDeleterService.DeleteArticle(articleId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteArticle_ArticleNull_ShouldReturnFalse()
        {
            // Arrange
            Guid articleId = _fixture.Create<Guid>();

            _articlesRepositoryMock.Setup(x => x.GetArticleAsync(It.IsAny<Guid>())).ReturnsAsync(null as Article);

            // Act
            bool actualResult = await _articlesDeleterService.DeleteArticle(articleId);

            // Assert
            actualResult.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteArticle_ArticleNotNull_ShouldReturnTrue()
        {
            // Arrange
            Guid articleId = _fixture.Create<Guid>();

            Article dummyArticle = _fixture.Build<Article>()
                .Without(x => x.Author)
                .Create();

            _articlesRepositoryMock.Setup(x => x.GetArticleAsync(It.IsAny<Guid>())).ReturnsAsync(dummyArticle);
            _articlesRepositoryMock.Setup(x => x.DeleteArticleAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            // Act
            bool actualResult = await _articlesDeleterService.DeleteArticle(articleId);

            // Assert
            actualResult.Should().BeTrue();
        }

        #endregion

        #region GetArticles
        [Fact]
        public async Task GetArticles_ParametersNotProvided_ShouldReturnListOfArticleResponses()
        {
            // Arrange
            List<Article> dummyArticles = _fixture.Build<Article>()
                .Without(x => x.Author)
                .CreateMany(10)
                .ToList();
            List<ArticleResponse> expectedArticleResponses = dummyArticles.Select(a => a.ToArticleResponse()).ToList();

            _articlesRepositoryMock.Setup(x => x.GetArticlesAsync()).ReturnsAsync(dummyArticles);
            _articleExpressionsProviderMock.Setup(x => x.GetSort(It.IsAny<SortAttributes>())).Returns((Expression<Func<Article, object>>)(a => a.DatePublished));

            // Act
            List<ArticleResponse> actualArticleResponses = await _articlesGetterService.GetArticles();

            // Assert
            actualArticleResponses.Should().BeOfType<List<ArticleResponse>>();
            actualArticleResponses.Should().BeEquivalentTo(expectedArticleResponses);
        }

        [Fact]
        public async Task GetArticles_SearchTermAndTermProvided_ShouldReturnListOfArticleResponses()
        {
            // Arrange
            List<Article> dummyArticles = _fixture.Build<Article>()
                .Without(x => x.Author)
                .CreateMany(10)
                .ToList();
            List<ArticleResponse> expectedArticleResponses = dummyArticles.Select(a => a.ToArticleResponse()).ToList();

            _articlesRepositoryMock.Setup(x => x.GetFilteredArticlesAsync(It.IsAny<Expression<Func<Article, bool>>>())).ReturnsAsync(dummyArticles);

            _articleExpressionsProviderMock.Setup(x => x.GetFilter(It.IsAny<SearchTerms>(), It.IsAny<string>())).Returns((Expression<Func<Article, bool>>)(a => a.AuthorId == Guid.Parse("dummyTerm")));
            _articleExpressionsProviderMock.Setup(x => x.GetSort(It.IsAny<SortAttributes>())).Returns((Expression<Func<Article, object>>)(a => a.DatePublished));

            // Act
            List<ArticleResponse> actualArticleResponses = await _articlesGetterService.GetFilteredArticles(SearchTerms.Author, "dummyTerm");

            // Assert
            actualArticleResponses.Should().BeOfType<List<ArticleResponse>>();
            actualArticleResponses.Should().BeEquivalentTo(expectedArticleResponses);
        }

        [Fact]
        public async Task GetArticles_SearchTermAndTermAndSortAttributeProvided_ShouldReturnListOfArticleResponses()
        {
            // Arrange
            List<Article> dummyArticles = _fixture.Build<Article>()
                .Without(x => x.Author)
                .CreateMany(10)
                .ToList();
            List<ArticleResponse> expectedArticleResponses = dummyArticles.Select(a => a.ToArticleResponse()).ToList();

            _articlesRepositoryMock.Setup(x => x.GetFilteredArticlesAsync(It.IsAny<Expression<Func<Article, bool>>>())).ReturnsAsync(dummyArticles);

            _articleExpressionsProviderMock.Setup(x => x.GetFilter(It.IsAny<SearchTerms>(), It.IsAny<string>())).Returns((Expression<Func<Article, bool>>)(a => a.AuthorId == Guid.Parse("dummyTerm")));
            _articleExpressionsProviderMock.Setup(x => x.GetSort(It.IsAny<SortAttributes>())).Returns((Expression<Func<Article, object>>)(a => a.DatePublished));

            // Act
            List<ArticleResponse> actualArticleResponses = await _articlesGetterService.GetFilteredArticles(SearchTerms.Author, "dummyTerm", SortAttributes.Date);

            // Assert
            actualArticleResponses.Should().BeOfType<List<ArticleResponse>>();
            actualArticleResponses.Should().BeEquivalentTo(expectedArticleResponses);
        }

        #endregion

        #region GetArticle

        [Fact]
        public async Task GetArticle_ArticleIdNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Guid? articleId = null;

            // Act
            Func<Task> action = async () =>
            {
                await _articlesGetterService.GetArticle(articleId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetArticle_ArticleIdEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            Guid articleId = Guid.Empty;

            // Act
            Func<Task> action = async () =>
            {
                await _articlesGetterService.GetArticle(articleId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetArticle_ArticleNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Guid articleId = _fixture.Create<Guid>();

            _articlesRepositoryMock.Setup(x => x.GetArticleAsync(It.IsAny<Guid>())).ReturnsAsync(null as Article);

            // Act
            ArticleResponse? result = await _articlesGetterService.GetArticle(articleId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetArticle_ArticleNotNull_ShouldReturnArticleResponse()
        {
            // Arrange
            Guid articleId = _fixture.Create<Guid>();

            Article dummyArticle = _fixture.Build<Article>()
                .Without(x => x.Author)
                .Create();
            ArticleResponse expectedArticleResponse = dummyArticle.ToArticleResponse();

            _articlesRepositoryMock.Setup(x => x.GetArticleAsync(It.IsAny<Guid>())).ReturnsAsync(dummyArticle);

            // Act
            ArticleResponse? actualArticleResponse = await _articlesGetterService.GetArticle(articleId);

            // Assert
            actualArticleResponse.Should().BeOfType<ArticleResponse>();
            actualArticleResponse.Should().BeEquivalentTo(expectedArticleResponse);
        }


        #endregion

        #region UpdateArticle

        [Fact]
        public async Task UpdateArticle_ArticleRequestNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ArticleUpdateRequest? articleRequest = null;

            // Act
            Func<Task> action = async () =>
            {
                await _articlesUpdaterService.UpdateArticle(articleRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateArticle_ArticleRequestInvalid_ShouldThrowArgumentException()
        {
            // Arrange
            ArticleUpdateRequest articleUpdateRequest = _fixture.Build<ArticleUpdateRequest>()
                .Without(x => x.Title)
                .Create();

            // Act
            Func<Task> action = async () =>
            {
                await _articlesUpdaterService.UpdateArticle(articleUpdateRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateArticle_ArticleRequestValid_ShouldReturnArticleResponse()
        {
            // Arrange
            ArticleUpdateRequest articleUpdateRequest = _fixture.Create<ArticleUpdateRequest>();

            Article dummyArticle = _fixture.Build<Article>()
                .Without(x => x.Author)
                .Create();
            ArticleResponse expectedArticleResponse = dummyArticle.ToArticleResponse();

            _articlesRepositoryMock.Setup(x => x.UpdateArticleAsync(It.IsAny<Article>())).ReturnsAsync(dummyArticle);

            // Act
            ArticleResponse actualArticleResponse = await _articlesUpdaterService.UpdateArticle(articleUpdateRequest);

            // Assert
            actualArticleResponse.Should().BeOfType<ArticleResponse>();
            actualArticleResponse.Should().BeEquivalentTo(expectedArticleResponse);
        }

        #endregion 
    }
}