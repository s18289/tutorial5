using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs.Requests;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IStudentServiceDb
    {
        IActionResult EnrollStudent(EnrollStudentRequest request);
        IActionResult PromoteStudents(Enrollment enrollment);
    }
}