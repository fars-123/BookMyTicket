namespace BookMyTicket.Models
{
    public class Booking
    {
        public int Booking_Id { get; set; }

        public int User_Id { get; set; }

        public int Screen_Id { get; set; }

        public DateTime ShowDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public decimal TicketPrice { get; set; }

        public int SeatCount { get; set; }

        public decimal TotalAmount { get; set; }

        public List<string>? Seats { get; set; }

        public string? PaymentStatus { get; set; }

        public string? RazorpayOrderId { get; set; }
        public string? RazorpayKeyId { get; set; }


        public string? RazorpayPaymentId { get; set; }
        public DateTime BookingDate { get; set; }



        public string? FullName { get; set; }
        public string? Movie_Name { get; set; }


    }
}
