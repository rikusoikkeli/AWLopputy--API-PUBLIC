using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomEndprojectAwApiRikun.Models
{
    public class CustomDomainEmotion
    {
        public string Domain { get; set; }
        public List<StartEndTableModel> StartEndTable { get; set; }
        public List<Emotion> Emotions { get; set; }
    }
}
