﻿using TOTPTokenGuard.Core.Models;

namespace TOTPTokenGuard.Core
{
    internal class EventManager
    {
        internal class AppThemeChangedEventArgs : EventArgs
        {
            internal ThemeSetting Theme { get; set; }

            internal enum EventSource
            {
                Navigation,
                Settings
            }

            internal EventSource Source { get; set; }
        }

        internal static event EventHandler<int> TokenUpdated = delegate { };
        internal static event EventHandler<AppThemeChangedEventArgs> AppThemeChanged = delegate { };

        internal static void EmitTokenUpdated(int tokenID)
        {
            TokenUpdated?.Invoke(null, tokenID);
        }

        internal static void EmitAppThemeChanged(
            ThemeSetting theme,
            AppThemeChangedEventArgs.EventSource source
        )
        {
            AppThemeChanged?.Invoke(
                null,
                new AppThemeChangedEventArgs { Theme = theme, Source = source }
            );
        }
    }
}
