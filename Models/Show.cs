namespace BookMyTicket.Models
{
    public class Show
    {
        public int Screen_Id { set; get; }
        public string? FullName { set; get; }

        public TimeSpan StartTime { set; get; }
        public string? Movie_Name { get; set; }



    }
}
