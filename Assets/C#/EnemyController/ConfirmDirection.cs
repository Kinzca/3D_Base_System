using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace C_.EnemyController
{
    public class ConfirmDirection : Conditional
    {
        //TargetValue的值1为左，2为中，三为右；
        public SharedInt Direction;
        public SharedBool KeepDistance;
        public int TargetValue;
        
        public override TaskStatus OnUpdate()
        {
            if (Direction.Value == TargetValue && !KeepDistance.Value)
            {

//                Debug.Log("directionValue" + Direction.Value + "KeepDistance" + KeepDistance.Value);
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}
