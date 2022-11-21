using System;
using System.ComponentModel.DataAnnotations;

namespace Titanic.Api.Dtos
{
    public record PassengerDto(string Id, int Survived, int Pclass, string Name, string Sex, int Age, int Siblings_Spouses_Aboard, int Parents_Children_Aboard, float Fare);
    public record CreatePassengerDto([Required] int Survived, int Pclass, string Name, string Sex, int Age, int Siblings_Spouses_Aboard, int Parents_Children_Aboard, float Fare);
    public record UpdatePassengerDto([Required] int Survived, int Pclass, string Name, string Sex, int Age, int Siblings_Spouses_Aboard, int Parents_Children_Aboard, float Fare);
}