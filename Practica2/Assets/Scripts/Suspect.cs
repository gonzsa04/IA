namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    public class Suspect : Character
    {
        public Suspect(Ficha fichaPrefab, int i) : base(fichaPrefab, i)
        {
        }

        public new void move(Position posL, Vector3 posP)
        {
            Player aux = (Player)gm.characters[gm.getTurn()];
            gm.changeTieneSuspect(ficha_.getLogicPosition().GetRow(), ficha_.getLogicPosition().GetColumn());
            base.move(posL, posP);
            aux.asked = true;
        }

        public override void onClicked() {
            if (!gm.GameOver)
            {
                gm.moveSuspect(this);
            }
        }
    }
}