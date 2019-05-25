namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class StrikeBall : Action
    {
        // The speed of the object
        public float speed = 0;

        public float left = 0, right = 0, up = 0, down = 0;

        public Rigidbody ballRB; 

        public Transform strikeTrans;

        public override void OnStart()
        {
            float zpos, ypos;

            zpos = Random.Range((-strikeTrans.localScale.z - right / 2) / 2, (strikeTrans.localScale.z + left / 2) / 2);
            ypos = Random.Range((-strikeTrans.localScale.y - up / 2) / 2, (strikeTrans.localScale.y + down / 2) / 2);

            Vector3 direction = new Vector3(strikeTrans.position.x - transform.position.x, 
                strikeTrans.position.y + ypos - transform.position.y,
                strikeTrans.position.z + zpos - transform.position.z);

            direction.Normalize();

            ballRB.velocity = direction * speed;

            GameManager.instance.clearBall();
        }
    }
}
