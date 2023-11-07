# NewsSiteSolution
## About
* ASP.NET 7 Core MVC
* Razor Pages
* Entity Framework Core (Code-first)
* Local MS SQL / Azure SQL
* Clean architecture
## Features
* Authorization and authentification system using Identity Framework
* Role-based authentification
* Possibility to read and comment articles (commenting only for authorized users)
* Possibility to become an author (for now without any limitations)
* Possibility to create, edit and delete articles
* Client-side and server-side validations
* Exception Handler middleware (only for Release stage)
* Website deployed on Azure (link in description)
## Installation
1. Clone the repository:
```
git clone https://github.com/EnskWt/NewsSiteSolution.git
```
2. Specify environment (use Development for Developer Exception Page and local MS SQL database, use Release for Exception Handler middleware and hosted database)
3. Change connection string in appsettings.json and appsettings.Development.json
4. Create database using NuGet Package Manager Console:
```
Update-Database
```
5. Run or deploy project
