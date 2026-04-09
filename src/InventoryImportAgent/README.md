# InventoryImportAgent

A console application that imports sports card data from an Excel file into the SportsCardStore API.

## Features

- Reads Excel files using ClosedXML library
- Maps Excel columns to SportsCard entity fields according to the schema in `docs/INVENTORY_IMPORT_SCHEMA.md`
- Validates data including:
  - Required fields (PlayerName, Team, Brand, SetName, CardNumber, Year, Sport, Price, Quantity)
  - Year range validation (1800-2100)
  - Price validation (> 0)
  - Quantity validation (≥ 1)
  - Raw cards cannot have grades
  - Boolean field parsing (true/false, yes/no, 1/0, y/n)
- Imports cards via POST requests to the SportsCardStore API
- Structured logging with import statistics
- Continues processing if individual rows fail
- Accepts command line arguments for flexibility

## Usage

```bash
InventoryImportAgent <API_BASE_URL> <EXCEL_FILE_PATH>
```

### Parameters

- `API_BASE_URL`: Base URL of the SportsCardStore API (e.g., `https://localhost:7000`)
- `EXCEL_FILE_PATH`: Path to the Excel file containing card data (e.g., `Sports_Cards_in_Order.xlsx`)

### Example

```bash
dotnet run --project src/InventoryImportAgent -- https://localhost:7000 Sports_Cards_in_Order.xlsx
```

## Excel File Format

The Excel file should have the following columns (case-insensitive):

| Column Name     | Type    | Required | Notes                                 |
| --------------- | ------- | -------- | ------------------------------------- |
| Player Name     | string  | Yes      | Max 100 characters                    |
| Team            | string  | Yes      | Max 50 characters                     |
| Brand           | string  | Yes      | Max 50 characters                     |
| Set Name        | string  | Yes      | Max 100 characters                    |
| Card Number     | string  | Yes      | Max 20 characters                     |
| Year            | integer | Yes      | Range: 1800-2100                      |
| Sport           | string  | Yes      | Baseball/Football/Basketball/Hockey   |
| Rookie          | boolean | No       | True/false, yes/no, 1/0, y/n          |
| Autograph       | boolean | No       | True/false, yes/no, 1/0, y/n          |
| Relic           | boolean | No       | True/false, yes/no, 1/0, y/n          |
| Grading Company | string  | Yes      | Raw/PSA/BGS/SGC                       |
| Card Grade      | decimal | No       | 0.0-10.0, leave blank for Raw cards   |
| Condition       | string  | No       | Max 200 characters                    |
| Price           | decimal | Yes      | Must be > 0                           |
| Quantity        | integer | Yes      | Must be ≥ 1                           |
| Description     | string  | No       | Max 1000 characters                   |
| Image Url       | string  | No       | Max 500 characters, must be valid URL |
| Is Available    | boolean | No       | Defaults to true if blank             |

## Validation Rules

1. **Required Fields**: PlayerName, Team, Brand, SetName, CardNumber cannot be empty
2. **Year Range**: Must be between 1800 and 2100
3. **Price**: Must be greater than 0
4. **Quantity**: Must be at least 1
5. **Grade**: Must be 0.0-10.0 if specified
6. **Raw Cards**: Cannot have a grade value
7. **Sport**: Must be one of: Baseball, Football, Basketball, Hockey
8. **Grading Company**: Must be one of: Raw, PSA, BGS, SGC

## Logging

The application uses structured logging with the following levels:

- **Information**: General progress and summary statistics
- **Warning**: Skipped rows with validation issues
- **Error**: Failed API calls or processing errors
- **Debug**: Detailed information about each successful import

## Error Handling

- Invalid rows are skipped and logged as warnings
- API call failures are logged as errors but don't stop processing
- The application continues processing remaining rows if individual rows fail
- Final summary shows counts of imported, skipped, and failed records

## Output

Upon completion, the application outputs:

- Total number of cards imported successfully
- Total number of rows skipped due to validation issues
- Total number of rows that failed during API import
- Detailed logs for troubleshooting

## Dependencies

- .NET 10.0
- ClosedXML (Excel reading)
- Microsoft.Extensions.Logging (structured logging)
- Microsoft.Extensions.DependencyInjection (dependency injection)
- SportsCardStore.Core (entity definitions)
- SportsCardStore.Shared (DTO models)

## Related Documentation

- [Inventory Import Schema](../docs/INVENTORY_IMPORT_SCHEMA.md) - Detailed column mapping specification
- [Project Plan](../PROJECT_PLAN.MD) - Overall project roadmap
