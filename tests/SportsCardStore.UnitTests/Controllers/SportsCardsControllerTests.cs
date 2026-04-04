using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SportsCardStore.API.Controllers;
using SportsCardStore.API.Models;
using SportsCardStore.Core.Entities;
using SportsCardStore.Core.Enums;
using SportsCardStore.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace SportsCardStore.UnitTests.Controllers;

/// <summary>
/// Unit tests for SportsCardsController covering all CRUD operations
/// and various response scenarios
/// </summary>
public class SportsCardsControllerTests
{
    private readonly Mock<ISportsCardService> _mockSportsCardService;
    private readonly Mock<ILogger<SportsCardsController>> _mockLogger;
    private readonly SportsCardsController _controller;

    public SportsCardsControllerTests()
    {
        _mockSportsCardService = new Mock<ISportsCardService>();
        _mockLogger = new Mock<ILogger<SportsCardsController>>();
        _controller = new SportsCardsController(_mockSportsCardService.Object, _mockLogger.Object);
    }

    /// <summary>
    /// Verifies that GetAllCards returns 200 OK with properly formatted 
    /// paged results when the service returns card data
    /// </summary>
    [Fact]
    public async Task GetAllCards_WhenServiceReturnsData_ShouldReturn200OkWithPagedResults()
    {
        // Arrange
        var sportsCards = new List<SportsCard>
        {
            new SportsCard
            {
                Id = 1,
                PlayerName = "Mike Trout",
                Year = 2023,
                Brand = "Topps",
                CardNumber = "1",
                Sport = Category.Baseball,
                Team = "Angels",
                Grade = 9.5m,
                GradingCompany = GradingCompany.PSA,
                Condition = "Mint",
                Price = 299.99m,
                Quantity = 1,
                IsAvailable = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            }
        };

        var pagedResult = new PagedResult<SportsCard>
        {
            Items = sportsCards,
            TotalCount = 1,
            Page = 1,
            PageSize = 10,
            TotalPages = 1,
            HasNextPage = false,
            HasPreviousPage = false
        };

        _mockSportsCardService
            .Setup(s => s.GetAllAsync(It.IsAny<Category?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(),
                It.IsAny<bool?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetAllCards();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        
        var response = okResult.Value as PagedSportsCardResponse;
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(1);
        response.TotalCount.Should().Be(1);
        response.Page.Should().Be(1);
        response.PageSize.Should().Be(10);
        
        var firstItem = response.Items.First();
        firstItem.PlayerName.Should().Be("Mike Trout");
        firstItem.Id.Should().Be(1);
    }

    /// <summary>
    /// Verifies that GetAllCards returns 200 OK with empty results 
    /// when no cards exist in the system
    /// </summary>
    [Fact]
    public async Task GetAllCards_WhenNoCardsExist_ShouldReturn200OkWithEmptyResults()
    {
        // Arrange
        var emptyPagedResult = new PagedResult<SportsCard>
        {
            Items = new List<SportsCard>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 10,
            TotalPages = 0,
            HasNextPage = false,
            HasPreviousPage = false
        };

        _mockSportsCardService
            .Setup(s => s.GetAllAsync(It.IsAny<Category?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(),
                It.IsAny<bool?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(emptyPagedResult);

        // Act
        var result = await _controller.GetAllCards();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        
        var response = okResult.Value as PagedSportsCardResponse;
        response.Should().NotBeNull();
        response!.Items.Should().BeEmpty();
        response.TotalCount.Should().Be(0);
        response.TotalPages.Should().Be(0);
    }

    /// <summary>
    /// Verifies that GetCard returns 200 OK with card data 
    /// when a card with the specified ID exists
    /// </summary>
    [Fact]
    public async Task GetCard_WhenCardExists_ShouldReturn200OkWithCardData()
    {
        // Arrange
        var cardId = 1;
        var sportsCard = new SportsCard
        {
            Id = cardId,
            PlayerName = "Aaron Judge",
            Year = 2022,
            Brand = "Panini",
            CardNumber = "99",
            Sport = Category.Baseball,
            Team = "Yankees",
            Grade = 10.0m,
            GradingCompany = GradingCompany.BGS,
            Condition = "Perfect",
            Price = 500.00m,
            Quantity = 1,
            IsAvailable = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _mockSportsCardService
            .Setup(s => s.GetByIdAsync(cardId))
            .ReturnsAsync(sportsCard);

        // Act
        var result = await _controller.GetCard(cardId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        
        var response = okResult.Value as SportsCardResponse;
        response.Should().NotBeNull();
        response!.Id.Should().Be(cardId);
        response.PlayerName.Should().Be("Aaron Judge");
        response.Price.Should().Be(500.00m);
    }

    /// <summary>
    /// Verifies that GetCard returns 404 NotFound when a card 
    /// with the specified ID does not exist
    /// </summary>
    [Fact]
    public async Task GetCard_WhenCardDoesNotExist_ShouldReturn404NotFound()
    {
        // Arrange
        var cardId = 99;
        _mockSportsCardService
            .Setup(s => s.GetByIdAsync(cardId))
            .ReturnsAsync((SportsCard?)null);

        // Act
        var result = await _controller.GetCard(cardId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFoundResult.Value.Should().Be($"Sports card with ID {cardId} not found");
    }

    /// <summary>
    /// Verifies that CreateCard returns 201 Created with location header 
    /// when the request model is valid and card is successfully created
    /// </summary>
    [Fact]
    public async Task CreateCard_WhenModelIsValid_ShouldReturn201CreatedWithLocationHeader()
    {
        // Arrange
        var createRequest = new CreateSportsCardRequest
        {
            PlayerName = "Ronald Acuña Jr.",
            Year = 2023,
            Brand = "Topps",
            CardNumber = "13",
            Sport = Category.Baseball,
            Team = "Braves",
            Grade = 9.0m,
            GradingCompany = GradingCompany.PSA,
            Condition = "Near Mint",
            Price = 150.00m,
            Quantity = 2,
            IsAvailable = true
        };

        var createdCard = new SportsCard
        {
            Id = 10,
            PlayerName = createRequest.PlayerName,
            Year = createRequest.Year,
            Brand = createRequest.Brand,
            CardNumber = createRequest.CardNumber,
            Sport = createRequest.Sport,
            Team = createRequest.Team,
            Grade = createRequest.Grade,
            GradingCompany = createRequest.GradingCompany,
            Condition = createRequest.Condition,
            Price = createRequest.Price,
            Quantity = createRequest.Quantity,
            IsAvailable = createRequest.IsAvailable,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _mockSportsCardService
            .Setup(s => s.CreateAsync(It.IsAny<SportsCard>()))
            .ReturnsAsync(createdCard);

        // Act
        var result = await _controller.CreateCard(createRequest);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult.ActionName.Should().Be(nameof(_controller.GetCard));
        createdResult.RouteValues!["id"].Should().Be(10);
        
        var response = createdResult.Value as SportsCardResponse;
        response.Should().NotBeNull();
        response!.Id.Should().Be(10);
        response.PlayerName.Should().Be("Ronald Acuña Jr.");
    }

    /// <summary>
    /// Verifies that CreateCard returns 400 BadRequest when the 
    /// model state is invalid (validation errors exist)
    /// </summary>
    [Fact]
    public async Task CreateCard_WhenModelStateIsInvalid_ShouldReturn400BadRequest()
    {
        // Arrange
        var createRequest = new CreateSportsCardRequest
        {
            PlayerName = "", // Invalid: Required field is empty
            Year = 2023,
            Brand = "Topps"
            // Missing other required fields
        };

        // Simulate invalid model state
        _controller.ModelState.AddModelError("PlayerName", "The PlayerName field is required.");

        // Act
        var result = await _controller.CreateCard(createRequest);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().BeOfType<SerializableError>();
    }

    /// <summary>
    /// Verifies that UpdateCard returns 200 OK with updated card data 
    /// when the card exists and the model is valid
    /// </summary>
    [Fact]
    public async Task UpdateCard_WhenCardExistsAndModelIsValid_ShouldReturn200Ok()
    {
        // Arrange
        var cardId = 5;
        var updateRequest = new UpdateSportsCardRequest
        {
            PlayerName = "Shohei Ohtani",
            Year = 2023,
            Brand = "Topps",
            CardNumber = "17",
            Sport = Category.Baseball,
            Team = "Angels",
            Grade = 10.0m,
            GradingCompany = GradingCompany.PSA,
            Condition = "Gem Mint",
            Price = 750.00m,
            Quantity = 1,
            IsAvailable = true
        };

        var existingCard = new SportsCard
        {
            Id = cardId,
            PlayerName = "Shohei Ohtani",
            Year = 2022, // Different from update
            Brand = "Panini", // Different from update
            CardNumber = "17",
            Sport = Category.Baseball,
            Team = "Angels",
            CreatedDate = DateTime.UtcNow.AddDays(-30),
            UpdatedDate = DateTime.UtcNow.AddDays(-30)
        };

        var updatedCard = new SportsCard
        {
            Id = cardId,
            PlayerName = updateRequest.PlayerName,
            Year = updateRequest.Year,
            Brand = updateRequest.Brand,
            CardNumber = updateRequest.CardNumber,
            Sport = updateRequest.Sport,
            Team = updateRequest.Team,
            Grade = updateRequest.Grade,
            GradingCompany = updateRequest.GradingCompany,
            Condition = updateRequest.Condition,
            Price = updateRequest.Price,
            Quantity = updateRequest.Quantity,
            IsAvailable = updateRequest.IsAvailable,
            CreatedDate = existingCard.CreatedDate,
            UpdatedDate = DateTime.UtcNow
        };

        _mockSportsCardService.Setup(s => s.GetByIdAsync(cardId)).ReturnsAsync(existingCard);
        _mockSportsCardService.Setup(s => s.UpdateAsync(cardId, It.IsAny<SportsCard>())).ReturnsAsync(updatedCard);

        // Act
        var result = await _controller.UpdateCard(cardId, updateRequest);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        
        var response = okResult.Value as SportsCardResponse;
        response.Should().NotBeNull();
        response!.Id.Should().Be(cardId);
        response.Year.Should().Be(2023); // Updated value
        response.Brand.Should().Be("Topps"); // Updated value
        response.Price.Should().Be(750.00m);
    }

    /// <summary>
    /// Verifies that UpdateCard returns 404 NotFound when the 
    /// card with the specified ID does not exist
    /// </summary>
    [Fact]
    public async Task UpdateCard_WhenCardDoesNotExist_ShouldReturn404NotFound()
    {
        // Arrange
        var cardId = 99;
        var updateRequest = new UpdateSportsCardRequest
        {
            PlayerName = "Non Existent Player",
            Year = 2023,
            Brand = "Topps",
            CardNumber = "1",
            Sport = Category.Baseball,
            Team = "Team",
            GradingCompany = GradingCompany.Raw,
            Price = 10.00m,
            Quantity = 1,
            IsAvailable = true
        };

        _mockSportsCardService.Setup(s => s.GetByIdAsync(cardId)).ReturnsAsync((SportsCard?)null);

        // Act
        var result = await _controller.UpdateCard(cardId, updateRequest);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFoundResult.Value.Should().Be($"Sports card with ID {cardId} not found");
    }

    /// <summary>
    /// Verifies that DeleteCard returns 204 NoContent when the 
    /// card exists and is successfully deleted
    /// </summary>
    [Fact]
    public async Task DeleteCard_WhenCardExists_ShouldReturn204NoContent()
    {
        // Arrange
        var cardId = 7;
        var existingCard = new SportsCard
        {
            Id = cardId,
            PlayerName = "Mookie Betts",
            Year = 2022,
            Brand = "Topps",
            CardNumber = "50",
            Sport = Category.Baseball,
            Team = "Dodgers",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _mockSportsCardService.Setup(s => s.GetByIdAsync(cardId)).ReturnsAsync(existingCard);
        _mockSportsCardService.Setup(s => s.DeleteAsync(cardId)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteCard(cardId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        var noContentResult = result as NoContentResult;
        noContentResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        
        // Verify service methods were called
        _mockSportsCardService.Verify(s => s.GetByIdAsync(cardId), Times.Once);
        _mockSportsCardService.Verify(s => s.DeleteAsync(cardId), Times.Once);
    }

    /// <summary>
    /// Verifies that DeleteCard returns 404 NotFound when the 
    /// card with the specified ID does not exist
    /// </summary>
    [Fact]
    public async Task DeleteCard_WhenCardDoesNotExist_ShouldReturn404NotFound()
    {
        // Arrange
        var cardId = 99;
        _mockSportsCardService.Setup(s => s.GetByIdAsync(cardId)).ReturnsAsync((SportsCard?)null);

        // Act
        var result = await _controller.DeleteCard(cardId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFoundResult.Value.Should().Be($"Sports card with ID {cardId} not found");
        
        // Verify delete was not called since card doesn't exist
        _mockSportsCardService.Verify(s => s.GetByIdAsync(cardId), Times.Once);
        _mockSportsCardService.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    /// <summary>
    /// Verifies that controller handles service exceptions by 
    /// returning 500 Internal Server Error for GetAllCards
    /// </summary>
    [Fact]
    public async Task GetAllCards_WhenServiceThrowsException_ShouldReturn500InternalServerError()
    {
        // Arrange
        _mockSportsCardService
            .Setup(s => s.GetAllAsync(It.IsAny<Category?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(),
                It.IsAny<bool?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await _controller.GetAllCards();

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        objectResult.Value.Should().Be("An error occurred while retrieving sports cards");
    }

    /// <summary>
    /// Verifies that controller properly validates UpdateCard model state
    /// and returns 400 BadRequest when validation fails
    /// </summary>
    [Fact]
    public async Task UpdateCard_WhenModelStateIsInvalid_ShouldReturn400BadRequest()
    {
        // Arrange
        var cardId = 1;
        var updateRequest = new UpdateSportsCardRequest
        {
            PlayerName = "", // Invalid: Required field is empty
            Year = 1500, // Invalid: Below minimum range
            Brand = new string('x', 100), // Invalid: Exceeds max length
            CardNumber = "1",
            Sport = Category.Baseball,
            Team = "Team",
            GradingCompany = GradingCompany.Raw,
            Price = 10.00m,
            Quantity = 1,
            IsAvailable = true
        };

        // Simulate invalid model state
        _controller.ModelState.AddModelError("PlayerName", "The PlayerName field is required.");
        _controller.ModelState.AddModelError("Year", "Year must be between 1800 and 2100.");

        // Act
        var result = await _controller.UpdateCard(cardId, updateRequest);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().BeOfType<SerializableError>();
    }
}