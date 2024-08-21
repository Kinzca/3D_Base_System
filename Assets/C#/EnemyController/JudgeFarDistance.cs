using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace C_.EnemyController
{
	public class JudgeFarDistance : Conditional
	{
		private Transform _playerPosition;

		public float FarDistance;
		public SharedBool DashAttack;
		
		public override void OnStart()
		{
			_playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
		}

		public override TaskStatus OnUpdate()
		{
			var distanceFromPlayer = Vector3.Distance(transform.position, _playerPosition.position);

			if (distanceFromPlayer > FarDistance || DashAttack.Value) 
			{
				DashAttack.Value = true;
				
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}
	}
}