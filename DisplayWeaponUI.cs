using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using TheForest.Utils;

namespace SwordQi
{
    public class DisplayWeaponUI : MonoBehaviour
    {

        public static string WeaponName;
        private void Awake()
        {
            
        }

        void Update()
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                if (WeaponName == "KatanaHeld")
                {
                    SwordQi.SwordQiWhole.CloseBack();
                    LocalPlayer.Inventory.StashEquipedWeapon(false);
                    SwordQi.SwordQiWhole.WeaponAlter(SwordQi.SwordQiWhole.yuan_KatanaHeld, "KatanaHeld", true);
                    //System.Threading.Thread.Sleep(500);//1000为1秒
                    LocalPlayer.Inventory.Equip(180, false);
                    
                }
                if (WeaponName == "AxePlaneHeld")
                {
                    SwordQi.SwordQiWhole.CloseBack();
                    LocalPlayer.Inventory.StashEquipedWeapon(false);
                    SwordQi.SwordQiWhole.WeaponAlter(SwordQi.SwordQiWhole.yuan_AxePlaneHeld, "AxePlaneHeld", true);
                    //System.Threading.Thread.Sleep(500);//1000为1秒
                    LocalPlayer.Inventory.Equip(80, false);
                    
                }
            }
        }


        public void OnMouseEnter()
        {
            

            if (this.gameObject.name == "ka_bak(Clone)")
            {
                WeaponName = "KatanaHeld";
                JudgeUI(SwordQi.SwordQiWhole.Weapon_ui, SwordQi.ModWeapon.Xing_KatanaHeld);
            }
            if (this.gameObject.name == "LD_bak(Clone)")
            {
                WeaponName = "AxePlaneHeld";
                JudgeUI(SwordQi.SwordQiWhole.Weapon_ui, SwordQi.ModWeapon.DeathScythe);
            }
            if (!SwordQi.SwordQiWhole.Weapon_ui.activeSelf)
            {
                SwordQi.SwordQiWhole.Weapon_ui.SetActive(true);
            }
        }
        public void OnMouseExit()
        {
            SwordQi.SwordQiWhole.Weapon_ui.SetActive(false);
            WeaponName = "";
        }


        public void JudgeUI(GameObject UIObject , Yuan_WeaponData.Yuan yuan_weapon)
        {
            
            UIObject.transform.Find("ui_bj/wqi_name").GetComponent<Text>().text = yuan_weapon.weaponName;
            UIObject.transform.Find("ui_bj/Dame/amo").GetComponent<Text>().text = yuan_weapon.weaponDamage.ToString();
            UIObject.transform.Find("ui_bj/Speed/amo").GetComponent<Text>().text = yuan_weapon.weaponSpeed.ToString();
            UIObject.transform.Find("ui_bj/Range/amo").GetComponent<Text>().text = yuan_weapon.weaponRange.ToString();

            UIObject.transform.Find("ui_bj/Dam_Slider").GetComponent<Slider>().value = yuan_weapon.weaponDamage;//最高20
            UIObject.transform.Find("ui_bj/Speed_Slider").GetComponent<Slider>().value = yuan_weapon.weaponSpeed;//最高10
            UIObject.transform.Find("ui_bj/Range_Slider").GetComponent<Slider>().value = yuan_weapon.weaponRange;//最高3



        }
    }
}
