using System;
using System.Collections.Generic;

#nullable disable

namespace RomEndprojectAwApiRikun.Models
{
    public partial class BrowsingEnd
    {
        public int Id { get; set; }
        public DateTime? Time { get; set; }
        public string SessionId { get; set; }
        public string Location { get; set; }
        public string DeviceId { get; set; }

        public virtual Device Device { get; set; }
        public virtual BrowsingStart Session { get; set; }
    }
}
