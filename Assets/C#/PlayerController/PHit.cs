using UnityEngine;

namespace C_.PlayerController
{
    public class PHit : IState
    {
        private PlayerFsm _manager;
        private PlayerParamenter _paramenter;

        public PHit(PlayerFsm manager)
        {
            this._manager = manager;
            _paramenter = manager.paramenter;
        }

        public void OnEnter()
        {
            _paramenter.animator.Play("hit01");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate()
        {
            AnimatorStateInfo stateInfo = _paramenter.animator.GetCurrentAnimatorStateInfo(0);

            Debug.Log("stateinfo:"+stateInfo.normalizedTime);
        
            if (stateInfo.normalizedTime >= 0.9) 
            {
                Debug.Log("从hit进入idle");
                _manager.TransitionState(StateType.Idle);
            }
        }

        public void OnExit()
        {
            
        }
    }
}
