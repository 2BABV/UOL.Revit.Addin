using Cadac.ClientTools;
using Microsoft.Extensions.DependencyInjection;
using UOL.Revit.SampleAddin.Properties;

namespace UOL.Revit.SampleAddin
{
    internal class RegistryHelper
    {
        private const string SupporterLanguages = "nl-NL;en-US;";
        private readonly IRegistryManager registryManager = ApplicationGlobals.ApplicationContext.GetService<IRegistryManager>();

        public string GetLanguage()
        {
            var registryItem = registryManager.GetItem(Resources.Registry_SubKey_User, Resources.Registry_Key_Language, false);

            if (registryItem == null || string.IsNullOrEmpty(registryItem.ValueData) || !SupporterLanguages.Contains(registryItem.ValueData))
            {
                registryItem = registryManager.GetItem(Resources.Registry_SubKey_User, Resources.Registry_Key_Language, true);
            }

            if (registryItem == null || string.IsNullOrEmpty(registryItem.ValueData) || !SupporterLanguages.Contains(registryItem.ValueData))
            {
                var currentCulture = "nl-NL";

                if (!SupporterLanguages.Contains(currentCulture))
                {
                    currentCulture = "nl-NL";
                }

                registryItem = new Cadac.ClientTools.Models.RegistryItem() { Key = Resources.Registry_SubKey_User, ValueName = Resources.Registry_Key_Language, ValueData = currentCulture };
            }

            return registryItem.ValueData;
        }
    }
}
