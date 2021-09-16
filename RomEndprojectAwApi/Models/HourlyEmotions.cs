using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomEndprojectAwApiRikun.Models
{
    public class HourlyEmotions
    {
        public int HourOrWeekdayNumber { get; set; }
        public int? Happiness { get; set; }
        public int? Sadness { get; set; }
        public int? Anger { get; set; }
        public int? Neutral { get; set; }
        public int? Surprise { get; set; }
        public int? Disgust { get; set; }
    }
}
