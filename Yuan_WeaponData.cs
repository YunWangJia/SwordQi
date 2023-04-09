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
        public struct Yuan
        {
            public float weaponDamage;    //武器伤害，武士刀：6
            public float weaponSpeed;     //武器速度，武士刀：8.5
            public float smashDamage;    //粉碎伤害，武士刀：10
            public float tiredSpeed;    //疲惫的速度，武士刀：6.5
            public float staminaDrain;  //耐力消耗，武士刀：7
            //public float soundDetectRange; //声音检测范围，武士刀：18
            //public float weaponRange;	//武器射程，武士刀：1.4
            public float blockDamagePercent;// 武器屏蔽，值为0 ~1，1为无，0为最高屏蔽

            public bool axe;//斧头,木棍启用这个之后架刀就是单手，飞机斧取消启用这个，就可以保留劈砍且双手架刀
            public bool noTreeCut;// 为真的时候，砍树没有伤害

        }

        public Yuan yuan_KatanaHeld;//原武士刀
        public Yuan yuan_AxePlaneHeld;//原飞机斧

        public IEnumerator YuanValueInit()
        {
            //游戏难度不影响武器基础数值
            //武士刀
            yuan_KatanaHeld.weaponDamage = 6f;
            yuan_KatanaHeld.weaponSpeed = 8.5f;
            yuan_KatanaHeld.smashDamage = 10f;
            yuan_KatanaHeld.tiredSpeed = 6.5f;
            yuan_KatanaHeld.staminaDrain = 7f;
            yuan_KatanaHeld.blockDamagePercent = 0;

            //飞机斧
            yuan_AxePlaneHeld.weaponDamage = 6f;
            yuan_AxePlaneHeld.weaponSpeed = 8.5f;
            yuan_AxePlaneHeld.smashDamage = 10f;
            yuan_AxePlaneHeld.tiredSpeed = 6.5f;
            yuan_AxePlaneHeld.staminaDrain = 7f;
            yuan_AxePlaneHeld.axe = true;
            yuan_AxePlaneHeld.noTreeCut = false;

            yield return null;
        }

    }
}
