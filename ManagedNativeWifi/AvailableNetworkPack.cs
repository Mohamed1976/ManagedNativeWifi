using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedNativeWifi
{
	/// <summary>
	/// Wireless LAN information on available network
	/// </summary>
	public class AvailableNetworkPack
	{
		/// <summary>
		/// Associated wireless interface information
		/// </summary>
		public InterfaceConnectionInfo Interface { get; }

		/// <summary>
		/// SSID (maximum 32 bytes)
		/// </summary>
		public NetworkIdentifier Ssid { get; }

		/// <summary>
		/// BSS network type
		/// </summary>
		public BssType BssType { get; }

		/// <summary>
		/// Signal quality (0-100)
		/// </summary>
		public int SignalQuality { get; }

		/// <summary>
		/// Whether security is enabled on this network
		/// </summary>
		public bool IsSecurityEnabled { get; }

		/// <summary>
		/// Associated wireless profile name
		/// </summary>
		public string ProfileName { get; }

		/// <summary>
		/// Indicates whether the network is connectable or not.
		/// </summary>
		public bool NetworkConnectable { get; }

		/// <summary>
		/// Indicates why a network cannot be connected to. This member is only valid when
		/// <see cref="NetworkConnectable"/> is <c>false</c>.
		/// </summary>
		public string WlanNotConnectableReason { get; }

		/// <summary>
		/// Authentication method of associated wireless LAN
		/// </summary>
		public AuthenticationMethod Authentication { get; }

		/// <summary>
		/// Encryption type of associated wireless LAN
		/// </summary>
		public EncryptionType Encryption { get; }

		/// <summary>
		/// Indicates whether the network is currently connected
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return Interface.IsConnected &&
					 string.Equals(this.ProfileName, Interface.ProfileName, StringComparison.Ordinal);
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public AvailableNetworkPack(
			InterfaceConnectionInfo interfaceInfo,
			NetworkIdentifier ssid,
			BssType bssType,
			int signalQuality,
			bool isSecurityEnabled,
			string profileName,
			bool isNetworkConnectable,
			string wlanNotConnectableReason,
			AuthenticationMethod authenticationMethod,
			EncryptionType encryptionType)
		{
			this.Interface = interfaceInfo;
			this.Ssid = ssid;
			this.BssType = bssType;
			this.SignalQuality = signalQuality;
			this.IsSecurityEnabled = isSecurityEnabled;
			this.ProfileName = profileName;
			this.NetworkConnectable = isNetworkConnectable;
			this.WlanNotConnectableReason = wlanNotConnectableReason;
			this.Authentication = authenticationMethod;
			this.Encryption = encryptionType;
		}
	}

	/// <summary>
	/// Wireless LAN information on available network and group of associated BSS networks
	/// </summary>
	public class AvailableNetworkGroupPack : AvailableNetworkPack
	{
		/// <summary>
		/// Associated BSS networks information
		/// </summary>
		public IReadOnlyCollection<BssNetworkPack> BssNetworks => Array.AsReadOnly(_bssNetworks);
		private readonly BssNetworkPack[] _bssNetworks;

		/// <summary>
		/// Link quality of associated BSS network which is the highest link quality
		/// </summary>
		public int LinkQuality { get; }

		/// <summary>
		/// Frequency (KHz) of associated BSS network which has the highest link quality
		/// </summary>
		public int Frequency { get; }

		/// <summary>
		/// Frequency band (GHz) of associated BSS network which has the highest link quality
		/// </summary>
		public float Band { get; }

		/// <summary>
		/// Channel of associated BSS network which has the highest link quality
		/// </summary>
		public int Channel { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		public AvailableNetworkGroupPack(
			InterfaceConnectionInfo interfaceInfo,
			NetworkIdentifier ssid,
			BssType bssType,
			int signalQuality,
			bool isSecurityEnabled,
			string profileName,
			bool isNetworkConnectable,
			string wlanNotConnectableReason,
			AuthenticationMethod authenticationMethod,
			EncryptionType encryptionType,
			IEnumerable<BssNetworkPack> bssNetworks) : base(
				interfaceInfo: interfaceInfo,
				ssid: ssid,
				bssType: bssType,
				signalQuality: signalQuality,
				isSecurityEnabled: isSecurityEnabled,
				profileName: profileName,
				isNetworkConnectable: isNetworkConnectable,
				wlanNotConnectableReason: wlanNotConnectableReason,
				authenticationMethod: authenticationMethod,
				encryptionType: encryptionType)
		{
			this._bssNetworks = bssNetworks.OrderByDescending(x => x.LinkQuality).ToArray();

			var highestLinkQualityNetwork = _bssNetworks.FirstOrDefault();
			if (highestLinkQualityNetwork != null)
			{
				LinkQuality = highestLinkQualityNetwork.LinkQuality;
				Frequency = highestLinkQualityNetwork.Frequency;
				Band = highestLinkQualityNetwork.Band;
				Channel = highestLinkQualityNetwork.Channel;
			}
		}
	}
}