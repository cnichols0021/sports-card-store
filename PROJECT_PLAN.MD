# Sports Card Store - Project Plan

## Project Overview

**Sports Card Store** is a modern ecommerce platform built with ASP.NET Core 8 for buying and selling sports cards. The platform will feature clean architecture, cloud-native design, and AI-powered features to enhance the trading card experience.

### Key Features

- User registration and authentication
- Sports card catalog with advanced search and filtering
- Shopping cart and secure checkout with Stripe
- Order management and tracking
- Inventory management for sellers
- Card condition grading system
- Wishlist and collection tracking
- Price history and market analytics
- AI-powered card recognition and valuation
- Admin dashboard for platform management

### Architecture Philosophy

The application follows Clean Architecture principles with clear separation of concerns:

- **API Layer**: Controllers and middleware
- **Core Layer**: Domain entities, interfaces, and business logic
- **Infrastructure Layer**: Data access, external service integrations
- **Tests**: Comprehensive unit and integration testing

## Personal Infrastructure and Data Assets

### Existing Card Inventory

- **Detailed Excel Spreadsheet**: Comprehensive tracking of current card inventory including player names, years, sets, conditions, and valuations
- **Primary Data Source**: This spreadsheet serves as the foundation for the Inventory Import Agent, enabling bulk upload of existing inventory to bootstrap the platform
- **Data Structure**: Already organized with consistent naming conventions and categorization that aligns with the planned database schema

### Image Digitization Equipment

- **Epson ES-400 II Duplex Scanner**: Professional-grade scanner capable of high-resolution card imaging
- **Scanning Pipeline**: Drives the design of the image upload pipeline with support for batch processing and automated image optimization
- **Quality Standards**: Ensures consistent, high-quality card images that meet marketplace standards for condition assessment

### Business Integration Opportunities

- **Active WhatNot Card Breaking Business**: Operating card breaking business on the WhatNot platform
- **Future Integration**: Potential for live selling features, real-time inventory synchronization, and cross-platform promotion
- **Market Insights**: Provides real-world pricing data and customer behavior patterns to inform platform features

## Entity List with Fields

### User

- `Id` (Guid, Primary Key)
- `Email` (string, unique, required)
- `FirstName` (string, required)
- `LastName` (string, required)
- `PhoneNumber` (string)
- `DateOfBirth` (DateTime?)
- `IsEmailVerified` (bool)
- `Role` (enum: Customer, Seller, Admin)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime)

### UserProfile

- `Id` (Guid, Primary Key)
- `UserId` (Guid, Foreign Key)
- `StoreName` (string) // For sellers
- `Bio` (string)
- `ProfileImageUrl` (string)
- `Address` (Address entity)
- `SellerRating` (decimal?)
- `TotalSales` (int, default: 0)

### Address

- `Id` (Guid, Primary Key)
- `Street` (string, required)
- `City` (string, required)
- `State` (string, required)
- `ZipCode` (string, required)
- `Country` (string, required)
- `IsDefault` (bool)

### Category

- `Id` (int, Primary Key)
- `Name` (string, required) // e.g., Baseball, Basketball, Football
- `Description` (string)
- `IsActive` (bool)

### Brand

- `Id` (int, Primary Key)
- `Name` (string, required) // e.g., Topps, Panini, Upper Deck
- `Description` (string)
- `LogoUrl` (string)
- `IsActive` (bool)

### Card

- `Id` (Guid, Primary Key)
- `Name` (string, required) // Player name
- `Year` (int, required)
- `BrandId` (int, Foreign Key)
- `CategoryId` (int, Foreign Key)
- `SetName` (string, required)
- `CardNumber` (string, required)
- `Description` (text)
- `IsRookie` (bool)
- `IsAutograph` (bool)
- `IsRelic` (bool)
- `PrintRun` (int?) // Number printed
- `CreatedAt` (DateTime)

### CardListing

- `Id` (Guid, Primary Key)
- `CardId` (Guid, Foreign Key)
- `SellerId` (Guid, Foreign Key)
- `Title` (string, required)
- `Description` (text)
- `Price` (decimal, required)
- `Condition` (enum: Mint, NearMint, Excellent, VeryGood, Good, Fair, Poor)
- `ConditionNotes` (string)
- `Quantity` (int, required)
- `IsActive` (bool)
- `FeaturedUntil` (DateTime?)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime)

### CardImage

- `Id` (Guid, Primary Key)
- `CardListingId` (Guid, Foreign Key)
- `ImageUrl` (string, required)
- `AltText` (string)
- `SortOrder` (int)
- `IsPrimary` (bool)

