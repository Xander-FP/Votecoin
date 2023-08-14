using System;

namespace BlockchainVoting.Data
{
	public class Admin:User
	{
		public Admin(string id, string palm) : base(id,palm){ }

		public override bool IsAdmin()
		{
			return true;
		}

		public bool AddParty(Node node, string party)
		{
			return false;
		}
		public bool RemoveParty(Node node, string party)
		{
			return false;
		}
		public bool RegisterUser(Node? node, User user)
		{
			if (node != null)
			{
				node.AddUser(user);
				return true;
			}
			return false;
		}
	}
}
