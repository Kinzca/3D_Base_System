using UnityEngine;

namespace C_.PlayerController
{
    public class PRightHeavyBlow : IState
    {
        private PlayerFsm _manager;
        private PlayerParamenter _paramenter;

        public PRightHeavyBlow(PlayerFsm manager)
        {
            this._manager = manager;
            _paramenter = manager.paramenter;
        }
        
        public void OnEnter()
        {
            _manager.paramenter.animator.Play("Attack_02");
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
