using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryBussinessLogic.Dto.ProductDto.cs
{
    public class GetallProductDto : BaseProductDto
    {
        public int ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
