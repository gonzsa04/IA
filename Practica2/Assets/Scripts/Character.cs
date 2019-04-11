namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    //Clase padre de los jugadores y los sospechosos
    public abstract class Character
    {
        public Ficha ficha_;    //Representacion grafica
        public int index;       //Que personaje eres en la lista de personajes

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

