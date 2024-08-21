using UnityEngine;

public class MyEventSubscriber : MonoBehaviour
{
    //脚本启用时订阅OnMyEvent事件
    private void Start()
    {
        MyEventPublisher.OnMyEvent += MyEventHandler;
    }

    //脚本禁用时取消订阅OnMyEvent实践
    private void OnDisable()
    {
        MyEventPublisher.OnMyEvent -= MyEventHandler;
    }

    //事件处理方法，事件触发时调用
    void MyEventHandler()
    {
        //输出调试信息到控制台
        Debug.Log("事件触发");
    }
}
