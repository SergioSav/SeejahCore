using System.Collections.Generic;
using UnityEngine.Localization.Settings;

namespace Assets.Scripts.Core.Utils
{
    public class LanguageService
    {
        public Dictionary<string, string> GetInfoForSelection()
        {
            var result = new Dictionary<string, string>();
            var availableLocales = LocalizationSettings.AvailableLocales;
            foreach (var lang in availableLocales.Locales)
                result[lang.Identifier.CultureInfo.Name] = lang.Identifier.CultureInfo.NativeName;

            return result;
        }

        public void SelectLocalization(string languageId)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(languageId);
        }
    }
}
