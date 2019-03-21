namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    public class Player : Character
    {
        private Libreta libreta_;

        public Player(Ficha fichaPrefab) : base(fichaPrefab)
        {
            /*libreta_ = new Libreta();
            libreta_.Initialize();*/
        }

        public new void move(Position posL, Vector3 posP)
        {
            GameManager.instance.changeTienePlayer(ficha_.getLogicPosition().GetRow(), ficha_.getLogicPosition().GetColumn());
            base.move(posL, posP);
        }

        public override void onClicked() {
            GameManager.instance.startCanMoveRoutine(2.0f);
        }
    }
}
