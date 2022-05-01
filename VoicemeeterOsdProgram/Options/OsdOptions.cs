﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using VoicemeeterOsdProgram.Types;

namespace VoicemeeterOsdProgram.Options
{
    public class OsdOptions : OsdOptionsBase
    {
        private bool m_dontShowIfVoicemeeterVisible = true;
        private bool m_isInteractable = false;
        private uint m_durationMs = 2000;
        private double m_backgroundOpacity = 0.9;
        private bool m_animationsEnabled = true;
        private bool m_waitForVmInit = true;
        private HashSet<StripElements> m_alwaysShowElements = new(new[] { StripElements.None });
        private HashSet<StripElements> m_neverShowElements = new(new[] { StripElements.None });
        private HashSet<uint> m_ignoreStripsIndexes = new();

        public OsdOptions()
        {
            m_alwaysShowElements.Add(StripElements.None);
        }

        [Description("Dont show OSD if Voicemeeter's window is visible (and not obstructed) or is active window")]
        public bool DontShowIfVoicemeeterVisible 
        {
            get => m_dontShowIfVoicemeeterVisible;
            set => HandlePropertyChange(ref m_dontShowIfVoicemeeterVisible, ref value, DontShowIfVoicemeeterVisibleChanged);
        }

        [Description("User can interact with UI elements in OSD (Gain Fader, Mute, Mono, etc)")]
        public bool IsInteractable 
        {
            get => m_isInteractable;
            set => HandlePropertyChange(ref m_isInteractable, ref value, IsInteractableChanged);
        }

        [Description("How long OSD is displayed (in milliseconds)")]
        public uint DurationMs
        {
            get => m_durationMs;
            set => HandlePropertyChange(ref m_durationMs, ref value, DurationMsChanged);
        }

        [Description("From 0.0 to 1.0. The recommended is 0.8 - 0.95")]
        public double BackgroundOpacity
        {
            get => m_backgroundOpacity;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 1)
                {
                    value = 1;
                }
                HandlePropertyChange(ref m_backgroundOpacity, ref value, BackgroundOpacityChanged);
            }
        }

        [Description("Enable animations for elements of OSD (Buttons, Faders)")]
        public bool AnimationsEnabled
        {
            get => m_animationsEnabled;
            set => HandlePropertyChange(ref m_animationsEnabled, ref value, AnimationsEnabledChanged);
        }

        [Description("When Voicemeeter is launched it may report changes for a few seconds, it will trigger OSD to show up. This option prevents it.")]
        public bool WaitForVoicemeeterInitialization
        {
            get => m_waitForVmInit;
            set => HandlePropertyChange(ref m_waitForVmInit, ref value, WaitForVoicemeeterInitializationChanged);
        }

        [Description("Always show these elements on any Strip change. Multiple values separated by commas. Example: AlwaysShowElements = Mute, Buses")]
        public HashSet<StripElements> AlwaysShowElements
        {
            get => m_alwaysShowElements;
            set
            {
                HandleStripElementsCollection(value);
                HandlePropertyChange(ref m_alwaysShowElements, ref value, AlwaysShowElementsChanged);
            }
        }

        [Description($"Never show these elements on any Strip change, prioritized over {nameof(AlwaysShowElements)}. Multiple values separated by commas. Example: NeverShowElements = Mute, Buses")]
        public HashSet<StripElements> NeverShowElements
        {
            get => m_neverShowElements;
            set
            {
                HandleStripElementsCollection(value);
                HandlePropertyChange(ref m_neverShowElements, ref value, NeverShowElementsChanged);
            }
        }

        [Description("Dont show changes from Inputs or Outputs with these indexes. Numbering is zero-based. Multiple value separated by commas. Example: IgnoreStripsIndexes = 0, 5, 12")]
        public HashSet<uint> IgnoreStripsIndexes
        {
            get => m_ignoreStripsIndexes;
            set => HandlePropertyChange(ref m_ignoreStripsIndexes, ref value, IgnoreStripsIndexesChanged);
        }

        public override IEnumerable<KeyValuePair<string, string>> ToDict() => ToDictAllTypes();

        public override void FromDict(Dictionary<string, string> dict) => FromDictAllTypes(dict);

        public override bool TryParseFrom(string toPropertyName, string fromVal)
        {
            switch (toPropertyName)
            {
                case nameof(AlwaysShowElements):
                    AlwaysShowElements = new(ParseEnumerableFrom<StripElements>(fromVal, ","));
                    return true;
                case nameof(NeverShowElements):
                    NeverShowElements = new(ParseEnumerableFrom<StripElements>(fromVal, ","));
                    return true;
                case nameof(IgnoreStripsIndexes):
                    IgnoreStripsIndexes = new(ParseEnumerableFrom<uint>(fromVal, ","));
                    return true;
                default:
                    return base.TryParseFrom(toPropertyName, fromVal);
            }
        }

        public override bool TryParseTo(string fromPropertyName, out string toVal)
        {
            switch (fromPropertyName)
            {
                case nameof(AlwaysShowElements):
                    toVal = string.Join(", ", AlwaysShowElements);
                    return true;
                case nameof(NeverShowElements):
                    toVal = string.Join(", ", NeverShowElements);
                    return true;
                case nameof(IgnoreStripsIndexes):
                    toVal = string.Join(", ", IgnoreStripsIndexes);
                    return true;
                default:
                    return base.TryParseTo(fromPropertyName, out toVal);
            }
        }

        private void HandleStripElementsCollection(ICollection<StripElements> elements)
        {
            if ((elements.Count > 1) && elements.Contains(StripElements.None))
            {
                elements.Remove(StripElements.None);
            }
            if (elements.Count == 0)
            {
                elements.Add(StripElements.None);
            }
        }

        public event EventHandler<bool> DontShowIfVoicemeeterVisibleChanged;
        public event EventHandler<bool> IsInteractableChanged;
        public event EventHandler<uint> DurationMsChanged;
        public event EventHandler<double> BackgroundOpacityChanged;
        public event EventHandler<bool> AnimationsEnabledChanged;
        public event EventHandler<bool> WaitForVoicemeeterInitializationChanged;
        public event EventHandler<HashSet<StripElements>> AlwaysShowElementsChanged;
        public event EventHandler<HashSet<StripElements>> NeverShowElementsChanged;
        public event EventHandler<HashSet<uint>> IgnoreStripsIndexesChanged;
    }
}
