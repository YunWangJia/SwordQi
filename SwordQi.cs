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
using TheForest.UI;

namespace SwordQi
{
    internal class SwordQi : MonoBehaviour
    {
        [ExecuteOnGameStart]
        private static void AddMeToScene()
        {
            new GameObject("SwordQi").AddComponent<SwordQi>();
        }

        public static SwordQi SwordQiWhole;//声明一个该类的静态变量，其他类就可以直接用这个变量访问到该类下的，公开方法和变量，而不需要把访问方法变为静态
        public static Yuan_WeaponData ModWeapon;//不过记得要赋值，好像只有在Start()里初始化才有用
        public static ResInspection Resload;

        public GameObject jqi;
        public GameObject jqibash;
        public GameObject shark;
        public GameObject jqi_4;
        public GameObject wuqi;
        public GameObject Menu;
        public GameObject yuan_KatanaHeld;
        public GameObject wq_LD;
        public GameObject ka_bak;
        public GameObject LD_bak;

        public GameObject WeaponUI;
        public GameObject Weapon_ui;
        

        public GameObject menu_1;
        public GameObject wuqi_1;
        public GameObject wuqi_LD_1;
        public GameObject SwordQiPack;
        //public GameObject yuan_stickHeldUpgraded;
        public GameObject yuan_AxePlaneHeld;
        //public GameObject TitleScreenMenu;
        public GameObject hudGui;

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
        public bool ItemsBackpack;


        public bool PackBool;

        public bool Loaded;

        public int sharkEnergy;
        public int qics;
        public int Yuan_KatDamage;

        public string Open = "已开启";
        public float Keytime = 0f;


        //==========================================测试的变量
        //public bool timeJudge = true;
        //public bool Butt = true;
        public bool jumpingAttack = false;

        public string pztag = "";
        //public static string pzname;
        
        public string LoadText = "";
        //public static int LoadValue;



        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!,不知道为什么在声明赋值new对象会导致游戏直接崩溃，请在方法内赋值或使用局部变量
        //public ResInspection ResIn = new ResInspection();

        void Awake()
        {
            
        }

        void Start()
        {
            //数值初始化
            jqiBool = true;
            ClashMods = false;
            MenuBoll = false;
            GetOriginalDataBool = false;
            ItemsBackpack = false;
            Loaded = false;

            Yuan_KatDamage = 0;

            SwordQiWhole = this;
            Resload = new ResInspection();
            ModWeapon = new Yuan_WeaponData();

            if (ModAPI.Mods.LoadedMods.ContainsKey("Simple_Player_Markers"))//判断是否存在冲突Mod
            {
                ClashMods = true;
                return;
            }

            

            StartCoroutine(Resload.AssetCheck());
            StartCoroutine(ModWeapon.YuanValueInit());

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

            if (GameSetup.IsSinglePlayer && NetworkBool)//检查网络模块是否应该启用
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
            if(SwordQiPack && !ItemsBackpack)
            {
                AddItemsBackpack(SwordQiPack.transform);
                ItemsBackpack = true;
            }

            if (ModAPI.Input.GetButtonDown("menu") && !PackBool && !TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Book) && !TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Menu)&& !TheForest.Utils.LocalPlayer.Stats.Dead)
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
                visible = !visible;

