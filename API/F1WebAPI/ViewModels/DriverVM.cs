using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.ViewModels
{
    public class DriverVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string TeamName { get; set; }
        public int TeamId { get; set; }

        public int Poles { get; set; }
        public int RaceWins { get; set; }
        public int Championships { get; set; }
        public int CurrentPoints { get; set; }
    }
}
