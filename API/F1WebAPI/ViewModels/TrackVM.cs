using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.ViewModels
{
    public class TrackVM
    {
        public long Id { get; set; }
        public string Location { get; set; }

        public decimal Length { get; set; }
        public decimal FastestLap { get; set; }
    }
}
