using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Reflection;
using System.Collections;
using UnityEngine.Video;
using System.Security.Cryptography;
using static TheForest.Items.Item;

namespace SwordQi
{
    public class ResInspection
    {

        public string jqiPath = "Mods/SwordQi/jqi.unity3d";
        public string jqiModMapPath = "Mods/SwordQi/swordqi_map.unity3d";
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
                    var ab = AB.LoadAsset<GameObject>("BeiBao");
                    AB.Unload(false);//检查文件时，忘记卸载包了，进游戏加载被占用出错了，哈哈哈
                    
                    Debug.Log("正在检查文件……");

                    if (ab == null)
                    {
                        File.Delete(jqiPath);
                        //CreateFile();
                        AssetLoad();
                        Debug.Log("文件版本老旧，重新生成并加载完成！");
                    }
                    else
                    {
                        AssetLoad();
                        Debug.Log("已是最新资源，加载完成");
                    }
                    
                }
                catch (Exception e)
                {
                    Debug.Log("文件检查出错……！！");
                    ModAPI.Log.Write("Error:jqi.unity3d文件更新检查出错" + "\n" + e.ToString());
                }
            }
            else
            {
                //CreateFile();
                //AssetLoad();
                Debug.Log("jqi.unity3d文件不存在！");
            }
            //if (File.Exists(jqiModMapPath))
            //{
            //    try
            //    {

            //        AssetBundle AB = AssetBundle.LoadFromFile(jqiPath);
            //        var ab = AB.LoadAsset<GameObject>("The_Map");
            //        AB.Unload(false);

            //        Debug.Log("正在检查文件……");

            //        if (ab == null)
            //        {
                        
            //            Debug.Log("文件版本老旧，重新生成并加载完成！");
            //        }
            //        else
            //        {
                        
            //            Debug.Log("已是最新资源，加载完成");
            //        }

            //    }
            //    catch (Exception e)
            //    {
            //        Debug.Log("文件检查出错……！！");
            //        ModAPI.Log.Write("Error:SwordQi_Map.unity3d文件检查出错" + "\n" + e.ToString());
            //    }
            //}
            //else
            //{
            //    Debug.Log("SwordQi_Map.unity3d文件不存在！");
            //}
                yield return null;
        }

        /// <summary>
        /// 使用C#内嵌资源创建文件
        /// </summary>
        //public void CreateFile()
        //{
        //    try 
        //    {
        //        Debug.Log("开始创建资源……");
        //        Assembly assembly = Assembly.GetExecutingAssembly();
        //        Stream stream = assembly.GetManifestResourceStream("SwordQi.jqi.unity3d");
        //        var fs = new FileStream(jqiPath, FileMode.Create, FileAccess.ReadWrite);

                

        //        stream.Position = 0;
        //        byte[] bytes = new byte[stream.Length];
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            int read;
        //            while ((read = stream.Read(bytes, 0, bytes.Length)) > 0)
        //            {
        //                ms.Write(bytes, 0, read);
        //            }
        //            bytes = ms.ToArray();
        //        }
        //        fs.Write(bytes, 0, bytes.Length);
        //        fs.Close();
                

        //    }
        //    catch (Exception e)
        //    {
        //        ModAPI.Log.Write("Error:生成unity3d文件失败" + "\n" + e.ToString());
        //    }
        //}

        /// <summary>
        /// 固定目录加载文件
        /// </summary>
        public void AssetLoad()
        {
            try
            {
                Debug.Log("进入资源加载！");
                AssetBundle AB = AssetBundle.LoadFromFile(jqiPath);
                //AssetBundle mapAB = AssetBundle.LoadFromFile(jqiModMapPath);
                //Debug.Log("读取Unnity3d成功！");
                SwordQi.SwordQiWhole.jqi = AB.LoadAsset<GameObject>("jianqi");
                SwordQi.SwordQiWhole.jqi_4 = AB.LoadAsset<GameObject>("jianqi_bash");//第4剑气
                SwordQi.SwordQiWhole.jqibash = AB.LoadAsset<GameObject>("zhongji");//重击
                SwordQi.SwordQiWhole.shark = AB.LoadAsset<GameObject>("sharkobj");//鲨鱼
                SwordQi.SwordQiWhole.wuqi = AB.LoadAsset<GameObject>("wqi_new_Katana");//新武士刀
                SwordQi.SwordQiWhole.Menu = AB.LoadAsset<GameObject>("SwordQiMenu");//菜单
                SwordQi.SwordQiWhole.wp_sickle = AB.LoadAsset<GameObject>("wp_sickle");//
                SwordQi.SwordQiWhole.WeaponUI = AB.LoadAsset<GameObject>("wuqi_ui");//
                SwordQi.SwordQiWhole.EnergyBar = AB.LoadAsset<GameObject>("EnergyBar");//
                SwordQi.SwordQiWhole.MapShouji = AB.LoadAsset<GameObject>("MapShouji_low");//手机模型
                SwordQi.SwordQiWhole.ShoujiMap = AB.LoadAsset<GameObject>("ShoujiMap");//手机界面，需要和模型分开加载在组合，否则无法正常生效
                SwordQi.SwordQiWhole.Lighting = AB.LoadAsset<GameObject>("lighting");//


                SwordQi.SwordQiWhole.Beibao = AB.LoadAsset<GameObject>("BeiBao");//背包里的模型
                

                SwordQi.SwordQiWhole.Forest_map = AB.LoadAsset<Sprite>("Forest_map");//地图图片
                SwordQi.SwordQiWhole.Cave_map = AB.LoadAsset<Sprite>("Cave_map");//

                SwordQi.SwordQiWhole.EnemyIcon_ui = AB.LoadAsset<GameObject>("Enemy_icon");//
                SwordQi.SwordQiWhole.FellowPlayer_ui = AB.LoadAsset<GameObject>("FellowPlayer_icon");//

                //SwordQi.SwordQiWhole.SwordQi_Map = mapAB.LoadAsset<GameObject>("The_Map");//

                //========================

                SwordQi.SwordQiWhole.jqi.AddComponent<Des>();
                SwordQi.SwordQiWhole.jqi.GetComponent<Des>().RotateSpeed = 50f;
                SwordQi.SwordQiWhole.jqi.GetComponent<Des>().DestroyTime = 3f;
                SwordQi.SwordQiWhole.jqi.GetComponent<Des>().skill_int = 1;
                SwordQi.SwordQiWhole.jqi.transform.GetChild(0).gameObject.AddComponent<Csm>();
                SwordQi.SwordQiWhole.jqi.transform.GetChild(0).gameObject.GetComponent<Csm>().skill_int = 1;

                SwordQi.SwordQiWhole.jqi_4.AddComponent<Des>(); 
                SwordQi.SwordQiWhole.jqi_4.GetComponent<Des>().RotateSpeed = 50f;
                SwordQi.SwordQiWhole.jqi_4.GetComponent<Des>().DestroyTime = 3f;
                SwordQi.SwordQiWhole.jqi_4.GetComponent<Des>().skill_int = 1;
                SwordQi.SwordQiWhole.jqi_4.transform.GetChild(0).gameObject.AddComponent<Csm>();
                SwordQi.SwordQiWhole.jqi_4.transform.GetChild(0).gameObject.GetComponent<Csm>().skill_int = 1;
                SwordQi.SwordQiWhole.jqi_4.transform.GetChild(1).gameObject.AddComponent<Csm>();
                SwordQi.SwordQiWhole.jqi_4.transform.GetChild(1).gameObject.GetComponent<Csm>().skill_int = 1;

                //重击
                SwordQi.SwordQiWhole.jqibash.AddComponent<Des>();
                SwordQi.SwordQiWhole.jqibash.GetComponent<Des>().RotateSpeed = 50f;
                SwordQi.SwordQiWhole.jqibash.GetComponent<Des>().DestroyTime = 3f;
                SwordQi.SwordQiWhole.jqibash.GetComponent<Des>().skill_int = 2;
                SwordQi.SwordQiWhole.jqibash.transform.GetChild(0).gameObject.AddComponent<Csm>();
                SwordQi.SwordQiWhole.jqibash.transform.GetChild(0).gameObject.GetComponent<Csm>().skill_int = 2;

                //鲨鱼
                SwordQi.SwordQiWhole.shark.AddComponent<Des>();
                AudioManagement.SetAudioClip(SwordQi.SwordQiWhole.shark, AB.LoadAsset<AudioClip>("sark_audio"));
                SwordQi.SwordQiWhole.shark.GetComponent<Des>().shark_obj = true;
                SwordQi.SwordQiWhole.shark.GetComponent<Des>().DestroyTime = 2.5f;
                SwordQi.SwordQiWhole.shark.GetComponent<Des>().skill_int = 3;
                SwordQi.SwordQiWhole.shark.transform.GetChild(0).gameObject.AddComponent<Csm>();
                SwordQi.SwordQiWhole.shark.transform.GetChild(0).gameObject.GetComponent<Csm>().skill_int = 3;
                //Debug.Log("添加类成功！");

                SwordQi.SwordQiWhole.WeaponUI.transform.GetChild(0).gameObject.AddComponent<UIFollowMouse>();//背包内武器信息UI
                SwordQi.SwordQiWhole.Menu.transform.Find("BJ/ke").gameObject.AddComponent<ItemBox>();//给界面添加可拖动代码，代码挂在了BJ/ke上。也就是画布里需要拖动的对象。
                                                                                                     //我去，打包资源的时候不小心把菜单界面删了，还没发现，导致这里一直报错！

                
                //Debug.Log("全部资源加载成功！");
                //SwordQi.SwordQiWhole.dalang = AB.LoadAsset<VideoClip>("DaLang");//开机视频
                AB.Unload(false);
                //mapAB.Unload(false);

                //SwordQi.SwordQiWhole.jqiPool.AddComponent<ObjectPool>();
                //SwordQi.SwordQiWhole.jqiPool.GetComponent<ObjectPool>().Initialize(5, SwordQi.SwordQiWhole.jqi, SwordQi.SwordQiWhole.jqiPool.transform);

                SwordQi.SwordQiWhole.LoadCompleted = true;

                //Pool.Initialize(SwordQi.SwordQiWhole.SQPools.transform, SwordQi.SwordQiWhole.jqi, 5);
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
