using System;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace C_.EnemyController
{
    public class MoveToPlayer : Action
    {
        private Transform _playerPosition;
        private Animator _animator;
        public float MoveSpeed;
        public String AnimatorName;

        public override void OnAwake()
        {
            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
            _animator = GetComponent<Animator>();
        }

        public override void OnStart()
        {
            if (_animator == null)
            {
                Debug.LogError("Animator component not found on the game object!");
                return;
            }

            // Check if the animator has the state in the base layer (layer 0)
            if (_animator.HasState(0, Animator.StringToHash(AnimatorName)))
            {
                _animator.Play(AnimatorName, 0);
            }
            else
            {
                Debug.LogError($"Animation state '{AnimatorName}' not found in the Animator!");
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_playerPosition == null)
            {
                Debug.LogError("Player position not found!");
                return TaskStatus.Failure;
            }

            // 计算玩家与敌人的矢量方向
            Vector3 direction = (_playerPosition.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * MoveSpeed);
            transform.position = Vector3.MoveTowards(transform.position, _playerPosition.position, MoveSpeed * Time.deltaTime);

            return TaskStatus.Success;
        }
    }
}