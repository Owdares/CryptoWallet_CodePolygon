using Blaved.Interfaces.Services;
using Blaved.Objects.Models.Configurations;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Resources;

namespace Bleved.Service
{
    public class InterfaceTranslatorService : IInterfaceTranslatorService
    {
        private readonly ConcurrentDictionary<string, Lazy<ResourceManager>> _resourceManagers;
        private readonly AppConfig _appConfig;
        public InterfaceTranslatorService(IOptions<AppConfig> appConfig)
        {
            _appConfig = appConfig.Value;
            _resourceManagers = new ConcurrentDictionary<string, Lazy<ResourceManager>>();
        }
        public string GetTranslation(string key, string language)
        {
            var lazyResourceManager = _resourceManagers.GetOrAdd(language, (lang) =>
            {
                return new Lazy<ResourceManager>(() =>
                {
                    return new ResourceManager($"{_appConfig.PathConfiguration.Translation}{lang}", System.Reflection.Assembly.Load("Blaved"));
                });
            });

            var translation = lazyResourceManager.Value.GetString(key);
            return translation ?? key;
        }
    }
}
