using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using ObstacleManagementSystem.Resources;

namespace ObstacleManagementSystem
{
    public class CustomLocalizer : StringLocalizer<Resources.Common>
    {
        private readonly IStringLocalizer _internalLocalizer;

        public CustomLocalizer(IStringLocalizerFactory factory) : base(factory)
        {

        }
        public CustomLocalizer(IStringLocalizerFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory)
        {
            CurrentLanguage = httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
            if(string.IsNullOrEmpty(CurrentLanguage) ||  CurrentLanguage.Length>2)
            {
                CurrentLanguage = "en";
            }
            //if (httpContextAccessor.HttpContext.RequestServices.GetService(typeof(RequestLocalizationOptions)) is RequestLocalizationOptions requestLocalizationOptions && !requestLocalizationOptions.SupportedCultures.Select(c => c.TwoLetterISOLanguageName)
            //    .Contains((CurrentLanguage)))
            //    throw new HttpException(HttpStatusCode.NotFound);
            _internalLocalizer = WithCulture(new CultureInfo(CurrentLanguage));
        }

        public override LocalizedString this[string name, params object[] arguments] => _internalLocalizer[name, arguments];

        public override LocalizedString this[string name] => _internalLocalizer[name];

        public string CurrentLanguage { get; set; }
    }
}