### Order

- `Id` (Guid, Primary Key)
- `BuyerId` (Guid, Foreign Key)
- `OrderNumber` (string, unique)
- `Status` (enum: Pending, Confirmed, Shipped, Delivered, Cancelled, Refunded)
- `SubTotal` (decimal)
- `TaxAmount` (decimal)
- `ShippingAmount` (decimal)
- `TotalAmount` (decimal)
- `ShippingAddress` (Address entity)
- `BillingAddress` (Address entity)
- `StripePaymentIntentId` (string)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime)

### OrderItem

- `Id` (Guid, Primary Key)
- `OrderId` (Guid, Foreign Key)
- `CardListingId` (Guid, Foreign Key)
- `Quantity` (int, required)
- `UnitPrice` (decimal, required)
- `TotalPrice` (decimal, required)

### ShoppingCart

- `Id` (Guid, Primary Key)
- `UserId` (Guid, Foreign Key)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime)

### ShoppingCartItem

- `Id` (Guid, Primary Key)
- `ShoppingCartId` (Guid, Foreign Key)
- `CardListingId` (Guid, Foreign Key)
- `Quantity` (int, required)
- `AddedAt` (DateTime)

### Wishlist

- `Id` (Guid, Primary Key)
- `UserId` (Guid, Foreign Key)
- `CardId` (Guid, Foreign Key)
- `MaxPrice` (decimal?)
- `AddedAt` (DateTime)

### PriceHistory

- `Id` (Guid, Primary Key)
- `CardId` (Guid, Foreign Key)
- `Condition` (enum)
- `Price` (decimal)
- `Source` (string) // e.g., "SportsCardStore", "External"
- `RecordedAt` (DateTime)

## Azure Services Needed

### Core Services

- **Azure App Service**: Host the ASP.NET Core 8 API
  - Standard tier for production scaling
  - Deployment slots for staging
  - Custom domain and SSL
  - Application Insights integration

- **Azure SQL Database**: Primary data storage
  - Basic/Standard S0 tier (~$5-15/month), appropriate for development and portfolio projects
  - Automated backups and point-in-time restore
  - Transparent Data Encryption
  - Connection pooling

- **Azure Blob Storage**: File and image storage
  - Hot tier for frequently accessed images
  - Cool tier for archived data
  - CDN integration for global performance
  - Private containers with SAS tokens

### Authentication & Security

- **Azure Key Vault**: Secure storage of secrets
  - Stripe API keys
  - Database connection strings
  - JWT signing keys
  - Third-party API keys

**Future Enhancement:**

- **Azure Active Directory B2C**: User authentication and management (Phase 1 will use ASP.NET Core Identity and JWT only)
  - Social login providers (Google, Facebook, Apple)
  - Custom user flows
  - Multi-factor authentication

### Monitoring & Performance

- **Application Insights**: Application monitoring and analytics
  - Performance monitoring
  - Error tracking and diagnostics
  - Custom telemetry for business metrics
  - Live metrics streaming

- **Azure Monitor**: Infrastructure monitoring
  - Log Analytics workspace
  - Custom dashboards
  - Alerting rules

### Additional Services

- **Azure CDN**: Global content distribution
  - Cache static assets and images
  - Improve page load performance
  - Custom caching rules

- **Azure Service Bus** (Future): Async processing
  - Order processing workflows
  - Email notifications
  - Inventory updates

- **Azure Cognitive Services** (Future): AI capabilities
  - Computer Vision for card recognition
  - Text Analytics for product descriptions

## API Endpoint Plan

### Authentication Endpoints

```
POST   /api/auth/register
POST   /api/auth/login
POST   /api/auth/refresh-token
POST   /api/auth/logout
POST   /api/auth/forgot-password
POST   /api/auth/reset-password
POST   /api/auth/verify-email
```

### User Management

```
GET    /api/users/profile
PUT    /api/users/profile
GET    /api/users/{id}
POST   /api/users/addresses
PUT    /api/users/addresses/{id}
DELETE /api/users/addresses/{id}
```

### Card Catalog

```
GET    /api/cards                    // Search and filter cards
GET    /api/cards/{id}              // Get card details
POST   /api/cards                   // Admin: Add new card
PUT    /api/cards/{id}              // Admin: Update card
GET    /api/cards/{id}/listings     // Get listings for a card
GET    /api/cards/{id}/price-history
```

### Card Listings

