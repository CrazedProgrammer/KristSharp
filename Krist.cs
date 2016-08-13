using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Globalization;

namespace Crazed.KristSharp
{
    public static class Krist
    {
        public static void Initialise(int timeoutmilliseconds = 2000, HttpClient client = null, string server = "krist.ceriat.net")
        {
            KristUtils.Client = (client == null) ? new HttpClient() : client;
            KristUtils.Client.Timeout = TimeSpan.FromMilliseconds(timeoutmilliseconds);
            KristUtils.ServerUrl = "http://" + server + "/";
        }

        public static async Task<KristAddress[]> GetAllAddresses(int limit = 50, int offset = 0)
        {
            KAddressesResult result = await KristUtils.GET<KAddressesResult>("addresses?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            KristAddress[] addresses = new KristAddress[result.addresses.Length];
            for (int i = 0; i < result.addresses.Length; i++)
            {
                addresses[i] = new KristAddress(result.addresses[i]);
            }
            return addresses;
        }

        public static async Task<KristAddress[]> GetRichestAddresses(int limit = 50, int offset = 0)
        {
            KAddressesResult result = await KristUtils.GET<KAddressesResult>("addresses/rich?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            KristAddress[] addresses = new KristAddress[result.addresses.Length];
            for (int i = 0; i < result.addresses.Length; i++)
            {
                addresses[i] = new KristAddress(result.addresses[i]);
            }
            return addresses;
        }
    }
}
