using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.ViewModels
{
    public class TeamVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Engine { get; set; }
        public bool CustomerEngine { get; set; }
        public int CurrentPoints { get; set; }
    }
}
