using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SwordQi
{
    public class PoolManager
    {
        //Pool[] PlayerJianQiPools;


        //void Start()
        //{

        //    Initialize(PlayerJianQiPools);
        //}

        public static void Initialize(Queue<GameObject> queue, Transform parent, GameObject prefab, int size)
        {
            Transform poolParent = new GameObject("Pool:" + prefab.name).transform;
            poolParent.parent = parent;
            //pool.Initialize(poolParent);
            //Queue<GameObject> queue = new Queue<GameObject>();
            Debug.Log("进入循环！");
            for (var i = 0; i < size; i++)
            {
                //var copy = GameObject.Instantiate(prefab, poolParent.transform);//实例化
                //copy.SetActive(false);
                queue.Enqueue(Copy(prefab, poolParent));//入列函数
            }
            Debug.Log("入列成功！");
        }
        public static void Initialize( Transform parent, GameObject prefab, int size)
        {
            Debug.Log("Pool初始化！");
            Transform poolParent = new GameObject("Pool:" + prefab.name).transform;
            poolParent.parent = parent;
            //pool.Initialize(poolParent);
            Queue<GameObject> queue = new Queue<GameObject>();
            Debug.Log("进入循环！");
            for (var i = 0; i < size; i++)
            {
                var copy = GameObject.Instantiate(prefab, poolParent.transform);//实例化
                copy.SetActive(false);
                queue.Enqueue(copy);//入列函数
            }
            Debug.Log("入列成功！");
        }

        static GameObject Copy(GameObject prefab, Transform parent)
        {
            var copy = GameObject.Instantiate(prefab, parent);//实例化
            copy.SetActive(false);
            return copy;
        }

        /// <summary>
        /// 可用对象
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        static GameObject AvailableObject(Queue<GameObject> queue, GameObject prefab, Transform poolParent)
        {
            GameObject availableobject = null;
            if (queue.Count > 0 && queue.Peek().activeSelf)
            {
                availableobject = queue.Dequeue();//出列函数
            }
            else
            {
                availableobject = Copy(prefab, poolParent);
            }
            queue.Enqueue(availableobject);

            return availableobject;

        }

        /// <summary>
        /// 准备好的对象
        /// </summary>
        /// <param name="queue">提取的目标池</param>
        /// <param name="prefab">备用变量，如果池对象不够，需要用此来生成</param>
        /// <param name="poolParent">生成时想放到哪个池里</param>
        /// <returns></returns>
        public GameObject PreparedObject(Queue<GameObject> queue, GameObject prefab, Transform poolParent)
        {
            GameObject preparedObject = AvailableObject(queue, prefab, poolParent);
            preparedObject.SetActive(true);
            return preparedObject;
        }

        
        public GameObject PreparedObject(Queue<GameObject> queue, GameObject prefab, Transform poolParent, Vector3 position)
        {
            GameObject preparedObject = AvailableObject(queue, prefab, poolParent);
            preparedObject.transform.position = position;
            preparedObject.SetActive(true);
            return preparedObject;
        }

        /// <summary>
        /// 准备好的对象
        /// </summary>
        /// <param name="queue">提取的目标池</param>
        /// <param name="prefab">备用变量，如果池对象不够，需要用此来生成</param>
        /// <param name="poolParent">若生成,想放到哪个池里</param>
        /// <param name="position">生效时的位置</param>
        /// <param name="rotation">生效时的旋转于方向</param>
        /// <returns></returns>
        public static GameObject PreparedObject(Queue<GameObject> queue, GameObject prefab, Transform poolParent, Vector3 position, Quaternion rotation)
        {
            GameObject preparedObject = AvailableObject(queue, prefab, poolParent);
            preparedObject.transform.position = position;
            preparedObject.transform.rotation = rotation;
            preparedObject.SetActive(true);
            return preparedObject;
        }

        
        public GameObject PreparedObject(Queue<GameObject> queue, GameObject prefab, Transform poolParent, Vector3 position, Quaternion rotation, Vector3 localScale)
        {
            GameObject preparedObject = AvailableObject(queue, prefab, poolParent);
            preparedObject.SetActive(true);
            preparedObject.transform.position = position;
            preparedObject.transform.rotation = rotation;
            preparedObject.transform.localScale = localScale;
            return preparedObject;
        }








    }
}
