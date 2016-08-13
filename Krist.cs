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

        // Address functions

        public static async Task<KristAddress[]> GetAllAddresses(int limit = 50, int offset = 0)
        {
            var result = await KristUtils.GET<KAddressesResult>("addresses?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            var addresses = new KristAddress[result.addresses.Length];
            for (int i = 0; i < result.addresses.Length; i++)
            {
                addresses[i] = new KristAddress(result.addresses[i]);
            }
            return addresses;
        }

        public static async Task<KristAddress[]> GetRichestAddresses(int limit = 50, int offset = 0)
        {
            var result = await KristUtils.GET<KAddressesResult>("addresses/rich?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            var addresses = new KristAddress[result.addresses.Length];
            for (int i = 0; i < result.addresses.Length; i++)
            {
                addresses[i] = new KristAddress(result.addresses[i]);
            }
            return addresses;
        }

        // Transaction functions

        public static async Task<KristTransaction[]> GetAllTransactions(int limit = 50, int offset = 0, bool excludemined = false)
        {
            var result = await KristUtils.GET<KTransactionsResult>("transactions?limit=" + limit.ToString() + "&offset=" + offset.ToString() + (excludemined ? "&excludeMined" : ""));
            var transactions = new KristTransaction[result.transactions.Length];
            for (int i = 0; i < transactions.Length; i++)
            {
                transactions[i] = new KristTransaction(result.transactions[i]);
            }
            return transactions;
        }

        public static async Task<KristTransaction[]> GetLatestTransactions(int limit = 50, int offset = 0, bool excludemined = false)
        {
            var result = await KristUtils.GET<KTransactionsResult>("transactions/latest?limit=" + limit.ToString() + "&offset=" + offset.ToString() + (excludemined ? "&excludeMined" : ""));
            var transactions = new KristTransaction[result.transactions.Length];
            for (int i = 0; i < transactions.Length; i++)
            {
                transactions[i] = new KristTransaction(result.transactions[i]);
            }
            return transactions;
        }

        // Name functions

        public static async Task<KristName> GetName(string name)
        {
            var result = await KristUtils.GET<KNameResult>("names/" + name);
            return new KristName(result.name);
        }

        public static async Task<KristName[]> GetAllNames(int limit = 50, int offset = 0)
        {
            var result = await KristUtils.GET<KNamesResult>("names?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            var names = new KristName[result.names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = new KristName(result.names[i]);
            }
            return names;
        }

        public static async Task<KristName[]> GetLatestNames(int limit = 50, int offset = 0)
        {
            var result = await KristUtils.GET<KNamesResult>("names/new?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            var names = new KristName[result.names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = new KristName(result.names[i]);
            }
            return names;
        }

        public static async Task<int> GetNameCost()
        {
            var result = await KristUtils.GET<KNameCostResult>("names/cost");
            return result.name_cost;
        }

        public static async Task<int> GetNameBonus()
        {
            var result = await KristUtils.GET<KNameCostResult>("names/bonus");
            return result.name_cost;
        }

        // Block functions

        public static async Task<KristBlock[]> GetAllBlocks(int limit = 50, int offset = 0)
        {
            var result = await KristUtils.GET<KBlocksResult>("blocks?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            var blocks = new KristBlock[result.blocks.Length];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = new KristBlock(result.blocks[i]);
            }
            return blocks;
        }

        public static async Task<KristBlock[]> GetLowestBlocks(int limit = 50, int offset = 0)
        {
            var result = await KristUtils.GET<KBlocksResult>("blocks/lowest?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            var blocks = new KristBlock[result.blocks.Length];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = new KristBlock(result.blocks[i]);
            }
            return blocks;
        }

        public static async Task<KristBlock[]> GetLatestBlocks(int limit = 50, int offset = 0)
        {
            var result = await KristUtils.GET<KBlocksResult>("blocks/latest?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            var blocks = new KristBlock[result.blocks.Length];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = new KristBlock(result.blocks[i]);
            }
            return blocks;
        }

        public static async Task<KristBlock> GetBlock(int height)
        {
            var result = await KristUtils.GET<KBlockResult>("blocks/" + height.ToString());
            return new KristBlock(result.block);
        }

        public static async Task<KristBlock> GetLastBlock()
        {
            var result = await KristUtils.GET<KBlockResult>("blocks/last");
            return new KristBlock(result.block);
        }

        public static async Task<KristBlock> SubmitBlock(long nonce)
        {
            var table = new Dictionary<string, string>()
            {
                ["nonce"] = nonce.ToString()
            };
            var result = await KristUtils.POST<KBlockSubmitResult>("submit", table);
            if (result.success)
            {
                return new KristBlock(result.block);
            }
            return null;
        }

        public static async Task<int> GetBaseBlockReward()
        {
            var result = await KristUtils.GET<KBaseBlockResult>("blocks/basevalue");
            return result.base_value;
        }

        public static async Task<int> GetBlockReward()
        {
            var result = await KristUtils.GET<KBlockRewardResult>("blocks/value");
            return result.value;
        }

        public static async Task<int> GetLatestWalletVersion()
        {
            var result = await KristUtils.GET<KWalletVersionResult>("walletversion");
            return result.walletVersion;
        }

        public static async Task<int> GetWork()
        {
            var result = await KristUtils.GET<KWorkResult>("work");
            return result.work;
        }

        public static async Task<KristMOTD> GetMOTD()
        {
            var result = await KristUtils.GET<KMOTDResult>("motd");
            return new KristMOTD(result);
        }

        public static async Task<long> GetSupply()
        {
            var result = await KristUtils.GET<KMoneySupplyResult>("supply");
            return result.money_supply;
        }

        public static async Task<int[]> GetWorkPast24Hrs()
        {
            var result = await KristUtils.GET<KWork24HrResult>("work/day");
            return result.work;
        }
    }
}
