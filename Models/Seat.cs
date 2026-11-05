using System.ComponentModel.DataAnnotations;

namespace BookMyTicket.Models
{
    public class Seat
    {
        public int Seat_Id { set; get; }
        public int Screen_Id { set; get; }

        [Required(ErrorMessage = "Enter Seat Name ")]
        public string? Seat_Name { set; get; }

        [Required(ErrorMessage = "Enter the Seat Row value")]

        public int Seat_Row { set; get; }

        [Required(ErrorMessage = "Enter the Seat Column value")]

        public int Seat_Columns { set; get; }

        public bool IsBooked { set; get; }
        public bool IsEnable { get; set; }
    }
}
