using System;
using System.Collections.Generic;

#nullable disable

namespace RomEndprojectAwApiRikun.Models
{
    public partial class Device
    {
        public Device()
        {
            BrowsingEnds = new HashSet<BrowsingEnd>();
            BrowsingStarts = new HashSet<BrowsingStart>();
            Emotions = new HashSet<Emotion>();
        }

        public string DeviceId { get; set; }
        public string Password { get; set; }
        public DateTime? LastActive { get; set; }

        public virtual ICollection<BrowsingEnd> BrowsingEnds { get; set; }
        public virtual ICollection<BrowsingStart> BrowsingStarts { get; set; }
        public virtual ICollection<Emotion> Emotions { get; set; }
    }
}
