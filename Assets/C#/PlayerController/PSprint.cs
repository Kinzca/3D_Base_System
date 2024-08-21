using UnityEngine;

namespace C_.PlayerController
{
    public class PSprint : IState
    {
        private PlayerFsm _manager;
        private PlayerParamenter _paramenter;

        private string _currentAnimation; 
        public PSprint(PlayerFsm manager)
        {
            _manager = manager;
            _paramenter = _manager.paramenter;
        }
        
        public void OnEnter()
        {
            WhichAnimation(true);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate()
        {
            WhichAnimation(false);
            JudgeReturnIdleOrWalk();
            JudgeEnterDuck();
            JudgeEnterCombo1();
            JudgeEnterCombo2();
            JudgeEnterCombo3();
            JudgeEnterDefence();
        }

        public void OnExit()
        {
            //可以添加退出时的逻辑
        }

        //判断哪一个方向的疾跑动画
        private void WhichAnimation(bool force)
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            var leftShiftInput = Input.GetButton("Fire3");
            
            string newAnimation = null;

            if (horizontalInput == 0 && verticalInput > 0)   
            {
                newAnimation = "run_strafe_front";
            }
            else if (horizontalInput == 0 && verticalInput < 0 && leftShiftInput)  
            {
                newAnimation = "run_strafe_back";
            }
            else if (horizontalInput > 0 && verticalInput == 0 && leftShiftInput)  
            {
                newAnimation = "run_strafe_right";
            }
            else if (horizontalInput < 0 && verticalInput == 0 && leftShiftInput)  
            {
                newAnimation = "run_strafe_left";
            }
            else if (horizontalInput > 0 && verticalInput > 0 && leftShiftInput) 
            {
                newAnimation = "run_strafe_frontright";
            }
            else if (horizontalInput < 0 && verticalInput > 0 && leftShiftInput) 
            {
                newAnimation = "run_strafe_frontleft";
            }
            else if (horizontalInput < 0 && verticalInput < 0 && leftShiftInput) 
            {
                newAnimation = "run_strafe_backleft";
            }
            else if (horizontalInput > 0 && verticalInput < 0 && leftShiftInput) 
            {
                newAnimation = "run_strafe_backright";
            }

            if (newAnimation != null && (force || newAnimation != _currentAnimation))
            {
                _currentAnimation = newAnimation;
                _paramenter.animator.CrossFade(_currentAnimation, 0.2f);
            }
        }
        
        //判断返回idle还是Walk
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

        private void JudgeEnterDuck()
        {
            if (Input.GetButtonDown("Duck"))
            {
                _manager.TransitionState(StateType.Duck);
            }
        }
        
        //第一段连招，右左右右
        private void JudgeEnterCombo1()
        {
            //var fire1Input = Input.GetButtonDown("Fire1");//左键
            var fire2Input = Input.GetButtonDown("Fire2");//右键
            //var leftShiftInput = Input.GetButton("Fire3");

            if (fire2Input) 
            {
                _manager.TransitionState(StateType.PCombo1);
            }
        }
        
        //第二段连招，C+右键，左，右，空格+右
        private void JudgeEnterCombo2()
        {
            var defendInput = Input.GetButton("Defend");
            var fire2Input = Input.GetButtonDown("Fire2");

            if (defendInput && fire2Input) 
            {
                _manager.TransitionState(StateType.PCombo2);
            }
        }
        
        //第三段连招，左，右，左，shift+右
        private void JudgeEnterCombo3()
        {
            var fire1Input = Input.GetButtonDown("Fire1");
            //var leftShiftInput = Input.GetButton("Fire3");
            
            if (fire1Input) 
            {
                _manager.TransitionState(StateType.PCombo3);
            }
        }
        
        private void JudgeEnterDefence()
        {
            var defendInput = Input.GetButtonDown("Defend");
            var fire2Input = Input.GetButtonDown("Fire2");  //右键

            if (defendInput && !fire2Input) 
            {
                _manager.TransitionState(StateType.Defence);
            }
        }
    }
}
