using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.Models.Cars;

namespace CarRental.Infrastructure.Data.Helpers
{
    class CarComparer : IEqualityComparer<Car>
    {
        public bool Equals(Car x, Car y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Id == y.Id && x.Model == y.Model;
        }
        public int GetHashCode(Car product)
        {
            if (Object.ReferenceEquals(product, null)) return 0;
            int hashProductName = product.Id == null ? 0 : product.Id.GetHashCode();
            int hashProductCode = product.Model.GetHashCode();
            return hashProductName ^ hashProductCode;
        }
    }
}
