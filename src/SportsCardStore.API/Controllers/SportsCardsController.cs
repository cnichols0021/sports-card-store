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
            [FromQuery] string? parallelName = null,
            [FromQuery] int? maxPrintRun = null,
            [FromQuery][Range(1, int.MaxValue)] int page = 1,
            [FromQuery][Range(1, 100)] int pageSize = 10)
        {
            try
            {
                var result = await _sportsCardService.GetAllAsync(
                    sport, brand, playerName, team, year, minPrice, maxPrice,
                    isAvailable, isBowmanFirst, parallelName, maxPrintRun, page, pageSize);

                var response = new PagedSportsCardResponse
                {
                    Items = result.Items.Select(card => MapToResponse(card)),
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
                    return NotFound($"Sports card with ID {id} not found");

                return Ok(MapToResponse(card));
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
                    return BadRequest(ModelState);

                var sportsCard = new SportsCard
                {
                    PlayerName = request.PlayerName,
                    Year = request.Year,
                    Brand = request.Brand,
                    SetName = request.SetName,
                    CardNumber = request.CardNumber,
                    Sport = request.Sport,
                    Team = request.Team,
                    IsRookie = request.IsRookie,
                    IsAutograph = request.IsAutograph,
                    IsRelic = request.IsRelic,
                    IsBowmanFirst = request.IsBowmanFirst,
                    ParallelName = request.ParallelName,
                    PrintRun = request.PrintRun,
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
                return CreatedAtAction(nameof(GetCard), new { id = createdCard.Id }, MapToResponse(createdCard));
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
                    return BadRequest(ModelState);

                var existingCard = await _sportsCardService.GetByIdAsync(id);
                if (existingCard == null)
                    return NotFound($"Sports card with ID {id} not found");

                existingCard.PlayerName = request.PlayerName;
                existingCard.Year = request.Year;
                existingCard.Brand = request.Brand;
                existingCard.SetName = request.SetName;
                existingCard.CardNumber = request.CardNumber;
                existingCard.Sport = request.Sport;
                existingCard.Team = request.Team;
                existingCard.IsRookie = request.IsRookie;
                existingCard.IsAutograph = request.IsAutograph;
                existingCard.IsRelic = request.IsRelic;
                existingCard.IsBowmanFirst = request.IsBowmanFirst;
                existingCard.ParallelName = request.ParallelName;
                existingCard.PrintRun = request.PrintRun;
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
                    return StatusCode(500, "An error occurred while updating the sports card");

                return Ok(MapToResponse(updatedCard));
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
                    return NotFound($"Sports card with ID {id} not found");

                await _sportsCardService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sports card {Id}", id);
                return StatusCode(500, "An error occurred while deleting the sports card");
            }
        }

        /// <summary>
        /// Maps a SportsCard entity to a SportsCardResponse DTO.
        /// Single mapping method ensures all fields are consistently returned
        /// across all endpoints — GET, POST, and PUT.
        /// </summary>
        private static SportsCardResponse MapToResponse(SportsCard card) => new()
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
            ParallelName = card.ParallelName,
            PrintRun = card.PrintRun,
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
    }
}
