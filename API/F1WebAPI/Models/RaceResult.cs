using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.Models
{
    public class RaceResult
    {
        [Key]
        public long Id { get; set; }
        public long TrackId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal FastestLapTime { get; set; }
        public int FastestLapDriver { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal PoleTime { get; set; }
        public int PoleDriver { get; set; }
        public DateTime RaceDate { get; set; }
    }
}
