using System.ComponentModel.DataAnnotations;

namespace BookMyTicket.Models
{
    public class Screen
    {
        public int Screen_Id { set; get; }
        public int User_Id { set; get; }

        public int Movie_Id { set; get; }
        
        public string? Movie_Name { set; get; }

        public List<Movie>? Movie_List { set; get; }

        [Required(ErrorMessage = "Enter Screen Name")]
        public string? ScreenName { set; get; }

        [Required(ErrorMessage = "Enter the Row value")]
        public int Rows { set; get; }

        [Required(ErrorMessage = "Enter the Column value")]
        public int Columns { set; get; }

        [Required(ErrorMessage = "Select Show Date")]
        [DataType(DataType.Date)]
        public DateTime ShowDate { set; get; }

        [Required(ErrorMessage = "Enter Start Time")]
        public TimeSpan StartTime { set; get; }

        [Required(ErrorMessage = "Enter End Time")]
        public TimeSpan EndTime { set; get; }

        [Required(ErrorMessage = "Enter Ticket Price")]
        public decimal TicketPrice { set; get; }
        public List<string>? UnavailableSeats { get; set; }
        public int TotalScreens { get; set; }
        public int TotalShows { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<Booking>? Booking_List { get; set; }
        public bool IsEnable { get; set; }






    }
}
