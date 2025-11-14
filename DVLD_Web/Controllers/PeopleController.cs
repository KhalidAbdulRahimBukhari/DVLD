using Microsoft.AspNetCore.Mvc;
using System;
using DVLD_DTO;
using DVLD_Buisness;

namespace DVLD_Web.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/People")]
    public class PeopleConroller : ControllerBase
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<PersonDTO> AddNewPerson(PersonDTO NewPersonDTO)
        {
            // Validate required fields (everything except ImagePath, Email, ThirdName)
            if (string.IsNullOrWhiteSpace(NewPersonDTO.FirstName) ||
                string.IsNullOrWhiteSpace(NewPersonDTO.SecondName) ||
                string.IsNullOrWhiteSpace(NewPersonDTO.LastName) ||
                string.IsNullOrWhiteSpace(NewPersonDTO.NationalNo) ||
                NewPersonDTO.DateOfBirth == default ||
                string.IsNullOrWhiteSpace(NewPersonDTO.Address) ||
                string.IsNullOrWhiteSpace(NewPersonDTO.Phone) ||
                NewPersonDTO.NationalityCountryID < 1)
            {
                return BadRequest("All fields except ImagePath, Email, and ThirdName are required. and National No can`t be < 1");
            }

            // Validate Gendor
            if (NewPersonDTO.Gendor != 0 && NewPersonDTO.Gendor != 1)
                return BadRequest("Gendor must be either 0 (female) or 1 (male).");

            // Validate age and date of birth
            if (NewPersonDTO.DateOfBirth > DateTime.Today.AddYears(-18))
                return BadRequest("Date of birth must be at least 18 years ago.");

            clsPerson CheckNationalID = clsPerson.Find(NewPersonDTO.NationalNo);

            if (CheckNationalID != null)
                return BadRequest("National ID Alreay exisits");


            clsPerson person = new clsPerson(
            new PersonDTO(
            personID: NewPersonDTO.PersonID,
            firstName: NewPersonDTO.FirstName,
            secondName: NewPersonDTO.SecondName,
            thirdName: NewPersonDTO.ThirdName,
            lastName: NewPersonDTO.LastName,
            nationalNo: NewPersonDTO.NationalNo,
            dateOfBirth: NewPersonDTO.DateOfBirth,
            gendor: NewPersonDTO.Gendor,
            address: NewPersonDTO.Address,
            phone: NewPersonDTO.Phone,
            email: NewPersonDTO.Email,
            nationalityCountryID: NewPersonDTO.NationalityCountryID,
            imagePath: NewPersonDTO.ImagePath
        )
    );


            if (person.Save())
            {
                NewPersonDTO.PersonID = person.PersonID;
                return CreatedAtRoute("GetPersonInfoByID", new { id = NewPersonDTO.PersonID }, NewPersonDTO);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        [HttpPut("{id}", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDTO> UpdatePerson(int id, PersonDTO NewPersonDTO)
        {
            clsPerson person = clsPerson.Find(id);
            if (person == null)
                return NotFound();

            // Validate required fields (everything except ImagePath, Email, ThirdName)
            if (string.IsNullOrWhiteSpace(NewPersonDTO.FirstName) ||
                string.IsNullOrWhiteSpace(NewPersonDTO.SecondName) ||
                string.IsNullOrWhiteSpace(NewPersonDTO.LastName) ||
                string.IsNullOrWhiteSpace(NewPersonDTO.NationalNo) ||
                NewPersonDTO.DateOfBirth == default ||
                string.IsNullOrWhiteSpace(NewPersonDTO.Address) ||
                string.IsNullOrWhiteSpace(NewPersonDTO.Phone) ||
                NewPersonDTO.NationalityCountryID == 0)
            {
                return BadRequest("All fields except ImagePath, Email, and ThirdName are required.");
            }

            // Validate Gendor
            if (NewPersonDTO.Gendor != 0 && NewPersonDTO.Gendor != 1)
                return BadRequest("Gendor must be either 0 (female) or 1 (male).");

            // Validate age and date of birth
            if (NewPersonDTO.DateOfBirth > DateTime.Today.AddYears(-18))
                return BadRequest("Date of birth must be at least 18 years ago.");

            // Update fields
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
            person.ImagePath = NewPersonDTO.ImagePath;

            if (person.Save())
                return Ok(person.PersonDTO);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }


        [HttpDelete("{id}", Name = "DeletePersonByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult DeletePersonByID(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (clsPerson.DeletePerson(id))

                return Ok($"Person with ID {id} has been deleted.");
            else
                return NotFound($"Person with ID {id} Not Deleted (it has data linked to it !) Internal Error");
        }

    }


}

