using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ModAPI.Attributes;
using TheForest.Utils;
using TheForest.Items;
using UnityEngine;
using UnityEngine.UI;
using ModAPI;
using UnityEngine.SceneManagement;


namespace SwordQi
{
    internal class SwordQi : MonoBehaviour
    {
        [ExecuteOnGameStart]
        private static void AddMeToScene()
        {
            new GameObject("SwordQi").AddComponent<SwordQi>();

            
        }

        public static GameObject jqi;
        public static GameObject jqibash;
        public static GameObject shark;
        public static GameObject jqi_4;

        protected GUIStyle labelStyle;
        public Texture2D HPBG;

        private bool ShouldEquipLeftHandAfter;
        private bool ShouldEquipRightHandAfter;
        private bool visible;
        public bool jqiBool = true;
        public bool ClashMods = false;

        public static bool Loaded = false;

        public static int sharkEnergy;
        public static int qics;
        public string Open = "已开启";
        public float Keytime = 0f;


        //==========================================测试变量
        //public bool timeJudge = true;
        //public bool Butt = true;
        //public bool sharkbool = false;

        public static string pztag = "";
        //public static string pzname;
        
        public static string LoadText = "";
        public static int LoadValue;

        

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!,不知道为什么在声明赋值new对象会导致游戏直接崩溃，请在方法内赋值或使用局部变量
        //public ResInspection ResIn = new ResInspection();


