using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomEndprojectAwApiRikun.Models
{
    public class StartEndTableModel
    {
        public string SessionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Domain { get; set; }
        public string DeviceId { get; set; }
    }
}
