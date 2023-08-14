using System.Collections;

namespace BlockchainVoting.Data
{
	public class Node
	{
		internal string? Address { get; set; }
		private Hashtable? Users { get; set; }
		internal Blockchain? Blockchain { get; set; }
		internal VotingStation? VotingStation { get; set; }

		public List<Block> GetFullChain()
		{
			return Blockchain.GetFullChain();
		}

		public User? GetUser(string id)
		{
			if (Users != null && Users.ContainsKey(id))
				return (User?)Users[id];
			return null;
		}

		public void AddUser(User user)
		{
			if (Users == null)
				Users = new Hashtable();
			if (!Users.ContainsKey(user.IdNumber))
				Users.Add(user.IdNumber, user);
		}

		public bool AddVote(Vote vote)
		{
			if (Blockchain == null)
				return false;
			return Blockchain.AddVote(vote);
		}
		internal bool HasVoted(string username)
		{
			User? user = GetUser(username);
			if (user == null) return false;
			return user.HasVoted();
		}
	}
}
