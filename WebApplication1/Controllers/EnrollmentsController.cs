using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/enrollment")]
    public class EnrollmentsController : Controller
    {
        private IDbService _dbService;

        public EnrollmentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        
    }
}