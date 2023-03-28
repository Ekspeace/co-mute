using System.Buffers;
using System.ComponentModel.DataAnnotations;

namespace CoMute.Models
{
    public class CarPool
    {
        public int Id { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }
        [Required]
        public string Origin { get; set; }
        //really not sure if the days available, is the difference of today date and departure time or difference between arrival time and departure time. IF so the field shouldn't be captured.
        public int Days { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public int AvailableSeats { get; set; }
        [Required]
        public int Owner { get; set; }
        public string? Notes { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
