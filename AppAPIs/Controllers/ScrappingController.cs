using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppAPIs.Repos.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AppAPIs.Controllers
{
    [ApiController]
    public class ScrappingController : ControllerBase
    {
        private readonly apiDBContext _apiDBContext;
        public ScrappingController(apiDBContext apiDBContext)
        {
            this._apiDBContext = apiDBContext;
        }

        private async Task SaveToDatabase(List<Dictionary<string, string>> tableData)
        {
            foreach (var rowData in tableData)
            {
                string slot = rowData["Slot"];

                // Iterate through the remaining keys in the rowData dictionary
                foreach (var kvp in rowData.Skip(1))
                {
                    string key = kvp.Key;
                    string value = kvp.Value;

                    // Check if the value is not empty
                    if (!string.IsNullOrEmpty(value))
                    {
                        // Extract the necessary information from the value
                        string[] parts = value.Split(new[] { " at ", " - " }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 3)
                        {
                            string classId = parts[0].Split('(')[0].Trim();
                            string subjectId = parts[0].Split('(')[1].TrimEnd(')');
                            string classroom = parts[1].Trim();
                            string lecturerName = parts[2].Trim();

                            // Extract the date from the key (e.g., "Tue(06/06)")
                            string dateString = key.Split('(')[1].TrimEnd(')');
                            DateTime date = DateTime.ParseExact(dateString, "MM/dd", CultureInfo.InvariantCulture);

                            // Retrieve the necessary entities from the database context
                            var @class = await _apiDBContext.Classes.FirstOrDefaultAsync(c => c.ClassId == classId);
                            var subject = await _apiDBContext.Subjects.FirstOrDefaultAsync(s => s.SubId == subjectId);
                            var classroomObj = await _apiDBContext.Classrooms.FirstOrDefaultAsync(cr => cr.Name == classroom);
                            var lecturer = await _apiDBContext.Lecturers.FirstOrDefaultAsync(l => l.Name == lecturerName);

                            if (@class == null)
                            {
                                // Class doesn't exist, create a new class entity
                                @class = new Class
                                {
                                    ClassId = classId,
                                    Name = classId // Set the name as classId for now, update with the actual name if available
                                };

                                _apiDBContext.Classes.Add(@class);
                            }

                            if (subject == null)
                            {
                                // Subject doesn't exist, create a new subject entity
                                subject = new Subject
                                {
                                    SubId = subjectId,
                                    Name = subjectId // Set the name as subjectId for now, update with the actual name if available
                                };

                                _apiDBContext.Subjects.Add(subject);
                            }

                            if (classroomObj == null)
                            {
                                // Classroom doesn't exist, create a new classroom entity
                                classroomObj = new Classroom
                                {
                                    Name = classroom,
                                    /*DepartmentId = 1*/ // Set the departmentId as 1 for now, update with the actual departmentId if available
                                };

                                _apiDBContext.Classrooms.Add(classroomObj);
                            }

                            if (lecturer == null)
                            {
                                // Lecturer doesn't exist, create a new lecturer entity
                                lecturer = new Lecturer
                                {
                                    Name = lecturerName
                                };

                                _apiDBContext.Lecturers.Add(lecturer);
                            }

                            // Create a new schedule entity and assign the relationships
                            var schedule = new Schedule
                            {
                                Class = @class,
                                Sub = subject,
                                Room = classroomObj,
                                Lecturer = lecturer,
                                Date = date,
                                // Set the StudentId as needed based on your business logic
                            };

                            _apiDBContext.Schedules.Add(schedule);
                        }
                    }
                }
            }

            await _apiDBContext.SaveChangesAsync();
        }



        [Route("GetScrapeResult")]
        [AcceptVerbs("GET")]
        public async Task<List<Dictionary<string, string>>> GetScrapeResult()
        {
            List<Dictionary<string, string>> tableData = new List<Dictionary<string, string>>();

            HttpClient hc = new HttpClient();

            // Add the cookie to the request headers
            string cookieValue = "ASP.NET_SessionId=c0l20fodpzwowdyptvkf0yj3; G_ENABLED_IDPS=google; G_AUTHUSER_H=0";
            hc.DefaultRequestHeaders.Add("Cookie", cookieValue);

            HttpResponseMessage result = await hc.GetAsync("https://fap.fpi.edu.vn/NewPage/WeeklyTimetable.aspx");
            Stream stream = await result.Content.ReadAsStreamAsync();
            HtmlDocument document = new HtmlDocument();
            document.Load(stream);

            var table = document.DocumentNode.SelectSingleNode("//table[@class='table table-bordered']");
            var rows = table.SelectNodes(".//tr");

            List<string> headers = new List<string>();
            List<string> dates = new List<string>();

            // Extract dates from the second row
            var dateCells = rows.ElementAtOrDefault(1)?.SelectNodes(".//th");
            if (dateCells != null)
            {
                for (int i = 0; i < dateCells.Count; i++)
                {
                    var dateCell = dateCells[i];
                    var dateText = dateCell.InnerText.Trim();

                    if (string.IsNullOrEmpty(dateText))
                    {
                        // Find the date value from the previous non-empty date cell
                        var previousDateCell = dateCells.Take(i).LastOrDefault(c => !string.IsNullOrEmpty(c.InnerText.Trim()));
                        if (previousDateCell != null)
                        {
                            var previousDateIndex = dateCells.IndexOf(previousDateCell);
                            var previousDate = dates[previousDateIndex];
                            dates.Add(previousDate);
                        }
                        else
                        {
                            dates.Add(string.Empty);
                        }
                    }
                    else
                    {
                        dates.Add(dateText);
                    }
                }
            }

            List<string> days = new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

            foreach (var row in rows)
            {
                var cells = row.SelectNodes(".//th|td");
                if (cells != null && cells.Count > 1)
                {
                    Dictionary<string, string> rowData = new Dictionary<string, string>();

                    var slotCell = cells.First();
                    var slotText = slotCell.InnerText.Trim();
                    rowData["Slot"] = slotText;

                    if (!slotText.Contains('/') && !days.Any(d => cells.Any(c => c.InnerText.Trim().StartsWith(d))))  // Exclude row with date values and days of the week
                    {
                        for (int i = 1; i < cells.Count; i++)
                        {
                            var cellText = cells[i].InnerText.Trim();
                            var day = days.ElementAtOrDefault(i - 1);
                            var date = dates.ElementAtOrDefault(i - 1);
                            rowData[day + "(" + date + ")"] = cellText;
                        }

                        if (rowData.Values.Any(value => !string.IsNullOrEmpty(value)))  // Exclude rows with all empty cells
                            tableData.Add(rowData);
                    }
                }
            }

            // Save the scraped data to the database
            await SaveToDatabase(tableData);

            return tableData;
        }
    }
}
