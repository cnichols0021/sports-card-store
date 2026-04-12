# Database Setup Guide

This guide will help you connect the Sports Card Store application to a real database instead of using mock data.

## Option 1: SQL Server LocalDB (Recommended for Development)

### Step 1: Install SQL Server LocalDB

1. Download and install SQL Server LocalDB from Microsoft
2. Verify installation by opening PowerShell and running:
   ```powershell
   sqllocaldb info
   ```

### Step 2: Start LocalDB Instance

```powershell
sqllocaldb start MSSQLLocalDB
```

### Step 3: Configure Connection String

Update `src/SportsCardStore.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SportsCardStoreDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### Step 4: Ensure SQL Server Provider

Make sure `Program.cs` uses SQL Server:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});
```

### Step 5: Create and Apply Migrations

```powershell
cd src/SportsCardStore.API
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Option 2: SQLite (Simpler Alternative)

### Step 1: Install SQLite Package

```powershell
cd src/SportsCardStore.API
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

### Step 2: Configure Connection String

Update `src/SportsCardStore.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=SportsCardStore.db"
  }
}
```

### Step 3: Update Program.cs for SQLite

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString);
});
```

### Step 4: Create Fresh Migrations for SQLite

```powershell
cd src/SportsCardStore.API
# Remove existing SQL Server migrations
rm -r Migrations/
dotnet ef migrations add SqliteInitial
dotnet ef database update
```

## Step 6: Remove Mock Data (Required for Both Options)

### Restore Real API Calls

Edit `src/SportsCardStore.Web/src/services/apiService.ts`:

1. Find the `getSportsCards` method
2. Remove the mock data section that returns hardcoded cards
3. Uncomment the original API call code that makes HTTP requests to the backend

The method should look like this:

```typescript
async getSportsCards(filters?: CardFilters): Promise<PagedSportsCardResponse> {
  const url = new URL(`${this.baseUrl}/api/sportscards`);

  if (filters) {
    // Add all filter parameters to URL
    if (filters.sport !== undefined) {
      url.searchParams.append("sport", filters.sport.toString());
    }
    // ... other filter parameters
  }

  const response = await fetch(url.toString());
  if (!response.ok) {
    throw new Error(`Failed to fetch sports cards: ${response.statusText}`);
  }
  return await response.json();
}
```

## Step 7: Start the Application

### Terminal 1 - API Server

```powershell
cd src/SportsCardStore.API
dotnet run
```

### Terminal 2 - Frontend Dev Server

```powershell
cd src/SportsCardStore.Web
npm run dev
```

## Step 8: Verify Database Connection

1. Check that the API starts without database errors
2. Visit `http://localhost:3000`
3. Verify that cards load from the database (seeded data should appear)
4. Check the admin panel at `http://localhost:3000/admin`

## Troubleshooting

### If LocalDB won't start:

```powershell
# Stop and restart LocalDB
sqllocaldb stop MSSQLLocalDB
sqllocaldb delete MSSQLLocalDB
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
```

### If migrations fail:

```powershell
# Reset database
dotnet ef database drop --force
dotnet ef database update
```

### If API can't connect:

1. Verify connection string matches your database type (SQL Server vs SQLite)
2. Check that the database service is running
3. Ensure firewall isn't blocking connections
4. Try restarting Visual Studio/VS Code

### If frontend shows errors:

1. Make sure API is running on `http://localhost:5281`
2. Check browser console for specific error messages
3. Verify CORS is configured correctly in API

## Expected Result

After completing these steps:

- Admin Panel will show real database statistics
- Card management will persist changes to the database
- Customer website will display actual inventory
- All CRUD operations (Create, Read, Update, Delete) will work with real data

The seeded data includes cards from Mike Trout, Ronald Acuna Jr., Derek Jeter, Tom Brady, and others to demonstrate the full functionality.

## Critical Frontend Type Fixes (Required for Form Submission)

After setting up the database, you may encounter "Bad Request" errors when creating cards through the admin panel. This section documents the type mismatches between frontend and backend that need to be resolved.

### Issue: Type Mismatches in CreateSportsCardRequest

The frontend and backend had different type definitions for the same models, causing validation failures.

#### Fix 1: Update Frontend Types

