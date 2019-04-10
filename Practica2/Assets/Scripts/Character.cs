namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    public abstract class Character
    {
        public Ficha ficha_;
        public int index;

        protected GameManager gm;

        public abstract void onClicked();

        public Character (Ficha fichaPrefab, int i)
        {
            ficha_ = Ficha.Instantiate(fichaPrefab);
            ficha_.setCharacter(this);
            index = i;
            gm = GameManager.instance;
        }

        protected void move(Position posL, Vector3 posP)
        {
            ficha_.setLogicPosition(posL);
            ficha_.setPosition(posP);
        }
    }
}

