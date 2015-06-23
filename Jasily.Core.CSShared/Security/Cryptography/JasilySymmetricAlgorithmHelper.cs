using System.Collections.Generic;
using System.Linq;

namespace System.Security.Cryptography
{
    public static class JasilySymmetricAlgorithmHelper
    {
        public static IEnumerable<int> TestValidKeySize(this SymmetricAlgorithm algorithm, int min, int max)
        {
            return Enumerable.Range(min, max - min).Where(algorithm.ValidKeySize);
        }

        public static byte[] Decrypt(this SymmetricAlgorithm algorithm, byte[] inputBuffer, int inputOffset, int inputCount)
        {
            using (var decryptor = algorithm.CreateDecryptor())
            {
                return decryptor.TransformFinalBlock(inputBuffer, inputOffset, inputCount);
            }
        }

        public static byte[] Decrypt(this SymmetricAlgorithm algorithm, byte[] inputBuffer)
        {
            using (var decryptor = algorithm.CreateDecryptor())
            {
                return decryptor.TransformFinalBlock(inputBuffer);
            }
        }

        public static byte[] Encrypt(this SymmetricAlgorithm algorithm, byte[] inputBuffer, int inputOffset, int inputCount)
        {
            using (var encryptor = algorithm.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(inputBuffer, inputOffset, inputCount);
            }
        }

        public static byte[] Encrypt(this SymmetricAlgorithm algorithm, byte[] inputBuffer)
        {
            using (var encryptor = algorithm.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(inputBuffer);
            }
        }
    }
}