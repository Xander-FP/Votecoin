namespace BlockchainVoting.Data
{
	public class User
	{
		public string IdNumber { get; private set; }
		internal string UID { get; private set; }
		private int VoteCoins { get; set; }
		private string PalmScan { get; set; }
		internal string OTP { get; }

		public User(string id, string palm) { 
			UID = Guid.NewGuid().ToString();
			VoteCoins = 1;
			IdNumber = id;
			PalmScan = palm;
			OTP = (new Random()).Next(10000, 100000).ToString();
		}

		public virtual bool IsAdmin() {
			return false;
		}

		public bool HasVoted()
		{
			return VoteCoins != 1;
		}

		public void Vote()
		{
			VoteCoins--;
		}

		public bool Authenticate(string palm, string otp)
		{
			return Blockchain.GetSha256(palm) == PalmScan && otp == OTP;
		}

	}
}
