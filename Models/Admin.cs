namespace BookMyTicket.Models
{
    public class Admin
    {
        public List<Movie> MovieList { get; set; } = new List<Movie>();

        public int PendingOwnerCount { get; set; }
        public int TotalMovies { get; set; }
        public int TotalUsers { get; set; }


        public int ApprovedOwnerCount { get; set; }
        public int TotalBookings { get; set; }
        public List<User_Register> Pending_TheatreOwners { get; set; }


       }
}
