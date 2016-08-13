using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Crazed.KristSharp
{
    static class KristUtils
    {
        public static HttpClient Client;
        public static string ServerUrl;
        
        private static async Task<string> HttpGET(string path)
        {
            HttpResponseMessage message = await Client.GetAsync(ServerUrl + path);
            return await message.Content.ReadAsStringAsync();
        }

        private static async Task<string> HttpPOST(string path, Dictionary<string, string> table)
        {
            var postcontent = new FormUrlEncodedContent(table);
            HttpResponseMessage message = await Client.PostAsync(ServerUrl + path, postcontent);
            return await message.Content.ReadAsStringAsync();
        }

        public static async Task<T> GET<T>(string path) where T : KResult
        {
            string content = await HttpGET(path);
            T result = JsonConvert.DeserializeObject<T>(content);
            if (!result.ok)
            {
                ThrowKristError(JsonConvert.DeserializeObject<Dictionary<string, string>>(content));
            }
            return result;
        }

        public static async Task<T> POST<T>(string path, Dictionary<string, string> table) where T : KResult
        {
            string content = await HttpPOST(path, table);
            T result = JsonConvert.DeserializeObject<T>(content);
            if (!result.ok)
            {
                ThrowKristError(JsonConvert.DeserializeObject<Dictionary<string, string>>(content));
            }
            return result;
        }

        private static void ThrowKristError(Dictionary<string, string> apierror)
        {
            if (apierror.ContainsKey("error"))
            {
                string error = apierror["error"];
                if (error == "invalid_parameter")
                {
                    string parameter = apierror["parameter"];
                    if (parameter == "address")
                        throw new KristInvalidAddressException(apierror);
                    if (parameter == "nonce")
                        throw new KristInvalidNonceException(apierror);
                    if (parameter == "name")
                        throw new KristInvalidNameException(apierror);
                }
                if (error == "address_not_found")
                    throw new KristAddressNotFoundException(apierror);
                if (error == "insufficient_funds")
                    throw new KristInsufficientFundsException(apierror);
                if (error == "name_taken")
                    throw new KristNameTakenException(apierror);
                if (error == "name_not_found")
                    throw new KristNameNotFoundException(apierror);
                if (error == "not_name_owner")
                    throw new KristNotNameOwnerException(apierror);
            }
            throw new KristErrorException(apierror);
        }

        // Krist private key and password generation
        private static SHA256Managed Hasher = new SHA256Managed();

        public static string Sha256(string key)
        {
            byte[] hash = Hasher.ComputeHash(Encoding.ASCII.GetBytes(key));
            StringBuilder sb = new StringBuilder(64);
            for (int i = 0; i < 32; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        private static char NumToChar(int num)
        {
            int index = num / 7;
            if (index < 10)
            {
                return (char)(index + 48);
            }
            else if (index < 36)
            {
                return (char)(index + 87);
            }
            else
            {
                return 'e';
            }
        }

        public static string GeneratePrivateKey(string walletpassword)
        {
            return Sha256("KRISTWALLET" + walletpassword) + "-000";
        }

        public static string GenerateV2Address(string privatekey)
        {
            string[] chars = new string[] { "", "", "", "", "", "", "", "", "" };
            StringBuilder prefix = new StringBuilder("k");
            string hash = Sha256(Sha256(privatekey));
            for (int i = 0; i < 9; i++)
            {
                chars[i] = hash.Substring(0, 2);
                hash = Sha256(Sha256(hash));
            }
            for (int i = 0; i < 9;)
            {
                int index = Convert.ToInt32(hash.Substring(i * 2, 2), 16) % 9;

                if (chars[index] == "")
                {
                    hash = Sha256(hash);
                }
                else
                {
                    prefix.Append(NumToChar(Convert.ToInt32(chars[index], 16)));
                    chars[index] = "";
                    i++;
                }
            }

            return prefix.ToString();
        }
    }
}
