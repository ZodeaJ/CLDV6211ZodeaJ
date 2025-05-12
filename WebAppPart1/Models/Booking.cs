using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebAppPart1.Models
{
    [Index(nameof(VenueId), nameof(BookingDate), IsUnique = true)]

    public class Booking
    {
        // Primary Key for Booking
        public int BookingId { get; set; }

        // Foreign Key to Event
        public int EventId { get; set; }

        // Foreign Key to Venue
        public int VenueId { get; set; }

        //Date for the Booking
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingDate { get; set; }

        public Event? Event { get; set; }
        public Venue? Venue { get; set; }

    }
}
