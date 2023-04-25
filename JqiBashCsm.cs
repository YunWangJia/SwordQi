using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SwordQi
{
    internal class JqiBashCsm : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.CompareTag("enemyRoot") || other.gameObject.CompareTag("Untagged") || other.gameObject.CompareTag("EndgameBoss"))
            {

                EnemyHealth EnHealth = other.GetComponentInChildren<EnemyHealth>();//不明白为什么用变量声明的方式才能访问到
                //GameObject ka = SwordQi.yuan_KatanaHeld;

                //int Default = 0;

                //if (ka)//成功获取武器对象
                //{
                //    weaponInfo katinfo = ka.transform.GetChild(0).GetComponent<weaponInfo>();
                //    if (katinfo)//成功获取武器信息
                //    {
                //        Default = (int)katinfo.WeaponDamage;
                //    }

                //}

                if (EnHealth)
                {
                    int dam = UnityEngine.Random.Range(40, 61);
                    int damWeap = dam + SwordQi.SwordQiWhole.Yuan_KatDamage;
                    EnHealth.Hit(damWeap);

                }

            }


            //tree/树
            if (other.gameObject.CompareTag("enemyCollide"))//是否与敌人碰撞
            {
                SwordQi.SwordQiWhole.sharkEnergy += 4;
                SwordQi.SwordQiWhole.sharkEnergy = Mathf.Clamp(SwordQi.SwordQiWhole.sharkEnergy, 0, 200);
                other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);

            }
            

            if (other.gameObject.name == "BurnTrigger" || other.gameObject.name == "BurnTrigger")
            {
                //other.gameObject.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
                other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
            }

            if (other.CompareTag("BreakableWood") || other.CompareTag("BreakableRock") || other.CompareTag("animalCollide") || other.CompareTag("lb_bird"))//易碎木材/易碎岩石--影响野人路标,/动物检测/鸟
            {
                SwordQi.SwordQiWhole.sharkEnergy += 2;
                SwordQi.SwordQiWhole.sharkEnergy = Mathf.Clamp(SwordQi.SwordQiWhole.sharkEnergy, 0, 200);
                other.gameObject.SendMessage("Hit", 100, SendMessageOptions.DontRequireReceiver);
                other.gameObject.SendMessage("LocalizedHit", new TheForest.World.LocalizedHitData(base.transform.position, 50f), SendMessageOptions.DontRequireReceiver);
                //global::FMODCommon.PlayOneshotNetworked(this.weaponHitEvent, base.transform, global::FMODCommon.NetworkRole.Server);
            }

            if (other.CompareTag("SmallTree") || other.CompareTag("Tree") || other.CompareTag("Fish"))
            {
                SwordQi.SwordQiWhole.sharkEnergy += 2;
                SwordQi.SwordQiWhole.sharkEnergy = Mathf.Clamp(SwordQi.SwordQiWhole.sharkEnergy, 0, 200);
                float num4 = Vector3.Distance(base.transform.position, other.transform.position);
                if (other.CompareTag("Fish"))
                {
                    other.gameObject.SendMessage("Explosion", num4, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    other.gameObject.SendMessageUpwards("Explosion", num4, SendMessageOptions.DontRequireReceiver);
                }
                other.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
            }


        }


    }
}
