using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titanic.Models;

namespace Titanic.Repositories
{
    public class InMemPassengersRepository : IPassengersRepository
    {
        private readonly List<Passenger> passengers = new()
        {
            new Passenger { Id = Guid.NewGuid(), FullName = "Mr Gray Chuks", Survived = 1, Pclass = 1, Sex = "male", Age = 29, Siblings_Spouses_Aboard = 0, Parents_Children_Aboard = 0, Fare = 100000.00f },
            new Passenger { Id = Guid.NewGuid(), FullName = "Mr Gray Chuks", Survived = 1, Pclass = 1, Sex = "male", Age = 29, Siblings_Spouses_Aboard = 0, Parents_Children_Aboard = 0, Fare = 100000.00f },
            new Passenger { Id = Guid.NewGuid(), FullName = "Mr Gray Chuks", Survived = 1, Pclass = 1, Sex = "male", Age = 29, Siblings_Spouses_Aboard = 0, Parents_Children_Aboard = 0, Fare = 100000.00f },
            new Passenger { Id = Guid.NewGuid(), FullName = "Mr Gray Chuks", Survived = 1, Pclass = 1, Sex = "male", Age = 29, Siblings_Spouses_Aboard = 0, Parents_Children_Aboard = 0, Fare = 100000.00f },
            new Passenger { Id = Guid.NewGuid(), FullName = "Mr Gray Chuks", Survived = 1, Pclass = 1, Sex = "male", Age = 29, Siblings_Spouses_Aboard = 0, Parents_Children_Aboard = 0, Fare = 100000.00f },
        };

        public async Task<IEnumerable<Passenger>> GetPassengersAsync()
        {
            return await Task.FromResult(passengers);
        }

        public async Task<Passenger> GetPassengerAsync(Guid id)
        {
            var passenger = passengers.Where(passenger => passenger.Id == id).SingleOrDefault();
            return await Task.FromResult(passenger);
        }

        public async Task CreatePassengerAsync(Passenger passenger)
        {
            passengers.Add(passenger);
            await Task.CompletedTask;
        }

        public async Task UpdatePassengerAsync(Passenger passenger)
        {
            var index = passengers.FindIndex(existingPassenger => existingPassenger.Id == passenger.Id);
            passengers[index] = passenger;
            await Task.CompletedTask;
        }

        public async Task DeletePassengerAsync(Guid id)
        {
            var index = passengers.FindIndex(existingPassenger => existingPassenger.Id == id);
            passengers.RemoveAt(index);
            await Task.CompletedTask;
        }

    }
}