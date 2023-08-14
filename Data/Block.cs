using System.Text;

namespace BlockchainVoting.Data
{
    public class Block
    {
        internal int Index { get; set; }
		internal DateTime Timestamp { get; set; }
		internal List<Vote> Votes { get; set; }
		internal int Proof { get; set; }
		internal string PreviousHash { get; set; }

        public override string ToString()
        {
			StringBuilder outputString = new StringBuilder("",50);
            outputString.Append(Index);
            outputString.Append($" [{Timestamp.ToString("yyyy-MM-dd HH:mm:ss")}] ");
            if (Proof != 0)
                outputString.Append($"Proof: {Proof} ");
			if (PreviousHash != null)
				outputString.Append($"| PrevHash: {PreviousHash} ");
            outputString.Append($"| Votes[ ");
			foreach (Vote vote in Votes)
            {
                outputString.Append(vote.ToString() + ">>>>");
            }
            outputString.Append(" ]");
            return outputString.ToString();
        }

        public Block Clone()
        {
            Block block = new();
            block.Index = Index;
            block.Timestamp = Timestamp;
            block.Votes = new();
            foreach (Vote t in Votes)
            {
                block.Votes.Add(t.Clone());
            }
            return block ;
        }
    }
}
