using Titanic.Api.Api.Dtos;
using Titanic.Api.Models;

namespace Titanic.Api.Api
{
    public static class Extensions
    {
        public static PassengerDto AsDto(this Passenger passenger)
        {
            return new PassengerDto(passenger.Id, passenger.Survived, passenger.Pclass, passenger.FullName, passenger.Sex, passenger.Age, passenger.Siblings_Spouses_Aboard, passenger.Parents_Children_Aboard, passenger.Fare);
        }
    }
}