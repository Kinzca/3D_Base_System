using C_.EventManager;
using UnityEngine;
public class C_CameraControls : MonoBehaviour
{
    private Camera _mainCamera; // 用于存储主摄像机的引用
    public float rotationSpeed; // 旋转速度，可以在Inspector中设置

    public float mouseSensitivity = 2.0f; // 鼠标灵敏度，可以在Inspector中设置
    public float rotationY = 0f; // 用于垂直旋转的角度

    private bool _canFollowCursor = true;  //鼠标跟踪的开关
    
    private void Start()
    {
        _mainCamera = Camera.main; // 获取主摄像机的引用
        EventManager.LockCursorEvent += SetCursor; //设置事件订阅
    }

    private void FixedUpdate()
    {
        CameraRotation(); // 在FixedUpdate中调用摄像机旋转方法
    }

    private void OnDisable()
    {
        EventManager.LockCursorEvent -= SetCursor;
    }

    // 摄像机旋转方法
    private void CameraRotation()
    {
        if (_canFollowCursor)
        {
            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // 获取鼠标X轴输入并乘以灵敏度
            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // 获取鼠标Y轴输入并乘以灵敏度

            rotationY += mouseY; // 累加Y轴旋转角度
            rotationY = Mathf.Clamp(rotationY, -10, 10); // 限制Y轴旋转角度在-10到10度之间

            transform.localEulerAngles = new Vector3(rotationY, 0, 0); // 设置局部旋转角度
            transform.parent.Rotate(Vector3.up * mouseX); // 绕Y轴旋转摄像机的父对象
        }
    }

    private void SetCursor()
    {
        _canFollowCursor = !_canFollowCursor;
    }
}
