using System;
using System.ComponentModel.DataAnnotations;

namespace Titanic.Api.Dtos
{
    public record PassengerDto(Guid Id, int Survived, int Pclass, string FullName, string Sex, int Age, int Siblings_Spouses_Aboard, int Parents_Children_Aboard, float Fare);
    public record CreatePassengerDto([Required] int Survived, int Pclass, string FullName, string Sex, int Age, int Siblings_Spouses_Aboard, int Parents_Children_Aboard, float Fare);
    public record UpdatePassengerDto([Required] int Survived, int Pclass, string FullName, string Sex, int Age, int Siblings_Spouses_Aboard, int Parents_Children_Aboard, float Fare);
}