        void Start()
        {

            if (ModAPI.Mods.LoadedMods.ContainsKey("Simple_Player_Markers"))//判断是否存在冲突Mod
            {
                ClashMods = true;
                return;
            }

            StartCoroutine(ResInspection.AssetCheck());

            if (!GameSetup.IsSinglePlayer)//不判断会导致进单人模式时游戏强退
            {
                new GameObject("SqNetworkManagerObj").AddComponent<Network.SqNetworkManager>();
                Network.SqNetworkManager.instance.onGetMessage += Network.SqCommandReader.OnCommand;
            }

            //new GameObject("NetworkManagerObj").AddComponent<Network.SqNetworkManager>();
            //Network.SqNetworkManager.instance.onGetMessage += Network.SqCommandReader.OnCommand;
            //GameObject.Find("NetworkManagerObj").SetActive(false);

            HPBG = new Texture2D(10, 10);

            //try
            //{
            //    Vector3 xyz = new Vector3(550f, 60f, 2055f);
            //    AssetBundle AB = AssetBundle.LoadFromFile("Mods/SwordQi/CaveMap.unity3d");
            //    GameObject obj = AB.LoadAsset<GameObject>("del");
            //    GameObject obj1 = AB.LoadAsset<GameObject>("rov");

            //    obj.transform.GetChild(0).gameObject.AddComponent<PlayerConvey>();
            //    obj1.GetComponent<Light>().intensity = 4;

            //    Instantiate(obj, new Vector3(437f, 51f, 1538f), new Quaternion(0f, 0f, 0f, 0f));//外部入口
            //    Instantiate(obj1, new Vector3(437f, 51f, 1538f), new Quaternion(0f, 0f, 0f, 0f));

            //    AB.Unload(false);

            //}
            //catch (Exception e)
            //{
            //    ModAPI.Log.Write("Error:CaveMap.unity3d文件不存在或读取失败" + "\n" + e.ToString());
            //}



        }
        void Update()
        {
            if (LocalPlayer.Stats == null || ClashMods)//游戏还没开始时,或存在冲突Mod时返回，不然Unity有报错，虽然不影响游戏.
            {
                return;
            }

            if (ModAPI.Input.GetButtonDown("menu"))
            {
                if (visible)
                {
                    //关闭界面
                    LocalPlayer.FpCharacter.UnLockView();
                    if (this.ShouldEquipLeftHandAfter)
                    {
                        LocalPlayer.Inventory.EquipPreviousUtility(false);
                    }
                    if (this.ShouldEquipRightHandAfter)
                    {
                        LocalPlayer.Inventory.EquipPreviousWeaponDelayed();
                    }//恢复之前手上装备

                }
                else
                {
                    //显示界面
                    LocalPlayer.FpCharacter.LockView(true);
                    //完美的隐藏武器和恢复
                    ShouldEquipLeftHandAfter = !LocalPlayer.Inventory.IsLeftHandEmpty();
                    ShouldEquipRightHandAfter = !LocalPlayer.Inventory.IsRightHandEmpty();

                    if (!LocalPlayer.Inventory.IsRightHandEmpty())
                    {
                        if (!LocalPlayer.Inventory.RightHand.IsHeldOnly)
                        {
                            LocalPlayer.Inventory.MemorizeItem(Item.EquipmentSlot.RightHand);
                        }
                        LocalPlayer.Inventory.StashEquipedWeapon(false);
                    }
                    if (!LocalPlayer.Inventory.IsLeftHandEmpty())
                    {
                        LocalPlayer.Inventory.MemorizeItem(Item.EquipmentSlot.LeftHand);
                        LocalPlayer.Inventory.StashLeftHand();
                    }
                }
                visible = !visible;

            }





            if (SceneManager.GetActiveScene().name == "TitleScene")
            {
                //if (GameObject.Find("NetworkManagerObj"))
                //{
                //    Destroy(GameObject.Find("NetworkManagerObj"));
                //}
                GameObject.Find("SqNetworkManagerObj").SetActive(false);

            }
            else
            {
                if (!GameSetup.IsSinglePlayer)//
                {
                    GameObject.Find("SqNetworkManagerObj").SetActive(true);
                    //new GameObject("NetworkManagerObj").AddComponent<Network.SqNetworkManager>();
                    //Network.SqNetworkManager.instance.onGetMessage += Network.SqCommandReader.OnCommand;
                }
            }






            if (!UnityEngine.Input.GetMouseButton(1))//如果处于架刀状态则退出
            {
                if (LocalPlayer.Stats.Stamina > 10f)//玩家耐力
                {
                    if (UnityEngine.Input.GetMouseButton(0) & jqiBool && LocalPlayer.Inventory.HasInSlot(Item.EquipmentSlot.RightHand, 180))//判断玩家右手是否存在固定武器
                    {
                        Keytime += Time.deltaTime;
                    }
                    else if (UnityEngine.Input.GetMouseButtonUp(0) && jqiBool && LocalPlayer.Inventory.HasInSlot(Item.EquipmentSlot.RightHand, 180))//长按判定，当松开鼠标时
                    {
                        
                        DelayJudgment(Keytime);
                        Keytime = 0f;

                    }
                }
            }
            


            if (ModAPI.Input.GetButtonDown("sharkjn") && sharkEnergy >= 100)// 
            {
                SendSwordQi( 2,Camera.main.transform.position, Camera.main.transform.rotation);
                Instantiate(shark, Camera.main.transform.position, Camera.main.transform.rotation);
                sharkEnergy -= 100;
                //Invoke("Shark", 10f);
            }

            

        }

        void OnGUI()
        {

            ClashModsCheck();

            if (visible)
            {
                if (this.labelStyle == null)
                {
                    this.labelStyle = new GUIStyle(GUI.skin.label);
                    this.labelStyle.fontSize = 24;
                    this.labelStyle.alignment = TextAnchor.MiddleCenter;
                }
                int width = Screen.width / 2;//屏幕宽度
                //int height = Screen.height;//屏幕高度
                GUI.Label(new Rect(width - 200f, 30, 400f, 250f), "剑气传说", GUI.skin.window);//界面位置

                GUI.Label(new Rect(width - 155f, 100f, 100f, 30f), "能量：" + sharkEnergy.ToString());
                GUI.Label(new Rect(width - 155f, 140, 200f, 30f), "测试：" + LoadText);
                GUI.Label(new Rect(width - 155f, 180, 200f, 30f), "测试：" + pztag);
                GUI.Label(new Rect(width - 155f, 220, 200f, 30f), "wepDam：" + LoadValue.ToString());

                if (GUI.Button(new Rect(width - 50f, 60, 100f, 30f), Open))
                {
                    jqiBool = !jqiBool;
                    if (jqiBool)
                    {
                        Open = "已开启";
                    }
                    else
                    {
                        Open = "已关闭";
                    }

                }


            }
            else
            {
                
                if (SceneManager.GetActiveScene().name == "TitleScene" || sharkEnergy <= 0)//标题场景/如果没有能量则返回
                {
                    return;
                }

                EnergyBarUI(sharkEnergy);
                //int width = Screen.width / 2;//屏幕宽度
                //GUI.Label(new Rect(width - 100f, 60, 200, 30),"Name:" + pzname);
                //GUI.Label(new Rect(width - 100f, 90, 200, 30),"Tag:" + pztag);



            }

            



            


        }

