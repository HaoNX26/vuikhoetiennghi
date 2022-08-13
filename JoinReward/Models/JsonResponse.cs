using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinReward.Models
{
    public class JsonResponse
    {
        public string success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public string data_string { get; set; }

        public JsonResponse()
        {
            success = "Y";
        }
    }
}
