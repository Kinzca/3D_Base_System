using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace C_.EnemyController
{
	public class EnemyIdle : Action
	{
		private Animator _animator;

		public override void OnAwake()
		{
			_animator = GetComponent<Animator>();
		}

		public override void OnStart()
		{
			_animator.Play("SwordAndShield_Idle_LookingAround");
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}
	}
}