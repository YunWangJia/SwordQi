using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SwordQi
{
    public class ObjectPool : MonoBehaviour
    {
        private Queue<GameObject> _objectQueue = new Queue<GameObject>();
        private int _objectPoolSize = 5; // 对象池大小  
        private GameObject _prefab; // 预设体  
        private Transform _poolTransform; // 对象池的Transform组件  

        public void Initialize(int size, GameObject prefab, Transform poolTransform)
        {
            _objectPoolSize = size;
            _prefab = prefab;
            _poolTransform = poolTransform;
            for (int i = 0; i < size; i++)
            {
                InstantiatePrefab();
            }
        }

        public GameObject GetObject()
        {
            if (_objectQueue.Count < 0 && _objectQueue.Peek().activeSelf)
            {
                InstantiatePrefab();
            }
            return _objectQueue.Dequeue();
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false); // 禁用对象，以便将其放回池中  
            _objectQueue.Enqueue(obj);
        }

        private void InstantiatePrefab()
        {
            GameObject obj = Instantiate(_prefab, _prefab.transform);
            obj.SetActive(false); // 禁用新创建的对象，以便将其放回池中  
            _objectQueue.Enqueue(obj);
        }
    }
}
