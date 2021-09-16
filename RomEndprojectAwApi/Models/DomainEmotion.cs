using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomEndprojectAwApiRikun
{
    public class DomainEmotion
    {
        public string Domain { get; set; }
        public int? Happiness { get; set; }
        public int? Sadness { get; set; }
        public int? Anger { get; set; }
        public int? Neutral { get; set; }
        public int? Surprise { get; set; }
        public int? Disgust { get; set; }

        //Remeber to do the constructor
        public DomainEmotion()
        {

        }

    }
}
