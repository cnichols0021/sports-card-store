# 🎯 Sports Card Store

> **Modern ASP.NET Core 8 ecommerce platform for buying and selling sports cards, showcasing AI-assisted full-stack development on Azure**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://docs.microsoft.com/en-us/aspnet/core/)
[![Azure](https://img.shields.io/badge/Azure-0078D4?style=for-the-badge&logo=microsoft-azure&logoColor=white)](https://azure.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-8.0-512BD4?style=for-the-badge&logo=microsoft&logoColor=white)](https://docs.microsoft.com/en-us/ef/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![Stripe](https://img.shields.io/badge/Stripe-008CDD?style=for-the-badge&logo=stripe&logoColor=white)](https://stripe.com/)
[![GitHub Copilot](https://img.shields.io/badge/GitHub%20Copilot-000000?style=for-the-badge&logo=github&logoColor=white)](https://github.com/features/copilot)

## 🎯 Project Overview

Sports Card Store is a comprehensive ecommerce platform built with **ASP.NET Core 8** that enables collectors to buy and sell sports cards in a secure, user-friendly marketplace. This project serves as a **portfolio demonstration** of modern full-stack development practices, clean architecture principles, and AI-assisted development workflows.

The platform bridges traditional card collecting with modern technology, featuring automated listing generation, price intelligence, and inventory management tools that streamline the trading card marketplace experience.

### 🚀 **Portfolio Highlights**

- **Clean Architecture** implementation with proper separation of concerns
- **AI-Assisted Development** using cutting-edge tools and techniques
- **Cloud-Native Design** optimized for Azure deployment
- **Modern .NET 8** features and performance optimizations
- **Production-Ready** security and scalability considerations

## 🛠️ Tech Stack

### **Backend**

- **ASP.NET Core 8** - Web API with controllers
- **Entity Framework Core 8** - ORM and database migrations
- **Azure SQL Database** - Primary data storage
- **MediatR** - CQRS pattern implementation
- **FluentValidation** - Input validation
- **AutoMapper** - Object mapping

### **Cloud & Infrastructure**

- **Azure App Service** - Web application hosting
- **Azure Blob Storage** - File and image storage
- **Azure Key Vault** - Secrets management
- **Application Insights** - Monitoring and analytics
- **Azure Pipelines** - CI/CD automation

### **Authentication & Payments**

- **ASP.NET Core Identity** - User authentication
- **JWT Bearer Tokens** - Stateless API authentication
- **Stripe** - Payment processing and webhooks

### **Testing & Quality**

- **xUnit** - Unit testing framework
- **FluentAssertions** - Test assertions
- **Moq** - Mocking framework
- **Swagger/OpenAPI** - API documentation

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────┐
│                 Architecture Diagram                │
│                                                     │
│  ┌─────────────┐    ┌──────────────┐    ┌────────┐ │
│  │   Web API   │───▶│     Core     │◀───│ Tests  │ │
│  │ Controllers │    │   Entities   │    │ xUnit  │ │
│  │  Swagger    │    │ Interfaces   │    │  Moq   │ │
│  └─────────────┘    │  Services    │    └────────┘ │
│         │            └──────────────┘               │
│         ▼                    ▲                      │
│  ┌─────────────┐    ┌──────────────┐               │
│  │Infrastructure│───▶│  Azure SQL   │               │
│  │  EF Context │    │   Database    │               │
│  │ Repositories│    │              │               │
│  └─────────────┘    └──────────────┘               │
│                                                     │
│         Hosted on Azure App Service                 │
└─────────────────────────────────────────────────────┘
```

_Detailed architecture diagram coming soon..._

## ✨ Key Features

### 🛒 **Ecommerce Core**

- User registration and authentication
- Advanced product catalog with search and filtering
- Shopping cart and secure checkout
- Order management and tracking
- Multi-condition card grading system

### 📊 **Business Intelligence**

- Price history tracking and analytics
- Market trend analysis
- Inventory management for sellers
- Sales performance dashboards

### 🤖 **AI-Powered Features**

- **Card Listing Agent** - Automated product listing generation
- **Inventory Import Agent** - Excel spreadsheet processing
- **Price Research Agent** - Market-based pricing recommendations

### 🔐 **Security & Performance**

- JWT-based authentication
- Input validation and sanitization
- Rate limiting and abuse prevention
- Image optimization and CDN integration
- Comprehensive error handling and logging

## 🤖 AI-Assisted Development Showcase

This project demonstrates modern AI-assisted development workflows using cutting-edge tools:

### **🚀 Primary Development Tools**

- **[GitHub Copilot](https://github.com/features/copilot)** - AI-powered code completion and generation in VS Code/Visual Studio
- **[Azure MCP Server](https://github.com/Azure/azure-mcp)** - Direct Azure resource management from VS Code chat using @azure/mcp package
- **[Playwright MCP](https://github.com/microsoft/playwright)** - Browser automation for Azure Portal verification and GUI testing
- **[Claude AI](https://claude.ai)** - Architecture planning, prompt engineering, and comprehensive code review

### **📚 AI Development Methodology**

- **Prompt Engineering Pipeline** - Structured prompts for consistent code generation
- **Agent-Based Architecture** - Modular AI agents for specific business functions
- **Iterative Refinement** - Continuous improvement through AI feedback loops
- **Living Documentation** - AI-generated and maintained technical documentation

### **🎯 Learning Objectives Demonstrated**

- Integration of multiple AI tools in a cohesive development workflow
- Practical application of AI agents in business logic
- Modern prompt engineering techniques
- AI-assisted testing and quality assurance

## 🚀 Getting Started

### **Prerequisites**

- **.NET 8 SDK** or later
- **Visual Studio 2022** or **VS Code** with C# extension
- **SQL Server** (LocalDB or Azure SQL Database)
- **Azure CLI** (for deployment)
- **Git** for version control

### **Development Tools** (Optional but Recommended)

- **GitHub Copilot** subscription
- **Azure subscription** for cloud resources
- **Stripe account** for payment testing

### **Local Development Setup**

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/sports-card-store.git
   cd sports-card-store
   ```

2. **Restore packages**

   ```bash
   dotnet restore
   ```

3. **Set up user secrets** (API project)

   ```bash
   cd src/SportsCardStore.API
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=SportsCardStoreDB;Trusted_Connection=true"
   dotnet user-secrets set "Stripe:SecretKey" "your-stripe-secret-key"
   dotnet user-secrets set "Stripe:PublishableKey" "your-stripe-publishable-key"
   ```

4. **Run database migrations**

   ```bash
   dotnet ef database update
   ```

5. **Start the application**

   ```bash
   dotnet run --project src/SportsCardStore.API
   ```

6. **Access the API**
   - API: `https://localhost:7001`
   - Swagger UI: `https://localhost:7001/swagger`

### **Running Tests**

```bash
dotnet test
```

## 📈 Project Status

### **🚧 Current Phase: Foundation & Core Development**

| Component                    | Status         | Description                                       |
| ---------------------------- | -------------- | ------------------------------------------------- |
| 🏗️ **Solution Architecture** | ✅ Complete    | Clean architecture with proper project separation |
| 🗃️ **Domain Models**         | 🔄 In Progress | Core entities and value objects                   |
| 🔌 **Data Layer**            | 🔄 In Progress | EF Core context and repositories                  |
| 🌐 **API Endpoints**         | ⏳ Planned     | RESTful controllers and DTOs                      |
| 🔐 **Authentication**        | ⏳ Planned     | ASP.NET Core Identity + JWT                       |
| 💳 **Payment Integration**   | ⏳ Planned     | Stripe payment processing                         |
| 🤖 **AI Agents**             | ⏳ Planned     | Card listing, inventory import, price research    |
| 🧪 **Testing Suite**         | ⏳ Planned     | Unit and integration tests                        |
| ☁️ **Azure Deployment**      | ⏳ Planned     | App Service, SQL Database, Blob Storage           |

### **🎯 Next Milestones**

- [ ] **Phase 1**: Complete core domain models and data layer
- [ ] **Phase 2**: Implement authentication and basic CRUD operations
- [ ] **Phase 3**: Add payment processing and order management
- [ ] **Phase 4**: Build AI-powered features and agents
- [ ] **Phase 5**: Azure deployment and production optimization

## 🤝 Contributing

This is a portfolio project, but feedback and suggestions are welcome! Feel free to:

- 🐛 Report bugs or issues
- 💡 Suggest new features or improvements
- 📖 Improve documentation
- 🔧 Submit pull requests

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Contact

**Chris Nichols**

- 💼 [LinkedIn](https://linkedin.com/in/yourprofile)
- 🐱 [GitHub](https://github.com/yourusername)
- 📧 Email: your.email@domain.com
- 💬 [Project Discussions](https://github.com/yourusername/sports-card-store/discussions)

---

> **💡 This project showcases modern software development practices including clean architecture, AI-assisted development, and cloud-native design. It serves as a comprehensive demonstration of full-stack .NET development skills for potential employers and collaborators.**

**⭐ If you find this project interesting, please give it a star!**
