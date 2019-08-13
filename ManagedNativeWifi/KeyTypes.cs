using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedNativeWifi
{
	/// <summary>
	/// The sharedKey (security) element contains shared key information.
	/// This element is only required if WEP or PSK keys are required for the authentication and encryption pair.
	/// </summary>
	/// <remarks>
	/// https://docs.microsoft.com/en-us/windows/desktop/nativewifi/wlan-profileschema-sharedkey-security-element
	/// </remarks>
	public enum KeyTypes
	{
		/// <summary>
		/// None (valid value)
		/// </summary>
		None,

		/// <summary>
		/// Shared key will be a network key
		/// </summary>
		NetworkKey,

		/// <summary>
		/// Shared key will be a pass phrase
		/// </summary>
		PassPhrase
	}

	internal static class KeyTypConverter
	{
		public static bool TryParse(string source, out KeyTypes keyType)
		{
			keyType = default(KeyTypes);
			bool isValid = false;

			switch (source)
			{
				case "networkKey":
					keyType = KeyTypes.NetworkKey;
					isValid = true;
					break;
				case "passPhrase":
					keyType = KeyTypes.PassPhrase;
					isValid = true;
					break;
			}

			return isValid;
		}
	}
}
