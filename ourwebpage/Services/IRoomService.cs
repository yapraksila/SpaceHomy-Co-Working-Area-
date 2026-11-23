using System.Collections.Generic;
using ourwebpage.Models;

namespace ourwebpage.Services
{
    public interface IRoomService
    {
        IEnumerable<Facility> GetAllFacilities();
        Facility GetFacilityById(int facilityId);
        IEnumerable<FacilityAmenity> GetAmenitiesByFacilityId(int facilityId);
    }
}