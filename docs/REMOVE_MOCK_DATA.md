# Reverting Mock Data to Real API Calls

## Current Issue

The frontend is currently using mock data that I added for demonstration purposes. To connect to the real database, you need to restore the original API calls.

## Step-by-Step Instructions

### 1. Open the API Service File

Open: `src/SportsCardStore.Web/src/services/apiService.ts`

### 2. Find the getSportsCards Method (around line 19)

Look for this section that starts with mock data:

```typescript
async getSportsCards(
  filters?: CardFilters,
): Promise<PagedSportsCardResponse> {
  // Mock data for demonstration - replace with actual API call once database is fixed
  const mockData = {
    items: [
      {
        id: 1,
        playerName: "Mike Trout",
        // ... lots of mock card data
      }
    ],
    totalCount: 3,
    page: 1,
    pageSize: 20,
  };

  // Add a small delay to simulate network request
  await new Promise((resolve) => setTimeout(resolve, 500));

  return mockData;

  /* Original API call - uncomment when database is working
  if (filters) {
    // ... filter code
  }
  const response = await fetch(url.toString());
  // ... rest of API call
  */
}
```

### 3. Replace the Entire Method

Replace the entire `getSportsCards` method with this corrected version:

```typescript
async getSportsCards(
  filters?: CardFilters,
): Promise<PagedSportsCardResponse> {
  const url = new URL(`${this.baseUrl}/api/sportscards`);

  if (filters) {
    if (filters.sport !== undefined) {
      url.searchParams.append("sport", filters.sport.toString());
    }
    if (filters.brand) {
      url.searchParams.append("brand", filters.brand);
    }
    if (filters.playerName) {
      url.searchParams.append("playerName", filters.playerName);
    }
    if (filters.team) {
      url.searchParams.append("team", filters.team);
    }
    if (filters.year !== undefined) {
      url.searchParams.append("year", filters.year.toString());
    }
    if (filters.minPrice !== undefined) {
      url.searchParams.append("minPrice", filters.minPrice.toString());
    }
    if (filters.maxPrice !== undefined) {
      url.searchParams.append("maxPrice", filters.maxPrice.toString());
    }
    if (filters.isAvailable !== undefined) {
      url.searchParams.append("isAvailable", filters.isAvailable.toString());
    }
    if (filters.isBowmanFirst !== undefined) {
      url.searchParams.append("isBowmanFirst", filters.isBowmanFirst.toString());
    }
    if (filters.parallelName) {
      url.searchParams.append("parallelName", filters.parallelName);
    }
    if (filters.maxPrintRun !== undefined) {
      url.searchParams.append("maxPrintRun", filters.maxPrintRun.toString());
    }
    if (filters.page !== undefined) {
      url.searchParams.append("page", filters.page.toString());
    }
    if (filters.pageSize !== undefined) {
      url.searchParams.append("pageSize", filters.pageSize.toString());
    }
  }

  const response = await fetch(url.toString());

  if (!response.ok) {
    throw new Error(`Failed to fetch sports cards: ${response.statusText}`);
  }

  return await response.json();
}
```

### 4. Save the File

Save the changes to `apiService.ts`

### 5. Test the Connection

1. Make sure your API server is running (`dotnet run` in the API project)
2. Make sure your React dev server is running (`npm run dev` in the Web project)
3. Refresh the website at `http://localhost:3000`
4. The site should now attempt to load real data from the database

## What This Fixes

- **Before**: Website shows 3 hardcoded demo cards (Mike Trout, etc.)
- **After**: Website loads actual cards from your database
- **Admin Panel**: Will work with real database operations (add/edit/delete cards)
- **Customer Site**: Will display your actual inventory

## Troubleshooting

If you get errors after making this change:

1. **"Failed to fetch sports cards: Internal Server Error"**
   - Your API server is running but can't connect to the database
   - Follow the DATABASE_SETUP.md guide to fix database connection

2. **"Failed to fetch sports cards: Network Error"**
   - API server is not running
   - Run `dotnet run` in the `src/SportsCardStore.API` folder

3. **Cards show but no data**
   - Database is connected but empty
   - The seeder should have added sample data automatically
   - Check API server logs for seeding errors

## Next Steps

After reverting to real API calls:

1. Follow DATABASE_SETUP.md to fix database connection
2. Restart both the API and frontend servers
3. Your admin panel and customer site will work with real data!