```
GET    /api/listings                // Browse all listings
GET    /api/listings/{id}          // Get listing details
POST   /api/listings               // Create new listing
PUT    /api/listings/{id}          // Update listing
DELETE /api/listings/{id}          // Delete listing
POST   /api/listings/{id}/images   // Upload listing images
```

### Shopping Cart

```
GET    /api/cart                   // Get user's cart
POST   /api/cart/items            // Add item to cart
PUT    /api/cart/items/{id}       // Update cart item
DELETE /api/cart/items/{id}       // Remove from cart
DELETE /api/cart                  // Clear cart
```

### Orders

```
GET    /api/orders                 // User's order history
GET    /api/orders/{id}           // Get order details
POST   /api/orders                // Create new order
PUT    /api/orders/{id}/status    // Update order status
GET    /api/orders/{id}/tracking  // Get tracking information
```

### Payment Processing

```
POST   /api/payments/create-intent      // Create Stripe payment intent
POST   /api/payments/confirm/{intentId} // Confirm payment
POST   /api/payments/webhooks/stripe    // Stripe webhook handler
```

### Wishlist

```
GET    /api/wishlist              // Get user's wishlist
POST   /api/wishlist             // Add card to wishlist
DELETE /api/wishlist/{cardId}    // Remove from wishlist
```

### Admin Endpoints

```
GET    /api/admin/users           // Manage users
GET    /api/admin/orders          // Order management
GET    /api/admin/analytics       // Platform analytics
POST   /api/admin/categories      // Manage categories
POST   /api/admin/brands         // Manage brands
```

### Utility Endpoints

```
GET    /api/categories           // Get all categories
GET    /api/brands              // Get all brands
GET    /api/search              // Global search
GET    /api/health              // Health check
```

## AI Agents Planned

### GitHub Copilot Integration

- **Code Generation**: Leverage Copilot for boilerplate code, API controllers, and service implementations
- **Test Writing**: Generate comprehensive unit and integration tests
- **Documentation**: Auto-generate API documentation and code comments
- **Code Reviews**: Use Copilot suggestions for code improvements and best practices

### Hands-On Agent Development

#### 1. Card Listing Agent

- **Purpose**: Accept a raw card description string and generate a complete optimized product listing
- **Technology**: C# class using HttpClient to call Claude API
- **Output**: Structured CardListing object with Title, Description, SuggestedPrice, Tags, and Category
- **Learning Goal**: Prompt pipelines, structured output parsing, calling AI APIs from C#

#### 2. Inventory Import Agent

- **Purpose**: Process existing Excel card tracker and import cards into the database
- **Technology**: C# with EPPlus or ClosedXML for Excel parsing, maps columns to CardListing entities
- **Output**: Validated SQL inserts or API calls to populate the database
- **Learning Goal**: File parsing, multi-step agent chains, tool use and function calling

#### 3. Price Research Agent

- **Purpose**: Research recent sold listings and return a suggested price range for a given card
- **Technology**: C# agent with web search integration
- **Output**: Price range recommendation with market data sources
- **Learning Goal**: Agents with web search tools, ReAct-style reasoning loops, grounding AI output in real data

### Future AI Features

#### Card Recognition Agent

- **Purpose**: Automatically identify sports cards from uploaded images
- **Technology**: Azure Computer Vision + Custom ML model
- **Features**:
  - Extract player name, year, brand, and set information
  - Suggest market value based on condition
  - Auto-populate listing details

#### Recommendation Engine

- **Purpose**: Personalized card recommendations
- **Technology**: Collaborative filtering + Content-based recommendations
- **Features**:
  - "Customers who bought this also bought"
  - Personalized homepage content
  - Investment opportunity alerts
  - Collection completion suggestions

#### Customer Service Bot

- **Purpose**: Handle common customer inquiries
- **Technology**: Azure Bot Service + Azure AI Language (or direct Claude/OpenAI API integration as simpler alternative)
- **Features**:
  - Order status inquiries
  - Return and refund policies
  - Card authentication questions
  - Platform navigation help

## Tech Stack Decisions

### Backend Framework

- **ASP.NET Core 8**: Latest LTS version with improved performance and features
- **Entity Framework Core 8**: ORM for database operations with strong typing
- **MediatR**: CQRS pattern implementation for clean request/response handling
- **FluentValidation**: Input validation with fluent syntax
- **AutoMapper**: Object-to-object mapping for DTOs

### Authentication & Authorization

**Phase 1 Implementation:**

- **ASP.NET Core Identity**: Built-in authentication system
- **JWT Bearer Tokens**: Stateless authentication for API
- **Policy-based Authorization**: Role and resource-based access control

