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
using System.Linq;

namespace SwordQi
{
    public class SwordQi : MonoBehaviour
    {
        [ExecuteOnGameStart]
        private static void AddMeToScene()
        {
            new GameObject("SwordQi").AddComponent<SwordQi>();
        }

        public static SwordQi SwordQiWhole;//声明一个该类的静态变量，其他类就可以直接用这个变量访问到该类下的，公开方法和变量，而不需要把访问方法变为静态
        public static Yuan_WeaponData ModWeapon;//不过记得要赋值，好像只有在Start()里初始化才有用
        public static ResInspection Resload;
        public static PlayerSfx playSfx;

        public GameObject jqi;
        public GameObject jqibash;
        public GameObject shark;
        public GameObject jqi_4;
        public GameObject wuqi;
        public GameObject Menu;
        public GameObject yuan_KatanaHeld;
        public GameObject wp_sickle;
        public GameObject Beibao;
        public GameObject EnergyBar;
        public GameObject MapShouji;
        public GameObject yuan_Compass;
        public GameObject Lighting;
        public GameObject Lighting_OnOff;

        public GameObject ShoujiMap;
        public GameObject WeaponUI;
        public GameObject Weapon_ui;
        public GameObject Energy_ui;
        public GameObject CurrentMap_ui;
        public GameObject FellowPlayer;
        public GameObject EnemyIcon;
        public GameObject FellowPlayer_ui;
        public GameObject EnemyIcon_ui;
        public GameObject ShouJiTime_ui;
        public GameObject day_ui;

        public GameObject Player_icon_ui;

        public Sprite Forest_map;
        public Sprite Cave_map;


        public GameObject menu_1;
        public GameObject wuqi_1;
        public GameObject wuqi_LD_1;
        public GameObject SwordQiPack;
        public GameObject MapShouji_low;
        public GameObject ShoujiMap_1;
        //public GameObject yuan_stickHeldUpgraded;
        public GameObject yuan_AxePlaneHeld;
        //public GameObject TitleScreenMenu;
        public GameObject hudGui;

        //public VideoClip dalang;

        protected GUIStyle labelStyle;
        //public Texture2D HPBG;

        //private bool ShouldEquipLeftHandAfter;//老版本收起武器用的变量
        //private bool ShouldEquipRightHandAfter;
        private bool visible;
        public bool jqiBool;
        public bool ClashMods;
        //public bool SwordQiPackBool = false;
        public bool NetworkBool;
        public bool GetOriginalDataBool;
        public bool MenuBool;
        public bool ItemsBackpack;
        public bool CurrentMap;


        public bool PackBool;

        //public bool ResLoaded;

        public int sharkEnergy;
        public int qics;
        public int Yuan_KatDamage;
        public int CurrentMap_ui_localScale;
        public int FellowPlayer_Count;
        public int EnemyIcon_Count;


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
            
            if (ModAPI.Mods.LoadedMods.ContainsKey("Simple_Player_Markers"))//判断是否存在冲突Mod
            {
                ClashMods = true;
                return;
            }
            //数值初始化
            jqiBool = true;
            ClashMods = false;
            MenuBool = false;
            GetOriginalDataBool = false;
            ItemsBackpack = false;
            CurrentMap = true;

            Yuan_KatDamage = 0;
            CurrentMap_ui_localScale = 0;
            FellowPlayer_Count = 0;
            EnemyIcon_Count = 0;


            SwordQiWhole = this;
            Resload = new ResInspection();
            ModWeapon = new Yuan_WeaponData();
            playSfx = new PlayerSfx();

            StartCoroutine(Resload.AssetCheck());
            StartCoroutine(ModWeapon.YuanValueInit());

            if (!GameSetup.IsSinglePlayer)//不判断会导致进单人模式时游戏强退
            {
                
                new GameObject("SqNetworkManagerObj").AddComponent<Network.SqNetworkManager>();
                Network.SqNetworkManager.instance.onGetMessage += Network.SqCommandReader.OnCommand;
                NetworkBool = true;
            }
            

