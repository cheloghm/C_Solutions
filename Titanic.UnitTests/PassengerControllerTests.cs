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
                new Passenger(){ FullName = "Jack"},
                new Passenger(){ FullName = "Rose"},
                new Passenger(){ FullName = "Soung"}
            };

            var nameToMatch = "Jack";

            repositoryStub.Setup(repo => repo.GetPassengersAsync())
                .ReturnsAsync(allPassengers);

            var controller = new PassengersController(repositoryStub.Object, loggerStub.Object);

            // Act
            IEnumerable<PassengerDto> foundPassengers = await controller.GetPassengersAsync(nameToMatch);

            // Assert
            foundPassengers.Should().OnlyContain(
                passenger => passenger.FullName == allPassengers[0].FullName || passenger.FullName == allPassengers[2].FullName
            );
        }

        [Fact]
        public async Task CreatePassengerAsync_WithPassengerToCreate_ReturnsCreatedPassenger()
        {
            // Arrange
            var passengerToCreate = new CreatePassengerDto(
                1,
                3,
                Guid.NewGuid().ToString(),
                "female",
                35,
                1,
                0,
                20000);

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
                1,
                3,
                Guid.NewGuid().ToString(),
                "female",
                35,
                1,
                0,
                20000
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
                FullName = Guid.NewGuid().ToString(),
                Survived = 0,
                Pclass = 2,
                Sex = "male",
                Age = 20,
                Siblings_Spouses_Aboard = 1,
                Parents_Children_Aboard = 3,
                Fare = 50000
            };
        }
    }
}
