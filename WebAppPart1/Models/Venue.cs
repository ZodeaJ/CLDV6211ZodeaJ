namespace WebAppPart1.Models
{
    public class Venue
    {
        //Primary Key for Venue
        public int VenueId { get; set; }

        //Name of the venue
        public string VenueName { get; set; }

        //Location of the Venue
        public string Location { get; set; }

        //Capacity for the Venue
        public int Capacity { get; set; }

        //Image url for the venue
        public string ImageUrl { get; set; }

    }
}
