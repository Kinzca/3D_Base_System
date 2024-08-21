using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;

namespace C_.EnemyController
{
	public class JudgeLessDistance : Conditional
	{
		private Transform _playerPosition;

		public float LessDistance;

		public override void OnStart()
		{
			_playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
		}

		public override TaskStatus OnUpdate()
		{
			var distanceFromPlayer = Vector3.Distance(transform.position, _playerPosition.position);
			
			if (distanceFromPlayer <= LessDistance)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}
	}
}