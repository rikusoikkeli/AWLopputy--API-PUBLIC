using System;
using System.Collections.Generic;

#nullable disable

namespace RomEndprojectAwApiRikun.Models
{
    public partial class BrowsingStart
    {
        public BrowsingStart()
        {
            BrowsingEnds = new HashSet<BrowsingEnd>();
        }

        public string SessionId { get; set; }
        public DateTime? Time { get; set; }
        public string Domain { get; set; }
        public string Location { get; set; }
        public string DeviceId { get; set; }

        public virtual Device Device { get; set; }
        public virtual ICollection<BrowsingEnd> BrowsingEnds { get; set; }
    }
}
