using UnityEngine;

namespace C_.PlayerController
{
    public class PIdle : IState
    {
        private PlayerFsm _manager;
        private PlayerParamenter _paramenter;

        public PIdle(PlayerFsm manager)
        {
            this._manager = manager;
            _paramenter = manager.paramenter;
        }
        
        public void OnEnter()
        {
            //Debug.Log("进入idle状态");
            _paramenter.animator.CrossFade("idle",0.3f);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate()
        {
            JudgeEnterSprint();
            JudgeEnterWalk();
            JudgeEnterLeftHeavyBlow();
            JudgeEnterRightHeavyBlow();
            JudgeEnterBuffDefend();
            JudgeEnterAssault();
            JudgeEnterCombo1();
            JudgeEnterCombo2();
            JudgeEnterCombo3();
            JudgeEnterDefence();
        }

        public void OnExit()
        {
            //推出该状态时运行的代码
        }
        
        //判断角色移动
        private void JudgeEnterWalk()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            
            if (horizontalInput != 0 || verticalInput != 0) 
            {
                _manager.TransitionState(StateType.Walk);
            }
        }
        
        private void JudgeEnterSprint()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            var directionInput = (horizontalInput != 0 || verticalInput != 0);
            
            var leftShiftInput = Input.GetButtonDown("Fire3");

            if (leftShiftInput && directionInput) 
            {
                _manager.TransitionState(StateType.Sprint);
            }
        }

        private void JudgeEnterLeftHeavyBlow()
        {
            var leftShiftInput = Input.GetButton("Fire3");
            var leftMouseInput = Input.GetButtonDown("Fire1");

            //按住Leftshift键的同时按下LeftMouse键
            if (leftShiftInput && leftMouseInput) 
            {
                _manager.TransitionState(StateType.LeftHeavyBlow);
            }
        }
        
        private void JudgeEnterRightHeavyBlow()
        {
            var leftShiftInput = Input.GetButton("Fire3");
            var rightMouseInput = Input.GetButtonDown("Fire2");

            //按住Leftshift键的同时按下LeftMouse键
            if (leftShiftInput && rightMouseInput) 
            {
                _manager.TransitionState(StateType.RightHeavyBlow);
            }
        }

        private void JudgeEnterBuffDefend()
        {
            var buffDefendInput = Input.GetButtonDown("Buff1");

            if (buffDefendInput)
            {
                _manager.TransitionState(StateType.PBuffDefend);
            }
        }
        
        private void JudgeEnterAssault()
        {
            var buffAssaultInput = Input.GetButtonDown("Buff2");

            if (buffAssaultInput)
            {
                _manager.TransitionState(StateType.PBuffAssault);
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

        //第一段连招，右左右右
        private void JudgeEnterCombo1()
        {
            //var fire1Input = Input.GetButtonDown("Fire1");//左键
            var fire2Input = Input.GetButtonDown("Fire2");//右键
            var leftShiftInput = Input.GetButton("Fire3");

            if (fire2Input && !leftShiftInput) 
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
            var leftShiftInput = Input.GetButton("Fire3");
            
            if (fire1Input && !leftShiftInput) 
            {
                _manager.TransitionState(StateType.PCombo3);
            }
        }
    }
}
