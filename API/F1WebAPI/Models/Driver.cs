using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.Models
{
    public class Driver
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public int TeamID { get; set; }

        public int Poles { get; set; }
        public int RaceWins { get; set; }
        public int Championships { get; set; }
        public int CurrentPoints { get; set; }
    }
}
