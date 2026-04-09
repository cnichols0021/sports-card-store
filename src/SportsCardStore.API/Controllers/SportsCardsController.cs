using Microsoft.AspNetCore.Mvc;
using SportsCardStore.Shared.Models;
using SportsCardStore.Core.Entities;
using SportsCardStore.Core.Enums;
using SportsCardStore.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SportsCardStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SportsCardsController : ControllerBase
    {
        private readonly ISportsCardService _sportsCardService;
        private readonly ILogger<SportsCardsController> _logger;

        public SportsCardsController(ISportsCardService sportsCardService, ILogger<SportsCardsController> logger)
        {
            _sportsCardService = sportsCardService ?? throw new ArgumentNullException(nameof(sportsCardService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all sports cards with optional filtering and pagination
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedSportsCardResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedSportsCardResponse>> GetAllCards(
            [FromQuery] Category? sport = null,
            [FromQuery] string? brand = null,
            [FromQuery] string? playerName = null,
            [FromQuery] string? team = null,
            [FromQuery] int? year = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] bool? isAvailable = null,
            [FromQuery] bool? isBowmanFirst = null,
            [FromQuery][Range(1, int.MaxValue)] int page = 1,
            [FromQuery][Range(1, 100)] int pageSize = 10)
        {
            try
            {
                var result = await _sportsCardService.GetAllAsync(
                    sport, brand, playerName, team, year, minPrice, maxPrice, isAvailable, isBowmanFirst, page, pageSize);

                var response = new PagedSportsCardResponse
                {
                    Items = result.Items.Select(card => new SportsCardResponse
                    {
                        Id = card.Id,
                        PlayerName = card.PlayerName,
                        Year = card.Year,
                        Brand = card.Brand,
                        SetName = card.SetName,
                        CardNumber = card.CardNumber,
                        Sport = card.Sport,
                        Team = card.Team,
                        IsRookie = card.IsRookie,
                        IsAutograph = card.IsAutograph,
                        IsRelic = card.IsRelic,
                        IsBowmanFirst = card.IsBowmanFirst,
                        Grade = card.Grade,
                        GradingCompany = card.GradingCompany,
                        Condition = card.Condition,
                        Price = card.Price,
                        Quantity = card.Quantity,
                        ImageUrl = card.ImageUrl,
                        Description = card.Description,
                        IsAvailable = card.IsAvailable,
                        CreatedDate = card.CreatedDate,
                        UpdatedDate = card.UpdatedDate
                    }),
                    TotalCount = result.TotalCount,
                    Page = result.Page,
                    PageSize = result.PageSize,
                    TotalPages = result.TotalPages,
                    HasNextPage = result.HasNextPage,
                    HasPreviousPage = result.HasPreviousPage
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting sports cards");
                return StatusCode(500, "An error occurred while retrieving sports cards");
            }
        }

        /// <summary>
        /// Get a specific sports card by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SportsCardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SportsCardResponse>> GetCard(int id)
        {
            try
            {
                var card = await _sportsCardService.GetByIdAsync(id);
                if (card == null)
                {
                    return NotFound($"Sports card with ID {id} not found");
                }

                var response = new SportsCardResponse
                {
                    Id = card.Id,
                    PlayerName = card.PlayerName,
                    Year = card.Year,
                    Brand = card.Brand,
                    SetName = card.SetName,
                    CardNumber = card.CardNumber,
                    Sport = card.Sport,
                    Team = card.Team,
                    IsRookie = card.IsRookie,
                    IsAutograph = card.IsAutograph,
                    IsRelic = card.IsRelic,
                    IsBowmanFirst = card.IsBowmanFirst,
                    Grade = card.Grade,
                    GradingCompany = card.GradingCompany,
                    Condition = card.Condition,
                    Price = card.Price,
                    Quantity = card.Quantity,
                    ImageUrl = card.ImageUrl,
                    Description = card.Description,
                    IsAvailable = card.IsAvailable,
                    CreatedDate = card.CreatedDate,
                    UpdatedDate = card.UpdatedDate
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting sports card {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the sports card");
            }
        }

        /// <summary>
        /// Create a new sports card
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SportsCardResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SportsCardResponse>> CreateCard([FromBody] CreateSportsCardRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var sportsCard = new SportsCard
                {
                    PlayerName = request.PlayerName,
                    Year = request.Year,
                    Brand = request.Brand,
                    CardNumber = request.CardNumber,
                    Sport = request.Sport,
                    Team = request.Team,
                    Grade = request.Grade,
                    GradingCompany = request.GradingCompany,
                    Condition = request.Condition,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    ImageUrl = request.ImageUrl,
                    Description = request.Description,
                    IsAvailable = request.IsAvailable,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                var createdCard = await _sportsCardService.CreateAsync(sportsCard);

                var response = new SportsCardResponse
                {
                    Id = createdCard.Id,
                    PlayerName = createdCard.PlayerName,
                    Year = createdCard.Year,
                    Brand = createdCard.Brand,
                    CardNumber = createdCard.CardNumber,
                    Sport = createdCard.Sport,
                    Team = createdCard.Team,
                    Grade = createdCard.Grade,
                    GradingCompany = createdCard.GradingCompany,
                    Condition = createdCard.Condition,
                    Price = createdCard.Price,
                    Quantity = createdCard.Quantity,
                    ImageUrl = createdCard.ImageUrl,
                    Description = createdCard.Description,
                    IsAvailable = createdCard.IsAvailable,
                    CreatedDate = createdCard.CreatedDate,
                    UpdatedDate = createdCard.UpdatedDate
                };

                return CreatedAtAction(nameof(GetCard), new { id = createdCard.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sports card for {PlayerName}", request.PlayerName);
                return StatusCode(500, "An error occurred while creating the sports card");
            }
        }

        /// <summary>
        /// Update an existing sports card
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SportsCardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SportsCardResponse>> UpdateCard(int id, [FromBody] UpdateSportsCardRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingCard = await _sportsCardService.GetByIdAsync(id);
                if (existingCard == null)
                {
                    return NotFound($"Sports card with ID {id} not found");
                }

                existingCard.PlayerName = request.PlayerName;
                existingCard.Year = request.Year;
                existingCard.Brand = request.Brand;
                existingCard.CardNumber = request.CardNumber;
                existingCard.Sport = request.Sport;
                existingCard.Team = request.Team;
                existingCard.Grade = request.Grade;
                existingCard.GradingCompany = request.GradingCompany;
                existingCard.Condition = request.Condition;
                existingCard.Price = request.Price;
                existingCard.Quantity = request.Quantity;
                existingCard.ImageUrl = request.ImageUrl;
                existingCard.Description = request.Description;
                existingCard.IsAvailable = request.IsAvailable;
                existingCard.UpdatedDate = DateTime.UtcNow;

                var updatedCard = await _sportsCardService.UpdateAsync(id, existingCard);
                if (updatedCard == null)
                {
                    return StatusCode(500, "An error occurred while updating the sports card");
                }

                var response = new SportsCardResponse
                {
                    Id = updatedCard.Id,
                    PlayerName = updatedCard.PlayerName,
                    Year = updatedCard.Year,
                    Brand = updatedCard.Brand,
                    CardNumber = updatedCard.CardNumber,
                    Sport = updatedCard.Sport,
                    Team = updatedCard.Team,
                    Grade = updatedCard.Grade,
                    GradingCompany = updatedCard.GradingCompany,
                    Condition = updatedCard.Condition,
                    Price = updatedCard.Price,
                    Quantity = updatedCard.Quantity,
                    ImageUrl = updatedCard.ImageUrl,
                    Description = updatedCard.Description,
                    IsAvailable = updatedCard.IsAvailable,
                    CreatedDate = updatedCard.CreatedDate,
                    UpdatedDate = updatedCard.UpdatedDate
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sports card {Id}", id);
                return StatusCode(500, "An error occurred while updating the sports card");
            }
        }

        /// <summary>
        /// Delete a sports card
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCard(int id)
        {
            try
            {
                var existingCard = await _sportsCardService.GetByIdAsync(id);
                if (existingCard == null)
                {
                    return NotFound($"Sports card with ID {id} not found");
                }

                await _sportsCardService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sports card {Id}", id);
                return StatusCode(500, "An error occurred while deleting the sports card");
            }
        }
    }
}