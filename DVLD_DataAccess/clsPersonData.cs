using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using DVLD_DTO;

namespace DVLD_DataAccess
{   
    public class clsPersonData
    {

        public static PersonDTO GetPersonInfoByID(int PersonID)
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
                                return new PersonDTO(
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

        public static PersonDTO GetPersonInfoByNationalNo(string nationalNo)
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
                                return new PersonDTO(
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

        public static int AddNewPerson(PersonDTO personDTO)
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

        public static bool UpdatePerson(PersonDTO personDTO)
        {
            int rowsAffected = 0;

            string query = @"
        UPDATE People
        SET FirstName = @FirstName,
            SecondName = @SecondName,
            ThirdName = @ThirdName,
            LastName = @LastName,
            NationalNo = @NationalNo,
            DateOfBirth = @DateOfBirth,
            Gendor = @Gendor,
            Address = @Address,
            Phone = @Phone,
            Email = @Email,
            NationalityCountryID = @NationalityCountryID,
            ImagePath = @ImagePath
        WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", personDTO.PersonID);
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
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobalData.LogError(ex);
                return false;
            }

            return (rowsAffected > 0);
        }

        public static List<PersonDisplayDTO> GetAllPeople()
        {
            List<PersonDisplayDTO> personList = new List<PersonDisplayDTO>();

            string query = @"
        SELECT 
            People.PersonID, 
            People.NationalNo,
            People.FirstName, 
            People.SecondName, 
            People.ThirdName, 
            People.LastName,
            People.DateOfBirth, 
            People.Gendor,  
            CASE WHEN People.Gendor = 0 THEN 'Male' ELSE 'Female' END AS GendorCaption,
            People.Address, 
            People.Phone, 
            People.Email, 
            People.NationalityCountryID, 
            Countries.CountryName, 
            People.ImagePath
        FROM People 
        INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID
        ORDER BY People.FirstName";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Handle nullables safely
                                string thirdName = reader["ThirdName"] != DBNull.Value ? Convert.ToString(reader["ThirdName"]) : "";
                                string email = reader["Email"] != DBNull.Value ? Convert.ToString(reader["Email"]) : "";
                                string imagePath = reader["ImagePath"] != DBNull.Value ? Convert.ToString(reader["ImagePath"]) : "";

                                // Create a new DTO from each record
                                PersonDisplayDTO person = new PersonDisplayDTO(
                                    Convert.ToInt32(reader["PersonID"]),
                                    Convert.ToString(reader["FirstName"]),
                                    Convert.ToString(reader["SecondName"]),
                                    thirdName,
                                    Convert.ToString(reader["LastName"]),
                                    Convert.ToString(reader["NationalNo"]),
                                    Convert.ToDateTime(reader["DateOfBirth"]),
                                    Convert.ToInt16(reader["Gendor"]), // safely converts byte/short
                                    Convert.ToString(reader["Address"]),
                                    Convert.ToString(reader["Phone"]),
                                    email,
                                    Convert.ToInt32(reader["NationalityCountryID"]),
                                    imagePath,
                                    Convert.ToString(reader["GendorCaption"]),
                                    Convert.ToString(reader["CountryName"])
                                );

                                personList.Add(person);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobalData.LogError(ex);
            }

            return personList;
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
