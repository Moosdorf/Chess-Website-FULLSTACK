using System.Security.Cryptography;

namespace DataLayer.HelperMethods
{
    public class Hashing
    {
        protected const int saltBitSize = 64;
        protected const byte saltByteSize = saltBitSize / 8;

        protected RandomNumberGenerator random = RandomNumberGenerator.Create();

        public (string hash, string salt) Hash(string password)
        {
            byte[] salt = new byte[saltByteSize]; // Create a byte array for the salt
            random.GetBytes(salt); // Generate a random salt

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); 

            return (hash: Convert.ToHexString(hash), salt: Convert.ToHexString(salt)); 
        }
        // <summary> Verify a password against a stored hash and salt </summary>
        public bool Verify(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromHexString(storedSalt); // Convert the stored salt from hex to byte array
            //  (Password-Based Key Derivation Function 2)
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256); // will iterate 100,000 times to derive the key, 32 bytes long, using SHA256
            byte[] computedHash = pbkdf2.GetBytes(32); // Generate the hash from the password and salt

            return Convert.ToHexString(computedHash) == storedHash; // Compare the computed hash with the stored hash
        }
    }
}
