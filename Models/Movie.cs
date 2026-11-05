using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookMyTicket.Models
{
    public class Movie
    {
        public int Movie_Id { get; set; }

        [Required(ErrorMessage = "Enter Movie Name")]
        public string? Movie_Name { get; set; }

        [Required(ErrorMessage = "Enter Movie Description")]

        public string? Movie_Description { get; set; }

        [Required(ErrorMessage = "Enter Language")]
        public string? Language { get; set; }

        public string? Movie_Image { get; set; }
        public IFormFile? Movie_Image_File { get; set; }

        [Required(ErrorMessage = "Enter Date")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Enter Genre")]
        public string? Genre { get; set; }

        [Required(ErrorMessage = "Enter Year")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Enter a valid 4-digit year")]

        public string? Year { get; set; }
        public bool IsEnable { get; set; }
        

    }
}
