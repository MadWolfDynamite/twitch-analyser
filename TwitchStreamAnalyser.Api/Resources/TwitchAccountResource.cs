﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Api.Resources
{
    public class TwitchAccountResource
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Display_Name { get; set; }

        public string Profile_Image_Url { get; set; }
        public int View_Count { get; set; }
    }
}
