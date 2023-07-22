using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TheForest.Utils;
using System.Collections;

namespace SwordQi
{
    public class ModTheWeapon
    {
        /// <summary>
        /// 克隆背包
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetOriginalData()
        {
            try
            {
                if (SwordQi.SwordQiWhole.yuan_KatanaHeld == null || SwordQi.SwordQiWhole.yuan_AxePlaneHeld == null || SwordQi.SwordQiWhole.yuan_Compass == null)
                {
                    foreach (GameObject go in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (go.name == "KatanaHeld")//查找隐藏对象
                        {
                            SwordQi.SwordQiWhole.yuan_KatanaHeld = go;
                            SwordQi.SwordQiWhole.KatanaHeldParent = go.transform.parent.gameObject;
                        }
                        if (go.name == "AxePlaneHeld")
                        {
                            SwordQi.SwordQiWhole.yuan_AxePlaneHeld = go;
                        }
                        if (go.name == "Compass_held")
                        {
                            SwordQi.SwordQiWhole.yuan_Compass = go;
                        }
                        if (SwordQi.SwordQiWhole.yuan_KatanaHeld && SwordQi.SwordQiWhole.yuan_AxePlaneHeld && SwordQi.SwordQiWhole.yuan_Compass)
                        {
                            break;
                        }

                    }
                }

                if (SwordQi.SwordQiWhole.SwordQiPack == null)//克隆原有背包，并清除不需要的数据
                {
                    foreach (GameObject go in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (go.name == "INVENTORY")//查找隐藏对象
                        {
                            SwordQi.SwordQiWhole.SwordQiPack = UnityEngine.Object.Instantiate(go);
                            SwordQi.SwordQiWhole.SwordQiPack.name = "SwordQiknapsack";

                            int cou = SwordQi.SwordQiWhole.SwordQiPack.transform.childCount;//如果放到for里，每次循环后数值可能会发生变化，导致销毁次数与物体不符
                            for (int i = 0; i < cou; i++)
                            {
                                if (SwordQi.SwordQiWhole.SwordQiPack.transform.GetChild(i).name == "BackPackVisuals")
                                {
                                    SwordQi.SwordQiWhole.SwordQiPack.transform.GetChild(i).transform.Find("Renderers/BackPack").gameObject.SetActive(false);
                                    SwordQi.SwordQiWhole.SwordQiPack.transform.GetChild(i).transform.Find("Camera").transform.localPosition = new Vector3(-0.1f, 0.5f, 0.4f);
                                    SwordQi.SwordQiWhole.SwordQiPack.transform.GetChild(i).transform.Find("Camera").transform.localEulerAngles = new Vector3(20f, 358.4f, 0f);
                                    //目标：x = 20,y = 358.4,z = 0
                                }
                                else
                                {
                                    UnityEngine.Object.Destroy(SwordQi.SwordQiWhole.SwordQiPack.transform.GetChild(i).gameObject);
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
        /// 设置武器
        /// </summary>
        /// <param name="WeaponInfo"></param>
        /// <param name="Toggle"></param>
        public void WeaponAlter(GameObject WeaponObject, string WeapName, bool Toggle)
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
                                if (SwordQi.SwordQiWhole.wuqi_1 == null)
                                {
                                    //wqi_KatanaHeld = yuan_KatanaHeld;//用赋值相当于引用本体，如果对其进行更改，受影响的是原对象,可以用实例化解决
                                    SwordQi.SwordQiWhole.wuqi_1 = UnityEngine.Object.Instantiate(SwordQi.SwordQiWhole.wuqi, WeaponObject.transform);
                                    //wuqi_1.gameObject.SetActive(false);

                                }
                                else if (SwordQi.SwordQiWhole.wuqi_1.gameObject.activeSelf)//阻止重复修改影响性能
                                {
                                    return;
                                }

                                WeaponObject.transform.GetChild(2).gameObject.SetActive(false);
                                SwordQi.SwordQiWhole.wuqi_1.gameObject.SetActive(Toggle);//替换武士刀

                                GameObject gameObject = WeaponObject.transform.GetChild(0).gameObject;

                                gameObject.GetComponent<weaponInfo>().thisCollider = SwordQi.SwordQiWhole.wuqi.GetComponent<BoxCollider>();

                                gameObject.GetComponent<weaponInfo>().weaponDamage = SwordQi.ModWeapon.Xing_KatanaHeld.weaponDamage;
                                gameObject.GetComponent<weaponInfo>().weaponSpeed = SwordQi.ModWeapon.Xing_KatanaHeld.weaponSpeed;
                                gameObject.GetComponent<weaponInfo>().smashDamage = SwordQi.ModWeapon.Xing_KatanaHeld.smashDamage;
                                gameObject.GetComponent<weaponInfo>().tiredSpeed = SwordQi.ModWeapon.Xing_KatanaHeld.tiredSpeed;
                                gameObject.GetComponent<weaponInfo>().weaponRange = SwordQi.ModWeapon.Xing_KatanaHeld.weaponRange;
                                gameObject.GetComponent<weaponInfo>().staminaDrain = SwordQi.ModWeapon.Xing_KatanaHeld.staminaDrain;
                                gameObject.GetComponent<weaponInfo>().blockDamagePercent = SwordQi.ModWeapon.Xing_KatanaHeld.blockDamagePercent;
                            }
                            else
                            {
                                if (SwordQi.SwordQiWhole.wuqi_1 == null || !SwordQi.SwordQiWhole.wuqi_1.gameObject.activeSelf)//当一次都没有装备过武器，或已经还原的时候返回，因为我在重写原游戏的准备方法里有调用
                                {
                                    return;
                                }
                                WeaponObject.transform.GetChild(2).gameObject.SetActive(true);
                                SwordQi.SwordQiWhole.wuqi_1.gameObject.SetActive(Toggle);//还原武士刀

                                GameObject gameObject = WeaponObject.transform.GetChild(0).gameObject;

                                gameObject.GetComponent<weaponInfo>().thisCollider = WeaponObject.GetComponent<BoxCollider>();

                                gameObject.GetComponent<weaponInfo>().weaponDamage = SwordQi.ModWeapon.yuan_KatanaHeld.weaponDamage;
                                gameObject.GetComponent<weaponInfo>().weaponSpeed = SwordQi.ModWeapon.yuan_KatanaHeld.weaponSpeed;
                                gameObject.GetComponent<weaponInfo>().smashDamage = SwordQi.ModWeapon.yuan_KatanaHeld.smashDamage;
                                gameObject.GetComponent<weaponInfo>().tiredSpeed = SwordQi.ModWeapon.yuan_KatanaHeld.tiredSpeed;
                                gameObject.GetComponent<weaponInfo>().weaponRange = SwordQi.ModWeapon.yuan_KatanaHeld.weaponRange;
                                gameObject.GetComponent<weaponInfo>().staminaDrain = SwordQi.ModWeapon.yuan_KatanaHeld.staminaDrain;
                                gameObject.GetComponent<weaponInfo>().blockDamagePercent = SwordQi.ModWeapon.yuan_KatanaHeld.blockDamagePercent;
                            }

                            break;

                        case "AxePlaneHeld":
                            if (Toggle)
                            {
                                if (SwordQi.SwordQiWhole.wuqi_LD_1 == null)
                                {
                                    SwordQi.SwordQiWhole.wuqi_LD_1 = UnityEngine.Object.Instantiate(SwordQi.SwordQiWhole.wp_sickle, WeaponObject.transform);
                                    //wuqi_LD_1.gameObject.SetActive(false);
                                }
                                else if (SwordQi.SwordQiWhole.wuqi_LD_1.gameObject.activeSelf)
                                {
                                    return;
                                }
                                WeaponObject.transform.GetChild(2).gameObject.SetActive(false);
                                SwordQi.SwordQiWhole.wuqi_LD_1.gameObject.SetActive(Toggle);//

                                GameObject gameObject = WeaponObject.transform.GetChild(0).gameObject;

                                gameObject.GetComponent<weaponInfo>().thisCollider = SwordQi.SwordQiWhole.wp_sickle.GetComponent<BoxCollider>();

                                gameObject.GetComponent<weaponInfo>().axe = SwordQi.ModWeapon.DeathScythe.axe;
                                gameObject.GetComponent<weaponInfo>().noTreeCut = SwordQi.ModWeapon.DeathScythe.noTreeCut;

                                gameObject.GetComponent<weaponInfo>().weaponDamage = SwordQi.ModWeapon.DeathScythe.weaponDamage;
                                gameObject.GetComponent<weaponInfo>().weaponSpeed = SwordQi.ModWeapon.DeathScythe.weaponSpeed;
                                gameObject.GetComponent<weaponInfo>().smashDamage = SwordQi.ModWeapon.DeathScythe.smashDamage;
                                gameObject.GetComponent<weaponInfo>().tiredSpeed = SwordQi.ModWeapon.DeathScythe.tiredSpeed;
                                gameObject.GetComponent<weaponInfo>().weaponRange = SwordQi.ModWeapon.DeathScythe.weaponRange;
                                gameObject.GetComponent<weaponInfo>().staminaDrain = SwordQi.ModWeapon.DeathScythe.staminaDrain;
                                gameObject.GetComponent<weaponInfo>().blockDamagePercent = SwordQi.ModWeapon.DeathScythe.blockDamagePercent;
                            }
                            else
                            {

                                if (SwordQi.SwordQiWhole.wuqi_LD_1 == null || !SwordQi.SwordQiWhole.wuqi_LD_1.gameObject.activeSelf)
                                {
                                    return;
                                }
                                WeaponObject.transform.GetChild(2).gameObject.SetActive(true);
                                SwordQi.SwordQiWhole.wuqi_LD_1.gameObject.SetActive(Toggle);//
                                                                                            //还原飞机斧数据

                                GameObject gameObject = WeaponObject.transform.GetChild(0).gameObject;

                                gameObject.GetComponent<weaponInfo>().thisCollider = WeaponObject.GetComponent<BoxCollider>();

                                gameObject.GetComponent<weaponInfo>().axe = SwordQi.ModWeapon.yuan_AxePlaneHeld.axe;
                                gameObject.GetComponent<weaponInfo>().noTreeCut = SwordQi.ModWeapon.yuan_AxePlaneHeld.noTreeCut;

                                gameObject.GetComponent<weaponInfo>().weaponDamage = SwordQi.ModWeapon.yuan_AxePlaneHeld.weaponDamage;
                                gameObject.GetComponent<weaponInfo>().weaponSpeed = SwordQi.ModWeapon.yuan_AxePlaneHeld.weaponSpeed;
                                gameObject.GetComponent<weaponInfo>().smashDamage = SwordQi.ModWeapon.yuan_AxePlaneHeld.smashDamage;
                                gameObject.GetComponent<weaponInfo>().tiredSpeed = SwordQi.ModWeapon.yuan_AxePlaneHeld.tiredSpeed;
                                gameObject.GetComponent<weaponInfo>().weaponRange = SwordQi.ModWeapon.yuan_AxePlaneHeld.weaponRange;
                                gameObject.GetComponent<weaponInfo>().staminaDrain = SwordQi.ModWeapon.yuan_AxePlaneHeld.staminaDrain;
                                gameObject.GetComponent<weaponInfo>().blockDamagePercent = SwordQi.ModWeapon.yuan_AxePlaneHeld.blockDamagePercent;
                            }
                            break;
                        case "Compass":
                            if (Toggle)
                            {
                                if (SwordQi.SwordQiWhole.MapShouji_low == null)
                                {
                                    SwordQi.SwordQiWhole.MapShouji_low = UnityEngine.Object.Instantiate(SwordQi.SwordQiWhole.MapShouji, WeaponObject.transform);//手机模型
                                    SwordQi.SwordQiWhole.ShoujiMap_1 = UnityEngine.Object.Instantiate(SwordQi.SwordQiWhole.ShoujiMap, SwordQi.SwordQiWhole.MapShouji_low.transform);//手机界面
                                    SwordQi.SwordQiWhole.Lighting_OnOff = UnityEngine.Object.Instantiate(SwordQi.SwordQiWhole.Lighting, SwordQi.SwordQiWhole.MapShouji_low.transform);//手机灯
                                    SwordQi.SwordQiWhole.Lighting_OnOff.SetActive(false);
                                    SwordQi.SwordQiWhole.ShoujiMap_1.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;

                                }
                                else
                                {
                                    SwordQi.SwordQiWhole.MapShouji_low.SetActive(Toggle);
                                }
                                WeaponObject.transform.GetChild(0).gameObject.SetActive(false);
                                WeaponObject.transform.GetChild(1).gameObject.SetActive(false);
                                WeaponObject.transform.GetChild(2).gameObject.SetActive(false);

                            }
                            else
                            {
                                if (SwordQi.SwordQiWhole.MapShouji_low == null || !SwordQi.SwordQiWhole.MapShouji_low.activeSelf)
                                {
                                    return;
                                }
                                else
                                {
                                    SwordQi.SwordQiWhole.MapShouji_low.SetActive(Toggle);
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

        //public void WeaponAlter(GameObject WeaponObject)
        //{
        //    //SwordQi.SwordQiWhole.KatanaHeldParent
        //    SwordQi.SwordQiWhole.KatanaHeld_1 = UnityEngine.Object.Instantiate(WeaponObject, SwordQi.SwordQiWhole.KatanaHeldParent.transform);

        //}






    }
}
