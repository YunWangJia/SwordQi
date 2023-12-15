using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SwordQi
{
	internal class Des : MonoBehaviour
	{
		public float RotateSpeed;
		public float DestroyTime;
        public bool shark_obj;
        public int skill_int;
        //public static bool jq_4;
        // Use this for initialization
        void Start()
		{
            
            if (skill_int == 1)
            {
                
                switch (SwordQi.SwordQiWhole.qics)
                {
                    case 1:
                        transform.Rotate(new Vector3(0f, 0f, -45f));
                        break;

                    case 2:
                        transform.Rotate(new Vector3(0f, 0f, 45f));
                        break;

                    case 3:
                        transform.Rotate(new Vector3(0f, 0f, 90f));
                        break;

                    default:
                        SwordQi.SwordQiWhole.qics = 0;
                        this.transform.GetChild(0).GetComponent<Csm>().csmJqi_4 = true;
                        this.transform.GetChild(1).GetComponent<Csm>().csmJqi_4 = true;
                        break;

                }
            }
            if(skill_int == 3)
            {
                this.gameObject.GetComponent<AudioSource>().Play();
            }

            //Invoke("TimedDisable", DestroyTime);
            Destroy(this.gameObject, DestroyTime);
		}

		// Update is called once per frame
		void Update()
		{
            if(!shark_obj)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * RotateSpeed);//移动
                                                                                    //transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed, Space.World);//旋转
            }

        }

        //void TimedDisable()
        //{
            
        //    this.transform.parent.GetComponent<ObjectPool>().ReturnObject(this.gameObject);
        //}

    }
}
