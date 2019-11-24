using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; }
        public int FromDate { get; set; }
        public int ToDate { get; set; }
        public int NumberOfNights { get; set; }
        public decimal CostPerNight { get; set; } //maybe this will go away
        public decimal TotalCost { get; set; } //will change
    }
}