                //TheForest.Utils.LocalPlayer.FpCharacter.jumping//是否在跳跃状态
                //TheForest.Utils.LocalPlayer.FpCharacter.drinking//是否在喝饮料/酒
                //TheForest.Utils.LocalPlayer.AnimControl.endGameCutScene//结束游戏场景
                //TheForest.Utils.LocalPlayer.FpCharacter.PushingSled//推雪橇
                //TheForest.Utils.LocalPlayer.Animator.GetBool("drawBowBool")//拉弓Bool
                //TheForest.Utils.LocalPlayer.AnimControl.slingShotAim//弹弓瞄准
                //TheForest.Utils.LocalPlayer.Stats.Dead//是否为死亡状态
                //TheForest.Utils.LocalPlayer.WaterViz.InWater//是否在水中
                //TheForest.UI.VirtualCursor.Instance.SetCursorType(TheForest.UI.VirtualCursor.CursorTypes.Inventory);//设置光标为库存图标
                //TheForest.UI.VirtualCursor.Instance.SetCursorType(TheForest.UI.VirtualCursor.CursorTypes.Hand);//设置光标为手，关闭库存后应用的
                //TheForest.Utils.LocalPlayer.Inventory.CurrentView = PlayerInventory.PlayerViews.ClosingInventory;//关闭库存更改视图
                //TheForest.Utils.LocalPlayer.Inventory.CurrentView = PlayerInventory.PlayerViews.Inventory;//打开库存更改视图
                //TheForest.Utils.Input.GetButtonDown("Take")//拿，应该是E键
                //TheForest.Utils.LocalPlayer.FpCharacter.jumpingAttack//跳跃攻击
                //TheForest.Utils.Input.GetButton("Jump")//跳跃按键
                //TheForest.Utils.LocalPlayer.Animator.GetBool("jumpBool");//为真时，跳跃可用，还在跳跃状态的时候应该为假
                //PlayerInventory.HideAllEquiped();//隐藏所有装备

