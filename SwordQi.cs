using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ModAPI.Attributes;
using TheForest.Utils;
using TheForest.Items;
using UnityEngine;
using UnityEngine.UI;
using ModAPI;
using UnityEngine.SceneManagement;
using TheForest.Items.Inventory;
using System.Collections;

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
        public static GameObject wuqi;
        public static GameObject Menu;
        public static GameObject yuan_KatanaHeld;
        public static GameObject wq_LD;

        public GameObject menu_1;
        public GameObject wuqi_1;
        public GameObject wuqi_LD_1;
        public GameObject SwordQiPack;
        //public GameObject yuan_stickHeldUpgraded;
        public GameObject yuan_AxePlaneHeld;
        public GameObject TitleScreenMenu;

        protected GUIStyle labelStyle;
        public Texture2D HPBG;

        //private bool ShouldEquipLeftHandAfter;//老版本收起武器用的变量
        //private bool ShouldEquipRightHandAfter;
        private bool visible;
        public bool jqiBool;
        public bool ClashMods;
        public bool MenuBoll;
        //public bool SwordQiPackBool = false;
        public bool NetworkBool;
        public bool GetOriginalDataBool;
        public bool MenuBool;
        public bool PackBool;

        public static bool Loaded;

        public static int sharkEnergy;
        public static int qics;
        public static int Yuan_KatDamage;

        public string Open = "已开启";
        public float Keytime = 0f;


        //==========================================测试的变量
        //public bool timeJudge = true;
        //public bool Butt = true;
        //public bool sharkbool = false;

        public static string pztag = "";
        //public static string pzname;
        
        public static string LoadText = "";
        //public static int LoadValue;

        

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!,不知道为什么在声明赋值new对象会导致游戏直接崩溃，请在方法内赋值或使用局部变量
        //public ResInspection ResIn = new ResInspection();


        void Start()
        {
            //数值初始化
            jqiBool = true;
            ClashMods = false;
            MenuBoll = false;
            GetOriginalDataBool = false;

            Loaded = false;

            Yuan_KatDamage = 0;


            if (ModAPI.Mods.LoadedMods.ContainsKey("Simple_Player_Markers"))//判断是否存在冲突Mod
            {
                ClashMods = true;
                return;
            }

            

            StartCoroutine(ResInspection.AssetCheck());
            Yuan_WeaponData yw = new Yuan_WeaponData();
            StartCoroutine(yw.YuanValueInit());

            if (!GameSetup.IsSinglePlayer)//不判断会导致进单人模式时游戏强退
            {
                
                new GameObject("SqNetworkManagerObj").AddComponent<Network.SqNetworkManager>();
                Network.SqNetworkManager.instance.onGetMessage += Network.SqCommandReader.OnCommand;
                NetworkBool = true;
            }
            

            HPBG = new Texture2D(10, 10);

        }
        void Update()
        {
            if (ClashMods)//存在冲突Mod时返回，不然Unity有报错，虽然不影响游戏.
            {
                return;
            }

            if (GameSetup.IsSinglePlayer && NetworkBool)
            {
                GameObject.Find("SqNetworkManagerObj").SetActive(false);
                NetworkBool = false;
            }
            if (LocalPlayer.Stats == null)//游戏还没开始时返回，不然Unity有报错，虽然不影响游戏.
            {
                return;
            }

            if(!GetOriginalDataBool)
            {
                InvokeRepeating("GetYuan_KatDamage", 5f, 10f);//第一次time秒后调用，后面每repeatRate秒调用一次
                StartCoroutine(GetOriginalData());
                GetOriginalDataBool = true;
            }

            if (ModAPI.Input.GetButtonDown("menu") && !PackBool)
            {
                //if (visible)
                //{
                //    //关闭界面
                //    LocalPlayer.FpCharacter.UnLockView();
                //    if (this.ShouldEquipLeftHandAfter)
                //    {
                //        LocalPlayer.Inventory.EquipPreviousUtility(false);
                //    }
                //    if (this.ShouldEquipRightHandAfter)
                //    {
                //        LocalPlayer.Inventory.EquipPreviousWeaponDelayed();
                //    }//恢复之前手上装备

                //}
                //else
                //{
                //    //显示界面
                //    LocalPlayer.FpCharacter.LockView(true);
                //    //完美的隐藏武器和恢复
                //    ShouldEquipLeftHandAfter = !LocalPlayer.Inventory.IsLeftHandEmpty();
                //    ShouldEquipRightHandAfter = !LocalPlayer.Inventory.IsRightHandEmpty();

                //    if (!LocalPlayer.Inventory.IsRightHandEmpty())
                //    {
                //        if (!LocalPlayer.Inventory.RightHand.IsHeldOnly)
                //        {
                //            LocalPlayer.Inventory.MemorizeItem(Item.EquipmentSlot.RightHand);//记忆项目,右手
                //        }
                //        LocalPlayer.Inventory.StashEquipedWeapon(false);
                //    }
                //    if (!LocalPlayer.Inventory.IsLeftHandEmpty())
                //    {
                //        LocalPlayer.Inventory.MemorizeItem(Item.EquipmentSlot.LeftHand);//记忆项目
                //        LocalPlayer.Inventory.StashLeftHand();
                //    }
                //}
                if (menu_1 == null)
                {
                    LocalPlayer.FpCharacter.LockView(true);
                    TheForest.Utils.Input.SetState(TheForest.Utils.InputState.Inventory, true);
                    menu_1 = Instantiate(Menu);
                    BindUI(menu_1);
                    MenuBoll = !MenuBoll;//第一次后为真
                    MenuBool = true;
                }
                else
                {
                    MenuBoll = !MenuBoll;
                    if (MenuBoll)
                    {
                        LocalPlayer.FpCharacter.LockView(true);//锁定视角
                        TheForest.Utils.Input.SetState(TheForest.Utils.InputState.Inventory, true);
                        MenuBool = true;
                    }
                    else
                    {
                        LocalPlayer.FpCharacter.UnLockView();
                        TheForest.Utils.Input.SetState(TheForest.Utils.InputState.Inventory, false);
                        MenuBool = false;
                    }
                    menu_1.SetActive(MenuBoll);
                }
                visible = !visible;

            }
            if (ModAPI.Input.GetButtonDown("SwordQiPack") && !MenuBool)//mod背包
            {
                
                if(SwordQiPack)
                {
                    MenuBoll = !MenuBoll;
                    if (MenuBoll)
                    {
                        LocalPlayer.FpCharacter.LockView(true);//锁定视角
                        Time.timeScale = 0;
                        TheForest.Utils.Input.SetState(TheForest.Utils.InputState.Inventory, true);
                        PackBool = true;
                    }
                    else
                    {
                        LocalPlayer.FpCharacter.UnLockView();
                        Time.timeScale = 1;
                        TheForest.Utils.Input.SetState(TheForest.Utils.InputState.Inventory, false);
                        PackBool = false;
                    }
                    SwordQiPack.SetActive(MenuBoll);
                }

            }



            //网络
            if (SceneManager.GetActiveScene().name == "TitleScene")
            {
                //try
                //{
                //    if (TitleScreenMenu == null)
                //    {
                //        TitleScreenMenu = GameObject.Find("AxePlaneHeld/TitleScreen/Menu");
                //    }
                //    if (SqNetworkManagerObj == null)
                //    {
                //        Instantiate(SqNetworkManagerObj);
                //        SqNetworkManagerObj.name = "SqNetworkManagerObj";
                //        SqNetworkManagerObj.AddComponent<Network.SqNetworkManager>();
                //        Network.SqNetworkManager.instance.onGetMessage += Network.SqCommandReader.OnCommand;

                //    }
                //    if (TitleScreenMenu)
                //    {
                //        if (TitleScreenMenu.transform.GetChild(1).name == "Panel - SP NewGame/Continue")
                //        {
                //            SqNetworkManagerObj.SetActive(false);
                //        }

                //    }
                //}
                //catch (Exception e)
                //{
                //    ModAPI.Log.Write("Error:网络模块加载出错！" + "\n" + e.ToString());
                //}
                
                

            }
            else
            {
                //try
                //{
                //    if (TitleScreenMenu == null)
                //    {
                //        TitleScreenMenu = GameObject.Find("AxePlaneHeld/TitleScreen/Menu");
                //    }
                //    if (TitleScreenMenu)
                //    {
                //        if (TitleScreenMenu.transform.GetChild(5).name == "Panel - MP Host/Join")
                //        {
                //            SqNetworkManagerObj.SetActive(true);
                //        }

                //    }
                //}
                //catch (Exception e)
                //{
                //    ModAPI.Log.Write("Error:网络模块加载出错！" + "\n" + e.ToString());
                //}
                
            }






            if (!UnityEngine.Input.GetMouseButton(1) && !MenuBoll)//如果处于架刀状态，或已打开菜单则退出
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


            if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            {
                
                

            }
            

        }

        void OnGUI()
        {

            ClashModsCheck();

            if (visible)
            {
                //if (this.labelStyle == null)
                //{
                //    this.labelStyle = new GUIStyle(GUI.skin.label);
                //    this.labelStyle.fontSize = 24;
                //    this.labelStyle.alignment = TextAnchor.MiddleCenter;
                //}
                //int width = Screen.width / 2;//屏幕宽度
                ////int height = Screen.height;//屏幕高度
                //GUI.Label(new Rect(width - 200f, 30, 400f, 250f), "剑气传说", GUI.skin.window);//界面位置

                //GUI.Label(new Rect(width - 155f, 100f, 100f, 30f), "能量：" + sharkEnergy.ToString());
                ////GUI.Label(new Rect(width - 155f, 140, 200f, 30f), "测试：" + LoadText);
                ////GUI.Label(new Rect(width - 155f, 180, 200f, 30f), "测试：" + pztag);
                //GUI.Label(new Rect(width - 155f, 220, 200f, 30f), "wepDam：" + LoadValue.ToString());

                //if (GUI.Button(new Rect(width - 50f, 60, 100f, 30f), Open))
                //{
                //    jqiBool = !jqiBool;
                //    if (jqiBool)
                //    {
                //        Open = "已开启";
                //    }
                //    else
                //    {
                //        Open = "已关闭";
                //    }

                //}

                //if (GUI.Button(new Rect(width - 155f, 140, 100f, 20f), "切换"))
                //{
                //    GameObject ka = GameObject.Find("player/player_BASE/jointsOffsetVR/char_Hips/char_Spine/char_Spine1/char_Spine2/char_RightShoulder/char_RightArm/char_RightForeArm/char_RightHand/char_RightHandWeapon/rightHandHeld/KatanaHeld");
                //    ka.transform.GetChild(2).gameObject.SetActive(false);
                //    Instantiate(wuqi, ka.transform);

                //}
                //if (GUI.Button(new Rect(width - 155f, 180, 100f, 20f), "库存"))
                //{
                //    GameObject kat = GameObject.Find("INVENTORY/Weapons/Katana_Sword_Inv");
                //    //GameObject wq = GameObject.Find("INVENTORY/Weapons/Katana_Sword_Inv/Katana_Sword_Inv_OFFSET");
                //    GameObject wuq = wuqi;
                //    kat.transform.GetChild(0).gameObject.SetActive(false);
                //    //kat.layer = 
                //    Instantiate(wuq,kat.transform);
                //}


            }
            else
            {
                
                if (SceneManager.GetActiveScene().name == "TitleScene" || sharkEnergy <= 0 || TheForest.Utils.Input.States[TheForest.Utils.InputState.Inventory] || TheForest.Utils.Input.States[TheForest.Utils.InputState.Menu])//标题场景/没有能量/如果已打开库存/在暂停菜单界面，则返回
                {
                    return;
                }

                EnergyBarUI(sharkEnergy);


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

        /// <summary>
        /// 怒气进度条
        /// </summary>
        /// <param name="currentValue"></param>
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

        public void BindUI(GameObject ui)
        {
            ui.transform.GetChild(0).gameObject.AddComponent<ItemBox>();//给界面添加可拖动代码，代码挂在了BJ上
            var btn1 = ui.transform.Find("BJ/ButTest").GetComponent<Button>();
            btn1.onClick.AddListener(() =>
            {//单击事件，匿名方法,也可以在AddListener()里添加自己的方法
                jqiBool = !jqiBool;
                if (jqiBool)
                {
                    btn1.transform.GetChild(0).GetComponent<Text>().text = "已开启";
                }
                else
                {
                    btn1.transform.GetChild(0).GetComponent<Text>().text = "已关闭";
                }
            });
            var To = ui.transform.Find("BJ/ToggleTest").GetComponent<Toggle>();
            To.onValueChanged.AddListener((value) =>//当侦听器有回传值的时候，方法需要有对应变量接收
            {
                GameObject ka = yuan_KatanaHeld;
                
                if(ka)
                {
                    if (value)
                    {
                        //ka.transform.GetChild(2).gameObject.SetActive(false);//原武士刀
                        if (wuqi_1 == null)
                        {
                            //wqi_KatanaHeld = yuan_KatanaHeld;//用赋值相当于引用本体，如果对其进行更改，受影响的是原对象,可以用实例化解决
                            ka.transform.GetChild(0).GetComponent<weaponInfo>().thisCollider = wuqi.GetComponent<BoxCollider>();
                            wuqi_1 = Instantiate(wuqi, ka.transform);

                        }

                        ka.transform.GetChild(2).gameObject.SetActive(false);
                        wuqi_1.gameObject.SetActive(value);//替换武士刀
                    }
                    else
                    {
                        
                        ka.transform.GetChild(2).gameObject.SetActive(true);
                        wuqi_1.gameObject.SetActive(value);//还原武士刀
                    }
                }
                
            });
            var ToLD = ui.transform.Find("BJ/ToggleTestLD").GetComponent<Toggle>();
            ToLD.onValueChanged.AddListener((value) => 
            {
                


                if (yuan_AxePlaneHeld)
                {
                    GameObject sh = yuan_AxePlaneHeld;
                    if (value)
                    {
                        
                        if (wuqi_LD_1 == null)
                        {
                            //allowBodyCut:是否可切割尸体
                            //spear，矛
                            //rock，岩石
                            //axe，斧头,木棍启用这个之后架刀就是单手，飞机斧取消启用这个，就可以保留劈砍且双手架刀
                            //smallAxe，小斧
                            //repairTool，维修工具
                            //machete，大刀
                            //chainSaw,电锯
                            //blockDamagePercent，武器屏蔽，值为0~1，1为无，0为最高屏蔽
                            //noTreeCut,为真的时候，砍树没有伤害

                            //weaponDamage;    //武器伤害，武士刀：6
                            //weaponSpeed;     //武器速度，武士刀：8.5
                            //smashDamage;    //粉碎伤害，武士刀：10
                            //tiredSpeed;    //疲惫的速度，武士刀：6.5
                            //staminaDrain;  //耐力消耗，武士刀：7
                            //soundDetectRange; //声音检测范围，武士刀：18
                            //weaponRange;	//武器射程，武士刀：1.4

                            sh.transform.GetChild(0).GetComponent<weaponInfo>().thisCollider = wq_LD.GetComponent<BoxCollider>();
                            sh.transform.GetChild(0).GetComponent<weaponInfo>().axe = false;
                            sh.transform.GetChild(0).GetComponent<weaponInfo>().noTreeCut = true;

                            sh.transform.GetChild(0).GetComponent<weaponInfo>().blockDamagePercent = 0.5f;
                            sh.transform.GetChild(0).GetComponent<weaponInfo>().weaponDamage = 15f;
                            sh.transform.GetChild(0).GetComponent<weaponInfo>().smashDamage = 20f;
                            sh.transform.GetChild(0).GetComponent<weaponInfo>().staminaDrain = 9f;

                            wuqi_LD_1 = Instantiate(wq_LD, sh.transform);

                        }

                        sh.transform.GetChild(2).gameObject.SetActive(false);
                        wuqi_LD_1.gameObject.SetActive(value);//
                    }
                    else
                    {

                        sh.transform.GetChild(2).gameObject.SetActive(true);
                        wuqi_LD_1.gameObject.SetActive(value);//
                    }
                }
            });


        }

        public IEnumerator GetOriginalData()
        {
            try
            {
                if(yuan_KatanaHeld == null || yuan_AxePlaneHeld == null)
                {
                    foreach (GameObject go in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (go.name == "KatanaHeld")//查找隐藏对象
                        {
                            yuan_KatanaHeld = go;
                        }
                        if (go.name == "AxePlaneHeld")
                        {
                            yuan_AxePlaneHeld = go;
                        }
                        if (yuan_KatanaHeld && yuan_AxePlaneHeld)
                        {
                            break;
                        }

                    }
                }

                if (SwordQiPack == null)//克隆原有背包，并清除不需要的数据
                {
                    foreach (GameObject go in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (go.name == "INVENTORY")//查找隐藏对象
                        {
                            SwordQiPack = Instantiate(go);
                            SwordQiPack.name = "SwordQiknapsack";

                            int cou = SwordQiPack.transform.childCount;//如果放到for里，每次循环后数值可能会发生变化，导致销毁次数与物体不符
                            for (int i = 0; i < cou; i++)
                            {
                                if (SwordQiPack.transform.GetChild(i).name == "BackPackVisuals")
                                {
                                    SwordQiPack.transform.GetChild(i).transform.Find("Renderers/BackPack").gameObject.SetActive(false);

                                }
                                else
                                {
                                    Destroy(SwordQiPack.transform.GetChild(i).gameObject);
                                }

                            }
                            break;
                        }

                    }

                }
            }
            catch (Exception e)
            {
                ModAPI.Log.Write("Error:获取武士刀/背包数据时出错" + "\n" + e.ToString());
            }
            yield return null;
        }

        public void GetYuan_KatDamage()
        {
            if (LocalPlayer.Inventory.HasInSlot(Item.EquipmentSlot.RightHand, 180))//如果已装备了武士刀，直接从固定路径获取，速度会快一点
            {
                Yuan_KatDamage = (int)GameObject.Find("player/player_BASE/jointsOffsetVR/char_Hips/char_Spine/char_Spine1/char_Spine2/char_RightShoulder/char_RightArm/char_RightForeArm/char_RightHand/char_RightHandWeapon/rightHandHeld/KatanaHeld").transform.GetChild(0).GetComponent<weaponInfo>().WeaponDamage;
            }
            else
            {
                
                foreach (GameObject go in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                {
                    if (go.name == "KatanaHeld")//查找隐藏对象
                    {
                        Yuan_KatDamage = (int)go.transform.GetChild(0).GetComponent<weaponInfo>().WeaponDamage;
                        break;
                    }

                }
            }
        }

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

        /// <summary>
        /// 剑气同步
        /// </summary>
        /// <param name="features">技能码</param>
        /// <param name="pos"></param>
        /// <param name="quat"></param>
        /// <param name="fourth">第几道剑气</param>
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
