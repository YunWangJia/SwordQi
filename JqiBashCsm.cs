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
                GameObject ka = GameObject.Find("player/player_BASE/jointsOffsetVR/char_Hips/char_Spine/char_Spine1/char_Spine2/char_RightShoulder/char_RightArm/char_RightForeArm/char_RightHand/char_RightHandWeapon/rightHandHeld/KatanaHeld");
                weaponInfo katinfo = ka.transform.GetChild(0).GetComponentInChildren<weaponInfo>();

                int Default = 0;

                if (ka)//成功获取武器对象
                {
                    if (katinfo)//成功获取武器信息
                    {
                        Default = (int)katinfo.WeaponDamage;
                    }

                }

                if (EnHealth)
                {
                    int dam = UnityEngine.Random.Range(40, 61);
                    int damWeap = dam + Default;
                    SwordQi.LoadValue = Default;
                    EnHealth.Hit(damWeap);

                }

            }


            //tree/树
            if (other.gameObject.CompareTag("enemyCollide"))//是否与敌人碰撞
            {
                SwordQi.sharkEnergy += 4;
                SwordQi.sharkEnergy = Mathf.Clamp(SwordQi.sharkEnergy, 0, 500);
                other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);

            }
            

            if (other.gameObject.name == "BurnTrigger" || other.gameObject.name == "BurnTrigger")
            {
                //other.gameObject.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
                other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
            }

            if (other.CompareTag("BreakableWood") || other.CompareTag("BreakableRock") || other.CompareTag("animalCollide") || other.CompareTag("lb_bird"))//易碎木材/易碎岩石--影响野人路标,/动物检测/鸟
            {
                SwordQi.sharkEnergy += 2;
                SwordQi.sharkEnergy = Mathf.Clamp(SwordQi.sharkEnergy, 0, 500);
                other.gameObject.SendMessage("Hit", 100, SendMessageOptions.DontRequireReceiver);
                other.gameObject.SendMessage("LocalizedHit", new TheForest.World.LocalizedHitData(base.transform.position, 50f), SendMessageOptions.DontRequireReceiver);
                //global::FMODCommon.PlayOneshotNetworked(this.weaponHitEvent, base.transform, global::FMODCommon.NetworkRole.Server);
            }

            if (other.CompareTag("SmallTree") || other.CompareTag("Tree") || other.CompareTag("Fish"))
            {
                SwordQi.sharkEnergy += 2;
                SwordQi.sharkEnergy = Mathf.Clamp(SwordQi.sharkEnergy, 0, 500);
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
