﻿namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    public class Player : Character
    {
        public Libreta libreta_;
        public List<string> cards_;

        public Player(Ficha fichaPrefab, Libreta lib, int i) : base(fichaPrefab, i)
        {
            libreta_ = lib;
            libreta_.Initialize();
            cards_ = new List<string>();
        }

        public new void move(Position posL, Vector3 posP)
        {
            GameManager.instance.changeTienePlayer(ficha_.getLogicPosition().GetRow(), ficha_.getLogicPosition().GetColumn());
            libreta_.estanciaActual = GameManager.instance.getTipoEstancia(posL.GetRow(), posL.GetColumn());
            base.move(posL, posP);
        }

        public override void onClicked() {
            GameManager.instance.startCanMoveRoutine(2.0f);
        }
    }
}