namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    // indica si la pelota esta en tu campo
    [TaskCategory("FootBall")]
    public class DefensiveCondition : Conditional
    {
        public TEAM enemyField;
        public Transform ballTrans;
        public float centerPos = 0;

        public override TaskStatus OnUpdate()
        {
            if (!GameManager.instance.getPause())
            {
                if ((enemyField != TEAM.B && ballTrans.position.x > centerPos) ||
                (enemyField != TEAM.A && ballTrans.position.x < centerPos))
                    return TaskStatus.Success;

                else return TaskStatus.Failure;
            }
            return TaskStatus.Running;
        }
    }
}

