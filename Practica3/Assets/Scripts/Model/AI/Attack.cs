namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class Attack : Action
    {
        public Transform goalTrans;
        public float minDistance = 10;
        public float ballDistance = -3;
        public float speed = 10;
        public Transform ballTrans;
        public Rigidbody ballRB;

        private GameManager gm;

        public override void OnStart()
        {
            ballRB.isKinematic = true;
            var targetPosition = goalTrans.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
            transform.position = Vector3.MoveTowards(ballTrans.position, goalTrans.position, ballDistance);
            ballRB.isKinematic = false;
        }

        public override TaskStatus OnUpdate()
        {
            float distance = Vector3.SqrMagnitude(transform.position - goalTrans.position);
            Debug.Log(distance);
            if (distance < minDistance)
            {
                return TaskStatus.Success;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, goalTrans.position, speed * Time.deltaTime);
                return TaskStatus.Running;
            }
        }
    }
}
