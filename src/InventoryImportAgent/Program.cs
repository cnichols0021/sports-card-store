using ClosedXML.Excel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportsCardStore.Shared.Models;
using SportsCardStore.Core.Enums;
using System.Net.Http.Json;
using System.Text.Json;

namespace InventoryImportAgent;

public class Program
{
    private static ILogger<Program>? _logger;
    private static readonly HttpClient _httpClient = new();
    
    public static async Task<int> Main(string[] args)
    {
        // Configure services
        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });
        
        var serviceProvider = services.BuildServiceProvider();
        _logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        
        try
        {
            // Parse command line arguments
            if (args.Length < 2)
            {
                _logger.LogError("Usage: InventoryImportAgent <API_BASE_URL> <EXCEL_FILE_PATH>");
                return 1;
            }
            
            string apiBaseUrl = args[0].TrimEnd('/');
            string excelFilePath = args[1];
            
            _logger.LogInformation("Starting inventory import from {ExcelFile} to API at {ApiUrl}", 
                excelFilePath, apiBaseUrl);
            
            // Validate inputs
            if (!File.Exists(excelFilePath))
            {
                _logger.LogError("Excel file not found: {ExcelFile}", excelFilePath);
                return 1;
            }
            
            if (!Uri.TryCreate(apiBaseUrl, UriKind.Absolute, out var apiUri))
            {
                _logger.LogError("Invalid API base URL: {ApiUrl}", apiBaseUrl);
                return 1;
            }
            
            // Process the Excel file
            var result = await ProcessExcelFile(excelFilePath, apiBaseUrl);
            
            _logger.LogInformation("Import completed. Imported: {Imported}, Skipped: {Skipped}, Failed: {Failed}",
                result.Imported, result.Skipped, result.Failed);
            
            return 0;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Fatal error during import process");
            return 1;
        }
    }
    
    private static async Task<ImportResult> ProcessExcelFile(string filePath, string apiBaseUrl)
    {
        var result = new ImportResult();
        
        try
        {
            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheets.First();
            
            _logger!.LogInformation("Processing worksheet: {WorksheetName}", worksheet.Name);
            
            // Find header row (assuming first row)
            var headerRow = worksheet.FirstRowUsed();
            var columnMapping = CreateColumnMapping(headerRow);
            
            // Process each data row
            var dataRows = worksheet.RowsUsed().Skip(1); // Skip header row
            
            foreach (var row in dataRows)
            {
                try
                {
                    var card = MapRowToSportsCard(row, columnMapping);
                    
                    if (card == null)
                    {
                        result.Skipped++;
                        continue;
                    }
                    
                    if (await ImportCard(card, apiBaseUrl))
                    {
                        result.Imported++;
                        _logger!.LogDebug("Successfully imported card: {Player} {Year} {Brand}", 
                            card.PlayerName, card.Year, card.Brand);
                    }
                    else
                    {
                        result.Failed++;
                    }
                }
                catch (Exception ex)
                {
                    result.Failed++;
                    _logger!.LogError(ex, "Error processing row {RowNumber}", row.RowNumber());
                }
            }
        }
        catch (Exception ex)
        {
            _logger!.LogError(ex, "Error opening Excel file: {FilePath}", filePath);
            throw;
        }
        
        return result;
    }
    
    private static Dictionary<string, int> CreateColumnMapping(IXLRow headerRow)
    {
        var mapping = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        
        for (int col = 1; col <= headerRow.CellsUsed().Count(); col++)
        {
            var cellValue = headerRow.Cell(col).GetString();
            if (!string.IsNullOrWhiteSpace(cellValue))
            {
                mapping[cellValue.Trim()] = col;
            }
        }
        
        _logger!.LogInformation("Found {ColumnCount} columns in Excel file", mapping.Count);
        _logger!.LogDebug("Column mapping: {Columns}", string.Join(", ", mapping.Keys));
        
        return mapping;
    }
    
    private static CreateSportsCardRequest? MapRowToSportsCard(IXLRow row, Dictionary<string, int> columnMapping)
    {
        try
        {
            // Get required fields
            var playerName = GetCellValue(row, columnMapping, "Player Name")?.Trim();
            if (string.IsNullOrEmpty(playerName))
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Player Name is empty", row.RowNumber());
                return null;
            }
            
            var team = GetCellValue(row, columnMapping, "Team")?.Trim();
            if (string.IsNullOrEmpty(team))
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Team is empty", row.RowNumber());
                return null;
            }
            
            var brand = GetCellValue(row, columnMapping, "Brand")?.Trim();
            if (string.IsNullOrEmpty(brand))
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Brand is empty", row.RowNumber());
                return null;
            }
            
            var setName = GetCellValue(row, columnMapping, "Set Name")?.Trim();
            if (string.IsNullOrEmpty(setName))
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Set Name is empty", row.RowNumber());
                return null;
            }
            
            var cardNumber = GetCellValue(row, columnMapping, "Card Number")?.Trim();
            if (string.IsNullOrEmpty(cardNumber))
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Card Number is empty", row.RowNumber());
                return null;
            }
            
            // Parse and validate year
            var yearText = GetCellValue(row, columnMapping, "Year");
            if (!int.TryParse(yearText, out int year) || year < 1800 || year > 2100)
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Invalid year '{Year}'", row.RowNumber(), yearText);
                return null;
            }
            
            // Parse sport
            var sportText = GetCellValue(row, columnMapping, "Sport")?.Trim();
            if (!Enum.TryParse<Category>(sportText, true, out var sport))
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Invalid sport '{Sport}'", row.RowNumber(), sportText);
                return null;
            }
            
            // Parse grading company
            var gradingCompanyText = GetCellValue(row, columnMapping, "Grading Company")?.Trim();
            if (!Enum.TryParse<GradingCompany>(gradingCompanyText, true, out var gradingCompany))
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Invalid grading company '{GradingCompany}'", 
                    row.RowNumber(), gradingCompanyText);
                return null;
            }
            
            // Parse grade (nullable for Raw cards)
            decimal? grade = null;
            var gradeText = GetCellValue(row, columnMapping, "Card Grade")?.Trim();
            if (!string.IsNullOrEmpty(gradeText))
            {
                if (decimal.TryParse(gradeText, out decimal gradeValue))
                {
                    if (gradeValue < 0 || gradeValue > 10)
                    {
                        _logger!.LogWarning("Skipping row {RowNumber}: Grade out of range '{Grade}'", 
                            row.RowNumber(), gradeValue);
                        return null;
                    }
                    grade = gradeValue;
                }
            }
            
            // Validate Raw cards have null grade
            if (gradingCompany == GradingCompany.Raw && grade.HasValue)
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Raw card cannot have grade", row.RowNumber());
                return null;
            }
            
            // Parse price
            var priceText = GetCellValue(row, columnMapping, "Price")?.Trim();
            if (!decimal.TryParse(priceText, out decimal price) || price <= 0)
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Invalid price '{Price}'", row.RowNumber(), priceText);
                return null;
            }
            
            // Parse quantity
            var quantityText = GetCellValue(row, columnMapping, "Quantity")?.Trim();
            if (!int.TryParse(quantityText, out int quantity) || quantity < 1)
            {
                _logger!.LogWarning("Skipping row {RowNumber}: Invalid quantity '{Quantity}'", 
                    row.RowNumber(), quantityText);
                return null;
            }
            
            // Parse boolean fields
            var isRookie = ParseBoolean(GetCellValue(row, columnMapping, "Rookie"));
            var isAutograph = ParseBoolean(GetCellValue(row, columnMapping, "Autograph"));
            var isRelic = ParseBoolean(GetCellValue(row, columnMapping, "Relic"));
            var isBowmanFirst = ParseBoolean(GetCellValue(row, columnMapping, "Bowman First"));
            
            // Optional fields
            var condition = GetCellValue(row, columnMapping, "Condition")?.Trim();
            var description = GetCellValue(row, columnMapping, "Description")?.Trim();
            var imageUrl = GetCellValue(row, columnMapping, "Image Url")?.Trim();
            
            // Parse IsAvailable (default to true if not specified)
            var isAvailableText = GetCellValue(row, columnMapping, "Is Available");
            var isAvailable = string.IsNullOrEmpty(isAvailableText) ? true : ParseBoolean(isAvailableText);
            
            return new CreateSportsCardRequest
            {
                PlayerName = playerName,
                Year = year,
                Brand = brand,
                SetName = setName,
                CardNumber = cardNumber,
                Sport = sport,
                Team = team,
                IsRookie = isRookie,
                IsAutograph = isAutograph,
                IsRelic = isRelic,
                IsBowmanFirst = isBowmanFirst,
                Grade = grade,
                GradingCompany = gradingCompany,
                Condition = condition,
                Price = price,
                Quantity = quantity,
                ImageUrl = imageUrl,
                Description = description,
                IsAvailable = isAvailable
            };
        }
        catch (Exception ex)
        {
            _logger!.LogError(ex, "Error mapping row {RowNumber} to SportsCard", row.RowNumber());
            return null;
        }
    }
    
    private static string? GetCellValue(IXLRow row, Dictionary<string, int> columnMapping, string columnName)
    {
        if (columnMapping.TryGetValue(columnName, out int columnIndex))
        {
            return row.Cell(columnIndex).GetString();
        }
        return null;
    }
    
    private static bool ParseBoolean(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;
            
        value = value.Trim().ToLowerInvariant();
        return value == "true" || value == "yes" || value == "1" || value == "y";
    }
    
    private static async Task<bool> ImportCard(CreateSportsCardRequest card, string apiBaseUrl)
    {
        try
        {
            var json = JsonSerializer.Serialize(card, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            _logger!.LogDebug("Posting card data: {Json}", json);
            
            var response = await _httpClient.PostAsJsonAsync($"{apiBaseUrl}/api/sportscards", card);
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger!.LogError("Failed to import card {Player} {Year} {Brand}. Status: {Status}, Error: {Error}",
                    card.PlayerName, card.Year, card.Brand, response.StatusCode, errorContent);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger!.LogError(ex, "Exception importing card {Player} {Year} {Brand}",
                card.PlayerName, card.Year, card.Brand);
            return false;
        }
    }
}

public class ImportResult
{
    public int Imported { get; set; }
    public int Skipped { get; set; }
    public int Failed { get; set; }
}