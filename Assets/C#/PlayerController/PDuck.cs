using UnityEngine;

namespace C_.PlayerController
{
    public class PDuck : IState
    {
        private PlayerFsm _manager;
        private PlayerParamenter _paramenter;

        public PDuck(PlayerFsm manager)
        {
            this._manager = manager;
            _paramenter = manager.paramenter;
        }
        
        public void OnEnter()
        {
            JudgeWhichAnimation();
        }

        public void OnUpdate()
        {
            AnimatorStateInfo stateInfo = _paramenter.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 0.9f) 
            {
                OnAnimationComplete();
            }
        }

        public void OnExit()
        {
            
        }
        
        //判断应用的哪一段方向的动画
        private void JudgeWhichAnimation()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            
            if (horizontalInput == 0 && verticalInput > 0)  
            {
                Debug.Log("roll_front");
                _paramenter.animator.Play("roll_front");
            }
            else if (horizontalInput == 0 && verticalInput < 0) 
            {
                Debug.Log("roll_back");
                _paramenter.animator.Play("roll_back");
            }
            else if (horizontalInput > 0 && verticalInput == 0) 
            {
                Debug.Log("roll_right");
                _paramenter.animator.Play("roll_right");
            }
            else if (horizontalInput < 0 && verticalInput == 0) 
            {
                Debug.Log("roll_left");
                _paramenter.animator.Play("roll_left");
            }
        }

        //动画完成后的判断逻辑
        private void OnAnimationComplete()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            var directionInput = (horizontalInput != 0 || verticalInput != 0);
            
            var leftShiftInput = Input.GetButton("Fire3");

            if (!directionInput)
            {
                JudgeReturnIdle();
            }
            else if (!leftShiftInput)
            {
                JudgeReturnWalk();
            }
            else
            {
                JudgeReturnSprint();
            }
        }

        //判断返回疾跑状态
        private void JudgeReturnSprint()
        {
            _manager.TransitionState(StateType.Sprint);
        }

        //判断返回Walk状态
        private void JudgeReturnWalk()
        {
            _manager.TransitionState(StateType.Walk);
        }

        //判断返回Idle状态
        private void JudgeReturnIdle()
        {
            _manager.TransitionState(StateType.Idle);
        }
    }
}
