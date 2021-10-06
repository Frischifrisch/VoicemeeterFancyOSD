﻿using AtgDev.Voicemeeter;
using System;

namespace VoicemeeterOsdProgram.Core.Types
{
    public class VoicemeeterParameter
    {
        private readonly string m_command;
        private readonly RemoteApiExtender m_api;
        private bool m_isInit;
        private float m_value;

        public VoicemeeterParameter(RemoteApiExtender api, string command)
        {
            m_api = api;
            m_command = command;
        }

        public float Value
        {
            get => m_value;
            private set
            {
                if ((value != m_value) && m_isInit)
                {
                    OnReadValueChanged(m_value, value);
                }
                m_value = value;
            }
        }

        public void Read()
        {
            ReadIsIgnoreEvent(false);
        }

        public void ReadNoEvent()
        {
            ReadIsIgnoreEvent(true);
        }

        public void ReadIsIgnoreEvent(bool isIgnore)
        {
            if ((m_api is null) || string.IsNullOrEmpty(m_command)) return;

            if (m_api.GetParameter(m_command, out float val) == 0)
            {
                if (isIgnore)
                {
                    m_value = val;
                } 
                else
                {
                    Value = val;
                }
                m_isInit = true;
            }
        }

        public void Write(float value)
        {
            if ((m_api is null) || string.IsNullOrEmpty(m_command)) return;

            if (m_api.SetParameter(m_command, value) == 0)
            {
                m_value = value;
            }
        }

        public event EventHandler<ValOldNew<float>> ReadValueChanged;

        private void OnReadValueChanged(float oldVal, float newVal)
        {
            ValOldNew<float> values = new(oldVal, newVal);
            ReadValueChanged?.Invoke(this, values);
        }
    }
}
