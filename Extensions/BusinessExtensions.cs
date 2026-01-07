using AngularWithASP.Server.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;

namespace AngularWithASP.Server.Extensions
{
    public static class BusinessExtensions
    {
        public static WebApplication AddBusinessMiddleware(this WebApplication app)
        {
            return app;
        }

        public static WebApplicationBuilder AddBusinessSwagger(this WebApplicationBuilder builder)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                              new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "basic"
                                    }
                                },
                                new string[] {}
                        }
                    });
            }
            );
            return builder;
        }

        public static WebApplicationBuilder AddBusinessControllerAuthorization(this WebApplicationBuilder builder)
        {
            //i controller possono essere autorizzati sia via Basic che Windows authentication

            builder.Services.AddControllersWithViews(o =>
            {
                var auth = new AuthorizeAttribute() { AuthenticationSchemes = $@"Basic, Negotiate" };
                o.ApplyAuthorizeAttributes(nameof(TreeController), auth);
                //.ApplyAuthorizeAttributes(nameof(EventsController), auth)
                //.ApplyAuthorizeAttributes(nameof(ActionResultsController), auth)
                //.ApplyAuthorizeAttributes(nameof(AlarmsController), auth);
            });


            return builder;
        }
    }

    public static class AuthorizeAttributeInjectionMvcOptionsExtensions
    {
        public static MvcOptions ApplyAuthorizeAttributes(this MvcOptions options, string controllerName, params AuthorizeAttribute[] authorizeAttributes)
        {
            return options.ApplyAuthorizeAttributes(controllerName, null, authorizeAttributes);
        }
        public static MvcOptions ApplyAuthorizeAttributes(this MvcOptions options, string controllerName, string? actionName, params AuthorizeAttribute[] authorizeAttributes)
        {
            options.Conventions.Add(new AuthorizeAttributeInjectingConvention(controllerName, actionName, authorizeAttributes));
            return options;
        }
        public static MvcOptions ApplyAuthorizationPolicy(this MvcOptions options, string controllerName, string actionName, params string[] policies)
        {
            return options.ApplyAuthorizeAttributes(controllerName, actionName, policies.Select(e => new AuthorizeAttribute(e)).ToArray());
        }
    }

    public class AuthorizeAttributeInjectingConvention : IApplicationModelConvention
    {
        readonly string _controller;
        readonly string? _action;
        readonly AuthorizeFilter[] _authorizeFilters;
        public AuthorizeAttributeInjectingConvention(string controllerName, params AuthorizeAttribute[] authorizeAttributes)
            : this(controllerName, null, authorizeAttributes)
        {
        }
        public AuthorizeAttributeInjectingConvention(string controllerName, string? actionName, params AuthorizeAttribute[] authorizeAttributes)
        {
            _controller = controllerName;
            _action = actionName;
            _authorizeFilters = authorizeAttributes.Select(e => new AuthorizeFilter(new[] { e })).ToArray();
        }

        public void Apply(ApplicationModel application)
        {
            var filterModels = application.Controllers
                                         .Where(e => string.Equals(e.ControllerType.Name, _controller, StringComparison.OrdinalIgnoreCase))
                                         .ToList<IFilterModel>();
            if (filterModels.Count > 0 && !string.IsNullOrWhiteSpace(_action))
            {
                filterModels = filterModels.Cast<ControllerModel>()
                                           .SelectMany(e => e.Actions.Where(o => string.Equals(o.ActionName, _action, StringComparison.OrdinalIgnoreCase)))
                                           .ToList<IFilterModel>();
            }
            foreach (var filterModel in filterModels)
            {
                foreach (var af in _authorizeFilters)
                {
                    filterModel.Filters.Add(af);
                }
            }
        }
    }
}
