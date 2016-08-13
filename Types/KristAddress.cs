using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crazed.KristSharp
{
    public class KristAddress : IEquatable<KristAddress>
    {
        public readonly string Address;
        private readonly string PrivateKey;
        public int Balance { get; private set; }
        public int TotalIn { get; private set; }
        public int TotalOut { get; private set; }
        public DateTime FirstSeen { get; private set; }

        public KristAddress(string address)
        {
            Address = address;
        }

        internal KristAddress(KAddress data)
        {
            Address = data.address;
            Parse(data);
        }

        internal KristAddress(string address, string privatekey)
        {
            Address = address;
            PrivateKey = privatekey;
        }

        public static KristAddress FromPassword(string password)
        {
            return FromPrivateKey(KristUtils.GeneratePrivateKey(password));
        }

        public static KristAddress FromPrivateKey(string privatekey)
        {
            string address = KristUtils.GenerateV2Address(privatekey);
            return new KristAddress(address, privatekey);
        }


        public async Task Update()
        {
            var result = await KristUtils.GET<KAddressResult>("addresses/" + Address);
            Parse(result.address);
        }

        private void Parse(KAddress data)
        {
            Balance = data.balance;
            TotalIn = data.totalin;
            TotalOut = data.totalout;
            FirstSeen = data.firstseen;
        }

        public async Task<KristTransaction> MakeTransaction(string recipient, int amount, string metadata = null)
        {
            if (PrivateKey == null)
                throw new KristPrivateKeyMissingException();
            Dictionary<string, string> table = new Dictionary<string, string>();
            table["privatekey"] = PrivateKey;
            table["to"] = recipient;
            table["amount"] = amount.ToString();
            if (metadata != null)
                table["metadata"] = metadata;
            KTransactionResult result = await KristUtils.POST<KTransactionResult>("transactions/", table);
            return new KristTransaction(result.transaction);
        }

        public Task<KristTransaction> MakeTransaction(KristAddress recipient, int amount, string metadata = null)
        {
            return MakeTransaction(recipient.Address, amount, metadata);
        }

        public async Task<KristTransaction[]> GetRecentTransactions(int limit = 50, int offset = 0)
        {
            var result = await KristUtils.GET<KTransactionsResult>("addresses/" + Address + "/transactions?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            var transactions = new KristTransaction[result.transactions.Length];
            for (int i = 0; i < result.transactions.Length; i++)
            {
                transactions[i] = new KristTransaction(result.transactions[i]);
            }
            return transactions;
        }

        public async Task<KristName[]> GetAllNames(int limit = 50, int offset = 0)
        {
            var result = await KristUtils.GET<KNamesResult>("addresses/" + Address + "/names?limit=" + limit.ToString() + "&offset=" + offset.ToString());
            var names = new KristName[result.names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = new KristName(result.names[i]);
            }
            return names;
        }

        public async Task<KristName> RegisterName(string name)
        {
            if (PrivateKey == null)
                throw new KristPrivateKeyMissingException();
            var table = new Dictionary<string, string>();
            table["privatekey"] = PrivateKey;
            await KristUtils.POST<KResult>("names/" + name, table);
            return await Krist.GetName(name);
        }

        public async Task<KristName> TransferName(string name, string address)
        {
            if (PrivateKey == null)
                throw new KristPrivateKeyMissingException();
            var table = new Dictionary<string, string>();
            table["privatekey"] = PrivateKey;
            table["address"] = address;
            await KristUtils.POST<KResult>("names/" + name + "/transfer", table);
            return await Krist.GetName(name);
        }

        public Task<KristName> TransferName(string name, KristAddress address)
        {
            return TransferName(name, address.Address);
        }

        public async Task<KristName> UpdateNameARecord(string name, string arecord)
        {
            if (PrivateKey == null)
                throw new KristPrivateKeyMissingException();
            var table = new Dictionary<string, string>();
            table["privatekey"] = PrivateKey;
            table["a"] = arecord;
            await KristUtils.POST<KResult>("names/" + name + "/transfer", table);
            return await Krist.GetName(name);
        }

        public async Task<bool> Authenticate()
        {
            if (PrivateKey == null)
                throw new KristPrivateKeyMissingException();
            var table = new Dictionary<string, string>();
            table["privatekey"] = PrivateKey;
            var result = await KristUtils.POST<KAuthenticateAddressResult>("login", table);
            return result.authed;
        }


        // Let this cluttered piece of shit die in vain.
        // No seriously, fire the guy who thought this was a good idea.
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            KristAddress address = obj as KristAddress;
            if (address == null)
                return false;
            else
                return this == address;
        }

        public bool Equals(KristAddress other)
        {
            return Equals((object)other);
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }

        public static bool operator ==(KristAddress address1, KristAddress address2)
        {
            if (object.ReferenceEquals(address1, address2))
                return true;
            if ((object)address1 == null || (object)address2 == null)
                return false;

            return address1.Address == address2.Address;
        }

        public static bool operator !=(KristAddress address1, KristAddress address2)
        {
            return !(address1 == address2);
        }

        public override string ToString()
        {
            return "Krist address: " + Address;
        }
    }
}
