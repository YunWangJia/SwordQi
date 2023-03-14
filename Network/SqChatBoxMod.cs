using Bolt;
using TheForest.UI.Multiplayer;

namespace SwordQi.Network
{
    internal class SqChatBoxMod : ChatBox
    {
		//聊天框模式
		//UNIQUE ID OF THE FAKE PLAYER
		//假玩家的唯一ID
		public const ulong ModSenderPacked = 999999421;

		public static NetworkId ModNetworkID;

		public static SqChatBoxMod instance = null;

		protected override void Awake()
		{
			if (instance == null)
			{
				instance = this;
				ModNetworkID = new NetworkId(ModSenderPacked);
			}

			base.Awake();
		}

		[ModAPI.Attributes.Priority(200)]
		public override void AddLine(NetworkId? playerId, string message, bool system)
		{
			if (playerId == ModNetworkID)
			{
				if (message.StartsWith("II") && BoltNetwork.isRunning)
				{
					base.AddLine(null, "\n\t" + message.Remove(0, 2), true);

					return;
				}
				SqNetworkManager.RecieveLine(SqNetworkManager.DecodeCommand(message));
			}
			else
			{
				base.AddLine(playerId, message, system);
			}
		}


	}
}