        /// <summary>
        /// 延迟判定
        /// </summary>
        /// <param name="time"></param>
        public void DelayJudgment(float time)
        {
            if(time == 0f)
            {
                return;
            }
            if(time <= 0.6f)
            {
                jqiBool = false;
                try
                {
                    qics++;
                    qics = Mathf.Clamp(qics, 0, 4);
                    SendSwordQi(1, Camera.main.transform.position, Camera.main.transform.rotation, qics);
                    LocalPlayer.Stats.Stamina -= 3f;
                    Keytime = 0f;
                    Invoke("JianQi", 0.4f);

                }
                catch
                {
                    jqiBool = true;
                }
            }
            else if(time >= 0.6f && time <= 3f)
            {
                Keytime = 0f;
                SendSwordQi(3, Camera.main.transform.position, Camera.main.transform.rotation);
                SyncJianQiBash(Camera.main.transform.position, Camera.main.transform.rotation);
                Invoke("BashTime", 1.8f);//重击的挥刀时间间隔限制
            }
        }

        

        /// <summary>
        /// Mod冲突检查
        /// </summary>
        public void ClashModsCheck()
        {
            if (LocalPlayer.Stats != null)//玩家数据不为空说明玩家开始游戏
            {
                if (ClashMods)
                {
                    if (LocalPlayer.FpCharacter != null)//说明玩家已加载
                    {
                        LocalPlayer.FpCharacter.LockView(true);//锁定视图
                        LocalPlayer.FpCharacter.MovementLocked = true;//锁定角色移动
                    }
                    if (this.labelStyle == null)
                    {
                        this.labelStyle = new GUIStyle(GUI.skin.label);
                        this.labelStyle.fontSize = 24;
                        this.labelStyle.alignment = TextAnchor.MiddleCenter;
                    }
                    int width = Screen.width / 2;//屏幕宽度
                    int height = Screen.height;//屏幕高度

                    GUI.Label(new Rect(0, 0, Screen.width, height), "Mod存在冲突", GUI.skin.window);

                    GUI.Label(new Rect(0, -150f, Screen.width, 600f), "剑气Mod(SwordQi)与Simple Player Markers冲突！\n\n<color=red>剑气Mod无法正常运行！</color>\n\n请请取消启用<color=green>Simple Player Markers</color>并重启游戏！", labelStyle);
                    if (GUI.Button(new Rect(width - 50f, (height / 2) + 100, 100f, 40f), "返回主菜单"))
                    {
                        SceneManager.LoadScene("TitleScene");
                    }
                }
            }


            if (ClashMods && SceneManager.GetActiveScene().name == "TitleScene")//主菜单界面提示
            {


                if (this.labelStyle == null)
                {
                    this.labelStyle = new GUIStyle(GUI.skin.label);
                    this.labelStyle.fontSize = 24;
                    this.labelStyle.alignment = TextAnchor.MiddleCenter;
                }

                int width = Screen.width;//屏幕宽度
                //int height = Screen.height;//屏幕高度

                GUI.Label(new Rect(0, -150f, width, 600f), "剑气Mod(SwordQi)与Simple Player Markers冲突！\n\n<color=red>剑气Mod无法正常运行！</color>\n\n请取消启用<color=green>Simple Player Markers</color>并重启游戏！", labelStyle);

            }
        }