Edit `src/SportsCardStore.Web/src/types/index.ts`:

**Problem**: `gradingCompany` was defined as `string` but backend expects `GradingCompany` enum.

```typescript
// BEFORE (WRONG):
export interface CreateSportsCardRequest {
  // ... other fields
  gradingCompany?: string; // ❌ Wrong type
  // ... other fields
}

// AFTER (CORRECT):
export interface CreateSportsCardRequest {
  // ... other fields
  gradingCompany: GradingCompany; // ✅ Correct enum type
  // ... other fields
}
```

#### Fix 2: Update Form Component

Edit `src/SportsCardStore.Web/src/pages/admin/CardFormPage.tsx`:

**Import the GradingCompany enum:**

```typescript
import {
  CreateSportsCardRequest,
  Category,
  GradingCompany,
} from "../../types/index";
```

**Initialize with proper enum value:**

```typescript
const [formData, setFormData] = useState<CreateSportsCardRequest>({
  // ... other fields
  gradingCompany: GradingCompany.Raw, // ✅ Use enum instead of empty string
  // ... other fields
});
```

**Convert grading company input to dropdown:**

```tsx
{
  /* BEFORE (WRONG): Text input */
}
<input
  type="text"
  id="gradingCompany"
  name="gradingCompany"
  value={formData.gradingCompany}
  onChange={handleInputChange}
  className="mt-1 block w-full border-gray-300 rounded-md shadow-sm"
/>;

{
  /* AFTER (CORRECT): Select dropdown */
}
<select
  id="gradingCompany"
  name="gradingCompany"
  value={formData.gradingCompany}
  onChange={handleInputChange}
  className="mt-1 block w-full border-gray-300 rounded-md shadow-sm"
>
  <option value={GradingCompany.Raw}>Raw (Ungraded)</option>
  <option value={GradingCompany.PSA}>PSA</option>
  <option value={GradingCompany.BGS}>BGS (Beckett)</option>
  <option value={GradingCompany.SGC}>SGC</option>
</select>;
```

#### Fix 3: Update Input Handler for Enum Conversion

**Problem**: The `handleInputChange` function wasn't converting enum strings to integers.

```typescript
// BEFORE (WRONG):
const handleInputChange = (e) => {
  const { name, value, type } = e.target;
  setFormData((prev) => ({
    ...prev,
    [name]:
      type === "checkbox"
        ? (e.target as HTMLInputElement).checked
        : type === "number"
          ? value === ""
            ? undefined
            : parseFloat(value)
          : value, // ❌ Enums sent as strings
  }));
};

// AFTER (CORRECT):
const handleInputChange = (e) => {
  const { name, value, type } = e.target;
  setFormData((prev) => ({
    ...prev,
    [name]:
      type === "checkbox"
        ? (e.target as HTMLInputElement).checked
        : type === "number"
          ? value === ""
            ? undefined
            : parseFloat(value)
          : name === "sport" || name === "gradingCompany"
            ? parseInt(value) // ✅ Convert enum strings to integers
            : value,
  }));
};
```

#### Fix 4: Update Card Editing Logic

```typescript
// BEFORE (WRONG):
gradingCompany: card.gradingCompany || "",  // ❌ Empty string fallback

// AFTER (CORRECT):
gradingCompany: card.gradingCompany || GradingCompany.Raw,  // ✅ Enum fallback
```

### Category Enum Values

**Important**: The `Category` enum starts at 1, not 0:

- Baseball = 1
- Football = 2
- Basketball = 3
- Hockey = 4

### Testing the Fixes

After applying these fixes:

1. Restart the React dev server: `npm run dev`
2. Navigate to the admin panel: `http://localhost:3000/admin/cards/new`
3. Fill out the form completely (all required fields)
4. Submit the form - it should redirect to the admin cards list
5. Verify the new card appears in both admin and customer views

### Error Symptoms Before Fixes

- "Bad Request" errors when submitting card forms
- Network requests showing string values for enums instead of integers
- Form validation failures due to type mismatches
- Cards not being created in the database

### Validation Success Indicators

- Form redirects to admin cards list after submission
- New card appears in admin table immediately
- Card visible on customer storefront with correct formatting
- No console errors during form submission
