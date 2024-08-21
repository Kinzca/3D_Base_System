using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace C_.EnemyController
{
	public class ConfirmDistance : Conditional
	{
		public SharedTransform PlayerTransform;
		public SharedBool keepDistance;
		public float TargetDistance;

		private Transform _enemyTransform;
		
		public override void OnStart()
		{
			_enemyTransform = transform;
		}

		public override TaskStatus OnUpdate()
		{
			float distance = Vector3.Distance(_enemyTransform.position, PlayerTransform.Value.position);

			if (distance >= TargetDistance)
			{
				keepDistance.Value = true;
//				Debug.Log("KeepDistance" + keepDistance);
				return TaskStatus.Success;
			}

			return TaskStatus.Failure;
		}
	}
}