using System;
using System.Collections.Generic;
using C_.Package;
using UnityEngine;

namespace C_.PlayerController
{
    public enum StateType
    {
        Idle,Walk,Sprint,Duck,LeftHeavyBlow,RightHeavyBlow,PBuffDefend,PBuffAssault,PCombo1,PCombo2,PCombo3,Defence,Hit
    }
    
    [Serializable]
    public class PlayerParamenter
    {
        public Animator animator;
        public bool canAcceptInput = true;
    }

    public class PlayerFsm : MonoBehaviour
    {
        //角色属性
        public PlayerParamenter paramenter;
        
        private IState _currentState;

        //使用字典使的状态种类与IState进行键值对匹配
        private Dictionary<StateType, IState> _states = new Dictionary<StateType, IState>();

        private void Start()
        {
            //获取必要组件
            paramenter.animator = GetComponent<Animator>();
            
            //初始化状态
            _states.Add(StateType.Idle,new PIdle(this));
            _states.Add(StateType.Walk,new PWalk(this));
            _states.Add(StateType.Sprint,new PSprint(this));
            _states.Add(StateType.Duck,new PDuck(this));
            _states.Add(StateType.LeftHeavyBlow,new PLeftHeavyBlow(this));
            _states.Add(StateType.RightHeavyBlow,new PRightHeavyBlow(this));
            _states.Add(StateType.PBuffDefend,new PBuffDefend(this));
            _states.Add(StateType.PBuffAssault,new PBuffAssault(this));
            _states.Add(StateType.PCombo1,new PCombo1(this));
            _states.Add(StateType.PCombo2,new PCombo2(this));
            _states.Add(StateType.PCombo3,new PCombo3(this));
            _states.Add(StateType.Defence,new PDefend(this));
            _states.Add(StateType.Hit,new PHit(this));
            //设置初始状态
            TransitionState(StateType.Idle);
            
            //隐藏鼠标指针
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            //启用事件订阅
            C_.EventManager.EventManager.LockCursorEvent += SetCanAcceptInput;
        }

        private void Update()
        {
            if (_currentState != null && paramenter.canAcceptInput)  
            {
                _currentState.OnUpdate();
            }
        }

        private void OnDisable()
        {
            if (_currentState != null)
            {
                _currentState.OnExit();
            }
            
            //取消事件订阅及其他操作
            C_.EventManager.EventManager.LockCursorEvent -= SetCanAcceptInput;
        }

        public void TransitionState(StateType type)
        {
            if (_currentState != null)
            {
                _currentState.OnExit();
            }

            _currentState = _states[type];
            _currentState.OnEnter();
        }

        private void SetCanAcceptInput()
        {
            paramenter.canAcceptInput = !paramenter.canAcceptInput;
        }
    }
}