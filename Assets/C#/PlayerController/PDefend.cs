using UnityEngine;

namespace C_.PlayerController
{
    public class PDefend : IState
    {
        private PlayerFsm _manager;
        private PlayerParamenter _paramenter;
        
        public PDefend(PlayerFsm manager)
        {
            _manager = manager;
            _paramenter = _manager.paramenter;
        }

        public void OnEnter()
        {
            _paramenter.animator.Play("defence");
        }

        public void OnUpdate()
        {
            AnimatorStateInfo stateInfo = _paramenter.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 0.9f)
            {
                JudgeReturnIdleOrWalk();
            }
        }

        public void OnExit()
        {
            //处理事件退出时的逻辑，比如取消事件订阅等
        }
        
        //判断返回idle还是Walk
        private void JudgeReturnIdleOrWalk()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            var directionInput = (horizontalInput != 0 || verticalInput != 0);//有一个不为0即为true，两个都为0是为false
            
            var leftShiftInput = Input.GetButton("Fire3");
            
            if (!directionInput)
            {
                //Debug.Log("进入idle状态");
                _manager.TransitionState(StateType.Idle);
            }
            else
            {
                //Debug.Log("进入walk状态");
                _manager.TransitionState(StateType.Walk);
            }
        }
    }
}
