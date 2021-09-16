using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomEndprojectAwApiRikun.Models
{
    public class TimeEmotions
    {
        public double Time { get; set; } //Time in minutes
        public int? Happiness { get; set; }
        public int? Sadness { get; set; }
        public int? Anger { get; set; }
        public int? Neutral { get; set; }
        public int? Surprise { get; set; }
        public int? Disgust { get; set; }

        public TimeEmotions(double time, int happiness, int sadness, int anger, int neutral, int surprise, int disgust)
        {
            Time = time;
            Happiness = happiness;
            Sadness = sadness;
            Anger = anger;
            Neutral = neutral;
            Surprise = surprise;
            Disgust = disgust;
        }
    }
}