using Microsoft.AspNetCore.Mvc;
using System;
using DVLD_DTO;
using DVLD_Buisness;

namespace DVLD_Web.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/Person")]
    public class PersonConroller : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PersonDisplayDTO>> GetALlPeople()
        {
            List<PersonDisplayDTO> people = clsPerson.GetAllPeople();

            return Ok(people);
        }

    }
}
