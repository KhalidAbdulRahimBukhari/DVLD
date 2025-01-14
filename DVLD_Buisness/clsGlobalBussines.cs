using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DVLD_Buisness
{
    public class clsGlobalBussines
    {

        public static string ComputeHash(string input)
        {

            using(SHA256 sha265 = SHA256.Create())
            {
                byte[] hashbytes = sha265.ComputeHash(Encoding.UTF8.GetBytes(input));

                return BitConverter.ToString(hashbytes).Replace("-", "").ToLower();
            }
        }

    }
}
