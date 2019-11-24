using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone
{
    public interface IReservationDAO
    {
        int MakeReservation(int siteId, string name, DateTime fromDate, DateTime toDate);

    }
}