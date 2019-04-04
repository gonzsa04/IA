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
            Player aux = (Player)GameManager.instance.characters[GameManager.instance.getTurn()];
            GameManager.instance.changeTieneSuspect(ficha_.getLogicPosition().GetRow(), ficha_.getLogicPosition().GetColumn());
            base.move(posL, posP);
            aux.asked = true;
        }

        public override void onClicked() {
            if (!GameManager.instance.GameOver)
            {
                GameManager.instance.moveSuspect(this);
            }
        }
    }
}