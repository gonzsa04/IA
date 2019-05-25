namespace Model.IA
{
    using UnityEngine;
    using UnityEngine.UI;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class GoToTarget : Action
    {
        private GameManager gm;
        private Team teamComp;
        private IHaveTheBall ihtb;

        // The speed of the object
        public float speed = 0;
        // The transform that the object is moving towards
        public Transform target;

        public TEAM team = TEAM.NONE;

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
                // Return a task status of success once we've reached the target
                if (Vector3.SqrMagnitude(transform.position - target.position) < minDistance)
                {
                    if (!idle)
                    {
                        gm.hasBall = teamComp.team;
                        gm.setIHaveTheBall(ihtb);
                        return TaskStatus.Success;
                    }
                    else
                    {
                        return TaskStatus.Running;
                    }
                }
                // We haven't reached the target yet so keep moving towards it
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                return TaskStatus.Running;
            }
            return TaskStatus.Running;
        }
    }
}
