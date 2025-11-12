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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<PersonDisplayDTO>> GetALlPeople()
        {
            List<PersonDisplayDTO> people = clsPerson.GetAllPeople();

            if(people == null || people.Count == 0)
            {
                return NotFound("No people found.");
            }

            return Ok(people);
        }

    }


}
