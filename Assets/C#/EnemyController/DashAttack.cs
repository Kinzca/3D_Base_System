using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace C_.EnemyController
{
	public class DashAttack : Action
	{
		private Animator _animator;
		public SharedBool DashAttackFinish;

		public override void OnAwake()
		{
			_animator = GetComponent<Animator>();
		}

		public override void OnStart()
		{
			_animator.Play("SwordAndShield_Attack_Forward_SpinSlash01");
			
		}

		public override TaskStatus OnUpdate()
		{
			AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
			if (info.IsName("SwordAndShield_Attack_Forward_SpinSlash01") && info.normalizedTime >= 0.9f &&
			    !_animator.IsInTransition(0)) 
			{
				DashAttackFinish.Value = true;
			}
			
			return TaskStatus.Success;
		}
	}
}