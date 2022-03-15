﻿using System;
using System.Windows;
using VoicemeeterOsdProgram.UiControls.OSD.Strip;
using VoicemeeterOsdProgram.Factories;

namespace VoicemeeterOsdProgram.Core
{
    partial class OsdWindowManager
    {
        private static void UpdateOsdElementsVis()
        {
            m_wpfControl.AllowAutoUpdateSeparators = false;

            var children = m_wpfControl.MainContent.Children;
            foreach (StripControl strip in children)
            {
                bool isAnyVisibleBtn = false;
                foreach (ButtonContainer btnCont in strip.BusBtnsContainer.Children)
                {
                    if (btnCont.Visibility == Visibility.Visible)
                    {
                        isAnyVisibleBtn = true;
                        break;
                    }
                }
                foreach (ButtonContainer btnCont in strip.ControlBtnsContainer.Children)
                {
                    if (btnCont.Visibility == Visibility.Visible)
                    {
                        isAnyVisibleBtn = true;
                        break;
                    }
                }
                foreach (ButtonContainer btnCont in strip.AdditionalControlBtns.Children)
                {
                    if (btnCont.Visibility == Visibility.Visible)
                    {
                        isAnyVisibleBtn = true;
                        break;
                    }
                }

                bool isVisibleChildren = (strip.FaderCont.Visibility == Visibility.Visible) || isAnyVisibleBtn;
                if (isVisibleChildren)
                {
                    strip.Visibility = Visibility.Visible;
                    UpdateAlwaysVisibleElements(strip);
                }
            }

            m_wpfControl.UpdateSeparators();
            m_wpfControl.AllowAutoUpdateSeparators = true;
        }

        private static void UpdateAlwaysVisibleElements(StripControl strip)
        {
            var options = Options.OptionsStorage.Osd;
            foreach (ButtonContainer btnCont in strip.BusBtnsContainer.Children)
            {
                if (!options.AlwaysShowBusBtns) break;

                btnCont.Visibility = Visibility.Visible;
            }
            foreach (ButtonContainer btnCont in strip.ControlBtnsContainer.Children)
            {
                bool hasAlwaysVisibleBtns = options.AlwaysShowMonoBtn || options.AlwaysShowMuteBtn || options.AlwaysShowSoloBtn;
                if (!hasAlwaysVisibleBtns) break;

                if (btnCont.IsAlwaysVisible?.Invoke() ?? false)
                {
                    btnCont.Visibility = Visibility.Visible;
                }
            }
        }

        private static void ApplyVisibilityToOsdElements(Visibility vis)
        {
            m_wpfControl.AllowAutoUpdateSeparators = false;

            var children = m_wpfControl.MainContent.Children;
            foreach (StripControl strip in children)
            {
                strip.Visibility = vis;
                strip.FaderCont.Visibility = vis;
                foreach (ButtonContainer btnCont in strip.BusBtnsContainer.Children)
                {
                    btnCont.Visibility = vis;
                }
                foreach (ButtonContainer btnCont in strip.ControlBtnsContainer.Children)
                {
                    btnCont.Visibility = vis;
                }
                foreach (ButtonContainer btnCont in strip.AdditionalControlBtns.Children)
                {
                    btnCont.Visibility = vis;
                }
            }

            m_wpfControl.UpdateSeparators();
            m_wpfControl.AllowAutoUpdateSeparators = true;
        }
    }
}
