using Domain.ReturnsModel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Server_Side.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Server_Side.Services.Classes
{

    public class EncreptionService : IEncreptionService
    {
        private readonly string IV = "qwd";
        private readonly string Passowrd = "55404";
        private readonly string Salt = "651كهيغعهلس";
        private string password_1 = "Look Its YoFi And He Is Aaying !#25#%$^";
        private string password_2 = "Hah How That Is Impossible Fu_wqu@#5$#487";
        private string password_3 = "No Its Him D: no waAaay";
        private byte[] salt_password_1;
        private byte[] salt_password_2;
        private byte[] salt_password_3;
        public EncreptionService()
        {

            this.salt_password_1 = Encoding.ASCII.GetBytes(password_1);
            this.salt_password_2 = Encoding.ASCII.GetBytes(password_2);
            this.salt_password_3 = Encoding.ASCII.GetBytes(password_3);

        }
        private byte[] Get_16_Byte_From(string key)
        {

            byte[] salt = Encoding.Unicode.GetBytes(Salt);
            var iterations = 1000;
            var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.Unicode.GetBytes(key),
                    salt,
                    iterations,
                    hashMethod,
                    desiredKeyLength);
        }
        public bool Compare_Hash(string word, string hash)
        {
            string final_1 = Hash_Prf(word, KeyDerivationPrf.HMACSHA512, salt_password_1);
            string final_2 = Hash_Prf(final_1, KeyDerivationPrf.HMACSHA256, salt_password_2);
            string final_3 = Hash_Prf(final_2, KeyDerivationPrf.HMACSHA512, salt_password_3);
            return final_3 == hash;
        }

        public EncinreptionpoeRes<string> De_Code(string words)
        {
            try
            {
                async Task<string> DecryptAsync()
                {

                    var getBytesFromWords = Convert.FromBase64String(words);
                    words = Encoding.UTF8.GetString(getBytesFromWords);
                    List<byte> wordsBytes = new List<byte>();
                    foreach (string str_byte in words.Split("-"))
                    {
                        wordsBytes.Add(Convert.ToByte(str_byte, 16));
                    }
                    using Aes aes = Aes.Create();
                    aes.Key = Get_16_Byte_From(Passowrd);
                    aes.IV = Get_16_Byte_From(IV);
                    using MemoryStream input = new(wordsBytes.ToArray());
                    using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
                    using MemoryStream output = new();
                    await cryptoStream.CopyToAsync(output);
                    return Encoding.Unicode.GetString(output.ToArray());
                }
                string final = DecryptAsync().GetAwaiter().GetResult();
                return new EncinreptionpoeRes<string>()
                {
                    Enc_Value = final,
                    IsOk = true,
                    Msg = "Ok"
                };
            }
            catch (Exception err)
            {
                return new EncinreptionpoeRes<string>()
                {
                    Enc_Value = null,
                    IsOk = false,
                    Msg = err.Message
                };
            }
        }

        public EncinreptionpoeRes<string> En_Code(string words)
        {
            try
            {
                async Task<byte[]> EncryptAsync()
                {
                    using Aes aes = Aes.Create();
                    aes.Key = Get_16_Byte_From(Passowrd);
                    aes.IV = Get_16_Byte_From(IV);
                    using MemoryStream output = new();
                    using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
                    await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(words));
                    await cryptoStream.FlushFinalBlockAsync();
                    return output.ToArray();
                }
                byte[] encodedBytes = EncryptAsync().GetAwaiter().GetResult();
                string final = BitConverter.ToString(encodedBytes);
                byte[] finalByte = Encoding.UTF8.GetBytes(final);
                return new EncinreptionpoeRes<string>()
                {
                    Enc_Value = Convert.ToBase64String(finalByte),
                    IsOk = true,
                    Msg = "Ok"
                };
            }
            catch (Exception err)
            {
                return new EncinreptionpoeRes<string>()
                {
                    Enc_Value = null,
                    IsOk = false,
                    Msg = err.Message
                };
            }
        }

        public string Hash(string word)
        {
            string final_1 = Hash_Prf(word, KeyDerivationPrf.HMACSHA512, salt_password_1);
            string final_2 = Hash_Prf(final_1, KeyDerivationPrf.HMACSHA256, salt_password_2);
            string final_3 = Hash_Prf(final_2, KeyDerivationPrf.HMACSHA512, salt_password_3);
            return final_3;
        }

        private string Hash_Prf(string word, KeyDerivationPrf prf, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: word!,
                salt: salt,
                prf: prf,
                iterationCount: 100000,
                numBytesRequested: 256 / 8)
            );
            return hashed;
        }
    }
}
