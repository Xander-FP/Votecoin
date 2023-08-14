using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Collections;

namespace BlockchainVoting.Data
{
	public class Blockchain
	{
		private List<Vote> _currentVotes = new List<Vote>();
		private List<Block> _chain = new List<Block>();
		private List<Node> _nodes = new List<Node>();
		private Block _lastBlock => _chain.Last();
		public Blockchain()
		{
			//Create the genesis block. The genesis block contains a 'Test' vote in order to check the encryption
			_currentVotes.Add( new() { Party = RSA.Encrypt("Test-Initial block") });
			CreateNewBlock(proof: 100, previousHash: "1"); 
		}

		public bool IsValidChain(List<Block> chain)
		{
			//Check that the entire chain is valid by checking the hashes and the proofs at each point
			Block? prevBlock = null;
			foreach (var currBlock in chain)
			{
				if (prevBlock != null)
				{
					if (GetHash(prevBlock) != currBlock.PreviousHash)
						return false;

					if (!IsValidProof(prevBlock.Proof, currBlock.Proof, currBlock.PreviousHash))
						return false;
				}
				else
				{
					if ("1" != currBlock.PreviousHash)
						return false;

					if (IsValidProof(100, currBlock.Proof, currBlock.PreviousHash))
						return false;
				}
				prevBlock = currBlock;
			}
			return true;
		}

		public bool ResolveConflicts()
		{
			//Mocked for this implementation
			return false;
		}

		public Block CreateNewBlock(int proof, string previousHash = null)
		{
			foreach (Vote v in _currentVotes)
			{
				v.TempId = String.Empty;
			}
			Block block = new Block
			{
				Index = _chain.Count,
				PreviousHash = previousHash ?? GetHash(_lastBlock),
				Proof = proof,
				Timestamp = DateTime.UtcNow,
				Votes = _currentVotes.ToList(),
			};
			_currentVotes.Clear();
			_chain.Add(block);
			return block;
		}

		public int CreateProofOfWork()
		{
			//Should be hard to compute, but easy to verify Something like the hash of the product of two numbers should end in 0
			int lastProof = _lastBlock.Proof;
			string previousHash = _lastBlock.PreviousHash;
			int proof = 0;
			while (!IsValidProof(lastProof, proof, previousHash))
			{
				proof++;
			}

			return proof;
		}

		private bool IsValidProof(int lastProof, int proof, string previousHash)
		{
			return lastProof == proof - 1;
		}

		private string GetHash(Block block)
		{
			return GetSha256(block.ToString());
		}

		internal static string GetSha256(string data)
		{
			var sha256 = new SHA256Managed();
			var hashBuilder = new StringBuilder();

			byte[] bytes = Encoding.Unicode.GetBytes(data);
			byte[] hash = sha256.ComputeHash(bytes);

			foreach (byte x in hash)
				hashBuilder.Append($"{x:x2}");

			return hashBuilder.ToString();
		}

		public void Mine()
		{
			int proof = CreateProofOfWork();
			CreateNewBlock(proof);
		}

		public List<Block> GetFullChain()
		{
			return _chain;
		}

		internal void RegisterNode(Node node)
		{
			_nodes.Add(node);
		}

		internal void RemoveNode(string adress)
		{
			foreach (Node n in _nodes)
				if (n.Address == adress)
				{
					_nodes.Remove(n);
					break;
				}
		}

		internal Node? GetNode(string adress)
		{
			foreach (Node n in _nodes)
				if (n.Address == adress)
					return n;
			return null;
		}

		internal List<Node> GetAllNodes()
		{
			return _nodes;
		}

		internal string Consensus()
		{
			//Mocked for this implementation
			//Calls ResolveConflicts and returns whether chain was replaced or is authoritive
			return "consensus reached";
		}

		internal bool AddVote(Vote vote)
		{
			//Check that the vote does not exist already
			foreach (Vote v in _currentVotes)
			{
				if (v.TempId == vote.TempId)
				{
					Console.WriteLine($"{vote} Already voted");
					return true;
				}
			}
			_currentVotes.Add(vote);
			return true;
		}

		internal List<Block> DecryptAllVotes()
		{
			List<Block> blocks = new();
			foreach (Block block in _chain)
			{
				Block decryptedBlock = block.Clone();
				foreach (Vote t in decryptedBlock.Votes)
				{
					Console.WriteLine(t.Party);
					t.Party = RSA.Decrypt(t.Party);
				}
				blocks.Add(decryptedBlock);
			}
			return blocks;
		}
	}
}
