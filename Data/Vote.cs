namespace BlockchainVoting.Data
{
    public class Vote
    {
        internal string VoteId { get; private set; }
        internal string? Timestamp { get; set; }
        internal string? Party { get; set; }
        internal string TempId { get; set; }

        public Vote(string? id = null)
        {
            if (id == null)
                VoteId = Guid.NewGuid().ToString();
            else
                VoteId = id;
        }

        public override string ToString()
        {
            return $"Id: {VoteId} ; Vote: {Party} ; timestamp: {Timestamp}";
        }

        public Vote Clone()
        {
            return new Vote(VoteId) { Timestamp = Timestamp, Party = Party };
        }
    }
}
