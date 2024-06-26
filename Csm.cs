﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Utils;
using UnityEngine;

namespace SwordQi
{
    public class Csm : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider other)
        {
            //SwordQi.pzname = other.gameObject.name;
            //SwordQi.pztag = other.gameObject.tag;

            if (other.gameObject.CompareTag("enemyRoot") || other.gameObject.CompareTag("Untagged") || other.gameObject.CompareTag("EndgameBoss"))
            {
                
                EnemyHealth EnHealth = other.GetComponentInChildren<EnemyHealth>();//不明白为什么用变量声明的方式才能访问到
                if (EnHealth)
                {

                    int dam = UnityEngine.Random.Range(5, 11);
                    int damWeap = dam + SwordQi.SwordQiWhole.Yuan_KatDamage;
                    EnHealth.HitReal(damWeap);
                    //Debug.Log("普通剑气伤害：" + damWeap.ToString());

                }

            }
            if (other.gameObject.CompareTag("enemyCollide"))//是否与敌人碰撞
            {

                SwordQi.SwordQiWhole.sharkEnergy += 2;
                SwordQi.SwordQiWhole.sharkEnergy = Mathf.Clamp(SwordQi.SwordQiWhole.sharkEnergy, 0, 200);//限制数值大小
                //other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);

            }

            if (Des.jq_4)//第四道剑气才能砍树和燃烧敌人
            {
                if (other.gameObject.CompareTag("enemyCollide"))//是否与敌人碰撞
                {
                    other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);

                }
                if (other.CompareTag("Tree"))
                {
                    float num4 = Vector3.Distance(base.transform.position, other.transform.position);
                    other.gameObject.SendMessageUpwards("Explosion", num4, SendMessageOptions.DontRequireReceiver);
                    other.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
                }
            }


            if (other.gameObject.name == "BurnTrigger" || other.gameObject.name == "BurnTrigger")//燃烧尸体
            {
                //other.gameObject.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
                other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
            }

            if (other.CompareTag("BreakableWood") || other.CompareTag("BreakableRock") || other.CompareTag("animalCollide") || other.CompareTag("lb_bird"))//易碎木材/易碎岩石--影响野人路标,/动物检测/鸟
            {
                other.gameObject.SendMessage("Hit", 50, SendMessageOptions.DontRequireReceiver);
                other.gameObject.SendMessage("LocalizedHit", new TheForest.World.LocalizedHitData(base.transform.position, 50f), SendMessageOptions.DontRequireReceiver);
                //global::FMODCommon.PlayOneshotNetworked(this.weaponHitEvent, base.transform, global::FMODCommon.NetworkRole.Server);
            }

            

            if (other.CompareTag("SmallTree")|| other.CompareTag("Fish"))
            {
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
