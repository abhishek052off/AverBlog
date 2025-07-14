using System.ComponentModel.DataAnnotations;

namespace AverBlog.Api.ApiDtos
{
    public class YearAndMonthInput
    {
        [Required]
        [Range(2000,2030)]
        public int? Year { get; set; }

        [Required]
        [Range(1,12)]
        public int? Month { get; set; }
    }
}
