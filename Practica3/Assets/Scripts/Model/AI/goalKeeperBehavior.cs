namespace Model.IA
{
    using UnityEngine;
    using UnityEngine.UI;
    using BehaviorDesigner.Runtime.Tasks;

    // mueve al jugador en uno de los ejes dependiendo de la posicion de la pelota
    // para proteger asi su porteria
    [TaskCategory("FootBall")]
    public class goalKeeperBehavior : Action
    {
        private Team teamComp;
        private bool saved = false;
        
        public float speed = 0;
        public Transform ballTrans;

        public Transform goalTrans;

        public Text state;
        public string stateText = "Following the ball´s axis";

        public override void OnStart()
        {
            state.text = stateText;
            teamComp = GetComponent<Team>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameManager.instance.getPause())
            {
                Vector3 target = new Vector3(transform.position.x, transform.position.y, ballTrans.position.z);

                if (target.z > goalTrans.position.z + goalTrans.localScale.z / 2) target.z = goalTrans.position.z + goalTrans.localScale.z / 2;
                else if (target.z < goalTrans.position.z - goalTrans.localScale.z / 2) target.z = goalTrans.position.z - goalTrans.localScale.z / 2;

                float distance = Vector3.Distance(ballTrans.position, transform.position);
                if (distance < 5)
                {
                    if (!saved)
                    {
                        if (teamComp.team == TEAM.A) GameManager.instance.numSavesA++;
                        else GameManager.instance.numSavesB++;
                        GameManager.instance.updateUI();
                        saved = true;
                    }
                }
                else if (saved)
                    saved = false;

                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                return TaskStatus.Running;
            }
            return TaskStatus.Running;
        }
    }
}