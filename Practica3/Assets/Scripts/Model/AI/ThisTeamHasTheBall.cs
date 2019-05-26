namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    // indica si el equipo seleccionado desde el editor tiene actualmente la pelota
    [TaskCategory("FootBall")]
    public class ThisTeamHasTheBall : Conditional
    {
        private GameManager gm;

        public TEAM team;

        public override void OnStart()
        {
            gm = GameManager.instance;
        }

        public override TaskStatus OnUpdate()
        {
            if (!gm.getPause())
            {
                if (gm.hasBall == team)
                    return TaskStatus.Success;
                else return TaskStatus.Failure;
            }
            return TaskStatus.Running;
        }
    }
}