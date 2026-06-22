using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.Models
{
    public class Track
    {
        [Key]
        public long Id { get; set; }
        public string Location { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Length { get; set; }
        public decimal FastestLap { get; set; }

    }
}
