using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource;
using UnityEngine;

namespace C_.EnemyController
{
	public class KeepDistance : Action
	{
		public SharedTransform PlayerTransform;
		public float MinDistance = 5.0f;
		public float MoveSpeed = 2.0f;

		private Transform _enemyTransform;
		private Animator _animator;

		public override void OnAwake()
		{
			_animator = GetComponent<Animator>();
		}

		public override void OnStart()
		{
			_enemyTransform = transform;
			_animator.Play("SwordAndShield_Walk_Back");
		}

		public override TaskStatus OnUpdate()
		{
			BackMove();
			return TaskStatus.Success;
		}

		private void BackMove()
		{
			float distance = Vector3.Distance(_enemyTransform.position, PlayerTransform.Value.position);

			if (distance < MinDistance) 
			{
				Vector3 directionToPlayer = (_enemyTransform.position - PlayerTransform.Value.position).normalized;
				
				Quaternion lookRotation = Quaternion.LookRotation(-directionToPlayer);
				_enemyTransform.rotation =
					Quaternion.Slerp(_enemyTransform.rotation, lookRotation, Time.deltaTime * MoveSpeed);
					
				_enemyTransform.position += directionToPlayer * MoveSpeed * Time.deltaTime;
			}
		}
	}
}