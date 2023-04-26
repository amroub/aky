namespace Akeneo.Authentication
{
	public class TokenRequest
	{
		public string GrantType => "password";
		public string Password { get; set; }
		public string Username { get; set; }

		public static TokenRequest For(string userName, string password)
		{
			return new TokenRequest
			{
				Username = userName,
				Password = password
			};
		}
	}
}