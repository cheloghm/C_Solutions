using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Titanic.Api.Models;

namespace Titanic.Api.Repositories
{
    public interface IPassengersRepository
    {
        Task<Passenger> GetPassengerAsync(string id);
        Task<IEnumerable<Passenger>> GetPassengersAsync();
        Task CreatePassengerAsync(Passenger passenger);
        Task UpdatePassengerAsync(Passenger passenger);
        Task DeletePassengerAsync(string id);
        //object ReadCSV<T>(Stream stream);
        //public IEnumerable<T> ReadCSV<T>(Stream file);
    }
}