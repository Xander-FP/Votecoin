using System.Security.Cryptography;

namespace BlockchainVoting.Data
{
    public class AccountService
    {
        internal Node? CurrNode { get; private set; }
        internal User? ActiveUser { get; private set; }

        public AccountService()
        {
            CurrNode = null;
            ActiveUser = null;
        }

        public void SetNode(Node node)
        {
            CurrNode = node;
        }

        public bool RegisterUser(string idNum, string palmScan)
        {
            if (ActiveUser != null && ActiveUser.IsAdmin())
            {
                ((Admin)ActiveUser).RegisterUser(CurrNode,new User(idNum, palmScan));
                return true;
            }
            return false;
        }

        public bool Login(string id, string palm, string otp)
        {
            Console.WriteLine(CurrNode);
            if (CurrNode == null)
            {
                ActiveUser = new Admin("admin","admin");
                return true;
            }
            User? u = CurrNode.GetUser(id);
            if (u == null)
                return false;
            bool success = u.Authenticate(palm, otp);
            if (success) 
                ActiveUser = u;
            return success;
        }
        public void Logout()
        {
            ActiveUser = null;
        }

        public string Vote(string party)
        {
			if (ActiveUser == null || CurrNode == null || ActiveUser.HasVoted()) 
                return String.Empty;

            if (CurrNode.GetUser(ActiveUser.IdNumber) == null)
                return String.Empty;

            Vote v = new() 
            { 
                Party = RSA.Encrypt(party),
				Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                TempId = ActiveUser.UID
			};

            bool res = CurrNode.AddVote(v);
            if (res)
            {
                ActiveUser.Vote();
                return v.VoteId;
			}
            return String.Empty;

		}

		public string GetOTP(string userId)
		{
            if (ActiveUser == null)
                return "No user logged in";
            if (CurrNode == null)
                return "Specify a node to search";
            User? user = CurrNode.GetUser(userId);
            if (user == null)
                return "No such user";
            if (ActiveUser.IsAdmin())
                return user.OTP;
            return "No permission";
		}

        public string GetActiveUser()
        {
            if (ActiveUser != null)
                return ActiveUser.IdNumber;
            return String.Empty;
        }

        public string GetCurrNode()
        {
            if (CurrNode != null)
                return CurrNode.VotingStation.ToString();
            return String.Empty;
        }
    }
}
