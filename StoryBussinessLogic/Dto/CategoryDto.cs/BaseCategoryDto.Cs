using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryBussinessLogic.Dto.CategoryDto.cs
{
    public class BaseCategoryDto
    {

        public DateTime date { get; set; } = DateTime.Now;
       
        public string CategoryName { get; set; } = null!;
    }
}
