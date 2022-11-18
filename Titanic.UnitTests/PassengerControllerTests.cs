using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Titanic.Api.Controllers;
using Titanic.Api.Api.Dtos;
using Titanic.Api.Models;
using Titanic.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Titanic.UnitTests
{
    public class PassengersControllerTests
    {
        private readonly Mock<IPassengersRepository> repositoryStub = new();
        private readonly Mock<ILogger<PassengersController>> loggerStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetPassengerAsync_WithUnexistingPassenger_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.GetPassengerAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Passenger)null);

            var controller = new PassengersController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetPassengerAsync(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetPassengerAsync_WithExistingPassenger_ReturnsExpectedPassenger()
        {
            // Arrange
            Passenger expectedPassenger = CreateRandomPassenger();

            repositoryStub.Setup(repo => repo.GetPassengerAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedPassenger);

            var controller = new PassengersController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetPassengerAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedPassenger);
        }

        [Fact]
        public async Task GetPassengersAsync_WithExistingPassengers_ReturnsAllPassengers()
        {
            // Arrange
            var expectedPassengers = new[] { CreateRandomPassenger(), CreateRandomPassenger(), CreateRandomPassenger() };

            repositoryStub.Setup(repo => repo.GetPassengersAsync())
                .ReturnsAsync(expectedPassengers);

            var controller = new PassengersController(repositoryStub.Object, loggerStub.Object);

            // Act
            var actualPassengers = await controller.GetPassengersAsync();

            // Assert
            actualPassengers.Should().BeEquivalentTo(expectedPassengers);
        }

        [Fact]
        public async Task GetPassengersAsync_WithMatchingPassengers_ReturnsMatchingPassengers()
        {
            // Arrange
            var allPassengers = new[]
            {
                new Passenger(){ Name = "Potion"},
                new Passenger(){ Name = "Antidote"},
                new Passenger(){ Name = "Hi-Potion"}
            };

            var nameToMatch = "Potion";

            repositoryStub.Setup(repo => repo.GetPassengersAsync())
                .ReturnsAsync(allPassengers);

            var controller = new PassengersController(repositoryStub.Object, loggerStub.Object);

            // Act
            IEnumerable<PassengerDto> foundPassengers = await controller.GetPassengersAsync(nameToMatch);

            // Assert
            foundPassengers.Should().OnlyContain(
                passenger => passenger.Name == allPassengers[0].Name || passenger.Name == allPassengers[2].Name
            );
        }

        [Fact]
        public async Task CreatePassengerAsync_WithPassengerToCreate_ReturnsCreatedPassenger()
        {
            // Arrange
            var passengerToCreate = new CreatePassengerDto(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                rand.Next(1000));

            var controller = new PassengersController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.CreatePassengerAsync(passengerToCreate);

            // Assert
            var createdPassenger = (result.Result as CreatedAtActionResult).Value as PassengerDto;
            passengerToCreate.Should().BeEquivalentTo(
                createdPassenger,
                options => options.ComparingByMembers<PassengerDto>().ExcludingMissingMembers()
            );
            createdPassenger.Id.Should().NotBeEmpty();
            createdPassenger.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }

        [Fact]
        public async Task UpdatePassengerAsync_WithExistingPassenger_ReturnsNoContent()
        {
            // Arrange
            Passenger existingPassenger = CreateRandomPassenger();
            repositoryStub.Setup(repo => repo.GetPassengerAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingPassenger);

            var passengerId = existingPassenger.Id;
            var passengerToUpdate = new UpdatePassengerDto(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                existingPassenger.Price + 3
            );

            var controller = new PassengersController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.UpdatePassengerAsync(passengerId, passengerToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeletePassengerAsync_WithExistingPassenger_ReturnsNoContent()
        {
            // Arrange
            Passenger existingPassenger = CreateRandomPassenger();
            repositoryStub.Setup(repo => repo.GetPassengerAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingPassenger);

            var controller = new PassengersController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.DeletePassengerAsync(existingPassenger.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        private Passenger CreateRandomPassenger()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
