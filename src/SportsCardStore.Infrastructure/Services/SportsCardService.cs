using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportsCardStore.Core.Entities;
using SportsCardStore.Core.Enums;
using SportsCardStore.Core.Interfaces;
using SportsCardStore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCardStore.Infrastructure.Services
{
    public class SportsCardService : ISportsCardService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SportsCardService> _logger;

        public SportsCardService(AppDbContext context, ILogger<SportsCardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PagedResult<SportsCard>> GetAllAsync(
            Category? sport = null,
            string? brand = null,
            string? playerName = null,
            string? team = null,
            int? year = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isAvailable = null,
            bool? isBowmanFirst = null,
            string? parallelName = null,
            int? maxPrintRun = null,
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100;

                var query = _context.SportsCards.AsQueryable();

                if (sport.HasValue)
                    query = query.Where(c => c.Sport == sport.Value);

                if (!string.IsNullOrWhiteSpace(brand))
                    query = query.Where(c => c.Brand.ToLower().Contains(brand.ToLower()));

                if (!string.IsNullOrWhiteSpace(playerName))
                    query = query.Where(c => c.PlayerName.ToLower().Contains(playerName.ToLower()));

                if (!string.IsNullOrWhiteSpace(team))
                    query = query.Where(c => c.Team.ToLower().Contains(team.ToLower()));

                if (year.HasValue)
                    query = query.Where(c => c.Year == year.Value);

                if (minPrice.HasValue)
                    query = query.Where(c => c.Price >= minPrice.Value);

                if (maxPrice.HasValue)
                    query = query.Where(c => c.Price <= maxPrice.Value);

                if (isAvailable.HasValue)
                    query = query.Where(c => c.IsAvailable == isAvailable.Value);
                else
                    query = query.Where(c => c.IsAvailable);

                if (isBowmanFirst.HasValue)
                    query = query.Where(c => c.IsBowmanFirst == isBowmanFirst.Value);

                if (!string.IsNullOrWhiteSpace(parallelName))
                    query = query.Where(c => c.ParallelName != null && c.ParallelName.ToLower().Contains(parallelName.ToLower()));

                if (maxPrintRun.HasValue)
                    query = query.Where(c => c.PrintRun != null && c.PrintRun <= maxPrintRun.Value);

                var totalCount = await query.CountAsync();

                var items = await query
                    .OrderByDescending(c => c.CreatedDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                return new PagedResult<SportsCard>
                {
                    Items = items,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    HasNextPage = page < totalPages,
                    HasPreviousPage = page > 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting sports cards");
                throw;
            }
        }

        public async Task<SportsCard?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.SportsCards.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting sports card with ID {Id}", id);
                throw;
            }
        }

        public async Task<SportsCard> CreateAsync(SportsCard sportsCard)
        {
            try
            {
                _context.SportsCards.Add(sportsCard);
                await _context.SaveChangesAsync();
                return sportsCard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating sports card");
                throw;
            }
        }

        public async Task<SportsCard?> UpdateAsync(int id, SportsCard sportsCard)
        {
            try
            {
                var existingCard = await _context.SportsCards.FindAsync(id);
                if (existingCard == null)
                    return null;

                _context.Entry(existingCard).CurrentValues.SetValues(sportsCard);
                await _context.SaveChangesAsync();
                return existingCard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating sports card with ID {Id}", id);
                throw;
            }
        }
        public async Task<SportsCard?> UpdateImageUrlAsync(int id, string? imageUrl)
        {
            try
            {
                var card = await _context.SportsCards.FindAsync(id);
                if (card == null)
                    return null;

                card.ImageUrl = imageUrl;
                card.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated image URL for sports card {Id}", id);
                
                return card;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating image URL for sports card with ID {Id}", id);
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var card = await _context.SportsCards.FindAsync(id);
                if (card == null)
                    return false;

                _context.SportsCards.Remove(card);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting sports card with ID {Id}", id);
                throw;
            }
        }
    }
}
