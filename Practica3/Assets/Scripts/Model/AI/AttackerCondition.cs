namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    // indica si la pelota esta libre y en el campo enemigo (condiciones para atacar)
    [TaskCategory("FootBall")]
    public class AttackerCondition : Conditional
    {
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
            if (!gm.getPause())
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
            return TaskStatus.Running;
        }
    }
}