        public void EnergyBarUI(int currentValue)
        {
            //HPBG.SetPixel(10, 10, new Color(0f, 0.8f, 0f));
            currentValue *= 2;//扩大2倍，这样的话传入值即可为0~100
            int width = Screen.width / 2;//屏幕宽度
                                         //GUI.Box(new Rect(width - 100f,60,200,30), "");
            currentValue = Mathf.Clamp(currentValue, 0, 200);//限制数值大小
            if (currentValue == 200)
            {
                for (int x = 0; x < HPBG.width; x++)
                {
                    for (int y = 0; y < HPBG.height; y++)
                    {
                        HPBG.SetPixel(x, y, new Color(1f, 0f, 0f));

                    }
                }
                HPBG.Apply();
            }
            if (currentValue < 200)
            {
                for (int x = 0; x < HPBG.width; x++)
                {
                    for (int y = 0; y < HPBG.height; y++)
                    {
                        HPBG.SetPixel(x, y, new Color(0f, 0.6f, 0f));

                    }
                }
                HPBG.Apply();
            }

            GUI.DrawTexture(new Rect(width - 100f, 30f, currentValue, 15f), HPBG);//能量条


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



        public void BashTime()
        {
            jqiBool = true;
        }


        public void JianQi()//剑气，本地的
        {
            
            if(qics == 4)
            {
                Instantiate(jqi_4, Camera.main.transform.position, Camera.main.transform.rotation);
            }
            else
            {
                Instantiate(jqi, Camera.main.transform.position, Camera.main.transform.rotation);
            }
            
            jqiBool = true;
        }
        public static void SyncJianQi(Vector3 SwordQiPosition, Quaternion SwordQiRotation,int fourth)//剑气,同步的
        {
            if (fourth == 4)
            {
                Instantiate(jqi_4, SwordQiPosition, SwordQiRotation);
            }
            else
            {
                Instantiate(jqi, SwordQiPosition, SwordQiRotation);
            }
        }
        public static void SyncJianQiBash(Vector3 SwordQiPosition, Quaternion SwordQiRotation)//重击
        {
            Instantiate(jqibash, SwordQiPosition, SwordQiRotation);
        }
        public static void SyncShark(Vector3 SwordQiPosition, Quaternion SwordQiRotation)//鲨鱼技能
        {
            Instantiate(shark, SwordQiPosition, SwordQiRotation);
        }
        

        private static void SendSwordQi(int features, Vector3 pos, Quaternion quat,int fourth = 0)
        {//发送同步
            if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)//是单人游戏/是联机房主
            {
                
                if (BoltNetwork.isRunning)//网络是否正在运行,如果是则说明自己是房主
                {
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())//使用内存流
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))//二进制写入程序
                        {
                            //w.Write(3);//特征码，用于switch判断再哪个分支
                            w.Write(features);//技能码，用于判断释放的是哪个技能
                            w.Write(fourth);//是否为第四道剑气

                            w.Write(pos.x);
                            w.Write(pos.y);
                            w.Write(pos.z);

                            w.Write(quat.x);
                            w.Write(quat.y);
                            w.Write(quat.z);
                            w.Write(quat.w);

                            w.Close();
                        }
                        Network.SqNetworkManager.SendLine(answerStream.ToArray(), Network.SqNetworkManager.Target.Others);
                        answerStream.Close();
                    }
                }
            }
            else if (GameSetup.IsMpClient)//是否为Mp客户端//玩家进入其他房间的状态
            {
                
                using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                {
                    using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                    {
                        //w.Write(3);
                        w.Write(features);
                        w.Write(fourth);

                        w.Write(pos.x);
                        w.Write(pos.y);
                        w.Write(pos.z);

                        w.Write(quat.x);
                        w.Write(quat.y);
                        w.Write(quat.z);
                        w.Write(quat.w);

                        w.Close();
                    }
                    Network.SqNetworkManager.SendLine(answerStream.ToArray(), Network.SqNetworkManager.Target.Others);
                    answerStream.Close();
                }
            }
        }




    }
}
