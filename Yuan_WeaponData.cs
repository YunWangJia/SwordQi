using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SwordQi
{
    public class Yuan_WeaponData
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
        public struct Yuan
        {
            public string weaponName;
            /// <summary>
            /// 武器伤害
            /// </summary>
            public float weaponDamage;    //武器伤害，武士刀：6
            /// <summary>
            /// 武器速度
            /// </summary>
            public float weaponSpeed;     //武器速度，武士刀：8.5
            /// <summary>
            /// 粉碎伤害
            /// </summary>
            public float smashDamage;    //粉碎伤害，武士刀：10
            /// <summary>
            /// 疲惫的速度
            /// </summary>
            public float tiredSpeed;    //疲惫的速度，武士刀：6.5
            /// <summary>
            /// 耐力消耗
            /// </summary>
            public float staminaDrain;  //耐力消耗，武士刀：7
            //public float soundDetectRange; //声音检测范围，武士刀：18
            /// <summary>
            /// 武器范围
            /// </summary>
            public float weaponRange;	//武器射程，武士刀：1.4
            /// <summary>
            /// 武器屏蔽
            /// </summary>
            public float blockDamagePercent;// 武器屏蔽，值为0 ~1，1为无，0为最高屏蔽

            public bool axe;//斧头。木棍启用这个之后架刀就是单手，飞机斧取消启用这个，就可以保留劈砍且双手架刀
            /// <summary>
            /// 为真的时候，砍树没有伤害
            /// </summary>
            public bool noTreeCut;// 为真的时候，砍树没有伤害
            public string Describe;

        }

        public Yuan yuan_KatanaHeld;//原武士刀
        public Yuan yuan_AxePlaneHeld;//原飞机斧

        public Yuan DeathScythe;
        public Yuan Xing_KatanaHeld;
        public Yuan DaLangShouji;

        public IEnumerator YuanValueInit()
        {
            //游戏难度不影响武器基础数值
            //武士刀
            yuan_KatanaHeld.weaponDamage = 6f;
            yuan_KatanaHeld.weaponSpeed = 8.5f;
            yuan_KatanaHeld.smashDamage = 10f;
            yuan_KatanaHeld.tiredSpeed = 6.5f;
            yuan_KatanaHeld.weaponRange = 1.4f;
            yuan_KatanaHeld.staminaDrain = 7f;
            yuan_KatanaHeld.blockDamagePercent = 1f;

            //飞机斧
            yuan_AxePlaneHeld.weaponDamage = 5f;
            yuan_AxePlaneHeld.weaponSpeed = 6.5f;
            yuan_AxePlaneHeld.smashDamage = 8f;
            yuan_AxePlaneHeld.tiredSpeed = 4f;
            yuan_AxePlaneHeld.weaponRange = 0.68f;
            yuan_AxePlaneHeld.staminaDrain = 8f;
            yuan_AxePlaneHeld.blockDamagePercent = 0.8f;

            yuan_AxePlaneHeld.axe = true;
            yuan_AxePlaneHeld.noTreeCut = false;

            //新武士刀
            Xing_KatanaHeld.weaponName = "新刃-鲨齿";
            Xing_KatanaHeld.weaponDamage = 8f;
            Xing_KatanaHeld.weaponSpeed = 6.5f;
            Xing_KatanaHeld.smashDamage = 15f;
            Xing_KatanaHeld.tiredSpeed = 6.5f;
            Xing_KatanaHeld.weaponRange = 1.6f;
            Xing_KatanaHeld.staminaDrain = 7f;
            Xing_KatanaHeld.blockDamagePercent = 0.8f;

            //死神镰刀
            DeathScythe.weaponName = "死神镰刀";
            DeathScythe.weaponDamage = 12f;
            DeathScythe.weaponSpeed = 5.5f;
            DeathScythe.smashDamage = 20f;
            DeathScythe.tiredSpeed = 7f;
            DeathScythe.weaponRange = 1.8f;
            DeathScythe.staminaDrain = 9f;
            DeathScythe.blockDamagePercent = 0.5f;

            DeathScythe.axe = false;
            DeathScythe.noTreeCut = true;

            //大郎手机
            DaLangShouji.weaponName = "大郎手机";
            DaLangShouji.Describe = "会做炊饼，喝药贼快。\n大郎手机！";

            yield return null;
        }

    }
}
