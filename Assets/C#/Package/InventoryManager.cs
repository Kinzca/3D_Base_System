using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace C_.Package
{
    public class InventoryManager : MonoBehaviour
    {
        public GameObject packageUI;
        public InventoryView inventoryView;
        public Transform packageCanvas; // 背包界面视图
        public Transform textCanvas; // 下标视图
        private List<InventoryItem> _items = new List<InventoryItem>(); // 定义一个新物品链表存储
        
        //物品列表
        public List<GameObject> cubes = new List<GameObject>();
        public List<GameObject> spheres = new List<GameObject>();

        InventorySlot _originalSlot;  //开始拖动时鼠标所在的槽位
        InventorySlot _endSlot; //结束时鼠标所在槽位
        private int _originalSid;
        private int _endSid;
        private bool _isDragSingleItem = false;// 标记是否拖拽单个物体
        private bool _isFirstDragSingleItem = false;
        //背包物品拖动时的元素数据
        private InventoryItem _draggedItem;
        private Sprite _draggedItemIcon;
        private string _draggedItemName;
        private int _draggedItemID;
        private int _draggedItemNum;
        private int _draggedItemSid;

        private void Start()
        {
            // 订阅PackageEvent背包事件
            C_.EventManager.EventManager.PackageEvent += AddItem;

            //订阅鼠标拖动的事件
            foreach (Transform child in inventoryView.transform)
            {
                var slot = child.GetComponent<InventorySlot>();
                if (slot != null)
                {
                    slot.PointerEnterEvent += PointerEnter;
                    slot.PointerExitEvent += PointerExit;
                    slot.OnDragEvent += OnDrag;
                    slot.EndDragEvent += EndDrag;
                    slot.SingleDragEvent += SingleDrag;
                }
            }
        }

        private void Update()
        {
            PackageManager();
            IconFollow();
        }


        private void AddItem(InventoryItem item)
        {
            if (item == null) 
            {
                Debug.LogError("itemToAdd is null");
                return;
            }
            
            //查找是否存在相同ID的物品
            InventoryItem existItem = _items.Find(i => i.ItemID == item.ItemID);

            if (existItem == null) //不存在添加新的物品
            {
                _items.Add(item);
            }
            else //如果存在则叠加数量
            {
                existItem.AddQuantity(item.Number);
            }
            
            //更新视图
            inventoryView.UpdateInventory(_items);
            
            Debug.Log("更新后的物品列表：");
            foreach (var inventoryItem in _items)
            {
                Debug.Log($"ID: {inventoryItem.ItemID}, 数量: {inventoryItem.Number}, SID: {inventoryItem.SlotID}");
            }
        }

        //移动后对链表进行再排列，传递物品和将要移动的位置索引
        private void MoveItem(InventoryItem item, int newSlotID)
        {
            if (item == null) 
            {
                Debug.LogError("itemToMove is null");
                return;
            }

            //获取鼠标结束时所在槽位的物体
            InventoryItem targetItem = _items.Find(i => i.SlotID == newSlotID);

            //槽位存在物体
            if (targetItem != null)
            {
                Debug.Log("槽位内存在物体");
                
                //物品数量++
                if (targetItem.ItemID == item.ItemID)
                {
                    targetItem.AddQuantity(item.Number);  //槽位物体数量增加
                    
                    RemoveItem(item);
                }
                else
                {
                    //交换物品索引
                    SwapIndex(item,targetItem);
                }
            }
            else//直接移动至该槽位
            {
                Debug.Log("槽位内不存在物体");
                
                //直接修改物体的索引值
                item.SlotID = newSlotID;
            }
        }

        private void RemoveItem(InventoryItem item)
        {
            if (item == null)
            {
                Debug.LogError("将移除的物品为空");
                return;
            }

            if (_items.Contains(item))
            {
                _items.Remove(item);
            }
            else
            {
                Debug.LogWarning("列表中不存在该物品!");
            }

            inventoryView.UpdateInventory(_items);

            Debug.Log("移除的物品名: " + item.ItemName);
            Debug.Log("更新后的列表长度: " + _items.Count);
            // 输出当前物品列表中的所有物品
            foreach (var invItem in _items)
            {
                Debug.Log($"物品名: {invItem.ItemName}, 物品的SID: {invItem.SlotID}");
            }
        }

        //处理鼠标进入
        private void PointerEnter(InventorySlot slot, PointerEventData pointerEventData)
        {
            if (slot != null)
            {
                var sid = slot.index;
                foreach (var item in _items) //查找是否存在与当前槽位索引值相同的InventoryItem的sid值
                {
                    if (item.SlotID == sid)
                    {
                        var itemName = item.ItemName;
                        inventoryView.ShowItemName(itemName, pointerEventData.position);
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("inventorySlot does not exist");
            }
        }

        //处理鼠标离开
        private void PointerExit()
        {
            inventoryView.HideItemName();
        }
        
        //拖动地初始化工作
        private void BeginDrag(InventorySlot slot)
        {
            //获取当前格子的索引值sid,获取被拖拽物体的数据
            var sid = slot.index;
            InitializeDraggedItem(sid);
            
            inventoryView.HideItem(slot);
        }

        private void InitializeDraggedItem(int sid)
        {
            //能查找到对应的sid值，将其赋值
            _draggedItem = _items.FirstOrDefault(item => item.SlotID == sid);
    
            if (_draggedItem != null)
            {
                _draggedItemIcon = _draggedItem.ItemIcon;
                _draggedItemNum = _draggedItem.Number;
                _draggedItemSid = _draggedItem.SlotID;
            }
            else
            {
                Debug.LogError("找不到对应拖拽物体");
            }
        }

        private void InitializeSingleItem()
        {
            if (_draggedItem != null && _draggedItem.Number > 0)
            {
                _draggedItemIcon = _draggedItem.ItemIcon;
                _draggedItemSid = _draggedItem.SlotID;
                _draggedItemName = _draggedItem.ItemName;
                _draggedItemID = _draggedItem.ItemID;

                if (_isFirstDragSingleItem) //第一次拖拽
                {
                    _draggedItemNum = 1;
                    _draggedItem.Number--;

                    if (_draggedItem.Number == 0)
                    {
                        RemoveItem(_draggedItem);
                        
                        Debug.Log("_draggedItem数量为0，清除该图标");
                    }
                    else
                    {
                        inventoryView.UpdateInventory(_items);
                    }
                    
                    _isFirstDragSingleItem = false;
                }
            }
        }

        private void HandleSameSlot()
        {
            if (_originalSlot.mark != null) //原槽位有标记
            {
                _draggedItemNum++;
                _draggedItem.Number--;

                if (_draggedItem.Number == 0) 
                {
                    RemoveItem(_draggedItem);
                }
                else
                {
                    inventoryView.UpdateInventory(_items);
                }
            }
            else //原槽位无标记
            {
                _draggedItem.Number = _draggedItemNum;
                ResetDragState();
            }
            
            Debug.Log($"结束位置等于原位置，拖拽物品名：{_draggedItem?.ItemName}, 拖拽物品数量：{_draggedItemNum}");
        }

        private void ResetDragState()
        {
            _draggedItem = null;
            _isDragSingleItem = false;
            _isFirstDragSingleItem = false;
        }

        //拖拽事件
        private void OnDrag(InventorySlot slot, PointerEventData eventData)
        {
            //如果定义的临时变量为空，将开始时鼠标停留的位置传入给开始时的初始化函数
            if (_draggedItem == null)
            {
                BeginDrag(slot);
                _originalSlot = slot; //开始时的槽位
            }

            Vector2 position = eventData.position;
            inventoryView.HandleDrag(_draggedItemIcon, _draggedItemNum, position);
        }

        private void EndDrag(InventorySlot slot)
        {
            if (_draggedItem == null)
            {
                Debug.LogError("EndDrag: _draggedItem is null!");
                return;
            }

            if (slot != null)
            {
                // 结束时的槽位索引sid
                var index = slot.index;
        
                // 物品的索引与结束时的索引不同
                if (_draggedItem.SlotID != index) 
                {
                    // 处理移动逻辑
                    MoveItem(_draggedItem, index);
                    inventoryView.HandleEndDrag(_items);

                    Debug.Log($"物品已移动到槽位: {index}");
                    ClearDraggedItem();
                }
                else //物品索引与开始时的索引相同
                {
                    // 重置开始时的槽位图标，处理结束拖拽逻辑，清除被拖拽物体数据
                    inventoryView.ResetItem(_originalSlot);
                    inventoryView.HandleEndDrag(_items);
                    ClearDraggedItem();
                }
            }
            else // 不在槽位内，丢弃物品
            {
                if (_draggedItem != null)
                {
                    DiscardItem(_draggedItem, _draggedItemNum);
                    
                    Debug.Log($"丢弃的物品为：{_draggedItem.ItemID},数量是：{_draggedItemNum}");
                }
                else
                {
                    Debug.LogWarning("未找到将被丢弃的物体");
                }
                inventoryView.HandleEndDrag(_items);
                ClearDraggedItem();
            }
        }

        //实现单个物品拖拽
        private void SingleDrag(InventorySlot slot, PointerEventData eventData)
        {
            var pointerEnter = eventData.pointerEnter; //获取鼠标进入时的物体
            if (pointerEnter == null) 
            {
                Debug.LogWarning("pointerEnter为空");
                return;
            }

            var pointerEnterSlot = pointerEnter.GetComponent<InventorySlot>(); //获取该物体的inventorySlot值
            if (pointerEnterSlot == null) 
            {
                Debug.LogWarning("pointerEnterSlot为空");
                return;
            }

            
            int enterIndex = pointerEnterSlot.index; //获取鼠标进入槽位的索引
            InventoryItem tempItem = _items.FirstOrDefault(item => item.SlotID == enterIndex); //根据索引查找列表中是否存在相匹配的物体
            
            //如果能鼠标所在位置查找sid得到的物体不为空，同时不在拖动状态，开始拖拽
            if (tempItem != null && !_isDragSingleItem)
            {
                StartSingleDrag(tempItem, slot); //传入鼠标停留的槽位的物体，和鼠标进入的槽位
            }
            else if(_isDragSingleItem)
            {
                _endSlot = pointerEnterSlot; //结束时的槽位
                _endSid = _endSlot.index; //结束时的物品索引
                
                InventoryItem endItem = _items.FirstOrDefault(i => i.SlotID == _endSid); //获取结束时的位置
                
                if (_draggedItem == null)
                {
                    Debug.LogError("_draggedItem is null in SingleDrag!"); //！！！
                    return;
                }
                
                if (endItem != null && _draggedItem.ItemID != endItem.ItemID)  //鼠标结束时的,该槽位的物体不为空,且与拖拽物品的id值不同 
                {
                    SwapItems(endItem); //将该槽位的物体传入该函数
                }
                else if(endItem != null && _draggedItem.ItemID == endItem.ItemID) //鼠标结束时，物品槽位存在物品，且与拖拽物品的id相同
                {
                    _draggedItem = endItem; //将endItem作为新的拖拽物品传入
                    
                    HandleSameSlot();
                }
                else if (endItem == null)  //鼠标结束时，该槽位的物体为空，直接把拖拽的物品索引修改为结束位置的索引
                {
                    MoveToEmptySlot();
                }
            }
        }
        
        private void StartSingleDrag(InventoryItem tempItem, InventorySlot slot)
        {
            _draggedItem = tempItem; //被拖拽物体
            _originalSlot = slot;
            _originalSid = _originalSlot.index;

            _isDragSingleItem = true;
            _isFirstDragSingleItem = true;
            
            InitializeSingleItem();
            Debug.Log("第一次拖拽");
        }

        //交换被拖拽物体，item的值是在鼠标原指向的物体
        private void SwapItems(InventoryItem item)
        {
            Debug.Log("移动物体到此槽位，交换此物体和被拖拽物体的数量");
            
            CreateNewItem(_endSid);//创建一个新物体，并添加至俩表
            
            UpdateDraggedItem(item);//将原物体的数据传入被拖拽物体的图标
            
            _items.Remove(item);
            inventoryView.UpdateInventory(_items);
        }

        //移动至空槽位
        private void MoveToEmptySlot()
        {
            Debug.Log("移动物体到此槽位，添加一个新物体");
            
            CreateNewItem(_endSid);

            if (_draggedItem.Number == 0)
            {
                _items.Remove(_draggedItem);
            }
            
            inventoryView.HandleEndDrag(_items);
            ResetDragState();
        }
        
        private void CreateNewItem(int slotID)
        {
            InventoryItem newItem = new InventoryItem(_draggedItemIcon, _draggedItemName, _draggedItemID,
                _draggedItemNum, slotID);
            
            _items.Add(newItem);//将这个新的物体添加到列表中
        }

        private void UpdateDraggedItem(InventoryItem item)
        {
            _draggedItem = item;
            _draggedItemNum = item.Number;
            _draggedItemIcon = item.ItemIcon;
            _draggedItemSid = item.SlotID;
            _draggedItemName = item.ItemName;
            _draggedItemID = item.ItemID;

        }
        
        //交换item和targetItem的索引
        private void SwapIndex(InventoryItem item, InventoryItem targetItem)
        {
            int tempSlotID = targetItem.SlotID;
            targetItem.SlotID = item.SlotID;
            item.SlotID = tempSlotID;
        }

        private void IconFollow()
        {
            if (_isDragSingleItem && _draggedItem != null)
            {
                Vector2 position = Input.mousePosition;
                inventoryView.HandleSingleDrag(_draggedItemIcon,_draggedItemNum,position);
            }
        }
        
        private void ClearDraggedItem()
        {
            _draggedItem = null;
            _draggedItemNum = 0;
            _draggedItemSid = 0;
            _draggedItemIcon = null;
            
            _originalSlot = null;
            _endSlot = null;
        }

        private void DiscardItem(InventoryItem item, int num)
        {
            // 从列表中将该物体删除
            RemoveItem(item);

            switch (item.ItemID)
            {
                case 1:
                    inventoryView.InstantiateGameObject(cubes, _draggedItemNum);
                    break;
                case 2:
                    inventoryView.InstantiateGameObject(spheres, _draggedItemNum);
                    break;
            }
        }

        
        // 管理背包UI开启关闭
        private void PackageManager()
        {
            if (Input.GetButtonDown("Package"))
            {
                bool isActive = packageUI.activeSelf;
                packageUI.SetActive(!isActive);

                Cursor.visible = !isActive;
                Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
            }
        }
        
        private void OnDestroy()
        {
            // 取消订阅PackageEvent背包事件
            C_.EventManager.EventManager.PackageEvent -= AddItem;

            if (inventoryView != null)
            {
                foreach (Transform child in inventoryView.transform)
                {
                    var slot = child.GetComponent<InventorySlot>();
                    if (slot != null)
                    {
                        slot.PointerEnterEvent -= PointerEnter;
                        slot.PointerExitEvent -= PointerExit;
                        slot.OnDragEvent -= OnDrag;
                        slot.EndDragEvent -= EndDrag;
                        slot.SingleDragEvent -= SingleDrag;
                    }
                }
            }
        }
    }
}
