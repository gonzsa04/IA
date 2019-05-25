namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class goalKeeperBehavior : Action
    {
        // The speed of the object
        public float speed = 0;
        // The transform that the object is moving towards
        public Transform ballTrans;

        public Transform goalTrans;

        public override TaskStatus OnUpdate()
        {
            // We haven't reached the target yet so keep moving towards it
            Vector3 target = new Vector3(transform.position.x, transform.position.y, ballTrans.position.z);

            if (target.z > goalTrans.position.z + goalTrans.localScale.z / 2) target.z = goalTrans.position.z + goalTrans.localScale.z / 2;
            else if (target.z < goalTrans.position.z - goalTrans.localScale.z / 2) target.z = goalTrans.position.z - goalTrans.localScale.z / 2;

            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            return TaskStatus.Running;
        }
    }
}