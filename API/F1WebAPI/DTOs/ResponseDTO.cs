using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F1WebAPI.DTOs
{
    public class ResponseDTO
    {

        public ResponseDTO(int totalRecords, object result= null)
        {
            Result = result;       
            TotalRecords = totalRecords;
        }

        public object Result { get; set; }
        public int TotalRecords { get; set; }

    }
}
