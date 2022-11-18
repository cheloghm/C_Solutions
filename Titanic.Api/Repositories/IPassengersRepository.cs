using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Titanic.Api.Models;

namespace Titanic.Api.Repositories
{
    public interface IPassengersRepository
    {
        Task<Passenger> GetPassengerAsync(Guid id);
        Task<IEnumerable<Passenger>> GetPassengersAsync();
        Task CreatePassengerAsync(Passenger passenger);
        Task UpdatePassengerAsync(Passenger passenger);
        Task DeletePassengerAsync(Guid id);
        //object ReadCSV<T>(Stream stream);
        //public IEnumerable<T> ReadCSV<T>(Stream file);
    }
}