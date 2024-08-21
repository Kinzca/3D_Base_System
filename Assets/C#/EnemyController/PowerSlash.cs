using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace C_.EnemyController
{
	public class PowerSlash : Action
	{
		private Animator _animator;
		public float MoveSpeed;
		public SharedTransform PlayerTransform;
		public SharedBool KeepDistance;
		
		public override void OnStart()
		{
			_animator = GetComponent<Animator>();
			
			_animator.Play("SwordAndShield_Attack_Forward_PowerSlash01");
		}

		public override TaskStatus OnUpdate()
		{
			Vector3 directionToPlayer = (PlayerTransform.Value.position - transform.position).normalized;

			Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * MoveSpeed);
			transform.position = Vector3.MoveTowards(transform.position, PlayerTransform.Value.position,
				MoveSpeed * Time.deltaTime);

			KeepDistance.Value = false;
			
			return TaskStatus.Success;
		}
	}
}