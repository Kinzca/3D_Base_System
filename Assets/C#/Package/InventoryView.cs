using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace C_.Package
{
    public class InventoryView : MonoBehaviour
    {
        public Transform inventoryPanel;
        public Transform cornerMarks;

        public Transform tipCanvas; // 提示文字的画布
        public GameObject tipName; // 提示文字组件

        public Transform itemCanvas; // 显示图标的画布
        public GameObject itemIcon;
        public GameObject itemNum;
        public GameObject itemParent;
        
        //物品抛投距离
        public float distance;
        
        private List<Image> _slots = new List<Image>();  // 图片列表
        private List<Text> _marks = new List<Text>();   // 角标列表

        private bool _isInitialized = false; // 标记是否已经初始化

        // 初始化方法
        private void Initialize()
        {
            if (_isInitialized) return;

            InitializeComponents();
            _isInitialized = true;
        }

        private void InitializeComponents()
        {
            foreach (Transform child in inventoryPanel)
            {
                var image = child.GetComponent<Image>();
                if (image != null)
                {
                    _slots.Add(image);
                }
            }

            foreach (Transform child in cornerMarks)
            {
                var text = child.GetComponent<Text>();
                if (text != null)
                {
                    _marks.Add(text);
                    text.text = null; // 重置角标
                }
            }
        }

        // 显示物品的方法，接受物品列表作为参数
        public void UpdateInventory(List<InventoryItem> items)
        {
            Initialize();
            ClearSlotsAndMarks();

            foreach (var item in items)
            {
                DisplayItem(item.SlotID, item);
                DisplayMark(item.SlotID, item);
            }

            Debug.Log("更新后的物品列表：");
            foreach (var inventoryItem in items)
            {
                Debug.Log($"ID: {inventoryItem.ItemID}, 数量: {inventoryItem.Number}, SID: {inventoryItem.SlotID}");
            }
        }


        // 清空所有槽位和角标
        private void ClearSlotsAndMarks()
        {
            foreach (var slot in _slots)
            {
                ClearSlot(slot);
            }

            foreach (var mark in _marks)
            {
                mark.text = null;
            }
        }

        // 清空单个槽位
        private void ClearSlot(Image slot)
        {
            slot.sprite = null;
            SetImageAlpha(slot, 0);
        }

        // 设置图片透明度
        private void SetImageAlpha(Image image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }

        private void DisplayItem(int sid, InventoryItem item)
        {
            if (sid < 0 || sid >= _slots.Count)
            {
                Debug.LogWarning("Invalid SID: " + sid);
                return;
            }

            var image = _slots[sid]; // 根据sid选择槽位
            image.sprite = item.ItemIcon;  // 设置图标
            SetImageAlpha(image, 1);
        }

        private void DisplayMark(int sid, InventoryItem item)
        {
            if (sid < 0 || sid >= _marks.Count)
            {
                Debug.LogWarning("Invalid SID: " + sid);
                return;
            }

            var text = _marks[sid]; // 根据sid选择槽位
            text.text = item.Number.ToString();
        }

        // 鼠标进入时
        public void ShowItemName(string itemName, Vector2 position)
        {
            var textComponent = tipName.GetComponent<Text>();
            textComponent.text = itemName;

            // 设置Text对象的位置为鼠标位置
            SetTipPosition(position);
            tipName.SetActive(true);
        }

        // 鼠标离开时
        public void HideItemName()
        {
            tipName.SetActive(false);
        }

        // 设置提示文字的位置
        private void SetTipPosition(Vector2 position)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(tipCanvas as RectTransform, position, null,
                out var localPoint);
            tipName.GetComponent<RectTransform>().anchoredPosition = localPoint;
        }

        //隐藏物品
        public void HideItem(InventorySlot slot)
        {
            var image = slot.GetComponent<Image>();
            if (image != null)
            {
                slot.mark.text = null;
                SetImageAlpha(image, 0);
            }
        }
        
        //重置物品图标
        public void ResetItem(InventorySlot slot)
        {
            var image = slot.GetComponent<Image>();
            if (image != null)
            {
                slot.mark.text = slot.mark.text;
                SetImageAlpha(image, 1);
            }
        }

        
        // 拖拽过程
        public void HandleDrag(Sprite icon, int num, Vector2 position)
        {
            //获取临时组件
            var iconImage = itemIcon.GetComponent<Image>();
            var numText = itemNum.GetComponent<Text>();
            var parentRectTransform = itemParent.GetComponent<RectTransform>();

            //将临时组件赋值并移动位置
            iconImage.sprite = icon;
            numText.text = num.ToString();
            
            SetItemPosition(position, parentRectTransform);
            itemParent.SetActive(true);
        }

        //处理单个物品的拖拽
        public void HandleSingleDrag(Sprite icon, int num, Vector2 position)
        {
            //获取临时组件
            var iconImage = itemIcon.GetComponent<Image>();
            var numText = itemNum.GetComponent<Text>();
            var parentRectTransform = itemParent.GetComponent<RectTransform>();
            
            //将临时组件赋值并移动位置
            iconImage.sprite = icon;
            numText.text = num.ToString();
            
            SetItemPosition(position, parentRectTransform);
            itemParent.SetActive(true);
        }
        
        // 设置物品图标的位置
        private void SetItemPosition(Vector2 position, RectTransform parentRectTransform)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(itemCanvas as RectTransform, position, null, out var localPoint);
            parentRectTransform.anchoredPosition = localPoint;
        }

        // 拖拽结束
        public void HandleEndDrag(List<InventoryItem> items)
        {
            itemParent.SetActive(false);
            UpdateInventory(items);

            Debug.Log("拖拽结束，更新背包视图。");
        }

        //实例化物体
        public void InstantiateGameObject(List<GameObject> obj, int num)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    
            int instantiatedCount = 0; // 记录成功实例化的物体数量

            for (int i = 0; i < obj.Count && instantiatedCount < num; i++)
            {
                if (!obj[i].activeSelf)
                {
                    obj[i].SetActive(true);
                    Vector3 newPosition = playerTransform.position + playerTransform.forward * distance;
                    newPosition.y += 2;
                    obj[i].transform.position = newPosition;

                    instantiatedCount++;
                }
            }

            if (instantiatedCount < num)
            {
                Debug.LogWarning($"只实例化了 {instantiatedCount} 个物体，目标数量为 {num}。可能没有足够的未激活物体可用。");
            }
        }

    }
}
