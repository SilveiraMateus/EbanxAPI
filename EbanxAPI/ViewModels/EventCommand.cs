using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbanxAPI.ViewModels
{
    public class EventCommand
    {
        public string type { get; set; }
        public string destination { get; set; }
        public string origin { get; set; }
        public decimal amount { get; set; }
    }
}