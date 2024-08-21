using UnityEngine;

namespace C_.PlayerController
{
    public class PLeftHeavyBlow : IState
    {
        private PlayerFsm _manager;
        private PlayerParamenter _paramenter;

        public PLeftHeavyBlow(PlayerFsm manager)
        {
            _manager = manager;
            _paramenter = _manager.paramenter;
        }


        public void OnEnter()
        {
            _manager.paramenter.animator.Play("Attack_01");
        }

        public void OnUpdate()
        {
            AnimatorStateInfo stateInfo = _paramenter.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 0.9f) 
            {
                _manager.TransitionState(StateType.Idle);
            }
        }

        public void OnExit()
        {
            //处理事件退出时的逻辑，比如取消事件订阅等
        }
    }
}
