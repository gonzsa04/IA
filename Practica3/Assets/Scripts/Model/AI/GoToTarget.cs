namespace Model.IA
{
    using UnityEngine;
    using UnityEngine.UI;
    using BehaviorDesigner.Runtime.Tasks;

    // lleva al jugador hasta una posicion seleccionada desde el editor
    [TaskCategory("FootBall")]
    public class GoToTarget : Action
    {
        private GameManager gm;
        private Team teamComp;
        private IHaveTheBall ihtb;
        
        public float speed = 0;
        public Transform target;

        public bool idle = false;

        public float minDistance = 0.1f;
        
        public Text state;
        public string stateText = "Going to the ball";

        public override void OnStart()
        {
            state.text = stateText;
            gm = GameManager.instance;
            teamComp = GetComponent<Team>();
            ihtb = gameObject.GetComponent<IHaveTheBall>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!gm.getPause())
            {
                if (Vector3.SqrMagnitude(transform.position - target.position) < minDistance)
                {
                    if (!idle)
                    {
                        gm.hasBall = teamComp.team;
                        gm.setIHaveTheBall(ihtb);
                    }
                    return TaskStatus.Success;
                }
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                return TaskStatus.Running;
            }
            return TaskStatus.Running;
        }
    }
}
