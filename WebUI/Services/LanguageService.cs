using Microsoft.Extensions.Localization;
using System.Reflection;
using WebUI.Resource;

namespace WebUI.Services
{
    public class LanguageService
    {
        private readonly IStringLocalizer _Localizer;

        public LanguageService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemplyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);

            _Localizer = factory.Create(nameof(SharedResource), assemplyName.Name);
        }

        public LocalizedString GetKey(string key)
        {
            return _Localizer[key];
        }


    }
}
