using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Titanic.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Titanic.Api.Repositories;
using System.Globalization;

namespace Titanic.Api.Api.Repositories
{
    public class MongoDbPassengersRepository : IPassengersRepository
    {
        private const string databaseName = "titanic";
        private const string collectionName = "passengers";
        private readonly IMongoCollection<Passenger> passengersCollection;
        private readonly FilterDefinitionBuilder<Passenger> filterBuilder = Builders<Passenger>.Filter;

        public MongoDbPassengersRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            passengersCollection = database.GetCollection<Passenger>(collectionName);
        }

        public async Task CreatePassengerAsync(Passenger passenger)
        {
            await passengersCollection.InsertOneAsync(passenger);
        }

        public async Task DeletePassengerAsync(Guid id)
        {
            var filter = filterBuilder.Eq(passenger => passenger.Id, id);
            await passengersCollection.DeleteOneAsync(filter);
        }

        public async Task<Passenger> GetPassengerAsync(Guid id)
        {
            var filter = filterBuilder.Eq(passenger => passenger.Id, id);
            return await passengersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Passenger>> GetPassengersAsync()
        {
            return await passengersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdatePassengerAsync(Passenger passenger)
        {
            var filter = filterBuilder.Eq(existingPassenger => existingPassenger.Id, passenger.Id);
            await passengersCollection.ReplaceOneAsync(filter, passenger);
        }
    }
}