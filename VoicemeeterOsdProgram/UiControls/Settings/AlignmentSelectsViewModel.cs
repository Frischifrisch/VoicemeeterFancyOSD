﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoicemeeterOsdProgram.Types;

namespace VoicemeeterOsdProgram.UiControls.Settings
{
    public class AlignmentSelectsViewModel : BaseViewModel
    {
        public Dictionary<HorAlignment, string> HAValues { get; } = new()
        {
            { HorAlignment.Left, "Left" },
            { HorAlignment.Center, "Center" },
            { HorAlignment.Right, "Right" }
        };

        public Dictionary<VertAlignment, string> VAValues { get; } = new()
        {
            { VertAlignment.Top, "Top" },
            { VertAlignment.Center, "Center" },
            { VertAlignment.Bottom, "Bottom" }
        };
    }
}
