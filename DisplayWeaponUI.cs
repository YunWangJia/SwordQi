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
                switch (WeaponName)
                {
                    case "KatanaHeld"://对应哪个原武器的意思
                        SwordQi.SwordQiWhole.CloseBack();
                        //LocalPlayer.Inventory.StashEquipedWeapon(false);
                        SwordQi.modTheWeapon.WeaponAlter(SwordQi.SwordQiWhole.yuan_KatanaHeld, "KatanaHeld", true);
                        //SwordQi.modTheWeapon.WeaponAlter(SwordQi.SwordQiWhole.yuan_KatanaHeld);
                        //System.Threading.Thread.Sleep(500);//1000为1秒
                        LocalPlayer.Inventory.Equip(180, false);
                        break;
                    case "AxePlaneHeld":
                        SwordQi.SwordQiWhole.CloseBack();
                        //LocalPlayer.Inventory.StashEquipedWeapon(false);
                        SwordQi.modTheWeapon.WeaponAlter(SwordQi.SwordQiWhole.yuan_AxePlaneHeld, "AxePlaneHeld", true);
                        //System.Threading.Thread.Sleep(500);//1000为1秒
                        LocalPlayer.Inventory.Equip(80, false);
                        break;
                    case "Compass":
                        SwordQi.SwordQiWhole.CloseBack();
                        SwordQi.modTheWeapon.WeaponAlter(SwordQi.SwordQiWhole.yuan_Compass, "Compass", true);
                        LocalPlayer.Inventory.Equip(173, false);//指南针
                        break;

                    default:
                        
                        break;
                }
                
            }
        }


        public void OnMouseEnter()
        {
            

            if (this.gameObject.name == "ka_bak")
            {
                WeaponName = "KatanaHeld";
                JudgeUI(SwordQi.SwordQiWhole.Weapon_ui, SwordQi.ModWeapon.Xing_KatanaHeld);
            }
            if (this.gameObject.name == "sickle_bak")
            {
                WeaponName = "AxePlaneHeld";
                JudgeUI(SwordQi.SwordQiWhole.Weapon_ui, SwordQi.ModWeapon.DeathScythe);
            }
            if (this.gameObject.name == "dalang")
            {
                WeaponName = "Compass";
                JudgeUI(SwordQi.SwordQiWhole.Weapon_ui, SwordQi.ModWeapon.DaLangShouji);
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
            if(yuan_weapon.weaponName == "大郎手机")
            {
                UIObject.transform.Find("ui_bj/xingxi").gameObject.SetActive(false);
                UIObject.transform.Find("ui_bj/Describe").gameObject.SetActive(true);
                UIObject.transform.Find("ui_bj/wqi_name").GetComponent<Text>().text = yuan_weapon.weaponName;
                UIObject.transform.Find("ui_bj/Describe").GetComponent<Text>().text = yuan_weapon.Describe;
            }
            else
            {
                UIObject.transform.Find("ui_bj/xingxi").gameObject.SetActive(true);
                UIObject.transform.Find("ui_bj/Describe").gameObject.SetActive(false);
                UIObject.transform.Find("ui_bj/wqi_name").GetComponent<Text>().text = yuan_weapon.weaponName;
                UIObject.transform.Find("ui_bj/xingxi/Dame/amo").GetComponent<Text>().text = yuan_weapon.weaponDamage.ToString();
                UIObject.transform.Find("ui_bj/xingxi/Speed/amo").GetComponent<Text>().text = yuan_weapon.weaponSpeed.ToString();
                UIObject.transform.Find("ui_bj/xingxi/Range/amo").GetComponent<Text>().text = yuan_weapon.weaponRange.ToString();
                UIObject.transform.Find("ui_bj/xingxi/blockDamage/amo").GetComponent<Text>().text = (1f - yuan_weapon.blockDamagePercent).ToString();

                UIObject.transform.Find("ui_bj/xingxi/Dam_Slider").GetComponent<Slider>().value = yuan_weapon.weaponDamage;//最高20
                UIObject.transform.Find("ui_bj/xingxi/Speed_Slider").GetComponent<Slider>().value = yuan_weapon.weaponSpeed;//最高10
                UIObject.transform.Find("ui_bj/xingxi/Range_Slider").GetComponent<Slider>().value = yuan_weapon.weaponRange;//最高3
                UIObject.transform.Find("ui_bj/xingxi/blockDamage_Slider").GetComponent<Slider>().value = 1f - yuan_weapon.blockDamagePercent;//屏蔽游戏里数据是反着来，应该是用了百分比
            }
            
            

        }
    }
}
