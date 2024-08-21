using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace C_.EnemyController
{
    public class DetectPlayer : Conditional
    {
        public float ViewRadius = 10f; // 视角半径
        public float ViewAngle = 45f; // 视角角度
        public LayerMask TargetMask; // 目标层
        public LayerMask ObstacleMask; // 障碍物层

        public SharedBool BattleState; // 使用自定义的共享变量

        public override TaskStatus OnUpdate()
        {
            return FindVisibleTarget();
        }

        private TaskStatus FindVisibleTarget()
        {
            Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, TargetMask);

            for (int i = 0; i < targetInViewRadius.Length; i++)
            {
                Transform target = targetInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized; // 归一化敌人到玩家的矢量方向

                if (Vector3.Angle(transform.forward, dirToTarget) < ViewAngle / 2) // 判断玩家是否在敌人视角中
                {
                    float disToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, ObstacleMask))
                    {
                        Debug.Log("检测到敌人");

                        BattleState.Value = true; // 只要一进入视野范围就将战斗状态调为 true，不可逆

                        return TaskStatus.Success;
                    }
                }
            }
            return TaskStatus.Failure;
        }

        public override void OnDrawGizmos()
        {
            if (ViewRadius <= 0)
            {
                Debug.LogError("ViewRadius must be greater than zero.");
                return;
            }

            if (ViewAngle <= 0 || ViewAngle > 360)
            {
                Debug.LogError("ViewAngle must be between 0 and 360 degrees.");
                return;
            }

            if (TargetMask == 0)
            {
                Debug.LogError("TargetMask is not set.");
                return;
            }

            if (ObstacleMask == 0)
            {
                Debug.LogError("ObstacleMask is not set.");
                return;
            }

            if (transform == null)
            {
                Debug.LogError("Transform is null.");
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ViewRadius);

            Vector3 viewAngleA = DirFromAngle(-ViewAngle / 2, false);
            Vector3 viewAngleB = DirFromAngle(ViewAngle / 2, false);

            Gizmos.DrawLine(transform.position, transform.position + viewAngleA * ViewRadius);
            Gizmos.DrawLine(transform.position, transform.position + viewAngleB * ViewRadius);
        }

        private Vector3 DirFromAngle(float angleInDegrees, bool anglesGlobal)
        {
            if (!anglesGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}
