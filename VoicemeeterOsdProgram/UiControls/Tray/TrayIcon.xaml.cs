﻿using System;
using System.Diagnostics;
using System.Windows;
using VoicemeeterOsdProgram.Core;
using VoicemeeterOsdProgram.Options;

namespace VoicemeeterOsdProgram.UiControls.Tray
{
    /// <summary>
    /// Interaction logic for TrayIcon.xaml
    /// </summary>
    public partial class TrayIcon : Window
    {
        public TrayIcon()
        {
            InitializeComponent();
#if DEBUG
            DebugWindowItem.Visibility = Visibility.Visible;
            DebugWindowItem.Click += OnDebugWindowClick;
#endif
        }

        private void TogglePaused()
        {
            if (OsdWindowManager.IsEnabled)
            {
                OsdWindowManager.IsEnabled = false;
                NotifyIcon.Icon = Properties.Resources.MainIconInactive;
                PausedItem.IsChecked = true;
                PausedItem.FontWeight = FontWeights.Bold;
            }
            else
            {
                OsdWindowManager.IsEnabled = true;
                NotifyIcon.Icon = Properties.Resources.MainIcon;
                PausedItem.IsChecked = false;
                PausedItem.FontWeight = FontWeights.Normal;
            }
        }

        private void OnOpenConfigClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using Process p = new();
                p.StartInfo.FileName = "explorer.exe";
                p.StartInfo.Arguments = $@"""{OptionsStorage.ConfigFilePath}""";
                _ = p.Start();
            }
            catch { }
        }

        private void OnPausedClick(object sender, RoutedEventArgs e)
        {
            TogglePaused();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            TogglePaused();
        }

#if DEBUG
        private DebugWindow m_debugWin;

        private void OnDebugWindowClick(object sender, EventArgs e)
        {
            if (m_debugWin is null)
            {
                m_debugWin = new DebugWindow();
                m_debugWin.Closing += OnDebugWin_Closing;
            }
            m_debugWin.Show();
            m_debugWin.Activate();
        }

        private void OnDebugWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            m_debugWin.Hide();
        }
#endif
    }
}
