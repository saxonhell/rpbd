using Microsoft.Extensions.Caching.Memory;
using TransportCompanyWeb.Data;
using TransportCompanyWeb.Models;

namespace TransportCompanyWeb.Service
{
    public class CachedDataService
    {
        private readonly TransportCompanyContext _context;
        private readonly IMemoryCache _cache;
        private const int RowCount = 20;

        public CachedDataService(TransportCompanyContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public IEnumerable<Car> GetCars()
        {
            if (!_cache.TryGetValue("Cars", out IEnumerable<Car> cars))
            {
                Console.WriteLine("123");
                cars = _context.Cars.Take(RowCount).ToList();
                _cache.Set("Cars", cars, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 17 + 240)
                });
            }
            return cars;
        }

        public IEnumerable<CarBrand> GetCarBrands()
        {
            if (!_cache.TryGetValue("CarBrands", out IEnumerable<CarBrand> carBrands))
            {
                carBrands = _context.CarBrands.Take(RowCount).ToList();
                _cache.Set("CarBrands", carBrands, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 17 + 240)
                });
            }
            return carBrands;
        }

        public IEnumerable<Cargo> GetCargos()
        {
            if (!_cache.TryGetValue("Cargos", out IEnumerable<Cargo> cargos))
            {
                cargos = _context.Cargos.Take(RowCount).ToList();
                _cache.Set("Cargos", cargos, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 17 + 240)
                });
            }
            return cargos;
        }


        public IEnumerable<CargoType> GetCargoTypes()
        {
            if (!_cache.TryGetValue("CargoTypes", out IEnumerable<CargoType> cargoTypes))
            {
                cargoTypes = _context.CargoTypes.Take(RowCount).ToList();
                _cache.Set("CargoTypes", cargoTypes, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 17 + 240)
                });
            }
            return cargoTypes;
        }

        public IEnumerable<CarType> GetCarTypes()
        {
            if (!_cache.TryGetValue("CarTypes", out IEnumerable<CarType> carTypes))
            {
                carTypes = _context.CarTypes.Take(RowCount).ToList();
                _cache.Set("CarTypes", carTypes, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 17 + 240)
                });
            }
            return carTypes;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            if (!_cache.TryGetValue("Employees", out IEnumerable<Employee> employees))
            {
                employees = _context.Employees.Take(RowCount).ToList();
                _cache.Set("Employees", employees, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 17 + 240)
                });
            }
            return employees;
        }

        public IEnumerable<Trip> GetTrips()
        {
            if (!_cache.TryGetValue("Trips", out IEnumerable<Trip> trips))
            {
                trips = _context.Trips.Take(RowCount).ToList();
                _cache.Set("Trips", trips, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 17 + 240)
                });
            }
            return trips;
        }
    }

}
