using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Data.Services;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

namespace Assets.Scripts.Core.Utils
{
    public class LanguageService
    {
        private Dictionary<string, string> _availableLocaleDict;

        private readonly ISaveService _saveService;
        private readonly IPlatformService _platformService;
        private GameSettingsSaveState _gameSettingsSave;

        public string CurrentLocale { get; private set; }

        public LanguageService(ISaveService saveService, IPlatformService platformService)
        {
            _saveService = saveService;
            _platformService = platformService;

            _gameSettingsSave = _saveService.GetSettingsSave();
            var localeId = !string.IsNullOrEmpty(_gameSettingsSave.CurrentLanguageId) ? _gameSettingsSave.CurrentLanguageId : _platformService.GetLanguageId();
            SelectLocalization(localeId);
            PrepareLocales();
        }

        public void SwitchLocaleTo(string langId)
        {
            SelectLocalization(langId);

            _gameSettingsSave.CurrentLanguageId = langId;
            _saveService.Save(_gameSettingsSave);
        }

        public Dictionary<string, string> GetInfoForSelection() => _availableLocaleDict;

        private void SelectLocalization(string languageId)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(languageId);
            CurrentLocale = languageId;
        }

        private void PrepareLocales()
        {
            _availableLocaleDict = new Dictionary<string, string>();
            var availableLocales = LocalizationSettings.AvailableLocales;
            foreach (var lang in availableLocales.Locales)
                _availableLocaleDict[lang.Identifier.CultureInfo.Name] = lang.Identifier.CultureInfo.NativeName;
        }
    }
}
