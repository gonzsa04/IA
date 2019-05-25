namespace Model.IA
{
    using UnityEngine;
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("FootBall")]
    public class HavingTheBall : Conditional
    {
        private IHaveTheBall ihtb;

        public override void OnStart()
        {
            ihtb = gameObject.GetComponent<IHaveTheBall>();
        }

        public override TaskStatus OnUpdate()
        {
            if (ihtb.getBool()) return TaskStatus.Success;
            else return TaskStatus.Failure;
        }
    }
}
