namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    // hace que el jugador chute la pelota en un rango de direcciones (apuntando hacia strikeTrans)
    // con parametros offset que lo hacen mas o menos preciso. Puede usarse tanto para chutar a porteria
    // como para despejar
    [TaskCategory("FootBall")]
    public class StrikeBall : Action
    {
        public float maxSpeed = 150;
        public float minSpeed = 40;

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

            float speed = Random.Range(minSpeed, maxSpeed);

            ballRB.velocity = direction * speed;

            GameManager.instance.playKickSound();
            GameManager.instance.clearBall();
        }
    }
}
