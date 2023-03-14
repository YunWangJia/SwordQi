using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

namespace SwordQi
{
    internal class SharkDes : MonoBehaviour
    {
		public float RotateSpeed = 40f;
		public float DestroyTime = 3f;
		// Use this for initialization
		void Start()
		{
			
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
