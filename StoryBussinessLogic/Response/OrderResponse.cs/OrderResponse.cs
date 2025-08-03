using StoryBussinessLogic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryBussinessLogic.Response.OrderResponse.cs
{
    public class OrderResponse : BaseResponse
    {

        public string? text { get; set; }
        public bool success { get; set; }


    }
}
