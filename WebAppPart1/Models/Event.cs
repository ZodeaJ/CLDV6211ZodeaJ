namespace WebAppPart1.Models
{
    public class Event
    {
        // Primary Key for Event
        public int EventId { get; set; }

        //Name for the Event
        public string EventName { get; set; }

        //Date of the Event
        public DateTime EventDate { get; set; }

        //Description for the Event
        public string Description { get; set; }

        //Foriegn Key to Venue
        public int VenueId { get; set; }

    }
}
