namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class AttackerCondition : Conditional
    {
        // The transform that the object is moving towards

        private GameManager gm;
        private IHaveTheBall ihtb;

        public TEAM enemyField;
        public Transform ballTrans;
        public float centerPos = 0;

        public override void OnStart()
        {
            gm = GameManager.instance;
            ihtb = gameObject.GetComponent<IHaveTheBall>();
        }

        public override TaskStatus OnUpdate()
        {
            if (gm.hasBall == TEAM.NONE)
            {
                if ((enemyField == TEAM.B && ballTrans.position.x > centerPos) ||
                    (enemyField == TEAM.A && ballTrans.position.x < centerPos))
                    return TaskStatus.Success;

                else return TaskStatus.Failure;
            }
            else return TaskStatus.Failure;
        }
    }
}
