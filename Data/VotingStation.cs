namespace BlockchainVoting.Data
{
	public class VotingStation
	{
		internal string StationID { get; set; }
		internal string? Province { get; set; }
		internal string? District { get; set; }

		public VotingStation()
		{
			StationID = Guid.NewGuid().ToString();
		}

		public override string ToString()
		{
			return $"Province: {Province}, District: {District}";
		}
	}
}
