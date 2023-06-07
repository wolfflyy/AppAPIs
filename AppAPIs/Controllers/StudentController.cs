using AppAPIs.Repos;
using AppAPIs.Repos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using AppAPIs.Services;

namespace AppAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly apiDBContext _apiDBContext;
        public StudentController(apiDBContext apiDBContext)
        {
            this._apiDBContext = apiDBContext;
        }

        [HttpGet("GetAllStudents")]
        public ServiceResponse<List<Student>> GetAllStudents()
        {
            var response = new ServiceResponse<List<Student>>();
            try
            {
                var students = _apiDBContext.Students.Include(s => s.Schedules).ToList();
                response.Data = students;
                response.Success = true;
                response.Message = "Students retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to retrieve students: " + ex.Message;
            }

            return response;
        }

        [HttpGet("Get/{id}")]
        public ServiceResponse<Student> GetStudentById(int id)
        {
            var response = new ServiceResponse<Student>();
            try
            {
                var student = _apiDBContext.Students.Include(s => s.Schedules).SingleOrDefault(s => s.Id == id);

                if (student == null)
                {
                    response.Success = false;
                    response.Message = "Student not found.";
                }
                else
                {
                    response.Data = student;
                    response.Success = true;
                    response.Message = "Student retrieved successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to retrieve student: " + ex.Message;
            }

            return response;
        }

        [HttpPost("Add")]
        public ServiceResponse<Student> AddStudent([FromBody] Student student)
        {
            var response = new ServiceResponse<Student>();
            try
            {
                if (student == null)
                {
                    response.Success = false;
                    response.Message = "Invalid student data.";
                    return response;
                }

                // Add the student to the context
                _apiDBContext.Students.Add(student);

                // Create a new schedule for the student
                var schedule = new Schedule
                {
                    Student = student,
                    // Set other properties of the schedule as needed
                };
                _apiDBContext.Schedules.Add(schedule);

                _apiDBContext.SaveChanges();

                response.Data = student;
                response.Success = true;
                response.Message = "Student added successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to add student: " + ex.Message;
            }

            return response;
        }

        [HttpPut("Update/{id}")]
        public ServiceResponse<bool> UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var student = _apiDBContext.Students.Find(id);
                if (student == null)
                {
                    response.Success = false;
                    response.Message = "Student not found.";
                    return response;
                }

                student.Fullname = updatedStudent.Fullname;
                student.Mssv = updatedStudent.Mssv;
                student.DoB = updatedStudent.DoB;
                student.Email = updatedStudent.Email;
                student.Major = updatedStudent.Major;
                student.Course = updatedStudent.Course;

                // Update schedules if provided
                if (updatedStudent.Schedules != null && updatedStudent.Schedules.Any())
                {
                    // Remove existing schedules for the student
                    var existingSchedules = _apiDBContext.Schedules.Where(s => s.StudentId == id);
                    _apiDBContext.Schedules.RemoveRange(existingSchedules);

                    // Add updated schedules
                    foreach (var schedule in updatedStudent.Schedules)
                    {
                        schedule.StudentId = id;
                        _apiDBContext.Schedules.Add(schedule);
                    }
                }

                _apiDBContext.SaveChanges();

                response.Data = true;
                response.Success = true;
                response.Message = "Student updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to update student: " + ex.Message;
            }

            return response;
        }

        [HttpDelete("Delete/{id}")]
        public ServiceResponse<bool> DeleteStudent(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var student = _apiDBContext.Students.Find(id);
                if (student == null)
                {
                    response.Success = false;
                    response.Message = "Student not found.";
                    return response;
                }

                // Remove associated schedules
                var schedules = _apiDBContext.Schedules.Where(s => s.StudentId == id);
                _apiDBContext.Schedules.RemoveRange(schedules);

                _apiDBContext.Students.Remove(student);
                _apiDBContext.SaveChanges();

                response.Data = true;
                response.Success = true;
                response.Message = "Student deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to delete student: " + ex.Message;
            }

            return response;
        }
    }
}
