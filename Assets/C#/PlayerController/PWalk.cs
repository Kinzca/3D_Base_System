using UnityEngine;

namespace C_.PlayerController
{
    public class PWalk : IState
    {
        private PlayerFsm _manager;
        private PlayerParamenter _parameter;

        private string _currentAnimation;

        public PWalk(PlayerFsm manager)
        {
            this._manager = manager;
            _parameter = manager.paramenter;
        }
        
        public void OnEnter()
        {
            WhichAnimation(true);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate()
        {
            WhichAnimation(false);
            JudgeEnterSprint();
            JudgeReturnIdle();
            JudgeEnterBuffDefend();
            JudgeEnterAssault();
            JudgeEnterCombo1();
            JudgeEnterCombo2();
            JudgeEnterCombo3();
            JudgeEnterDefence();
        }

        public void OnExit()
        {
            // 可以添加退出状态时的逻辑
        }
        
        //判断行走所用的哪一个动画
        private void WhichAnimation(bool force)
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            string newAnimation = null;

            if (horizontalInput == 0 && verticalInput > 0)  
            {
                newAnimation = "walk_strafe_front";
            }
            else if (horizontalInput == 0 && verticalInput < 0) 
            {
                newAnimation = "walk_strafe_back";
            }
            else if (horizontalInput > 0 && verticalInput == 0) 
            {
                newAnimation = "walk_strafe_right";
            }
            else if (horizontalInput < 0 && verticalInput == 0) 
            {
                newAnimation = "walk_strafe_left";
            }
            else if (horizontalInput > 0 && verticalInput > 0)
            {
                newAnimation = "walk_strafe_frontright";
            }
            else if (horizontalInput < 0 && verticalInput > 0)
            {
                newAnimation = "walk_strafe_frontleft";
            }
            else if (horizontalInput < 0 && verticalInput < 0)
            {
                newAnimation = "walk_strafe_backleft";
            }
            else if (horizontalInput > 0 && verticalInput < 0)
            {
                newAnimation = "walk_strafe_backright";
            }

            //判断是否需要切换动画，进入该状态或切换新的状态，才需要切换动画
            if (newAnimation != null && (force || newAnimation != _currentAnimation))
            {
                _currentAnimation = newAnimation;
                _parameter.animator.CrossFade(_currentAnimation, 0.1f);
            }
        }

        //判断是否进入疾跑
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
        
        //判断是否返回闲置状态
        private void JudgeReturnIdle()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");

            if (horizontalInput == 0 && verticalInput == 0) 
            {
                _manager.TransitionState(StateType.Idle);
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
