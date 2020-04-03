using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult getStudents()
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("entries/{id}")]
        public IActionResult getStudentsEntries(int id)
        {
            return Ok(_dbService.GetStudentsEntries(id));
        }

        [HttpGet("{id}")]
        public IActionResult getStudent(int id) 
        {
            if (id == 1)
            {
                return Ok ("Katarzyna");
            }
            else if (id == 2)
            {
                return Ok ("Jan");
            }
            return NotFound("Student was not found");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult getUpdate(int id)
        {
            return Ok("Update complete");
        }

        [HttpDelete("{id}")]
        public IActionResult getDelete(int id)
        {
            return Ok("Delete complete");
        }
    }
}