using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace C_.EnemyController
{
	public class JudgeDashAttack : Conditional
	{
		public SharedBool DashAttack;
		public bool TargetBool;
		
		public override TaskStatus OnUpdate()
		{
			if (DashAttack.Value == TargetBool)
			{
				return TaskStatus.Success;
			}

			return TaskStatus.Failure;
		}
	}
}