using System;
using System.Collections.Generic;

namespace ourwebpage.Services
{
    public interface IAvailabilityService
    {
        bool IsAvailable(int facilityId, DateTime bookingDay, TimeSpan startTime, TimeSpan endTime);
        IEnumerable<TimeSpan> GetBookedStartTimes(int facilityId, DateTime bookingDay);
    }
}
