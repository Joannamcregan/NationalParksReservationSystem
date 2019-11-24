using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public int Accessible { get; set; }
        public int MaxLength { get; set; }
        public int Utilities { get; set; }
        public decimal CostPerDay { get; set; }
        public string CampgroundName { get; set; }

        public static IList<string> Heading()
        {
            IList<string> heading = new List<string>();

            heading.Add("Results Matching Your Search Criteria");
            heading.Add("========================================");
            heading.Add("Site No.".PadRight(15) + "Max Occup.".PadRight(15) + "Accessible?".PadRight(12) + "RV Len".PadRight(10) + "Utility".PadRight(12) + "Cost".PadRight(30));
            return heading;
        }

        public static IList<string> HeadingForParkWide()
        {
            IList<string> heading = new List<string>();

            heading.Add("Results Matching Your Search Criteria");
            heading.Add("========================================");
            heading.Add("Campground".PadRight(13) +"Site No.".PadRight(15) + "Max Occup.".PadRight(15) + "Accessible?".PadRight(12) + "RV Len".PadRight(10) + "Utility".PadRight(12) + "Cost".PadRight(30));
            return heading;
        }

        public string printListing(int duration)
        {
            string accessible = Accessible == 0 ? "No" : "Yes";
            string utilities = Utilities == 0 ? "N/A" : "Yes";
            string rvLength = MaxLength == 0 ? "N/A" : MaxLength.ToString();
            
            decimal totalCost = duration * CostPerDay;
            return SiteNumber.ToString().PadRight(15)+MaxOccupancy.ToString().PadRight(15)+accessible.PadRight(12)+rvLength.PadRight(10)+utilities.PadRight(12)+$"{totalCost:C}";
        }

        public string printListingForParkWide(int duration)
        {
            string accessible = Accessible == 0 ? "No" : "Yes";
            string utilities = Utilities == 0 ? "N/A" : "Yes";
            string rvLength = MaxLength == 0 ? "N/A" : MaxLength.ToString();

            decimal totalCost = duration * CostPerDay;
            return CampgroundName.ToString().PadRight(15) + SiteId.ToString().PadRight(15) + MaxOccupancy.ToString().PadRight(15) + accessible.PadRight(12) + rvLength.PadRight(10) + utilities.PadRight(12) + $"{totalCost:C}";
        }
    }
}
