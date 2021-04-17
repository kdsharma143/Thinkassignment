using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAccess
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Status = 0;
            Data = null;
            Message = "";
        }
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
