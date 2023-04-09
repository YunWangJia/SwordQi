using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pathfinding.Ionic.Zip;
using UnityEngine;

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
}
