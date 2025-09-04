# Weather Dashboard & WeatherProxy

This project consists of a Vue 3 frontend for displaying weather data and an ASP.NET Core backend that proxies weather information from external APIs.

---

## Frontend: `weather-dashboard` (Vue 3 + Vite)

### Features
- Search for weather by city
- View hourly and daily forecasts
- Interactive charts (Chart.js, vue-chartjs)
- Responsive UI (Tailwind CSS)

### Setup

1. **Install dependencies:**
```
npm install
```
2. **Run in development mode:**
```
npm run dev
```
3. **Build for production:**
```
npm run build
```

### Configuration
- The frontend expects the backend API to be available at `http://localhost:5000` (default ASP.NET Core port).
- Update API endpoints in your Vue components if needed.

---

## Backend: `WeatherProxy` (ASP.NET Core 8)

### Features
- REST API endpoint: `/weather/{city}?days={n}`
- CORS enabled for frontend origin
- Caching for repeated requests
- Swagger UI for API documentation
- Proxies weather data from Open-Meteo

### Setup

1. **Restore and build:**
```
dotnet restore dotnet build
```
2. **Run the application:**
```
dotnet run
```
3. **API Usage Example:**
```
GET /weather/London?days=3
```

### Configuration
- CORS origin is set via `FrontendOrigin` in `appsettings.json`.
- Swagger UI available at `/swagger` in development mode.

---

## Development Notes

- Frontend uses Vite for fast development and hot-reload.
- Backend uses in-memory caching and external HTTP client for weather data.
- Both projects are decoupled and communicate via HTTP.

---

## License

MIT