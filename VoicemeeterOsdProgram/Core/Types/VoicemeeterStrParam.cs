﻿using AtgDev.Voicemeeter;
using System;

namespace VoicemeeterOsdProgram.Core.Types
{
    public class VoicemeeterStrParam : VoicemeeterParameterBase<string>
    {
        protected readonly char[] m_strGetValueBuffer = new char[512];

        public VoicemeeterStrParam(RemoteApiExtender api, string command) : base(api, command)
        {
            m_value = string.Empty;
        }

        unsafe public override int GetParameter(out string val)
        {
            val = "";
            fixed (char* buffPtr = m_strGetValueBuffer)
            {
                fixed (byte* command = m_nameBuffer)
                {
                    var res = m_api.GetParameter((IntPtr)command, (IntPtr)buffPtr);
                    if (res == ResultCodes.Ok)
                    {
                        val = new string(buffPtr);
                    }
                    return res;
                }
            }
        }

        unsafe public override int SetParameter(string value)
        {
            int res = IsOptimized ? 
                m_api.GetParameter((IntPtr)NameBuffer, (IntPtr)buffPtr) : 
                m_api.GetParameter(Name, (IntPtr)buffPtr);
            if (res == ResultCodes.Ok)
            {
<<<<<<< Updated upstream
                return m_api.SetParameter((IntPtr)command, value);
=======
                val = new string(buffPtr);
>>>>>>> Stashed changes
            }
            return res;
        }
    }
<<<<<<< Updated upstream
=======

    unsafe public override int SetParameter(string value) => IsOptimized ?
            m_api.SetParameter((IntPtr)NameBuffer, value) :
            m_api.SetParameter(Name, value);
>>>>>>> Stashed changes
}
