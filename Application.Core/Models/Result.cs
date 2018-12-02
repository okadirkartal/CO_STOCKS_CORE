using System.Collections.Generic;

namespace Application.Core.Models
{
    public class Result
    {
        public string ReturnMessage { get; set; }

        public  List<string> ReturnMessageList { get; set; }


        public int ReturnCode { get; set; }

        public bool IsSuccess { get; set; }
    }
}