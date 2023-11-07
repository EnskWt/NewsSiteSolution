using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsSite.Core.Domain.Models.IdentityModels;
using NewsSite.Core.Domain.RepositoryContracts;
using NewsSite.Core.ServiceContracts.ArticlesCommentsContracts;
using NewsSite.Core.ServiceContracts.ArticlesContracts;
using NewsSite.Core.ServiceContracts.ArticlesViewsContracts;
using NewsSite.Core.Services.ArticlesCommentsServices;
using NewsSite.Core.Services.ArticlesServices;
using NewsSite.Core.Services.ArticlesViewsServices;
using NewsSite.Infrastructure.DatabaseContext;
using NewsSite.Infrastructure.Repositories;

namespace NewsSite.UI.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();

            services.AddScoped<IArticlesRepository, ArticlesRepository>();
            services.AddScoped<IArticlesCommentsRepository, ArticlesCommentsRepository>();
            services.AddScoped<IArticlesViewsRepository, ArticlesViewsRepository>();

            services.AddSingleton<IArticleExpressionsProvider, ArticleExpressionProvider>();

            services.AddScoped<IArticlesValidatorService, ArticlesValidatorService>();
            services.AddScoped<IArticlesAdderService, ArticlesAdderService>();
            services.AddScoped<IArticlesDeleterService, ArticlesDeleterService>();
            services.AddScoped<IArticlesGetterService, ArticlesGetterService>();
            services.AddScoped<IArticlesUpdaterService, ArticlesUpdaterService>();

            services.AddScoped<IArticlesCommentsValidatorService, ArticlesCommentsValidatorService>();
            services.AddScoped<IArticlesCommentsAdderService, ArticlesCommentsAdderService>();
            services.AddScoped<IArticlesCommentsDeleterService, ArticlesCommentsDeleterService>();
            services.AddScoped<IArticlesCommentsGetterService, ArticlesCommentsGetterService>();
            services.AddScoped<IArticlesCommentsUpdaterService, ArticlesCommentsUpdaterService>();

            services.AddScoped<IArticlesViewsHandlerService, ArticlesViewsHandlerService>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                options.AddPolicy("NotAuthorized", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return !context.User.Identity!.IsAuthenticated;
                    });
                });
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/accessdenied";
            });

            return services;
        }
    }
}
