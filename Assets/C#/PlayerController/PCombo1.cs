using UnityEngine;

namespace C_.PlayerController
{
    public class PCombo1 : IState
    {
        private PlayerFsm _manager;
        private PlayerParamenter _paramenter;
        private int _comboStep;
        private bool _canPlayNextCombo;
        private bool _endPoint;

        public PCombo1(PlayerFsm manager)
        {
            this._manager = manager;
            _paramenter = manager.paramenter;
            _comboStep = 1;
            _canPlayNextCombo = false;
            _endPoint = true;
        }

        public void OnEnter()
        {
            _comboStep = 1;  // 确保每次进入状态时从第一段攻击开始
            PlayComboAnimation();
        }

        public void OnUpdate()
        {
            JudgeCombo1();  //判断combo连段
            CheckComboAnimation();  //检查并播放combo动画
        }

        public void OnExit()
        {
            //处理事件退出时的逻辑，比如取消事件订阅等
            _comboStep = 1;  // 重置 combo 步骤
            _canPlayNextCombo = false;  // 重置连击标志
            _endPoint = true;
        }

        private void JudgeCombo1()
        {
            AnimatorStateInfo stateInfo = _paramenter.animator.GetCurrentAnimatorStateInfo(0);
            var fire1Input = Input.GetButtonDown("Fire1");  //左键
            var fire2Input = Input.GetButtonDown("Fire2");  //右键

            if (stateInfo.normalizedTime >= 0.5f && stateInfo.normalizedTime < 0.9f)
            {
                if (_comboStep == 1 && fire1Input && _endPoint) // 当前第一段攻击，左键触发 Combo 2
                {
                    _comboStep++;
                    _canPlayNextCombo = true;
                    _endPoint = false;
                    Debug.Log("_comboStep: " + _comboStep);
                }
                else if (_comboStep == 2 && fire2Input && _endPoint) // 当前第二段攻击，右键触发 Combo 3
                {
                    _comboStep++;
                    _canPlayNextCombo = true;
                    _endPoint = false;
                    Debug.Log("_comboStep: " + _comboStep);
                }
                else if (_comboStep == 3 && fire2Input && _endPoint) // 当前第三段攻击，右键触发 Combo 4
                {
                    _comboStep++;
                    _canPlayNextCombo = true;
                    _endPoint = false;
                    Debug.Log("_comboStep: " + _comboStep);
                }
            }
            else if (stateInfo.normalizedTime >= 0.9f && !_canPlayNextCombo)
            {
                // 动作即将结束且未输入，重置为 idle 状态
                _manager.TransitionState(StateType.Idle);
                _comboStep = 1;  // 重新开始 combo
            }
        }

        private void CheckComboAnimation()
        {
            AnimatorStateInfo stateInfo = _paramenter.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 0.90f && _canPlayNextCombo) // 调整为 0.95f 确保动画基本播放完成
            {
                PlayComboAnimation();
                _canPlayNextCombo = false; // 播放完下一段动画后，将可以进行攻击连段调为 false 进行下一次的攻击检测
                _endPoint = true;
            }
        }

        private void PlayComboAnimation()
        {
            switch (_comboStep)
            {
                case 1:
                    _paramenter.animator.Play("Combo_01_1");
                    break;
                case 2:
                    _paramenter.animator.Play("Combo_01_2");
                    break;
                case 3:
                    _paramenter.animator.Play("Combo_01_3");
                    break;
                case 4:
                    _paramenter.animator.Play("Combo_01_4");
                    break;
            }
        }
    }
}
