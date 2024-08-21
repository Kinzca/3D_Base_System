using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace C_.EnemyController
{
	public class JudgeDashAttackFinish : Conditional
	{
		public SharedBool DashAttackFinish;
		public bool TargetBool;
		
		public override TaskStatus OnUpdate()
		{
            
			if (DashAttackFinish.Value == TargetBool)
			{
				return TaskStatus.Success;
			}

			return TaskStatus.Failure;
		}
	}
}