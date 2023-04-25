using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SwordQi
{
	internal class Des : MonoBehaviour
	{
		public float RotateSpeed = 50f;
		public float DestroyTime = 3f;
		public static bool jq_4;
		// Use this for initialization
		void Start()
		{
			jq_4 = false;
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

				case 4:
					jq_4 = true;
					SwordQi.SwordQiWhole.qics = 0;
					break;

				default:
					
					break;

			}
			Destroy(this.gameObject, DestroyTime);
		}

		// Update is called once per frame
		void Update()
		{
			transform.Translate(Vector3.forward * Time.deltaTime * RotateSpeed);//移动
			//transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed, Space.World);//旋转
		}


	}
}
