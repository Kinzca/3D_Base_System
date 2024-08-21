using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace C_.EnemyController
{
	public class ComboReady : Action
	{
		private Animator _animator;
		
		public override void OnStart()
		{
			_animator = GetComponent<Animator>();
			
			_animator.Play("SwordAndShield_Idle_CombatReady");
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}
	}
}