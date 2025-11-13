using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using DVLD_DTO;


namespace DVLD_Buisness
{
    public  class clsPerson
    {
        private enum enMode { AddNew = 0, Update = 1 };

        private enMode Mode = enMode.AddNew;

        public PersonDTO PersonDTO
        {
            get
            {
                return new PersonDTO(this.PersonID,this.FirstName,this.SecondName,this.ThirdName,
                    this.LastName,this.NationalNo,this.DateOfBirth,this.Gendor,this.Address,
                    this.Phone,this.Email,this.NationalityCountryID,this.ImagePath
                );
            }
        }
        public int PersonID { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }
        public string NationalNo { set; get; }
        public DateTime DateOfBirth { set; get; }
        public short Gendor { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }

        public clsCountry CountryInfo;

        private string _ImagePath;
      
        public string ImagePath   
        {
            get { return _ImagePath; }   
            set { _ImagePath = value; }  
        }

        public clsPerson()

        {
            this.PersonID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            // Gendor was no written
            this.Gendor = -1;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";

            Mode = enMode.AddNew;
        }

        public clsPerson(PersonDTO personDTO)
        {
            this.PersonID = personDTO.PersonID;
            this.FirstName = personDTO.FirstName;
            this.SecondName = personDTO.SecondName;
            this.ThirdName = personDTO.ThirdName;
            this.LastName = personDTO.LastName;
            this.NationalNo = personDTO.NationalNo;
            this.DateOfBirth = personDTO.DateOfBirth;
            this.Gendor = personDTO.Gendor;
            this.Address = personDTO.Address;
            this.Phone = personDTO.Phone;
            this.Email = personDTO.Email;
            this.NationalityCountryID = personDTO.NationalityCountryID;
            this.ImagePath = personDTO.ImagePath;
            this.CountryInfo = clsCountry.Find(personDTO.NationalityCountryID);
            //this.Mode = enMode.AddNew;// No need to assign it it`s already AddNew unless explicitlychanged in Find Method
        }


        private bool _AddNewPerson()
        {

            this.PersonID = clsPersonData.AddNewPerson(this.PersonDTO);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            //call DataAccess Layer 

            return clsPersonData.UpdatePerson(this.PersonDTO);
        }

        public static clsPerson Find(int PersonID)
        {

            PersonDTO PerosnDTO = clsPersonData.GetPersonInfoByID(PersonID);

            if( PerosnDTO != null )
            {
                var newPerson =  new clsPerson(PerosnDTO);
                newPerson.Mode = enMode.Update;
                return newPerson;
            }
            return null;

        }

        public static clsPerson Find(string NationalNo)
        {
            PersonDTO PerosnDTO = clsPersonData.GetPersonInfoByNationalNo(NationalNo);

            if (PerosnDTO != null)
            {
                var newPerson = new clsPerson(PerosnDTO);
                newPerson.Mode = enMode.Update;
                return newPerson;
            }
            return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();

            }

            return false;
        }

        public static List<PersonDisplayDTO> GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID); 
        }

        public static bool isPersonExist(int ID)
        {
           return clsPersonData.IsPersonExist(ID);
        }

        public static bool isPersonExist(string NationlNo)
        {
            return clsPersonData.IsPersonExist(NationlNo);
        }

    }
}
