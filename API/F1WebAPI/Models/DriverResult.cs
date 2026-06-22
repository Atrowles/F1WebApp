using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.Models
{
    public class DriverResult
    {
        [Key]
        public long Id { get; set; }
        public long ResultId { get; set; }
        public long DriverId { get; set; }
        public int Position { get; set; }
        public float Gap { get; set; }
    }
}
