namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class StrikeBall : Action
    {
        // The transform that the object is moving towards
        public Rigidbody ballRB;
        public Transform ballTrans;
        public Transform goalTrans;
        public float minDistance = -3;

        private GameManager gm;
        private Team teamComp;

        public override void OnStart()
        {
            gm = GameManager.instance;
            teamComp = GetComponent<Team>();
            ballRB.isKinematic = true;
            var targetPosition = goalTrans.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
            transform.position = Vector3.MoveTowards(ballTrans.position, goalTrans.position, minDistance);
            ballRB.isKinematic = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (gm.hasBall == TEAM.NONE)
            {
                gm.hasBall = teamComp.team;
                return TaskStatus.Success;
            }
            else return TaskStatus.Failure;
        }
    }
}
