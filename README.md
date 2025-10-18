### Setup Instructions

## Prerequisites

- .NET 8 SDK and Docker need to be installed on system.

1. **Start Redis Cache:**

   ```bash
   docker run --name redis-teamsync -p 6379:6379 -d redis:alpine
   ```

2. **Install Dependencies:**

   ```bash
   dotnet restore
   ```

3. **Run the API:**

   ```bash
   dotnet run
   ```

4. **API Base URL:** `http://localhost:5208`
