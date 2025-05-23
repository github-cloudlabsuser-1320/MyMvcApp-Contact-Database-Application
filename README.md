# MyMvcApp-Contact-Database-Application

MyMvcApp-Contact-Database-Application is an ASP.NET Core MVC web application for managing a contact database. It allows users to create, view, edit, search, and delete contact records through a modern web interface.

## Features
- Add, edit, and delete contacts
- Search contacts by name or other fields
- View detailed information for each contact
- Responsive UI using Bootstrap
- MVC architecture for maintainability
- Unit tests for controllers

## Project Structure
- `Controllers/` – MVC controllers (e.g., `UserController.cs`)
- `Models/` – Data models (e.g., `User.cs`, `ErrorViewModel.cs`)
- `Views/` – Razor views for UI (e.g., `User/Index.cshtml`, `User/Create.cshtml`)
- `wwwroot/` – Static files (CSS, JS, images)
- `MyMvcApp.Tests/` – Unit tests
- `Program.cs` – Application entry point
- `appsettings.json` – Configuration

## Getting Started
### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- (Optional) Visual Studio 2022 or VS Code

### Build and Run Locally
1. Clone the repository:
   ```powershell
   git clone <your-repo-url>
   cd MyMvcApp-Contact-Database-Application
   ```
2. Restore dependencies:
   ```powershell
   dotnet restore
   ```
3. Build the project:
   ```powershell
   dotnet build
   ```
4. Run the application:
   ```powershell
   dotnet run
   ```
5. Open your browser and navigate to `https://localhost:5001` or the URL shown in the terminal.

### Running Tests
```powershell
cd MyMvcApp.Tests
 dotnet test
```

## Deployment Guide
### Deploying to Azure App Service
1. Publish the app:
   ```powershell
   dotnet publish -c Release -o ./publish
   ```
2. Deploy the contents of the `./publish` folder to your Azure App Service using Azure CLI, Visual Studio, or GitHub Actions.
3. Update `appsettings.json` and `appsettings.Production.json` as needed for production settings (e.g., connection strings).

### Environment Variables
- Set environment variables for sensitive data (e.g., connection strings) in Azure portal or your deployment environment.

## CI/CD with GitHub Actions
This project includes a sample GitHub Actions workflow for building, testing, and deploying the app to Azure App Service.

### Example Workflow: `.github/workflows/dotnet.yml`
```yaml
name: Build and Deploy ASP.NET Core App

on:
  push:
    branches: [ "master" ]
  pull_request:
    branches: [ "master" ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore --configuration Release
      - name: Test
        run: dotnet test --no-build --verbosity normal
      - name: Publish
        run: dotnet publish -c Release -o ./publish
      - name: Deploy to Azure Web App
        uses: azure/webapps-deploy@v3
        with:
          app-name: <YOUR_AZURE_APP_NAME>
          publish-profile: ${{ secrets.AZUREAPPSERVICE_PUBLISHPROFILE }}
          package: ./publish
```

#### Steps to Use the Workflow
1. Create your Azure App Service and download its publish profile.
2. Add the publish profile as a GitHub secret named `AZUREAPPSERVICE_PUBLISHPROFILE`.
3. Replace `<YOUR_AZURE_APP_NAME>` in the workflow with your Azure App Service name.
4. Commit the workflow file to `.github/workflows/dotnet.yml`.

## License
This project is licensed under the MIT License.