                if (menu_1 == null)//Mod界面
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
                    CloseMenu();
                }
                

            }
            if (ModAPI.Input.GetButtonDown("SwordQiPack") && !MenuBool)//mod背包
            {
                
                if(SwordQiPack)
                {
                    if(Weapon_ui == null)
                    {
                        Weapon_ui = Instantiate(WeaponUI, SwordQiPack.transform);
                        Weapon_ui.transform.GetChild(0).gameObject.AddComponent<UIFollowMouse>();
                        Weapon_ui.SetActive(false);
                    }

                    //MenuBoll = !MenuBoll;
                    if (!TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Inventory) && !TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Menu) && !TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Book) && !TheForest.Utils.LocalPlayer.FpCharacter.jumping && !TheForest.Utils.LocalPlayer.Stats.Dead)
                    {
                        if(hudGui == null)
                        {
                            hudGui = GameObject.Find("HudGui/HUD_Ngui/Camera_HUD");//隐藏房子等图标
                            hudGui.SetActive(false);
                        }
                        else
                        {
                            hudGui.SetActive(false);
                        }
                        
                        Time.timeScale = 0;
                        TheForest.Utils.Input.SetState(TheForest.Utils.InputState.Inventory, true);//设置视图为库存
                        //TheForest.Utils.LocalPlayer.Inventory.CurrentView = PlayerInventory.PlayerViews.Inventory;//更改这个后，在背包界面才看不见房子之类的图标,后果：会造成鼠标被替换，导致用不了Unity的鼠标碰撞体检测，就无法实现装备的信息显示
                        TheForest.UI.VirtualCursor.Instance.SetCursorType(TheForest.UI.VirtualCursor.CursorTypes.Inventory);
                        LocalPlayer.FpCharacter.LockView(true);//锁定视角
                        TheForest.Utils.LocalPlayer.Sfx.PlayOpenInventory();//有打开仓库的声音

                        
                        PackBool = true;
                        SwordQiPack.SetActive(true);
                        
                    }
                    else if(PackBool)//不加PackBool，在原背包界面时会直接进入，导致更改了原游戏的视图值，从而引起背包重叠与混乱
                    {
                        CloseBack();
                    }
                    
                }

            }
            if (TheForest.Utils.Input.GetButtonDown("Esc"))
            {
                CloseBack();
            }

            if (!UnityEngine.Input.GetMouseButton(1) && !MenuBoll && !PackBool)//如果处于架刀状态，或已打开菜单则退出
            {
                if (LocalPlayer.Stats.Stamina > 10f)//玩家耐力
                {
                    if (UnityEngine.Input.GetMouseButton(0) & jqiBool && LocalPlayer.Inventory.HasInSlot(Item.EquipmentSlot.RightHand, 180))//判断玩家右手是否存在固定武器
                    {
                        Keytime += Time.deltaTime;
                    }
                    else if (UnityEngine.Input.GetMouseButtonUp(0) && jqiBool && LocalPlayer.Inventory.HasInSlot(Item.EquipmentSlot.RightHand, 180) && !TheForest.Utils.LocalPlayer.FpCharacter.jumpingAttack)//长按判定，当松开鼠标时
                    {
                        
                        DelayJudgment(Keytime);
                        Keytime = 0f;

                    }
                    else
                    {
                        jumpingAttack = true;
                    }
                }
            }
            
            if (ModAPI.Input.GetButtonDown("sharkjn") && sharkEnergy >= 100)// 鲨鱼
            {
                SendSwordQi( 2,Camera.main.transform.position, Camera.main.transform.rotation);
                Instantiate(shark, Camera.main.transform.position, Camera.main.transform.rotation);
                sharkEnergy -= 100;
                //Invoke("Shark", 10f);
            }
            


            //if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            //{
            //    //LocalPlayer.FpCharacter.LockView(true);//锁定视角
            //    //Cursor.lockState = CursorLockMode.None;
            //    //LocalPlayer.Inventory.Equip(80, false);//飞机斧
            //    LocalPlayer.Inventory.StashEquipedWeapon(false);
            //}
            

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
                
                if (SceneManager.GetActiveScene().name == "TitleScene" || sharkEnergy <= 0 || TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Inventory) || TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Menu))//标题场景/没有能量/如果已打开库存/在暂停菜单界面，则返回
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
            var btnClose = ui.transform.Find("BJ/ButTestClose").GetComponent<Button>();
            btnClose.onClick.AddListener(() =>
            {
                CloseMenu();
            });
            //var To = ui.transform.Find("BJ/ToggleTest").GetComponent<Toggle>();
            //To.onValueChanged.AddListener((bool value) => //匿名方法
            //{
            //    WeaponAlter(yuan_KatanaHeld, "KatanaHeld", value);
            //});
            //var ToLD = ui.transform.Find("BJ/ToggleTestLD").GetComponent<Toggle>();
            //ToLD.onValueChanged.AddListener((bool value) => //匿名方法
            //{

            //    WeaponAlter(yuan_AxePlaneHeld, "AxePlaneHeld", value);

            //});
            //var tobk = ui.transform.Find("BJ/ToggleBack").GetComponent<Toggle>();
            //tobk.onValueChanged.AddListener((bool value) =>
            //{
            //    LocalPlayer.FpCharacter.LockView(true);//锁定视角
            //});
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
                                    SwordQiPack.transform.GetChild(i).transform.Find("Camera").transform.localPosition = new Vector3( -0.1f, 0.5f, 0.4f);
                                    SwordQiPack.transform.GetChild(i).transform.Find("Camera").transform.localEulerAngles = new Vector3(20f, 358.4f, 0f);
                                    //目标：x = 20,y = 358.4,z = 0
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
        /// <summary>
        /// 设置武器
        /// </summary>
        /// <param name="WeaponInfo"></param>
        /// <param name="Toggle"></param>
        public void WeaponAlter(GameObject WeaponObject,string WeapName, bool Toggle)
        {
            try
            {
                if (WeaponObject)
                {
                    switch (WeapName)
                    {
                        case "KatanaHeld":
                            if (Toggle)
                            {
                                //ka.transform.GetChild(2).gameObject.SetActive(false);//原武士刀
                                if (wuqi_1 == null)
                                {
                                    //wqi_KatanaHeld = yuan_KatanaHeld;//用赋值相当于引用本体，如果对其进行更改，受影响的是原对象,可以用实例化解决
                                    wuqi_1 = Instantiate(wuqi, WeaponObject.transform);
                                    wuqi_1.gameObject.SetActive(false);

                                }
                                if(wuqi_1.gameObject.activeSelf)
                                {
                                    return;
                                }

                                WeaponObject.transform.GetChild(2).gameObject.SetActive(false);
                                wuqi_1.gameObject.SetActive(Toggle);//替换武士刀

                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().thisCollider = wuqi.GetComponent<BoxCollider>();

                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponDamage = ModWeapon.Xing_KatanaHeld.weaponDamage;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponSpeed = ModWeapon.Xing_KatanaHeld.weaponSpeed;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().smashDamage = ModWeapon.Xing_KatanaHeld.smashDamage;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().tiredSpeed = ModWeapon.Xing_KatanaHeld.tiredSpeed;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponRange = ModWeapon.Xing_KatanaHeld.weaponRange;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().staminaDrain = ModWeapon.Xing_KatanaHeld.staminaDrain;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().blockDamagePercent = ModWeapon.Xing_KatanaHeld.blockDamagePercent;
                            }
                            else
                            {

                                WeaponObject.transform.GetChild(2).gameObject.SetActive(true);
                                wuqi_1.gameObject.SetActive(Toggle);//还原武士刀
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().thisCollider = WeaponObject.GetComponent<BoxCollider>();

                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponDamage = ModWeapon.yuan_KatanaHeld.weaponDamage;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponSpeed = ModWeapon.yuan_KatanaHeld.weaponSpeed;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().smashDamage = ModWeapon.yuan_KatanaHeld.smashDamage;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().tiredSpeed = ModWeapon.yuan_KatanaHeld.tiredSpeed;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponRange = ModWeapon.yuan_KatanaHeld.weaponRange;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().staminaDrain = ModWeapon.yuan_KatanaHeld.staminaDrain;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().blockDamagePercent = ModWeapon.yuan_KatanaHeld.blockDamagePercent;
                            }

                            break;

                        case "AxePlaneHeld":
                            if (Toggle)
                            {
                                if (wuqi_LD_1 == null)
                                {
                                    wuqi_LD_1 = Instantiate(wq_LD, WeaponObject.transform);
                                    wuqi_LD_1.gameObject.SetActive(false);
                                }
                                if(wuqi_LD_1.gameObject.activeSelf)
                                {
                                    return;
                                }
                                WeaponObject.transform.GetChild(2).gameObject.SetActive(false);
                                wuqi_LD_1.gameObject.SetActive(Toggle);//
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().thisCollider = wq_LD.GetComponent<BoxCollider>();

                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().axe = ModWeapon.DeathScythe.axe;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().noTreeCut = ModWeapon.DeathScythe.noTreeCut;

                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponDamage = ModWeapon.DeathScythe.weaponDamage;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponSpeed = ModWeapon.DeathScythe.weaponSpeed;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().smashDamage = ModWeapon.DeathScythe.smashDamage;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().tiredSpeed = ModWeapon.DeathScythe.tiredSpeed;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponRange = ModWeapon.DeathScythe.weaponRange;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().staminaDrain = ModWeapon.DeathScythe.staminaDrain;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().blockDamagePercent = ModWeapon.DeathScythe.blockDamagePercent;
                            }
                            else
                            {

                                WeaponObject.transform.GetChild(2).gameObject.SetActive(true);
                                wuqi_LD_1.gameObject.SetActive(Toggle);//
                                                                       //还原飞机斧数据
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().thisCollider = WeaponObject.GetComponent<BoxCollider>();

                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().axe = ModWeapon.yuan_AxePlaneHeld.axe;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().noTreeCut = ModWeapon.yuan_AxePlaneHeld.noTreeCut;

                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponDamage = ModWeapon.yuan_AxePlaneHeld.weaponDamage;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponSpeed = ModWeapon.yuan_AxePlaneHeld.weaponSpeed;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().smashDamage = ModWeapon.yuan_AxePlaneHeld.smashDamage;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().tiredSpeed = ModWeapon.yuan_AxePlaneHeld.tiredSpeed;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().weaponRange = ModWeapon.yuan_AxePlaneHeld.weaponRange;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().staminaDrain = ModWeapon.yuan_AxePlaneHeld.staminaDrain;
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().blockDamagePercent = ModWeapon.yuan_AxePlaneHeld.blockDamagePercent;
                            }
                            break;

                        default:

                            break;
                    }
                }
            }
            catch (Exception e)
            {
                ModAPI.Log.Write("Error:更改武武器据时出错" + "\n" + e.ToString());
            }
        }

        public void AddItemsBackpack(Transform GameTran)
        {
            //inventoryItemSpreadTrigger//视图触发器，配合InventoryItemView一起使用
            //inventoryItemSpreadSetup//模型上浮

            //if(UnityEngine.Input.GetMouseButton(0))//鼠标点击模型触发XX事件，示例
            //{
            //    Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);//在鼠标位置创建一个由相机发射向前的射线
            //    RaycastHit hit;//接收器
            //    if (Physics.Raycast(ray, out hit))//如果射线有命中物体
            //    {
            //        if (hit.collider.gameObject.name == "")
            //        {

            //        }
            //    }
            //}
            

            GameObject x_kat = Instantiate(ka_bak, GameTran);//新武士刀
            x_kat.AddComponent<DisplayWeaponUI>();
            //x_kat.layer = 23;
            //x_kat.transform.GetChild(0).gameObject.layer = 23;
            x_kat.transform.localPosition = new Vector3(-2.8f, 0.2f, 2f);
            x_kat.transform.localEulerAngles = new Vector3(90f, 189f, 0f);//Rotation:用的是欧拉角

            x_kat.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            GameObject x_LD = Instantiate(LD_bak, GameTran);//死神镰刀
            x_LD.AddComponent<DisplayWeaponUI>();
            //x_LD.layer = 23;
            //x_LD.transform.GetChild(0).gameObject.layer = 23;
            x_LD.transform.localPosition = new Vector3(-2.5f, 0.2f, 1.8f);
            x_LD.transform.localEulerAngles = new Vector3(90f, 10f, 0f);

            x_LD.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }

        public void CloseBack()
        {
            if (SwordQiPack && PackBool)
            {
                DisplayWeaponUI.WeaponName = "";
                Weapon_ui.SetActive(false);
                hudGui.SetActive(true);
                Time.timeScale = 1;
                TheForest.Utils.Input.SetState(TheForest.Utils.InputState.Inventory, false);
                //TheForest.Utils.LocalPlayer.Inventory.CurrentView = PlayerInventory.PlayerViews.ClosingInventory;
                TheForest.UI.VirtualCursor.Instance.SetCursorType(TheForest.UI.VirtualCursor.CursorTypes.None);
                LocalPlayer.FpCharacter.UnLockView();
                TheForest.Utils.LocalPlayer.Sfx.PlayCloseInventory();
                PackBool = false;
                SwordQiPack.SetActive(false);
            }
        }

        public void CloseMenu()
        {
            MenuBoll = !MenuBoll;
            if (MenuBoll)
            {
                LocalPlayer.FpCharacter.LockView(true);//锁定视角
                TheForest.Utils.Input.SetState(TheForest.Utils.InputState.Inventory, true);//设置视图为库存
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
        public void SyncJianQi(Vector3 SwordQiPosition, Quaternion SwordQiRotation,int fourth)//剑气,同步的
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
        public void SyncJianQiBash(Vector3 SwordQiPosition, Quaternion SwordQiRotation)//重击
        {
            Instantiate(jqibash, SwordQiPosition, SwordQiRotation);
        }
        public void SyncShark(Vector3 SwordQiPosition, Quaternion SwordQiRotation)//鲨鱼技能
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
        private void SendSwordQi(int features, Vector3 pos, Quaternion quat,int fourth = 0)
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
