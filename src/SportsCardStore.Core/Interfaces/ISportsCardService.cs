using SportsCardStore.Core.Entities;
using SportsCardStore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsCardStore.Core.Interfaces
{
    public interface ISportsCardService
    {
        Task<PagedResult<SportsCard>> GetAllAsync(
            Category? sport = null,
            string? brand = null,
            string? playerName = null,
            string? team = null,
            int? year = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isAvailable = null,
            bool? isBowmanFirst = null,
            int page = 1,
            int pageSize = 10);

        Task<SportsCard?> GetByIdAsync(int id);
        Task<SportsCard> CreateAsync(SportsCard sportsCard);
        Task<SportsCard?> UpdateAsync(int id, SportsCard sportsCard);
        Task<bool> DeleteAsync(int id);
    }

    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}