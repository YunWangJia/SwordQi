using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheForest.Utils;
using UnityEngine;

namespace SwordQi.Network
{
    public class SqCommandReader
    {//命令读取器
        private delegate void Command(BinaryReader r);

		public static void OnCommand(byte[] bytes)//OnCommand（联机命令）
		{
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				using (BinaryReader r = new BinaryReader(stream))//二进制读取器
				{
					int cmdIndex = r.ReadInt32();
					try
					{
						switch (cmdIndex)
						{
							case 1:
								int fourth = r.ReadInt32();
								Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
								Quaternion quat = new Quaternion(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
								SwordQi.SyncJianQi(pos, quat, fourth);
								break;

							case 2:
								int fourth_1 = r.ReadInt32();
								Vector3 pos2 = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
								Quaternion quat2 = new Quaternion(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
								SwordQi.SyncShark(pos2, quat2);
								break;

							case 3:
								int fourth_2 = r.ReadInt32();
								Vector3 pos3 = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
								Quaternion quat3 = new Quaternion(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
								SwordQi.SyncJianQiBash(pos3, quat3);
								break;


							default:

								break;

						}
						

					}
					catch (Exception e)
					{

						ModAPI.Log.Write("Error: " + cmdIndex + "\n" + e.ToString());
					}
					r.Close();
				}
				stream.Close();
			}
		}
	}
}
