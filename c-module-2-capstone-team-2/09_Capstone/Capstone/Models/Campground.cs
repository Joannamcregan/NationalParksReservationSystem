using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {

        public int CampgroundId { get; set; }

        public int ParkId { get; set; }

        public string CampName { get; set; }

        public int Open_From_MM { get; set; }

        public int Open_To_MM { get; set; }

        public decimal Daily_Fee { get; set; }



    }
}
