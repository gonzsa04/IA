namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    public class Player : Character
    {
        private System.Random rnd = new System.Random();

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

        public void showAllCards()
        {
            int turno = GameManager.instance.getTurn();
            for(int i = 0; i < cards_.Count; i++)
            {
                for (int j = 0; j < GameManager.instance.numPlayers; j++)
                {
                    if (j != turno)
                    {
                        Player aux = (Player)GameManager.instance.characters[j];
                        aux.libreta_.receiveCard(cards_[i], turno);
                    }
                }
            }
        }

        public override void onClicked() {
            if (!GameManager.instance.GameOver)
            {
                int turn = GameManager.instance.getTurn();
                Player aux = (Player)GameManager.instance.characters[turn];
                if (libreta_.estanciaActual == aux.libreta_.estanciaActual)
                {
                    aux.libreta_.receiveCard(cards_[rnd.Next(0, cards_.Count)], turn);
                    GameManager.instance.nextTurn();
                }
                else
                {
                    GameManager.instance.startCanMoveRoutine(2.0f);
                }
            }
        }
    }
}
