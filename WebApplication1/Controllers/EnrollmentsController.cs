using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs.Requests;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/enrollment")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {

        private IStudentServiceDb _service;

        public EnrollmentsController(IStudentServiceDb service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            return _service.EnrollStudent(request);
        }

        [HttpPost("promote")]
        public IActionResult PromoteStudents()
        {
            return Ok();
        }
    }
}