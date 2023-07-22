using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Buildings.Creation;
using TheForest.Items.Inventory;
using TheForest.UI;
using TheForest.Utils;
using UnityEngine;

namespace SwordQi
{
    public class Override_Method : PlayerInventory
    {
        override public void ToggleInventory()
        {
            if(SwordQi.SwordQiWhole.PackBool)//作用是拦截原游戏背包的Esc键关闭，因为在打开mod背包的情况下，按Esc会报错，我mod也想用Esc关闭背包，不拦截会导致我mod代码被报错截胡了
            {
                return;
            }
            base.ToggleInventory();//执行原方法，上面相当于前置补丁
        }

        override public void TogglePauseMenu()
        {
            if (SwordQi.SwordQiWhole.PackBool)//更改mod背包打开方式后，需另外拦截"Esc"到暂停菜单
            {
                return;
            }
            base.TogglePauseMenu();
        }
    }

    public class Override_Method_2 : Create
    {
        override public void OpenBook()
        {
            if (SwordQi.SwordQiWhole.PackBool)//mod背包与书本也有冲突，隔绝，背包界面按B键也会报错
            {
                return;
            }
            base.OpenBook();
        }
    }

    public class Override_Method_3 : PlayerInventory
    {
        public override bool Equip(int itemId, bool pickedUpFromWorld)
        {
            if (!SwordQi.SwordQiWhole.PackBool && TheForest.Utils.Input.GetState(TheForest.Utils.InputState.Inventory))
            {

                switch (itemId)
                {
                    case 180:
                        //LocalPlayer.Inventory.StashEquipedWeapon(false);
                        SwordQi.modTheWeapon.WeaponAlter(SwordQi.SwordQiWhole.yuan_KatanaHeld, "KatanaHeld", false);

                        break;

                    case 80:
                        //LocalPlayer.Inventory.StashEquipedWeapon(false);
                        SwordQi.modTheWeapon.WeaponAlter(SwordQi.SwordQiWhole.yuan_AxePlaneHeld, "AxePlaneHeld", false);
                        
                        break;
                    case 173:
                        //LocalPlayer.Inventory.StashEquipedWeapon(false);
                        SwordQi.modTheWeapon.WeaponAlter(SwordQi.SwordQiWhole.yuan_Compass, "Compass", false);

                        break;

                    default:

                        break;
                }
            }
            return base.Equip(itemId, pickedUpFromWorld);
        }
    }

    /// <summary>
    /// 客户的活敌人
    /// </summary>
    public class SQLiveEnemyForClients : MonoBehaviour
    {
        [ModAPI.Attributes.ExecuteOnGameStart]
        public static void Init()
        {
            liveEnemies = new List<GameObject>();
        }
        internal static List<GameObject> liveEnemies;
        void OnEnable()
        {
            if (!liveEnemies.Contains(gameObject))
                liveEnemies.Add(gameObject);
        }
        void Start()
        {
            if (!liveEnemies.Contains(gameObject))
                liveEnemies.Add(gameObject);
        }
        void OnDisable()
        {
            if (liveEnemies.Contains(gameObject))
                liveEnemies.Remove(gameObject);
        }
        void OnDelete()
        {
            if (liveEnemies.Contains(gameObject))
                liveEnemies.Remove(gameObject);
        }

    }
    /// <summary>
    /// 敌人武器近战扩展
    /// </summary>
    public class EnemyWeaponMeleeExtension : enemyWeaponMelee
    {
        protected override void Start()
        {
            if (transform.root.gameObject.GetComponent<SQLiveEnemyForClients>() == null)
                transform.root.gameObject.AddComponent<SQLiveEnemyForClients>();
            base.Start();
        }
    }
    /// <summary>
    /// 变异人工智能网络扩展
    /// </summary>
    public class MutantAINetworkExtension : mutantAI_net
    {
        protected override void Start()
        {
            if (transform.root.gameObject.GetComponent<SQLiveEnemyForClients>() == null)
                transform.root.gameObject.AddComponent<SQLiveEnemyForClients>();
            base.Start();
        }
    }

}
