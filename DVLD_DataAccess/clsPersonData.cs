using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsPersonDTO
    {
        public clsPersonDTO(int personID, string firstName, string secondName, string thirdName, string lastName,
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

    public class clsPersonData
    {

        public static clsPersonDTO GetPersonInfoByID(int PersonID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SELECT * FROM People WHERE PersonID = @PersonID", connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Read each value and handle nulls
                                string thirdName = reader["ThirdName"] != DBNull.Value ? Convert.ToString(reader["ThirdName"]) : "";
                                string email = reader["Email"] != DBNull.Value ? Convert.ToString(reader["Email"]) : "";
                                string imagePath = reader["ImagePath"] != DBNull.Value ? Convert.ToString(reader["ImagePath"]) : "";

                                // Create and return a new PersonDTO
                                return new clsPersonDTO(
                                    Convert.ToInt32(reader["PersonID"]),
                                    Convert.ToString(reader["FirstName"]),
                                    Convert.ToString(reader["SecondName"]),
                                    thirdName,
                                    Convert.ToString(reader["LastName"]),
                                    Convert.ToString(reader["NationalNo"]),
                                    Convert.ToDateTime(reader["DateOfBirth"]),
                                    Convert.ToInt16(reader["Gendor"]), // safely converts byte/short to short
                                    Convert.ToString(reader["Address"]),
                                    Convert.ToString(reader["Phone"]),
                                    email,
                                    Convert.ToInt32(reader["NationalityCountryID"]),
                                    imagePath
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobalData.LogError(ex);
            }

            // Return null if no record was found
            return null;
        }

        public static clsPersonDTO GetPersonInfoByNationalNo(string nationalNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SELECT * FROM People WHERE NationalNo = @NationalNo", connection))
                    {
                        command.Parameters.AddWithValue("@NationalNo", nationalNo);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Read each value and handle nulls
                                string thirdName = reader["ThirdName"] != DBNull.Value ? Convert.ToString(reader["ThirdName"]) : "";
                                string email = reader["Email"] != DBNull.Value ? Convert.ToString(reader["Email"]) : "";
                                string imagePath = reader["ImagePath"] != DBNull.Value ? Convert.ToString(reader["ImagePath"]) : "";

                                // Create and return a new PersonDTO
                                return new clsPersonDTO(
                                    Convert.ToInt32(reader["PersonID"]),
                                    Convert.ToString(reader["FirstName"]),
                                    Convert.ToString(reader["SecondName"]),
                                    thirdName,
                                    Convert.ToString(reader["LastName"]),
                                    Convert.ToString(reader["NationalNo"]),
                                    Convert.ToDateTime(reader["DateOfBirth"]),
                                    Convert.ToInt16(reader["Gendor"]), // safely converts byte/short to short
                                    Convert.ToString(reader["Address"]),
                                    Convert.ToString(reader["Phone"]),
                                    email,
                                    Convert.ToInt32(reader["NationalityCountryID"]),
                                    imagePath
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobalData.LogError(ex);
            }

            // Return null if no record was found
            return null;
        }

        public static int AddNewPerson(clsPersonDTO personDTO)
        {
            // this function will return the new person id if succeeded and -1 if not.

            string query = @"
        INSERT INTO People 
            (FirstName, SecondName, ThirdName, LastName, NationalNo, 
             DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
        VALUES 
            (@FirstName, @SecondName, @ThirdName, @LastName, @NationalNo, 
             @DateOfBirth, @Gendor, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
        SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", personDTO.FirstName);
                        command.Parameters.AddWithValue("@SecondName", personDTO.SecondName);

                        if (!string.IsNullOrEmpty(personDTO.ThirdName))
                            command.Parameters.AddWithValue("@ThirdName", personDTO.ThirdName);
                        else
                            command.Parameters.AddWithValue("@ThirdName", DBNull.Value);

                        command.Parameters.AddWithValue("@LastName", personDTO.LastName);
                        command.Parameters.AddWithValue("@NationalNo", personDTO.NationalNo);
                        command.Parameters.AddWithValue("@DateOfBirth", personDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@Gendor", personDTO.Gendor);
                        command.Parameters.AddWithValue("@Address", personDTO.Address);
                        command.Parameters.AddWithValue("@Phone", personDTO.Phone);

                        if (!string.IsNullOrEmpty(personDTO.Email))
                            command.Parameters.AddWithValue("@Email", personDTO.Email);
                        else
                            command.Parameters.AddWithValue("@Email", DBNull.Value);

                        command.Parameters.AddWithValue("@NationalityCountryID", personDTO.NationalityCountryID);

                        if (!string.IsNullOrEmpty(personDTO.ImagePath))
                            command.Parameters.AddWithValue("@ImagePath", personDTO.ImagePath);
                        else
                            command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            personDTO.PersonID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobalData.LogError(ex);
            }

            return personDTO.PersonID;
        }

        public static bool UpdatePerson(int PersonID,  string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gendor, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  People  
                            set FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName, 
                                NationalNo = @NationalNo,
                                DateOfBirth = @DateOfBirth,
                                Gendor=@Gendor,
                                Address = @Address,  
                                Phone = @Phone,
                                Email = @Email, 
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath =@ImagePath
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

          
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "" && Email != null)
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsGlobalData.LogError(ex);
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


        public static DataTable GetAllPeople()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = 
              @"SELECT People.PersonID, People.NationalNo,
              People.FirstName, People.SecondName, People.ThirdName, People.LastName,
			  People.DateOfBirth, People.Gendor,  
				  CASE
                  WHEN People.Gendor = 0 THEN 'Male'

                  ELSE 'Female'

                  END as GendorCaption ,
			  People.Address, People.Phone, People.Email, 
              People.NationalityCountryID, Countries.CountryName, People.ImagePath
              FROM            People INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID
                ORDER BY People.FirstName";




            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                clsGlobalData.LogError(ex);
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }

        public static bool DeletePerson(int PersonID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete People 
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsGlobalData.LogError(ex);
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                clsGlobalData.LogError(ex);
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                clsGlobalData.LogError(ex);
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


    }
}
