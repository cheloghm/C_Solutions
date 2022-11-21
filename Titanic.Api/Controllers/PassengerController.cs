using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titanic.Api.Dtos;
using Titanic.Api.Models;
using Titanic.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Titanic.Api;

namespace Titanic.Api.Controllers
{
    [ApiController]
    [Route("passengers")]
    public class PassengersController : ControllerBase
    {
        private readonly IPassengersRepository _ipassengersRepository;
        private readonly ILogger<PassengersController> logger;

        public PassengersController(IPassengersRepository ipassengersRepository, ILogger<PassengersController> logger)
        {
            _ipassengersRepository = ipassengersRepository;
            this.logger = logger;
        }

        // GET /passengers
        [HttpGet("all")]
        public async Task<IEnumerable<Passenger>> GetPassengerAsync()
        {
            var passengers = await _ipassengersRepository.GetPassengersAsync();
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {passengers.Count()} passengers");

            return passengers;
        }
        

        // GET /passengers
        [HttpGet("search")]
        public async Task<IEnumerable<PassengerDto>> GetPassengersAsync(string fullname = null)
        {
            var passengers = (await _ipassengersRepository.GetPassengersAsync())
                        .Select(passenger => passenger.AsDto());

            if (!string.IsNullOrWhiteSpace(fullname))
            {
                passengers = passengers.Where(passenger => passenger.Name.Contains(fullname, StringComparison.OrdinalIgnoreCase));
            }

            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {passengers.Count()} passengers");

            return passengers;
        }

        // GET /passengers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PassengerDto>> GetPassengerAsync(string id)
        {
            var passenger = await _ipassengersRepository.GetPassengerAsync(id);

            if (passenger is null)
            {
                return NotFound();
            }

            return passenger.AsDto();
        }

        // POST /passengers
        [HttpPost]
        public async Task<ActionResult<PassengerDto>> CreatePassengerAsync(CreatePassengerDto passengerDto)
        {
            Passenger passenger = new()
            {
                // Id = Guid.NewGuid(),
                Survived = passengerDto.Survived,
                Pclass = passengerDto.Pclass,
                Name = passengerDto.Name,
                Sex = passengerDto.Sex,
                Age = passengerDto.Age,
                Siblings_Spouses_Aboard = passengerDto.Siblings_Spouses_Aboard,
                Parents_Children_Aboard = passengerDto.Parents_Children_Aboard,
                Fare = passengerDto.Fare
            };

            await _ipassengersRepository.CreatePassengerAsync(passenger);

            return CreatedAtAction(nameof(GetPassengerAsync), new { id = passenger.Id }, passenger.AsDto());
        }

        // PUT /passengers/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePassengerAsync(string id, UpdatePassengerDto passengerDto)
        {
            var existingPassenger = await _ipassengersRepository.GetPassengerAsync(id);

            if (existingPassenger is null)
            {
                return NotFound();
            }

            existingPassenger.Name = passengerDto.Name;
            existingPassenger.Sex = passengerDto.Sex;
            existingPassenger.Age = passengerDto.Age;
            existingPassenger.Survived = passengerDto.Survived;
            existingPassenger.Pclass = passengerDto.Pclass;
            existingPassenger.Siblings_Spouses_Aboard = passengerDto.Siblings_Spouses_Aboard;
            existingPassenger.Parents_Children_Aboard = passengerDto.Parents_Children_Aboard;
            existingPassenger.Fare = passengerDto.Fare;

            await _ipassengersRepository.UpdatePassengerAsync(existingPassenger);

            return NoContent();
        }

        // DELETE /passengers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePassengerAsync(string id)
        {
            var existingPassenger = await _ipassengersRepository.GetPassengerAsync(id);

            if (existingPassenger is null)
            {
                return NotFound();
            }

            await _ipassengersRepository.DeletePassengerAsync(id);

            return NoContent();
        }

    }
}