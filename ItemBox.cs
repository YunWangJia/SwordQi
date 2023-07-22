using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwordQi
{
    public class ItemBox : MonoBehaviour, IDragHandler // , IPointerDownHandler
    {
        private RectTransform dragTarget;
        private Canvas canvas;

        private void Awake()
        {
            if (dragTarget == null) dragTarget = transform.parent.GetComponent<RectTransform>();//需移动的Bj图层
            if (canvas == null) canvas = transform.parent.transform.parent.GetComponent<Canvas>();//画布
        }

        public void OnDrag(PointerEventData eventData)//OnDrag（打开拖动）
        {
            // 移动拖拽框的位置
            dragTarget.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        //public void OnPointerDown(PointerEventData eventData)
        //{
        //    // 把当前选中的拖拽框显示在最前面,当所有窗口都在同一个画布下的时候生效
        //    dragTarget.SetAsLastSibling();
        //}
    }
}
