﻿namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    public class Suspect : Character
    {
        public Suspect(Ficha fichaPrefab) : base(fichaPrefab)
        {
        }

        public new void move(Position posL, Vector3 posP)
        {
            GameManager.instance.changeTieneSuspect(ficha_.getLogicPosition().GetRow(), ficha_.getLogicPosition().GetColumn());
            base.move(posL, posP);
        }

        public override void onClicked() {
            GameManager.instance.moveSuspect(this);
        }
    }
}