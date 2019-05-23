namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class CanCatchBall : Conditional
    {
        // The transform that the object is moving towards

        private GameManager gm;
        private Team teamComp;

        public override void OnStart()
        {
            gm = GameManager.instance;
            teamComp = GetComponent<Team>();
        }

        public override TaskStatus OnUpdate()
        {
            if (gm.hasBall == TEAM.NONE)
            {
                gm.hasBall = teamComp.team;
                return TaskStatus.Success;
            }
            else return TaskStatus.Failure;
        }
    }
}
