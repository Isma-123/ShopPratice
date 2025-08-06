using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryBussinessLogic.Dto.CategoryDto.cs
{
    public class GetCategoryDto : BaseCategoryDto
    {
        public int CategoryId { get; set; }
        public bool IsActive { get; internal set; }
    }
}
