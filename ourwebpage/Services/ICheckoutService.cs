using System.Collections.Generic;
using ourwebpage.Models;

namespace ourwebpage.Services
{
    public interface ICheckoutService
    {
        decimal CalculateCartTotal(int roleId, IEnumerable<int> selectedAmenityIds = null);
        Booking ProcessCheckout(int roleId, string paymentMethod, IEnumerable<int> selectedAmenityIds = null);
    }
}
