using System;
using System.Data;


namespace DVLD_DTO
{
    public class PersonDTO
    {
        public PersonDTO(int personID, string firstName, string secondName, string thirdName, string lastName,
            string nationalNo, DateTime dateOfBirth, short gendor, string address, string phone,
            string email, int nationalityCountryID, string imagePath)
        {
            this.PersonID = personID;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;
            this.NationalNo = nationalNo;
            this.DateOfBirth = dateOfBirth;
            this.Gendor = gendor;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.NationalityCountryID = nationalityCountryID;
            this.ImagePath = imagePath;
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

        private string _ImagePath;

        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }

    }

    // this DTo includes extra parameters : string GendorCaption and stirng CountryName (For display purposes)
    public class PersonDisplayDTO
    {
        public PersonDisplayDTO(int personID, string firstName, string secondName, string thirdName, string lastName,
            string nationalNo, DateTime dateOfBirth, short gendor, string address, string phone,
            string email, int nationalityCountryID, string imagePath , string GendorCaption , string CountryName)
        {
            this.PersonID = personID;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;
            this.NationalNo = nationalNo;
            this.DateOfBirth = dateOfBirth;
            this.Gendor = gendor;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.NationalityCountryID = nationalityCountryID;
            this.ImagePath = imagePath;
            this.GendorCaption = GendorCaption;
            this.CountryName = CountryName;
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

        private string _ImagePath;

        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }
        public string GendorCaption { get; set; } // Extra
        public string CountryName { get; set; } // Extra

    }
}

//private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
//                                                       "FirstName", "SecondName", "ThirdName", "LastName",
//                                                       "GendorCaption", "DateOfBirth", "CountryName",
//                                                       "Phone", "Email");