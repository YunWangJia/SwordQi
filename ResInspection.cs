using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Reflection;
using System.Collections;

namespace SwordQi
{
    public class ResInspection
    {

        public string jqiPath = "Mods/SwordQi/jqi.unity3d";
        public string SwordQiContents = "Mods/SwordQi";

        //private int LoadValue = 0;
        
        public IEnumerator AssetCheck()
        {
            if (!Directory.Exists(SwordQiContents))
            {
                Directory.CreateDirectory(SwordQiContents);
            }

            if (File.Exists(jqiPath))
            {
                //File.Delete("Mods/SwordQi");//删除老版本文件
                try
                {
                    AssetBundle AB = AssetBundle.LoadFromFile(jqiPath);
                    var ab = AB.LoadAsset<GameObject>("LD_bak");
                    AB.Unload(false);//检查文件时，忘记卸载包了，进游戏加载被占用出错了，哈哈哈
                    if (ab == null)
                    {
                        File.Delete(jqiPath);
                        CreateFile();
                        AssetLoad();
                    }
                    else
                    {
                        AssetLoad();
                    }

                }
                catch (Exception e)
                {
                    ModAPI.Log.Write("Error:jqi.unity3d文件检查时出错" + "\n" + e.ToString());
                }
            }
            else
            {
                CreateFile();
                AssetLoad();
            }
            SwordQi.SwordQiWhole.Loaded = true;
            yield return null;
        }

        /// <summary>
        /// 使用C#内嵌资源创建文件
        /// </summary>
        public void CreateFile()
        {
            try 
            {
                
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("SwordQi.jqi.unity3d");
                var fs = new FileStream(jqiPath, FileMode.Create, FileAccess.ReadWrite);

                

                stream.Position = 0;
                byte[] bytes = new byte[stream.Length];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(bytes, 0, bytes.Length)) > 0)
                    {
                        ms.Write(bytes, 0, read);
                    }
                    bytes = ms.ToArray();
                }
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                

            }
            catch (Exception e)
            {
                ModAPI.Log.Write("Error:生成unity3d文件失败" + "\n" + e.ToString());
            }
        }

        /// <summary>
        /// 固定目录加载文件
        /// </summary>
        public void AssetLoad()
        {
            try
            {
                

                AssetBundle AB = AssetBundle.LoadFromFile(jqiPath);
                SwordQi.SwordQiWhole.jqi = AB.LoadAsset<GameObject>("jianqi");
                SwordQi.SwordQiWhole.jqi_4 = AB.LoadAsset<GameObject>("jianqibash");//第4剑气
                SwordQi.SwordQiWhole.jqibash = AB.LoadAsset<GameObject>("zhongji");//重击
                SwordQi.SwordQiWhole.shark = AB.LoadAsset<GameObject>("sharkobj");//鲨鱼
                SwordQi.SwordQiWhole.wuqi = AB.LoadAsset<GameObject>("wqi");//新武士刀
                SwordQi.SwordQiWhole.Menu = AB.LoadAsset<GameObject>("SwordQiMenu");//菜单
                SwordQi.SwordQiWhole.wq_LD = AB.LoadAsset<GameObject>("wp_LD");//
                SwordQi.SwordQiWhole.WeaponUI = AB.LoadAsset<GameObject>("wuqi_ui");//
                SwordQi.SwordQiWhole.ka_bak = AB.LoadAsset<GameObject>("ka_bak");//
                SwordQi.SwordQiWhole.LD_bak = AB.LoadAsset<GameObject>("LD_bak");//



                SwordQi.SwordQiWhole.jqi.AddComponent<Des>();
                SwordQi.SwordQiWhole.jqi.transform.GetChild(0).gameObject.AddComponent<Csm>();

                SwordQi.SwordQiWhole.jqi_4.AddComponent<Des>();
                SwordQi.SwordQiWhole.jqi_4.transform.GetChild(0).gameObject.AddComponent<Csm>();
                SwordQi.SwordQiWhole.jqi_4.transform.GetChild(1).gameObject.AddComponent<Csm>();

                SwordQi.SwordQiWhole.jqibash.AddComponent<SharkDes>();
                SwordQi.SwordQiWhole.jqibash.transform.GetChild(0).gameObject.AddComponent<JqiBashCsm>();

                SwordQi.SwordQiWhole.shark.AddComponent<SharkDes>();
                SwordQi.SwordQiWhole.shark.transform.GetChild(0).gameObject.AddComponent<SharkCsm>();

                AB.Unload(false);

                
            }
            catch (Exception e)
            {
                ModAPI.Log.Write("Error:jqi.unity3d文件读取失败" + "\n" + e.ToString());
            }
        }

        //public void LoadUI(int CurrentProgressBar, string text)
        //{
        //    //int width = Screen.width / 2;//屏幕宽度
        //    SwordQi Sq = new SwordQi();
        //    CurrentProgressBar *= 2;//扩大2倍，这样的话传入值即可为0~100
        //    int height = Screen.height;

        //    CurrentProgressBar = Mathf.Clamp(CurrentProgressBar, 0, 200);//限制数值大小
        //    if (CurrentProgressBar == 200)
        //    {
        //        for (int x = 0; x < Sq.HPBG.width; x++)
        //        {
        //            for (int y = 0; y < Sq.HPBG.height; y++)
        //            {
        //                Sq.HPBG.SetPixel(x, y, new Color(0f, 0.7f, 0f));

        //            }
        //        }
        //        Sq.HPBG.Apply();
        //    }
        //    if (CurrentProgressBar < 200)
        //    {
        //        for (int x = 0; x < Sq.HPBG.width; x++)
        //        {
        //            for (int y = 0; y < Sq.HPBG.height; y++)
        //            {
        //                Sq.HPBG.SetPixel(x, y, new Color(0f, 0f, 0.7f));

        //            }
        //        }
        //        Sq.HPBG.Apply();
        //    }

        //    GUI.DrawTexture(new Rect(0, height - 15, CurrentProgressBar, 15), Sq.HPBG);//进度条
        //    GUI.Label(new Rect(0, height - 30f, 200f, 20f), text);
        //}



    }
}
