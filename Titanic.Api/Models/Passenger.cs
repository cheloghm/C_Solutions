using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Titanic.Api.Models
{
    public class Passenger
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public int Survived { get; set; }
        public int Pclass { get; set; }
        [Required, StringLength(500)]
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Age {get; set;}
        public int Siblings_Spouses_Aboard {get; set;}
        public int Parents_Children_Aboard {get; set;}
        public float Fare {get; set;}
    }
}