using System;
using C_.Package;
using UnityEngine;
using UnityEngine.EventSystems;

namespace C_.EventManager
{
    // 委托事件的定义
    public delegate void LockCursorDelegate();
    
    //引入必要的接口
    public class EventManager : MonoBehaviour
    {
        // 定义静态事件
        public static event LockCursorDelegate LockCursorEvent;
        public static event Action<InventoryItem> PackageEvent;
        
        private void Update()
        {
            OnLockCursor();
        }

        // 鼠标视角的锁定与解锁
        private static void OnLockCursor()
        {
            if (Input.GetButtonDown("Package"))
            {
                // 调用订阅所有此事件的方法
                LockCursorEvent?.Invoke();
            }
        }

        public static void OnPackageEvent(InventoryItem item)
        {
            PackageEvent?.Invoke(item);
        }
    }
}