            //HPBG = new Texture2D(10, 10);

        }
        void Update()
        {
            try
            {
                if (ClashMods)//存在冲突Mod时返回，不然Unity有报错，虽然不影响游戏.
                {
                    return;
                }

                if (GameSetup.IsSinglePlayer && NetworkBool)//检查网络模块是否应该启用
                {
                    //Debug.Log("单人模式!");
                    GameObject.Find("SqNetworkManagerObj").SetActive(false);
                    NetworkBool = false;
                }
                if (LocalPlayer.Stats == null)//游戏还没开始时返回，不然Unity有报错，虽然不影响游戏.
                {
                    //Debug.Log("游戏还没开始!");
                    return;
                }

                if (!GetOriginalDataBool)//第一次生效
                {
                    InvokeRepeating("GetYuan_KatDamage", 5f, 10f);//第一次time秒后调用，后面每repeatRate秒调用一次
                    StartCoroutine(GetOriginalData());
                    GetOriginalDataBool = true;
                }
                if (SwordQiPack && !ItemsBackpack)//第一次生效
                {
                    AddItemsBackpack(SwordQiPack.transform);
                    ItemsBackpack = true;
                }
                

                if (ModAPI.Input.GetButtonDown("menu") && !PackBool && !TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Book) && !TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Menu) && !TheForest.Utils.LocalPlayer.Stats.Dead)
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
                        MenuBool = !MenuBool;//第一次后为真
                    }
                    else
                    {
                        CloseMenu();
                    }

                    visible = !visible;

                }
                //-------------

                if (ModAPI.Input.GetButtonDown("SwordQiPack") && !MenuBool)//mod背包
                {

                    if (SwordQiPack)
                    {
                        if (Weapon_ui == null)
                        {
                            Weapon_ui = Instantiate(WeaponUI, SwordQiPack.transform);
                            Weapon_ui.SetActive(false);
                        }

                        //MenuBoll = !MenuBoll;
                        if (!TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Inventory) && !TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Menu) && !TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Book) && !TheForest.Utils.LocalPlayer.FpCharacter.jumping && !TheForest.Utils.LocalPlayer.Stats.Dead)
                        {
                            if (hudGui == null)
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
                        else if (PackBool)//不加PackBool，在原背包界面时会直接进入，导致更改了原游戏的视图值，从而引起背包重叠与混乱
                        {
                            CloseBack();
                        }

                    }

                }
                if (TheForest.Utils.Input.GetButtonDown("Esc"))
                {
                    CloseBack();
                }

                if (!UnityEngine.Input.GetMouseButton(1) && !MenuBool && !PackBool)//如果处于架刀状态，或已打开菜单则退出
                {
                    if (Camera.main.transform.localEulerAngles.x <= 43.33f || Camera.main.transform.localEulerAngles.x > 72.5f)
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
                }

                if (ModAPI.Input.GetButtonDown("sharkjn") && sharkEnergy >= 100)// 鲨鱼
                {
                    SendSwordQi(2, Camera.main.transform.position, Camera.main.transform.rotation);
                    Instantiate(shark, Camera.main.transform.position, Camera.main.transform.rotation);
                    sharkEnergy -= 100;
                    //Invoke("Shark", 10f);
                }
                if(UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if(Lighting_OnOff)
                    {
                        Lighting_OnOff.SetActive(true);
                    }
                }
                if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (Lighting_OnOff)
                    {
                        Lighting_OnOff.SetActive(false);
                    }
                }
                
                
                //if (UnityEngine.Input.GetKeyDown(KeyCode.O))
                //{

                //    //LocalPlayer.FpCharacter.LockView(true);//锁定视角
                //    //Cursor.lockState = CursorLockMode.None;
                //    //LocalPlayer.Inventory.Equip(80, false);//飞机斧
                //    //LocalPlayer.Inventory.StashEquipedWeapon(false);
                //}

                PlayerMapPos();
                FellowPlayerPos();
                EnemyIconPos();

                if(ShoujiMap_1)
                {
                    if(day_ui == null)
                    {
                        day_ui = ShoujiMap_1.transform.Find("MapWin/zhantailang/zhouyei").gameObject;
                    }
                    if (Clock.Dark)
                    {
                        day_ui.gameObject.GetComponent<Text>().text = "夜晚：";
                    }
                    else
                    {
                        day_ui.gameObject.GetComponent<Text>().text = "白天：";
                    }

                    if (ShouJiTime_ui == null)
                    {
                        ShouJiTime_ui = ShoujiMap_1.transform.Find("MapWin/zhantailang/time").gameObject;
                        
                    }
                    else
                    {
                        //88-270为晚上
                        //TimeOfDay / 15后，18约为早上6点，0为中午12点，6为晚上18点。反过来
                        float time = TheForestAtmosphere.Instance.TimeOfDay / 15f;
                        float xiaoshi = Mathf.Floor(time);//向下取整，不四舍五入
                        float num = Mathf.Floor((time - xiaoshi) * 100);//
                        int fezhon = (int)Mathf.Floor(num / 1.65f);
                        string fe = fezhon.ToString();

                        if (xiaoshi < 12f)
                        {
                            xiaoshi += 12;
                        }
                        else
                        {
                            xiaoshi -= 12;
                        }
                        if(fezhon < 10)
                        {
                            fe = "0" + fezhon.ToString();
                        }
                        ShouJiTime_ui.gameObject.GetComponent<Text>().text = xiaoshi.ToString() + "：" + fe;
                    }
                }
                

            }
            catch 
            {
                
            }
        }

        void OnGUI()
        {

            ClashModsCheck();
            

            if (Energy_ui == null)
            {
                Energy_ui = Instantiate(EnergyBar);
            }

            if (Energy_ui != null && (SceneManager.GetActiveScene().name == "TitleScene" || sharkEnergy <= 0 || TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Inventory) || TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Menu) || TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Book)))//标题场景/没有能量/如果已打mod背包/在暂停菜单界面，则返回
            {
                Energy_ui.gameObject.SetActive(false);
            }
            else if (Energy_ui != null && !Energy_ui.gameObject.activeSelf)
            {
                Energy_ui.gameObject.SetActive(true);
            }
            EnergyUI(sharkEnergy);

            if (visible)
            {
               

            }
            else
            {
                
                //EnergyBarUI(sharkEnergy);


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
        /// 能量条显示
        /// </summary>
        /// <param name="EnergyValue"></param>
        public void EnergyUI(int EnergyValue)
        {
            if(Energy_ui == null)
            {
                return;
            }
            Energy_ui.transform.Find("Underlay/FillImage").GetComponent<Image>().fillAmount = EnergyValue / 200f;
            if (EnergyValue < 100)
            {
                Energy_ui.transform.Find("Underlay/FillImage").GetComponent<Image>().color = new Color(0f, 0.6f, 0f);

            }else  if (EnergyValue >= 100 && EnergyValue != 200)
            {
                Energy_ui.transform.Find("Underlay/FillImage").GetComponent<Image>().color = new Color( 1f, 0.76f, 0f);

            }else if(EnergyValue == 200)
            {
                Energy_ui.transform.Find("Underlay/FillImage").GetComponent<Image>().color = new Color(1f, 0f, 0f);
            }
        }

        /// <summary>
        /// 给mod界面按钮添加触发事件
        /// </summary>
        /// <param name="ui"></param>
        public void BindUI(GameObject ui)
        {
            ui.SetActive(true);
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
            
        }

        /// <summary>
        /// 克隆背包
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetOriginalData()
        {
            try
            {
                if(yuan_KatanaHeld == null || yuan_AxePlaneHeld == null || yuan_Compass == null)
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
                        if(go.name == "Compass_held")
                        {
                            yuan_Compass = go;
                        }
                        if (yuan_KatanaHeld && yuan_AxePlaneHeld && yuan_Compass)
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

        /// <summary>
        /// 获取武士刀伤害
        /// </summary>
        public void GetYuan_KatDamage()
        {
            if (LocalPlayer.Inventory.HasInSlot(Item.EquipmentSlot.RightHand, 180))//如果已装备了武士刀，直接从固定路径获取，速度会快一点。没有准备的话无法通过Find找到
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
                                    //wuqi_1.gameObject.SetActive(false);

                                }
                                else if(wuqi_1.gameObject.activeSelf)//阻止重复修改影响性能
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
                                if(wuqi_1 == null || !wuqi_1.gameObject.activeSelf)//当一次都没有装备过武器，或已经还原的时候返回，因为我在重写原游戏的准备方法里有调用
                                {
                                    return;
                                }
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
                                    wuqi_LD_1 = Instantiate(wp_sickle, WeaponObject.transform);
                                    //wuqi_LD_1.gameObject.SetActive(false);
                                }
                                else if(wuqi_LD_1.gameObject.activeSelf)
                                {
                                    return;
                                }
                                WeaponObject.transform.GetChild(2).gameObject.SetActive(false);
                                wuqi_LD_1.gameObject.SetActive(Toggle);//
                                WeaponObject.transform.GetChild(0).GetComponent<weaponInfo>().thisCollider = wp_sickle.GetComponent<BoxCollider>();

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
                                
                                if (wuqi_LD_1 == null || !wuqi_LD_1.gameObject.activeSelf)
                                {
                                    return;
                                }
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
                        case "Compass":
                            if (Toggle)
                            {
                                if (MapShouji_low == null)
                                {
                                    MapShouji_low = Instantiate(MapShouji, WeaponObject.transform);//手机模型
                                    ShoujiMap_1 = Instantiate(ShoujiMap, MapShouji_low.transform);//手机界面
                                    Lighting_OnOff = Instantiate(Lighting, MapShouji_low.transform);//手机灯
                                    Lighting_OnOff.SetActive(false);
                                    ShoujiMap_1.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
                                    
                                }
                                else
                                {
                                    MapShouji_low.SetActive(Toggle);
                                }
                                WeaponObject.transform.GetChild(0).gameObject.SetActive(false);
                                WeaponObject.transform.GetChild(1).gameObject.SetActive(false);
                                WeaponObject.transform.GetChild(2).gameObject.SetActive(false);

                            }
                            else
                            {
                                if (MapShouji_low == null || !MapShouji_low.activeSelf)
                                {
                                    return;
                                }
                                else
                                {
                                    MapShouji_low.SetActive(Toggle);
                                    WeaponObject.transform.GetChild(0).gameObject.SetActive(true);
                                    WeaponObject.transform.GetChild(1).gameObject.SetActive(true);
                                    WeaponObject.transform.GetChild(2).gameObject.SetActive(true);
                                }
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
            
            GameObject x_kat = Instantiate(Beibao, GameTran);//新武士刀
            for(int i = 0; i < x_kat.transform.childCount; i++)
            {
                x_kat.transform.GetChild(i).gameObject.AddComponent<DisplayWeaponUI>();
            }
            
            //x_kat.layer = 23;//已在Unity中更改图层
            //x_kat.transform.GetChild(0).gameObject.layer = 23;
            //x_kat.transform.localPosition = new Vector3(-2.8f, 0.2f, 2f);
            //x_kat.transform.localEulerAngles = new Vector3(90f, 189f, 0f);//Rotation:用的是欧拉角
            //x_kat.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            //GameObject x_LD = Instantiate(sickle_bak, GameTran);//死神镰刀
            //x_LD.AddComponent<DisplayWeaponUI>();
            //x_LD.transform.localPosition = new Vector3(-2.5f, 0.2f, 1.8f);
            //x_LD.transform.localEulerAngles = new Vector3(90f, 10f, 0f);
            //x_LD.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
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
                Invoke("SetPackBool", 0.3f);
                SwordQiPack.SetActive(false);
            }
        }

        public void CloseMenu()
        {
            MenuBool = !MenuBool;
            if (MenuBool)
            {
                LocalPlayer.FpCharacter.LockView(true);//锁定视角
                TheForest.Utils.Input.SetState(TheForest.Utils.InputState.Inventory, true);//设置视图为库存
            }
            else
            {
                LocalPlayer.FpCharacter.UnLockView();
                TheForest.Utils.Input.SetState(TheForest.Utils.InputState.Inventory, false);
            }
            menu_1.SetActive(MenuBool);
        }

        /// <summary>
        /// 更新玩家在手机上的位置
        /// </summary>
        public void PlayerMapPos()
        {
            try
            {
                if (MapShouji_low)
                {
                    if (Player_icon_ui == null)//获取玩家图标对象
                    {
                        Player_icon_ui = ShoujiMap_1.transform.Find("MapWin/map/Player_icon").gameObject;
                    }
                    else
                    {
                        Player_icon_ui.transform.localPosition = new Vector3(-TheForest.Utils.LocalPlayer.Transform.position.x, -TheForest.Utils.LocalPlayer.Transform.position.z, 0f);
                        Player_icon_ui.transform.localEulerAngles = new Vector3(0f, 0f, -TheForest.Utils.LocalPlayer.Transform.rotation.eulerAngles.y - 90f);
                    }


                    if (CurrentMap_ui == null)
                    {
                        CurrentMap_ui = ShoujiMap_1.transform.Find("MapWin/map").gameObject;
                        CurrentMap_ui.transform.localPosition = new Vector3(-TheForest.Utils.LocalPlayer.Transform.position.x * CurrentMap_ui.transform.localScale.x, TheForest.Utils.LocalPlayer.Transform.position.y * CurrentMap_ui.transform.localScale.y, 0f);
                    }
                    else
                    {
                        //更新地图位置，使玩家位置居中于手机中心
                        //限制地图移动范围
                        float mapMax_x = ((3500 * CurrentMap_ui.transform.localScale.x) - 150) / 2;//需要加双重括号，否则它的结果=450？？
                        float mapMax_y = ((3500 * CurrentMap_ui.transform.localScale.y) - 300) / 2;
                        float Current_mapMax_x = -TheForest.Utils.LocalPlayer.Transform.position.x * CurrentMap_ui.transform.localScale.x;
                        float Current_mapMax_y = TheForest.Utils.LocalPlayer.Transform.position.z * CurrentMap_ui.transform.localScale.y;
                        if (Current_mapMax_x < -mapMax_x)
                        {
                            Current_mapMax_x = -mapMax_x;
                        }
                        if(Current_mapMax_x > mapMax_x)
                        {
                            Current_mapMax_x = mapMax_x;
                        }
                        
                        if (Current_mapMax_y < -mapMax_y)
                        {
                            Current_mapMax_y = -mapMax_y;
                        }
                        if(Current_mapMax_y > mapMax_y)
                        {
                            Current_mapMax_y = mapMax_y;
                        }
                        
                        CurrentMap_ui.transform.localPosition = new Vector3(Current_mapMax_x, Current_mapMax_y, 0f);
                    }

                    if (UnityEngine.Input.GetMouseButtonDown(2))//鼠标中键固定改变地图缩放
                    {
                        CurrentMap_ui_localScale++;
                        switch (CurrentMap_ui_localScale)
                        {
                            case 1:
                                CurrentMap_ui.transform.localScale = new Vector3(0.25f, 0.25f, 0f);
                                break;
                            case 2:
                                CurrentMap_ui.transform.localScale = new Vector3(0.4f, 0.4f, 0f);
                                break;
                            //case 3:
                            //    CurrentMap_ui.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                            //    break;

                            default:
                                CurrentMap_ui.transform.localScale = new Vector3(0.15f, 0.15f, 0f);
                                CurrentMap_ui_localScale = 0;
                                break;

                        }

                    }

                    //切换地图
                    if (TheForest.Utils.LocalPlayer.IsInCaves && CurrentMap)
                    {
                        CurrentMap_ui.GetComponent<Image>().sprite = Cave_map;
                        CurrentMap = false;
                    }
                    if (!TheForest.Utils.LocalPlayer.IsInCaves && !CurrentMap)
                    {
                        CurrentMap_ui.GetComponent<Image>().sprite = Forest_map;
                        CurrentMap = true;
                    }


                }

            }
            catch
            {
                Debug.Log("玩家位置更新出错！");
            }
        }
        /// <summary>
        /// 更新队友位置
        /// </summary>
        public void FellowPlayerPos()
        {
            try
            {
                if (MapShouji_low == null)
                {
                    return;
                }
                if (BoltNetwork.isRunning && TheForest.Utils.Scene.SceneTracker != null && TheForest.Utils.Scene.SceneTracker.allPlayerEntities != null)
                {
                    // Refresh players
                    //刷新玩家
                    SQPlayerManager.Players.Clear();
                    SQPlayerManager.Players.AddRange(TheForest.Utils.Scene.SceneTracker.allPlayerEntities
                        .Where(o => o.isAttached &&
                                    o.StateIs<IPlayerState>() &&
                                    LocalPlayer.Entity != o &&
                                    o.gameObject.activeSelf &&
                                    o.gameObject.activeInHierarchy &&
                                    o.GetComponent<BoltPlayerSetup>() != null)
                        .OrderBy(o => o.GetState<IPlayerState>().name)
                        .Select(o => new SQPlayer(o)));
                }
                if (BoltNetwork.isRunning)
                {
                    if (FellowPlayer == null)
                    {
                        FellowPlayer = ShoujiMap_1.transform.Find("MapWin/map/FellowPlayer").gameObject;
                    }
                    if (SQPlayerManager.Players.Count < 1)
                    {
                        //Destroy(FellowPlayer.transform.GetChild(0).gameObject);
                    }
                    if (SQPlayerManager.Players.Count != FellowPlayer_Count)
                    {
                        if (SQPlayerManager.Players.Count > FellowPlayer_Count)
                        {
                            int fell = SQPlayerManager.Players.Count - FellowPlayer_Count;
                            for (int i = 0; i < fell; i++)
                            {
                                Instantiate(FellowPlayer_ui, FellowPlayer.transform).layer = 1;
                                FellowPlayer_Count++;
                            }

                        }
                        else
                        {
                            int fell = FellowPlayer_Count - SQPlayerManager.Players.Count;
                            for (int i = 0; i < fell; i++)
                            {
                                Destroy(FellowPlayer.transform.GetChild(0).gameObject);
                                FellowPlayer_Count--;
                            }
                        }
                    }
                    int count = 0;
                    foreach (var player in SQPlayerManager.Players)//遍历玩家列表
                    {

                        FellowPlayer.transform.GetChild(count).transform.localPosition = new Vector3(-player.Position.x, -player.Position.z, 0f);
                        //FellowPlayer.transform.GetChild(count).transform.localEulerAngles = new Vector3( 0f, 0f,-player.Transform.rotation.eulerAngles.y - 90f);
                        count++;


                    }
                }
            }
            catch
            {
                Debug.Log("队友位置更新出错！");
            }
        }
        
        /// <summary>
        /// 更新敌人位置
        /// </summary>
        public void EnemyIconPos()
        {
            try
            {
                if (MapShouji_low == null)
                {
                    return;
                }
                List<GameObject> allMutants;
                if (GameSetup.IsMpClient)
                {
                    allMutants = SQLiveEnemyForClients.liveEnemies;
                }
                else
                {
                    if (TheForest.Utils.LocalPlayer.IsInCaves)
                    {
                        allMutants = new List<GameObject>(TheForest.Utils.Scene.MutantControler.activeCaveCannibals);
                        foreach (GameObject current in TheForest.Utils.Scene.MutantControler.activeInstantSpawnedCannibals)
                        {
                            if (!allMutants.Contains(current))
                            {
                                allMutants.Add(current);
                            }
                        }
                        allMutants.RemoveAll((GameObject o) => o == null);
                        allMutants.RemoveAll((GameObject o) => o != o.activeSelf);
                    }
                    else
                    {
                        allMutants = new List<GameObject>(TheForest.Utils.Scene.MutantControler.activeWorldCannibals);
                        foreach (GameObject current in TheForest.Utils.Scene.MutantControler.activeInstantSpawnedCannibals)
                        {
                            if (!allMutants.Contains(current))
                            {
                                allMutants.Add(current);
                            }
                        }
                        allMutants.RemoveAll((GameObject o) => o == null);
                        allMutants.RemoveAll((GameObject o) => o != o.activeSelf);
                    }
                }

                if (allMutants.Count > 0)//如果敌人总数大于0
                {
                    if (EnemyIcon == null)
                    {
                        EnemyIcon = ShoujiMap_1.transform.Find("MapWin/map/EnemyIcon").gameObject;
                    }
                    if (allMutants.Count != EnemyIcon_Count)
                    {
                        EnemyIcon_Count = EnemyIcon.transform.childCount;
                        if (allMutants.Count > EnemyIcon_Count)
                        {
                            int enem = allMutants.Count - EnemyIcon_Count;
                            for (int i = 0; i < enem; i++)
                            {
                                Instantiate(EnemyIcon_ui, EnemyIcon.transform).layer = 1;
                                EnemyIcon_Count++;
                            }
                        }
                        else
                        {
                            int enem = EnemyIcon_Count - allMutants.Count;
                            for (int i = 0; i < enem; i++)
                            {
                                Destroy(EnemyIcon.transform.GetChild(0).gameObject);
                                EnemyIcon_Count--;
                            }
                        }
                    }

                    int count = 0;
                    foreach (GameObject mutant in allMutants)
                    {
                        if (mutant != null)//如果敌人还存在
                        {

                            EnemyIcon.transform.GetChild(count).transform.localPosition = new Vector3(-mutant.transform.position.x, -mutant.transform.position.z, 0f);
                            EnemyIcon.transform.GetChild(count).transform.localEulerAngles = new Vector3(0f, 0f, -mutant.transform.GetChild(0).rotation.eulerAngles.y - 90f);
                            count++;

                        }
                    }
                }
                else if (EnemyIcon.transform.childCount > 0)//解决地图上没敌人时，图标残留问题
                {
                    for (int i = 0; i < EnemyIcon.transform.childCount; i++)
                    {
                        Destroy(EnemyIcon.transform.GetChild(0).gameObject);
                    }
                    EnemyIcon_Count = 0;
                }
            }
            catch
            {
                Debug.Log("敌人位置更新出错！");
            }
}

        //===========================================
        public void SetPackBool()
        {
            PackBool = false;
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
