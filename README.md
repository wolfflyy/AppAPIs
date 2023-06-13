# AppAPIs

use "Scaffold-DbContext -connection name=constring Microsoft.EntityFrameworkCore.SqlServer -OutputDir Repos/Models -context apiDBContext -contextDir Repos -DataAnnotations -f" command in the "Package Manager Console" in the Visual Studio 2022-2023 to perform modification on the current database model and database context on the project (Noted. This will automatically overrwrite the current model without the need to delete it yourselves).

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/41faaba4-0f9d-4483-8e19-e2a85b4b6746)

How to access "Package Manager Console" ? Simply choose the "Tools" on the top menu, Hover on the "NuGet Package Manager" and choose "Package Manager Console"

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/b85e80da-47a3-4444-8b1f-5d73da8b9130)

Please update the string inside the (string cookieValue = "") to suit your login information (e.g. ASP.NET_SessionId=...; G_ENABLED_IDPS=google; G_AUTHUSER_H=0). IMPORTANT: this cookie is needed for the "GetScrapeResult" function to work as intended.

Here is where "string cookieValue" is:

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/2670c05d-7919-40c5-9d02-616fc9f38e74)

Where to get the cookie? Simply go to your/someone else FAP.FPI.EDU.VN website, specifically the "Weekly Timetable" tab:

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/a4be25c8-b15b-4c70-9ea7-180d508896b1)

Now we are here, how do we get the cookie? Once you are in the "Weekly Timetable" tab, right click on the timetable, look down and choose the "inspect" option:

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/f8ad1672-4c38-4820-b07b-5dbf4c5fd4d4)

After you have clicked on the "inspect" option, you will be greeted with this window:

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/0e4919b0-83bf-4124-9355-d99cc35d8c50)

Once here, what you need to look for is the "Network" tab of the browser inspect:

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/5b734eec-6af0-42fb-9742-a45e0f189430)

Click on this and you will see the network log:

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/a957a0fd-7145-4666-9800-d7752e9fc492)

The network log will be empty at first. To populate the log, simply click on the page refresh while you are still on the network inspect. Once done, the log will now be successfully populated:

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/fc2bbec0-9d71-464f-b21d-a82baba3bdb2)

As you can see, there are a lot to see here. But what you need is the "WeeklyTimetable.aspx" file of the log. Click on that:

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/21451b5a-28a6-4644-880d-651b1ada00c7)

You will be redirected to the "Response" tab of this file. But this isn't what we need. Now, at the same menu as  the "Response" tab, look to the left you will see the "Headers" option. Click on that:

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/728dcd89-616e-4962-b846-867d75e48fa4)

Once there, you will see a lot of things. But what you need here is the cookie from the "Request Headers" section:

![image](https://github.com/wolfflyy/AppAPIs/assets/90817807/8d957ae7-905b-4f50-8114-b8809fd109c9)

Each individual will have different cookie string. I have censored mine to avoid any complicated situation. This cookie is what you need to input to the (string cookieValue="").

There is a "StudentSchedule.bak" file uploaded here. This is a database backup file, import this file to your SQL SERVER EXPRESS. REMEMBER to change the connection string configuration inside the "appsettings.json" of the project to better suit your situation.
