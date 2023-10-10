
namespace Web3Ai.Service.Models.Rpc;

//TODO: fix the casing
    public class SolanaGetTransactionResponse
    {
        public string jsonrpc { get; set; }
        public Result result { get; set; }
        public int id { get; set; }
    }
    
    public class AccountKey
    {
        public string pubkey { get; set; }
        public bool signer { get; set; }
        public string source { get; set; }
        public bool writable { get; set; }
    }

    public class Info
    {
        public string destination { get; set; }
        public int lamports { get; set; }
        public string source { get; set; }
    }

    public class Instruction
    {
        public List<object> accounts { get; set; }
        public string data { get; set; }
        public string programId { get; set; }
        public object stackHeight { get; set; }
        public Parsed parsed { get; set; }
        public string program { get; set; }
    }

    public class Message
    {
        public List<AccountKey> accountKeys { get; set; }
        public List<Instruction> instructions { get; set; }
        public string recentBlockhash { get; set; }
    }

    public class Meta
    {
        public int computeUnitsConsumed { get; set; }
        public object err { get; set; }
        public int fee { get; set; }
        public List<object> innerInstructions { get; set; }
        public List<string> logMessages { get; set; }
        public List<int> postBalances { get; set; }
        public List<object> postTokenBalances { get; set; }
        public List<int> preBalances { get; set; }
        public List<object> preTokenBalances { get; set; }
        public List<object> rewards { get; set; }
        public Status status { get; set; }
    }

    public class Parsed
    {
        public Info info { get; set; }
        public string type { get; set; }
    }

    public class Result
    {
        public int blockTime { get; set; }
        public Meta meta { get; set; }
        public int slot { get; set; }
        public Transaction transaction { get; set; }
    }

    public class Status
    {
        public object Ok { get; set; }
    }

    public class Transaction
    {
        public Message message { get; set; }
        public List<string> signatures { get; set; }
    }

