using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aky.EmailService.Controllers
{
    [Produces("application/json")]
    [Route("/")]
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult index()
        {
            return this.Ok("ok");
        }
    }
}