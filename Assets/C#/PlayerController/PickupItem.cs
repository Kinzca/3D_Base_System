using System;
using System.Collections.Generic;
using C_.Package;
using UnityEngine;
using UnityEngine.UI;

namespace C_.PlayerController
{
    public class PickupItem : MonoBehaviour
    {
        public GameObject packageCanvas;
        
        private readonly Dictionary<string, (int id, string name,string path, int num)> _itemData =
            new Dictionary<string, (int id, string name, string path, int num)>
            {
                { "Cube", (1, "Cube","Icons/Cube", 1) },
                { "Sphere", (2, "Sphere","Icons/Sphere", 1) }
            };
        
        //玩家碰撞管理
        private void OnCollisionEnter(Collision other)
        {
            if (_itemData.ContainsKey(other.gameObject.tag))
            {
                var data = _itemData[other.gameObject.tag];
                AddItem(data.id, data.name, data.path, data.num, SelectFirstEmptySlot());
                other.gameObject.SetActive(false);
            }
        }

        private void AddItem(int id, string nam,string path, int num ,int sid)
        {
            Sprite icon = Resources.Load<Sprite>(path);
            InventoryItem item = new InventoryItem(icon, nam,id, num, sid);
            C_.EventManager.EventManager.OnPackageEvent(item);
        }
        
        //获取第一个空槽位的索引值
        private int SelectFirstEmptySlot()
        {
            for (int i = 0; i < packageCanvas.transform.childCount; i++)
            {
                Transform child = packageCanvas.transform.GetChild(i);

                Image image = child.GetComponent<Image>();

                if (image != null && image.sprite == null)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
