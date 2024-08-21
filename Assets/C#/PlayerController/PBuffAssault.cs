using UnityEngine;

namespace C_.PlayerController
{
    public class PBuffAssault : IState
    {
        private PlayerFsm _manager;
        private PlayerParamenter _paramenter;

        public PBuffAssault(PlayerFsm manager)
        {
            _manager = manager;
            _paramenter = manager.paramenter;
        }

        public void OnEnter()
        {
            _manager.paramenter.animator.Play("Buff_02");
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
        
        private void JudgeReturnIdleOrWalk()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            var directionInput = (horizontalInput != 0 || verticalInput != 0);
            
            var leftShiftInput = Input.GetButton("Fire3");

            if (!directionInput)
            {
                //Debug.Log("进入idle状态");
                _manager.TransitionState(StateType.Idle);
            }
            else if (!leftShiftInput)
            {
                //Debug.Log("进入walk状态");
                _manager.TransitionState(StateType.Walk);
            }
        }
    }
}
