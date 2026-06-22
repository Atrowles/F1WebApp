using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.Models
{
    public class Team
    {
        [Key]
        public long Id { get; set; }       
        public string Name { get; set; }        
        public string Engine { get; set; }
        public bool CustomerEngine { get; set; }
        public int CurrentPoints { get; set; }
    }
}
