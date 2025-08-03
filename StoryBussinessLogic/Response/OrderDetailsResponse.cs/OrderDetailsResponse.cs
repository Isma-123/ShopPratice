

using StoryBussinessLogic.Core;
using StoryBussinessLogic.Dto.OrderDetailsDto.cs;

namespace StoryBussinessLogic.Response.OrderDetailsResponse.cs
{
    public class OrderDetailsResponse : BaseResponse
    {
        public string? text { get; set; }
        public bool success { get; set; }

    }
}
