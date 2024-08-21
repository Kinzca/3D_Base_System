using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace C_.EnemyController
{
	public class JudgeBattleStatus : Conditional
	{
		public SharedBool BattleState;
		public bool TargetBool;
		
		public override TaskStatus OnUpdate()
		{

			//Debug.Log("BattleState:" + BattleState.Value);
			if (BattleState.Value == TargetBool)
			{
				return TaskStatus.Success;
			}

			return TaskStatus.Failure;
		}
	}
}