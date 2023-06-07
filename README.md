# AppAPIs

use "Scaffold-DbContext -connection name=constring Microsoft.EntityFrameworkCore.SqlServer -OutputDir Repos/Models -context apiDBContext -contextDir Repos -DataAnnotations -f" command in the "Package Manager Console" in the Visual Studio 2022-2023 to perform modification on the current database model and database context on the project (Noted. This will automatically overrwrite the current model without the need to delete it yourselves).


Please update the string inside the <<string cookieValue = "">> to suit your login information (e.g. ASP.NET_SessionId=...; G_ENABLED_IDPS=google; G_AUTHUSER_H=0). IMPORTANT: this cookie is needed for the "GetScrapeResult" function to work as intended.
