namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class DefensiveCondition : Conditional
    {
        public TEAM enemyField;
        public Transform ballTrans;
        public float centerPos = 0;

        public override TaskStatus OnUpdate()
        {
            if ((enemyField != TEAM.B && ballTrans.position.x > centerPos) ||
                (enemyField != TEAM.A && ballTrans.position.x < centerPos))
                return TaskStatus.Success;

            else return TaskStatus.Failure;
        }
    }
}

