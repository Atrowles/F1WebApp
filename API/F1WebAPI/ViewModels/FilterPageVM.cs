using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.ViewModels
{
    public class FilterPageVM
    {
        public string SearchStr { get; set; }
        public int PageNo { get; set; }
        public int ItemsPerPage { get; set; }
        public string SortStr { get; set; }
    }
}
