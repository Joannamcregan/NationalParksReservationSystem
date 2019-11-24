using Capstone.Models;
using System.Collections.Generic;

namespace Capstone
{
    public interface ICampgroundDAO
    {
        IList<Campground> GetAllCampgrounds(Park selectedPark);
    }
}