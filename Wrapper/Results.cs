using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crazed.KristSharp
{
    class KResult
    {
        public bool ok;
    }

    class KAddressResult : KResult
    {
        public KAddress address;
    }

    class KAddressesResult : KResult
    {
        public int count;
        public int total;
        public KAddress[] addresses;
    }

    class KTransactionsResult : KResult
    {
        public int count;
        public int total;
        public KTransaction[] transactions;
    }

    class KNamesResult : KResult
    {
        public int count;
        public int total;
        public KName[] names;
    }

    class KAllBlocksResult : KResult
    {
        public int count;
        public int total;
        public KBlock[] blocks;
    }

    class KBlocksResult : KResult
    {
        public int count;
        public KBlock[] blocks;
    }

    class KBlockResult : KResult
    {
        public KBlock block;
    }

    class KBlockSubmitResult : KResult
    {
        public bool success;
        public int work;
        public KAddress address;
        public KBlock block;
    }

    class KBaseBlockResult : KResult
    {
        public int base_value;
    }

    class KBlockRewardResult : KResult
    {
        public int value;
    }

    class KTransactionResult : KResult
    {
        public KTransaction transaction;
    }

    class KNameResult : KResult
    {
        public KName name;
    }

    class KNameCostResult : KResult
    {
        public int name_cost;
    }

    class KNameBonusResult: KResult
    {
        public int name_bonus;
    }

    class KAuthenticateAddressResult : KResult
    {
        public bool authed;
        public string address;
    }

    class KWalletVersionResult : KResult
    {
        public int walletVersion;
    }

    class KWorkResult : KResult
    {
        public int work;
    }

    class KMOTDResult : KResult
    {
        public string motd;
        public string psa;
        public string shrodingers_cat;
        public DateTime set;
    }

    class KMoneySupplyResult : KResult
    {
        public long money_supply;
    }

    class KWork24HrResult : KResult
    {
        public int[] work;
    }

    class KAddress
    {
        public string address;
        public int balance;
        public int totalin;
        public int totalout;
        public DateTime firstseen;
    }

    class KTransaction
    {
        public int id;
        public string from;
        public string to;
        public int value;
        public DateTime time;
        public string name;
        public string metadata;
    }

    class KName
    {
        public string name;
        public string owner;
        public DateTime registered;
        public DateTime updated;
        public string a;
    }

    class KBlock
    {
        public int height;
        public string address;
        public string hash;
        public string short_hash;
        public int value;
        public long difficulty;
        public DateTime time;
    }
}
