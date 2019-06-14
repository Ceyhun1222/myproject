using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ObstacleManagementSystem
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public const string LangCode = "lang";

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(LangCode))
            {
                return false;
            }

            var lang = values[LangCode].ToString();
            //return httpContext.Features.Get<IOptions<RequestLocalizationOptions>>().Value.SupportedCultures.Select(c => c.TwoLetterISOLanguageName).Contains(lang);
            return lang == "en" || lang == "ru";
        }
    }
}
