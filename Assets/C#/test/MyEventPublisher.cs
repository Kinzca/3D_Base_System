using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定义一个委托类型，名为MyDelegate
public delegate void MyDelegate();
public class MyEventPublisher : MonoBehaviour
{
    //定义一个静态事件，名为OnMyEvent,类型为MyDelegate;
    public static event MyDelegate OnMyEvent;

    private void Update()
    {
        //检测是否按下防御键——C
        if (Input.GetButtonDown("Defend"))
        {
            //如果事件不为空（有订阅者），调用所有订阅此事件的方法。
            OnMyEvent?.Invoke();
        }
    }
}
