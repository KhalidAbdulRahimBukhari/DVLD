using Microsoft.AspNetCore.Mvc;
using System;
using DVLD_DTO;
using DVLD_Buisness;

namespace DVLD_Web.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/People")]
    public class PersonConroller : ControllerBase
    {
        [HttpGet("All", Name = "GetAllPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<PersonDisplayDTO>> GetALlPeople()
        {
            List<PersonDisplayDTO> people = clsPerson.GetAllPeople();

            if (people == null || people.Count == 0)
            {
                return NotFound("No people found.");
            }

            return Ok(people);
        }


        [HttpGet("{id}", Name = "GetPersonInfoByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<PersonDTO> GetPersonByID(int id)
        {
            clsPerson person = clsPerson.Find(id);
            if (person == null)
            {
                return NotFound($"Person with ID {id} not found.");
            }
            return Ok(person.PersonDTO);
        }

        [HttpGet("nationalno/{NationalNo}", Name = "GetPersonInfoByNationalNo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<PersonDTO> GetPersonByNationalNo(string NationalNo)
        {
            clsPerson person = clsPerson.Find(NationalNo);
            if (person == null)
            {
                return NotFound($"Person with ID {NationalNo} not found.");
            }
            return Ok(person.PersonDTO);
        }

        [HttpPost("AddNew", Name = "AddNewPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<PersonDTO> AddNerPerson(PersonDTO NewPersonDTO)
        {
            clsPerson person = new clsPerson();
            person.FirstName = NewPersonDTO.FirstName;
            person.SecondName = NewPersonDTO.SecondName;
            person.ThirdName = NewPersonDTO.ThirdName;
            person.LastName = NewPersonDTO.LastName;
            person.NationalNo = NewPersonDTO.NationalNo;
            person.DateOfBirth = NewPersonDTO.DateOfBirth;
            person.Gendor = NewPersonDTO.Gendor;
            person.Address = NewPersonDTO.Address;
            person.Phone = NewPersonDTO.Phone;
            person.Email = NewPersonDTO.Email;
            person.NationalityCountryID = NewPersonDTO.NationalityCountryID;

            if(person.Save())
            {
                NewPersonDTO.PersonID = person.PersonID;
                return Ok(person.PersonDTO);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


    }





}
