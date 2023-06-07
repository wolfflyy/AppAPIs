# AppAPIs

use "Scaffold-DbContext -connection name=constring Microsoft.EntityFrameworkCore.SqlServer -OutputDir Repos/Models -context apiDBContext -contextDir Repos -DataAnnotations -f" command in the "Package Manager Console" in the Visual Studio 2022-2023 to perform modification on the current database model and database context on the project (Noted. This will automatically overrwrite the current model without the need to delete it yourselves).
