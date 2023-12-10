using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Utils;
using UnityEngine;

namespace SwordQi
{
    public class Csm : MonoBehaviour
    {

        public bool csmJqi_4;
        public int skill_int;
        private void OnTriggerEnter(Collider other)
        {
            //SwordQi.pzname = other.gameObject.name;
            //SwordQi.pztag = other.gameObject.tag;

            
            if (other.gameObject.CompareTag("enemyRoot") || other.gameObject.CompareTag("Untagged") || other.gameObject.CompareTag("EndgameBoss"))
            {
                
                EnemyHealth EnHealth = other.GetComponentInChildren<EnemyHealth>();//不明白为什么用变量声明的方式才能访问到
                if (EnHealth)
                {
                    int dam;
                    int damWeap;
                    switch (skill_int)
                    {
                        case 1:
                            dam = UnityEngine.Random.Range(5, 11);
                            damWeap = dam + SwordQi.SwordQiWhole.Yuan_KatDamage;
                            break;

                        case 2://重击
                            dam = UnityEngine.Random.Range(30, 41);
                            damWeap = dam + SwordQi.SwordQiWhole.Yuan_KatDamage;
                            break;

                        case 3://鲨鱼技能
                            dam = UnityEngine.Random.Range(60, 81);//取值不包括最大值，如果想取到100，则加1
                            damWeap = dam + SwordQi.SwordQiWhole.Yuan_KatDamage;
                            break;

                        default:
                            
                            damWeap = 0;
                            break;
                    }

                    EnHealth.HitReal(damWeap);

                }

            }
            if (other.gameObject.CompareTag("enemyCollide"))//是否与敌人碰撞
            {
                //
                switch (skill_int)
                {
                    case 1:
                        SwordQi.SwordQiWhole.sharkEnergy += 2;
                        //other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
                        break;
                    case 2:
                        SwordQi.SwordQiWhole.sharkEnergy += 4;
                        //other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
                        break;

                    case 3://施加爆炸效果
                        float num4 = Vector3.Distance(base.transform.position, other.transform.position);
                        other.gameObject.SendMessageUpwards("Explosion", num4, SendMessageOptions.DontRequireReceiver);
                        other.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
                        break;

                    default:
                        //SwordQi.SwordQiWhole.sharkEnergy += 2;
                        break;
                }

                SwordQi.SwordQiWhole.sharkEnergy = Mathf.Clamp(SwordQi.SwordQiWhole.sharkEnergy, 0, 200);//限制数值大小
            }

            if (other.CompareTag("Tree"))
            {
                if (csmJqi_4 || skill_int == 2 || skill_int == 3)//第四道剑气才能砍树和燃烧敌人
                {
                    if (!other.CompareTag("Player"))
                    {
                        other.gameObject.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
                    }

                    float num4 = Vector3.Distance(base.transform.position, other.transform.position);
                    other.gameObject.SendMessageUpwards("Explosion", num4, SendMessageOptions.DontRequireReceiver);
                    other.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
                    if (!csmJqi_4)
                    {
                        SwordQi.SwordQiWhole.sharkEnergy += 2;
                        SwordQi.SwordQiWhole.sharkEnergy = Mathf.Clamp(SwordQi.SwordQiWhole.sharkEnergy, 0, 200);
                    }

                }
            }

            

            if (other.gameObject.name == "BurnTrigger" || other.gameObject.name == "BurnTrigger")//燃烧尸体
            {
                //other.gameObject.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
                other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
            }

            if (other.CompareTag("BreakableWood") || other.CompareTag("BreakableRock") || other.CompareTag("animalCollide") || other.CompareTag("lb_bird"))//易碎木材/易碎岩石--影响野人路标,/动物检测/鸟
            {
               

                switch (skill_int)
                {
                    case 1://剑气
                        other.gameObject.SendMessage("Hit", 50, SendMessageOptions.DontRequireReceiver);
                        other.gameObject.SendMessage("LocalizedHit", new TheForest.World.LocalizedHitData(base.transform.position, 50f), SendMessageOptions.DontRequireReceiver);
                        break;

                    case 2://重击
                        SwordQi.SwordQiWhole.sharkEnergy += 2;
                        SwordQi.SwordQiWhole.sharkEnergy = Mathf.Clamp(SwordQi.SwordQiWhole.sharkEnergy, 0, 200);
                        other.gameObject.SendMessage("Hit", 100, SendMessageOptions.DontRequireReceiver);
                        other.gameObject.SendMessage("LocalizedHit", new TheForest.World.LocalizedHitData(base.transform.position, 50f), SendMessageOptions.DontRequireReceiver);
                        break;

                    case 3://鲨鱼技能
                        other.gameObject.SendMessage("Hit", 100, SendMessageOptions.DontRequireReceiver);
                        other.gameObject.SendMessage("LocalizedHit", new TheForest.World.LocalizedHitData(base.transform.position, 50f), SendMessageOptions.DontRequireReceiver);

                        float num4 = Vector3.Distance(base.transform.position, other.transform.position);
                        other.gameObject.SendMessageUpwards("Explosion", num4, SendMessageOptions.DontRequireReceiver);
                        other.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
                        break;

                    default:
                        //other.gameObject.SendMessage("Hit", 50, SendMessageOptions.DontRequireReceiver);
                        //other.gameObject.SendMessage("LocalizedHit", new TheForest.World.LocalizedHitData(base.transform.position, 50f), SendMessageOptions.DontRequireReceiver);
                        break;
                }
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
