namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class NearestToGoal : Conditional
    {
        private Team teamComp;
        public Transform goalTrans;

        public override void OnStart()
        {
            teamComp = GetComponent<Team>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameManager.instance.getPause())
            {
                if (GameManager.instance.getMinPlayerToTarget(teamComp.team, goalTrans) == this.gameObject)
                    return TaskStatus.Success;
                else return TaskStatus.Failure;
            }
            return TaskStatus.Running;
        }
    }
}
