namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class ThisTeamHasTheBall : Conditional
    {
        // The transform that the object is moving towards

        private GameManager gm;

        public TEAM team;

        public override void OnStart()
        {
            gm = GameManager.instance;
        }

        public override TaskStatus OnUpdate()
        {
            if (gm.hasBall == team)
                    return TaskStatus.Success;
            else return TaskStatus.Failure;
        }
    }
}