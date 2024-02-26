﻿using System.Windows;
using TOTPTokenGuard.Core.Models;
using TOTPTokenGuard.Core.Storage;

namespace TOTPTokenGuard.Core
{
    class I18n
    {
        // Define supported languages in AppSettings enum too
        private static readonly Dictionary<string, string> fullLanguageNames =
            new()
            {
                { "system", "System" },
                { "en", "English" },
                { "de", "Deutsch" }
            };
        private static readonly LanguageSetting defaultLanguage = LanguageSetting.EN;
        private static LanguageSetting currentLanguage = defaultLanguage;
        private static ResourceDictionary? dict;

        public static LanguageSetting GetLanguage()
        {
            if (SettingsManager.Settings.Language != LanguageSetting.System)
            {
                return SettingsManager.Settings.Language;
            }

            LanguageSetting? systemLang = GetSystemLanguageIfSupported();
            if (systemLang != null)
            {
                return systemLang.Value;
            }

            return defaultLanguage;
        }

        public static LanguageSetting? GetSystemLanguageIfSupported()
        {
            string lang = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            if (Enum.TryParse<LanguageSetting>(lang, true, out LanguageSetting result))
            {
                return result;
            }

            return null;
        }

        public static void InitI18n()
        {
            currentLanguage = GetLanguage();
            dict = new ResourceDictionary
            {
                Source = new Uri(
                    @"Resources/Strings." + currentLanguage.ToString().ToLower() + ".xaml",
                    UriKind.Relative
                )
            };
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        public static async void ChangeLanguage(LanguageSetting lang)
        {
            if (dict == null)
            {
                return;
            }

            if (lang == currentLanguage)
            {
                return;
            }

            if (lang == LanguageSetting.System)
            {
                lang = GetSystemLanguageIfSupported() ?? defaultLanguage;
            }

            dict.Source = new Uri(
                @"Resources/Strings." + lang.ToString().ToLower() + ".xaml",
                UriKind.Relative
            );
            currentLanguage = lang;
            SettingsManager.Settings.Language = lang;
            await SettingsManager.Save();
        }

        public static LanguageSetting GetCurrentLanguage()
        {
            return currentLanguage;
        }

        public static string GetString(string key)
        {
            if (dict == null)
            {
                return "Error!";
            }
            if (dict[$"i.{key.ToLower()}"] is not string content)
            {
                return $"??? {key} ???";
            }
            return content;
        }

        public static string GetFullLanguageName(string lang)
        {
            return fullLanguageNames[lang.ToLower()];
        }
    }
}
