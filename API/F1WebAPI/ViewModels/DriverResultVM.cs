using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.ViewModels
{
    public class DriverResultVM
    {
        public long Id { get; set; }
        public long RaceResultId { get; set; }
        public string DriverName { get; set; }
        public int Position { get; set; }
        public float Gap { get; set; }

    }
}

