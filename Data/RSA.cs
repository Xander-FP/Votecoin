using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainVoting.Data
{
	public class RSA
	{
		private static string _publicKey = null!;
		private static string _privateKey = null!;

		public static string Encrypt(string data)
		{
			if (_publicKey == null)
				GenerateKeys();
			RSACryptoServiceProvider rsa = new();
			rsa.FromXmlString(_publicKey);

			byte[] dataToEncrypt = Encoding.ASCII.GetBytes(data);
			byte[] encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();

			return Convert.ToBase64String(encryptedByteArray);
		}

		public static string Decrypt(string data)
		{
			//Check that the service is connected to the internet and that the date is past the election date
			RSACryptoServiceProvider rsa = new();
			rsa.FromXmlString(_privateKey);

			byte[] dataByte = Convert.FromBase64String(data);
			byte[] decryptedByte = rsa.Decrypt(dataByte, false);

			return Encoding.UTF8.GetString(decryptedByte);
		}

		public static void GenerateKeys()
		{
			RSACryptoServiceProvider rsa = new();
			_publicKey = rsa.ToXmlString(false);
			_privateKey = rsa.ToXmlString(true);
		}
	}
}
