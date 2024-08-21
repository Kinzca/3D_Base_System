using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace C_.EnemyController
{
	public class JudgeWhichDirection : Conditional
	{
		public SharedInt Direction=0;
		public SharedBool FindDirection;
		
		public override void OnStart()
		{
			
		}

		public override TaskStatus OnUpdate()
		{
			if (!FindDirection.Value)
			{
				Direction.Value = UnityEngine.Random.Range(1, 4);

				FindDirection.Value = true;
				
				Debug.Log("direction:" + Direction);
				return TaskStatus.Success;
			}
			
			return TaskStatus.Failure;
		}
	}
}