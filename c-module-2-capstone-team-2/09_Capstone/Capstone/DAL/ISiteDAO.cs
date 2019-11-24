using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone
{
    public interface ISiteDAO
    {
        IList<Site> ListAvailableSites(int selectedCampgroundId, DateTime arrivalDate, DateTime departureDate);
        IList<Site> ListAvailableSitesInPark(Park selectedPark, DateTime desiredArrival, DateTime desiredDeparture);
    }
}