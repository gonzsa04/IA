namespace Model.IA
{
    using UnityEngine;
    using UnityEngine.UI;
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

        public Text state;
        public string stateText = "Trying to shoot";

        public override void OnStart()
        {
            state.text = stateText;
            ballRB.isKinematic = true;
            var targetPosition = goalTrans.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
            ballRB.isKinematic = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameManager.instance.getPause())
            {
                float distance = Vector3.SqrMagnitude(transform.position - goalTrans.position);
                if (distance < minDistance)
                {
                    return TaskStatus.Success;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(ballTrans.position, goalTrans.position, ballDistance);
                    transform.position = Vector3.MoveTowards(transform.position, goalTrans.position, speed * Time.deltaTime);
                    return TaskStatus.Running;
                }
            }
            return TaskStatus.Running;
        }
    }
}