**Future Enhancements:**

- **Azure AD B2C**: External identity provider for social logins (keeps Phase 1 complexity manageable for portfolio project)

### Database & Caching

- **Azure SQL Database**: Primary relational database
- **Entity Framework Core**: Database access and migrations
- **Redis Cache** (Future): Session storage and performance optimization
- **Connection Resilience**: Retry policies and circuit breakers

### File Storage

- **Azure Blob Storage**: Scalable file storage for images
- **Image Processing**: Resize and optimize images on upload
- **CDN Integration**: Global content delivery for performance

### Payment Processing

- **Stripe**: Secure payment processing
- **Stripe Elements**: Frontend payment UI components
- **Webhook Handling**: Secure payment status updates
- **PCI Compliance**: Secure handling of payment data

### API Design

- **RESTful APIs**: Standard HTTP methods and status codes
- **OpenAPI/Swagger**: API documentation and testing
- **JSON Serialization**: System.Text.Json for performance
- **API Versioning**: Support for future API evolution
- **Rate Limiting**: Prevent abuse and ensure fair usage

### Cross-Cutting Concerns

- **Logging**: Serilog with structured logging to Application Insights
- **Error Handling**: Global exception handling middleware
- **Health Checks**: Application and dependency health monitoring
- **Configuration**: Azure App Configuration for environment-specific settings
- **Secrets Management**: Azure Key Vault integration

### Testing Strategy

- **xUnit**: Primary testing framework
- **FluentAssertions**: More readable test assertions
- **Moq**: Mocking framework for unit tests
- **TestContainers**: Integration testing with real databases
- **SpecFlow** (Future): Behavior-driven development

### DevOps & Deployment

- **Azure Pipelines**: CI/CD pipeline automation
- **Azure Resource Manager**: Infrastructure as Code
- **Application Insights**: Performance monitoring and diagnostics
- **Feature Flags**: Safe feature rollouts and A/B testing

**Future Considerations:**

- **Docker**: Containerization for consistent deployments

### Development Tools

- **GitHub Copilot**: AI-assisted code development
- **Visual Studio 2022**: Primary IDE with debugging tools
- **Postman/Thunder Client**: API testing and documentation
- **Azure Data Studio**: Database management and querying

### AI-Assisted Development Tools

- **GitHub Copilot**: Primary AI coding assistant in VS Code and Visual Studio 2022
- **Azure MCP Server**: Used in Copilot Agent Mode to create and configure Azure resources directly from VS Code chat. Configured via .vscode/mcp.json using @azure/mcp package.
- **Playwright MCP**: Browser automation of Azure Portal for verification and GUI tasks not covered by Azure MCP
- **Claude**: Architecture planning, prompt refinement, and code review
- **Prompt Engineering Log**: Living document tracking all prompts used, output ratings, and lessons learned throughout development

### Performance Considerations

- **Async/Await**: Non-blocking operations throughout
- **Connection Pooling**: Efficient database connections
- **Image Optimization**: WebP format and multiple sizes
- **Caching Strategy**: HTTP caching headers and server-side caching
- **Database Indexing**: Optimized queries for search and filtering

### Security Measures

- **HTTPS Everywhere**: TLS encryption for all communications
- **Input Sanitization**: XSS and SQL injection prevention
- **CORS Configuration**: Secure cross-origin requests
- **Content Security Policy**: Additional XSS protection
- **Regular Security Updates**: Dependency scanning and updates

---

## Project Structure

```
SportsCardStore/
├── src/
│   ├── SportsCardStore.API/          # Web API project
│   ├── SportsCardStore.Core/         # Domain entities and interfaces
│   ├── SportsCardStore.Infrastructure/ # Data access and external services
│   └── SportsCardStore.Shared/       # Shared DTOs and utilities
├── tests/
│   ├── SportsCardStore.UnitTests/    # Unit tests
│   ├── SportsCardStore.IntegrationTests/ # Integration tests
│   └── SportsCardStore.FunctionalTests/  # End-to-end tests
├── docs/                             # Documentation
├── scripts/                          # Build and deployment scripts
└── README.md
```

## Getting Started

1. **Prerequisites**: .NET 8 SDK, Azure CLI, Visual Studio 2022
2. **Clone Repository**: Set up local development environment
3. **Azure Resources**: Create required Azure services
4. **Database Setup**: Run EF Core migrations
5. **Configuration**: Set up local secrets and app settings
6. **Run Application**: Start API and test endpoints

---

_This project plan will be updated as development progresses and new requirements are identified._
