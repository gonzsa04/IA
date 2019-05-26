namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    // inidica si el jugador es el que mas cerca esta de su equipo a su porteria (condicion de ser portero)
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
