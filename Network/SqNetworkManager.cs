using System;
using Bolt;
using TheForest.Utils;
using UnityEngine;

namespace SwordQi.Network
{
	public class SqNetworkManager : MonoBehaviour
	{//网络管理器
		public enum Target
		{
			OnlyServer, Everyone, Clients, Others
		}

		public delegate void OnGetMessage(byte[] arr);

		public OnGetMessage onGetMessage;
		public static SqNetworkManager instance;

		/// <summary>
		/// Sets the 'instance'
		/// 设置“实例”
		/// </summary>
		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(this);
			}
		}

		/// <summary>
		/// Sends a string to all players on the server
		/// 向服务器上的所有玩家发送字符串
		/// </summary>
		/// <param name="s">Content of the message, make sure it ends with ';'</param>
		public static void SendLine(byte[] bytearray, Target target)//发送行
		{
			if (GameSetup.IsSinglePlayer || !BoltNetwork.isRunning)
			{
				RecieveLine(bytearray);
			}
			else
			{
				if (BoltNetwork.isRunning)
				{
					ChatEvent chatEvent = null;
					switch (target)
					{
						case Target.OnlyServer:
							chatEvent = ChatEvent.Create(GlobalTargets.OnlyServer);
							break;

						case Target.Everyone:
							chatEvent = ChatEvent.Create(GlobalTargets.Everyone);
							break;

						case Target.Clients:
							chatEvent = ChatEvent.Create(GlobalTargets.AllClients);
							break;

						case Target.Others:
							chatEvent = ChatEvent.Create(GlobalTargets.Others);
							break;

						default:
							break;
					}
					chatEvent.Message = EncodeCommand(bytearray);
					chatEvent.Sender = SqChatBoxMod.ModNetworkID;
					chatEvent.Send();
				}
			}
		}

		public static void SendText(string text, Target target)//发送文本
		{
			{
				if (BoltNetwork.isRunning)
				{
					ChatEvent chatEvent = null;
					switch (target)
					{
						case Target.OnlyServer:
							chatEvent = ChatEvent.Create(GlobalTargets.OnlyServer);
							break;

						case Target.Everyone:
							chatEvent = ChatEvent.Create(GlobalTargets.Everyone);
							break;

						case Target.Clients:
							chatEvent = ChatEvent.Create(GlobalTargets.AllClients);
							break;

						case Target.Others:
							chatEvent = ChatEvent.Create(GlobalTargets.Others);
							break;

						default:
							break;
					}
					chatEvent.Message = text;
					chatEvent.Sender = SqChatBoxMod.ModNetworkID;
					chatEvent.Send();
				}
			}
		}

		public static void SendLine(byte[] bytearray, BoltConnection con)
		{
			if (GameSetup.IsSinglePlayer || !BoltNetwork.isRunning)
			{
				RecieveLine(bytearray);
			}
			else
			{
				if (BoltNetwork.isRunning)
				{
					ChatEvent chatEvent = ChatEvent.Create(con);
					chatEvent.Message = EncodeCommand(bytearray);
					chatEvent.Sender = SqChatBoxMod.ModNetworkID;
					chatEvent.Send();
				}
			}
		}

		public static byte[] DecodeCommand(string cmd)//解码命令
		{
			var a = cmd.ToCharArray();
			var b = new byte[a.Length];
			for (int i = 0; i < a.Length; i++)
			{
				b[i] = (byte)a[i];
			}
			return b;
		}

		public static string EncodeCommand(byte[] b)//编码命令
		{
			string s = string.Empty;
			for (int i = 0; i < b.Length; i++)
			{
				s += (char)b[i];
			}
			return s;
		}

		/// <summary>
		/// Called on recieving a message
		/// 收到消息时调用
		/// </summary>
		/// <param name="s"></param>
		public static void RecieveLine(byte[] array)//接收线
		{
			try
			{
				instance.onGetMessage(array);
			}
			catch (Exception ex)
			{
				ModAPI.Log.Write(ex.ToString());
			}
		}

		public static ulong lastDropID = 10;

		

		
		//public static void SendPlayerHitmarker(Vector3 pos, int amount)//发送玩家命中标记
		//{
		//	using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
		//	{
		//		using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
		//		{
		//			w.Write(21);
		//			w.Write(amount);
		//			w.Write(pos.x);
		//			w.Write(pos.y);
		//			w.Write(pos.z);
		//			w.Close();
		//		}
		//		Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
		//		answerStream.Close();
		//	}
		//}

		
		
	}
}
