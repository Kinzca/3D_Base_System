using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace C_.Package
{
    public class InventorySlot :MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IEndDragHandler,IPointerClickHandler
    {
        public Sprite sprite;
        public Text mark;
        public int index;

        public event Action<InventorySlot,PointerEventData> PointerEnterEvent;
        public event Action PointerExitEvent;
        public event Action<InventorySlot,PointerEventData> OnDragEvent;
        public event Action<InventorySlot> EndDragEvent;
        public event Action<InventorySlot,PointerEventData> SingleDragEvent;
        
        private void Awake()
        {
            //初始化组件的一些元素
            var image = GetComponent<Image>();
            if (image != null)
            {
                sprite = image.sprite;
            }
            index = transform.GetSiblingIndex();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            var image = eventData.pointerEnter.GetComponent<Image>();
            if (image.sprite != null) 
            {
                PointerEnterEvent?.Invoke(this,eventData);  //触发进入时的事件
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var image = eventData.pointerEnter.GetComponent<Image>();
            if (image.sprite != null) 
            {
                PointerExitEvent?.Invoke();  //鼠标离开时触发的事件
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            var image = eventData.pointerDrag.GetComponent<Image>();

            //格子内存在物品才处理拖动逻辑
            if (image.sprite != null && eventData.button==PointerEventData.InputButton.Left)     
            {
                OnDragEvent?.Invoke(this, eventData);
            }
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            //获取结束时鼠标停留的位置
            var endSlot = eventData.pointerEnter;
            InventorySlot slot = endSlot.GetComponent<InventorySlot>();
            
            EndDragEvent?.Invoke(slot);  //结束拖拽时触发，将结束时停留在的slot位置传递给订阅的方法
        }

        //鼠标右键响应更新
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)  
            {
                SingleDragEvent?.Invoke(this,eventData);
            }
        }
    }
}