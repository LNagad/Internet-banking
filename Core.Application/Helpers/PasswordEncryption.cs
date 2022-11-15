using System.Security.Cryptography;
using System.Text;


namespace Core.Application.Helpers
{
    public class PasswordEncryption
    {
        public static string ComputeSha256Hash(string password)
        {
            //CREATE SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                //Convert bytes array to string

                StringBuilder builder = new();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
