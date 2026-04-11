# Sports Card Store - Frontend

A modern React + TypeScript frontend for the Sports Card Store application, built with Vite and styled with Tailwind CSS.

## Features

- **Card Browsing**: View sports cards in a responsive grid layout
- **Advanced Filtering**: Filter by sport, brand, player name, price range, and more
- **Pagination**: Navigate through large collections efficiently
- **Card Details**: View detailed information for individual cards
- **Responsive Design**: Optimized for desktop, tablet, and mobile devices
- **TypeScript**: Full type safety and better developer experience

## Tech Stack

- **React 18** - UI library
- **TypeScript** - Type safety and development experience
- **Vite** - Fast build tool and dev server
- **Tailwind CSS** - Utility-first CSS framework
- **React Router** - Client-side routing

## Getting Started

### Prerequisites

- Node.js 16 or higher
- npm or yarn

### Installation

1. Navigate to the project directory:

   ```bash
   cd src/SportsCardStore.Web
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Create environment file:

   ```bash
   cp .env.example .env
   ```

4. Update the `.env` file with your API URL:
   ```
   VITE_API_BASE_URL=http://localhost:5000
   ```

### Development

Start the development server:

```bash
npm run dev
```

The application will be available at `http://localhost:3000`.

### Building for Production

Build the application for production:

```bash
npm run build
```

Preview the production build:

```bash
npm run preview
```

## Project Structure

```
src/SportsCardStore.Web/
├── src/
│   ├── components/         # Reusable UI components
│   │   ├── CardFilters.tsx
│   │   ├── CardItem.tsx
│   │   ├── ErrorMessage.tsx
│   │   ├── LoadingSpinner.tsx
│   │   └── Pagination.tsx
│   ├── pages/             # Page components
│   │   ├── CardDetailPage.tsx
│   │   └── CardListPage.tsx
│   ├── services/          # API services
│   │   └── apiService.ts
│   ├── types/             # TypeScript type definitions
│   │   └── index.ts
│   ├── utils/             # Utility functions
│   │   └── formatters.ts
│   ├── App.tsx            # Main application component
│   ├── main.tsx           # Application entry point
│   └── index.css          # Global styles
├── public/                # Static assets
├── .env.example           # Environment variables template
├── package.json           # Dependencies and scripts
├── tailwind.config.js     # Tailwind CSS configuration
├── tsconfig.json          # TypeScript configuration
└── vite.config.ts         # Vite configuration
```

## Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run lint` - Run ESLint
- `npm run preview` - Preview production build

## Environment Variables

- `VITE_API_BASE_URL` - Base URL for the API (default: http://localhost:5000)

## API Integration

The frontend integrates with the Sports Card Store API using the following endpoints:

- `GET /api/sportscards` - List sports cards with filtering and pagination
- `GET /api/sportscards/{id}` - Get specific card details

### Supported Filters

- Sport (Baseball, Football, Basketball, Hockey)
- Brand (text search)
- Player Name (text search)
- Bowman First (checkbox)
- Is Autograph (checkbox)
- Price Range (min/max)
- Parallel Name (text search)
- Max Print Run (number)

## Features

### Card List Page (/)

- Responsive grid layout for card display
- Advanced filtering panel
- Pagination controls
- Loading states and error handling
- Search and filter persistence

### Card Detail Page (/cards/:id)

- Full card information display
- High-resolution image support
- Card grading and condition information
- Special badges for rookie cards, autographs, etc.
- Navigation back to card list

## Styling

The application uses Tailwind CSS for styling with a custom color palette:

- Primary colors: Blue shades (primary-50 to primary-950)
- Component styling: Cards, buttons, forms, and layouts
- Responsive design: Mobile-first approach with breakpoints

## Type Safety

The application includes comprehensive TypeScript interfaces that match the backend API models:

- `SportsCardResponse` - Individual card data
- `PagedSportsCardResponse` - Paginated card list response
- `CardFilters` - Filter parameters
- `Category` and `GradingCompany` enums

## Error Handling

- Network error handling with retry functionality
- Loading states for all async operations
- User-friendly error messages
- Graceful fallbacks for missing data

## Performance

- Lazy loading for card images
- Efficient pagination to limit data transfer
- Debounced filter inputs
- Optimized bundle size with Vite
