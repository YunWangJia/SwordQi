using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pathfinding.Ionic.Zip;
using UnityEngine;
using Bolt;
using TheForest.Utils;
using Steamworks;

namespace SwordQi
{
    public class ZipHelper
    {
        

        //public static void ExtractZipFile(string zipFilePath, string extractFolderPath)
        //{

        //    using (ZipFile zip = ZipFile.Read(zipFilePath))
        //    {
        //        zip.TempFileFolder = extractFolderPath;
        //        foreach (ZipEntry entry in zip)
        //        {
        //            if (entry.FileName.EndsWith(".tmp"))
        //            {
        //                string tmpFilePath = Path.Combine(zip.TempFileFolder, entry.FileName);
        //                if (File.Exists(tmpFilePath))
        //                {
        //                    // 如果文件存在，则重命名为没有.tmp后缀
        //                    string newFilePath = Path.Combine(zip.ExtractExistingFile == ExtractExistingFileAction.OverwriteSilently ? zip.TempFileFolder : Directory.GetCurrentDirectory(),
        //                        entry.FileName.Substring(0, entry.FileName.LastIndexOf(".tmp")));
        //                    File.Move(tmpFilePath, newFilePath);
        //                }
        //            }
        //            else
        //            {
        //                // 如果文件没有.tmp后缀，则正常解压缩
        //                entry.Extract();
        //            }
        //        }
        //    }


        //}



    }

    public class SQPlayerManager
    {
        public static List<SQPlayer> Players { get; } = new List<SQPlayer>();

        public SQPlayerManager(SwordQi instance)
        {
        }

        public SQPlayer GetPlayerBySteamId(ulong steamId)
        {
            return Players.FirstOrDefault(o => o.SteamId == steamId);
        }
    }

    public class SQPlayer
    {
        private static readonly Dictionary<string, ulong> CachedIds = new Dictionary<string, ulong>();

        public BoltEntity Entity { get; }
        public ulong SteamId =>
            CachedIds.ContainsKey(Name)
                ? CachedIds[Name]
                : (CachedIds[Name] = CoopLobby.Instance.AllMembers.FirstOrDefault(o => SteamFriends.GetFriendPersonaName(o) == Name).m_SteamID);

        public string Name => Entity.GetState<IPlayerState>().name;
        public BoltPlayerSetup PlayerSetup => Entity.GetComponent<BoltPlayerSetup>();
        public CoopPlayerRemoteSetup CoopPlayer => Entity.GetComponent<CoopPlayerRemoteSetup>();

        public Transform Transform => Entity.transform;
        public Vector3 Position => Transform.position;
        public NetworkId NetworkId => Entity.networkId;

        public SQPlayer(BoltEntity player)
        {
            Entity = player;
        }
    }

}
