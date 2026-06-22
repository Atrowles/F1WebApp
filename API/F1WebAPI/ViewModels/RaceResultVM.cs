using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.ViewModels
{
    public class RaceResultVM
    {
        public long Id { get; set; }
        public string TrackLocation { get; set; }
        public long TrackId { get; set; }
        public decimal FastestLapTime { get; set; }
        public string FastestLapDriver { get; set; }
        public decimal PoleTime { get; set; }
        public string PoleDriver { get; set; }

        public string RaceWinner { get; set; }

        public string RaceDate { get; set; }

    }
}
