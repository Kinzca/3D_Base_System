using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace C_.EnemyController
{
	public class HaveDirection : Conditional
	{
		public SharedBool FindDirection;
		public bool Target;

		public override TaskStatus OnUpdate()
		{
			if (FindDirection.Value == Target)
			{
				return TaskStatus.Success;
			}
			return 0;
		}
	}
}