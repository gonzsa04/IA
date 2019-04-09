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
        public bool moved = false;
        public bool asked = false;
        public bool supposed = false;

        public Player(Ficha fichaPrefab, Libreta lib, int i) : base(fichaPrefab, i)
        {
            libreta_ = lib;
            libreta_.Initialize();
            cards_ = new List<string>();
        }

        public virtual void update() { }

        public new void move(Position posL, Vector3 posP)
        {
            GameManager.instance.changeTienePlayer(ficha_.getLogicPosition().GetRow(), ficha_.getLogicPosition().GetColumn());
            libreta_.estanciaActual = GameManager.instance.getTipoEstancia(posL.GetRow(), posL.GetColumn());
            base.move(posL, posP);
            moved = true;
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
                        aux.libreta_.receiveCard(cards_[i], index);
                    }
                }
            }
        }

        public override void onClicked() {
            if (!GameManager.instance.GameOver)
            {
                int turno = GameManager.instance.getTurn();
                Player aux = (Player)GameManager.instance.characters[turno];
                if(ficha_.name_ != aux.ficha_.name_){
                    if (libreta_.estanciaActual == aux.libreta_.estanciaActual)
                    {
                        GameManager.instance.estanciasSupPanel.SetActive(true);
                        GameManager.instance.playerPreguntado = ficha_.name_;
                        aux.supposed = true;
                    }
                    else
                    {
                        GameManager.instance.startCanMoveRoutine(2.0f);
                    }
                }
            }
        }

        public void makeSugestion()
        {
            if (!GameManager.instance.GameOver)
            {
                List<string> coincidentes = new List<string>();
                int turno = GameManager.instance.getTurn();
                Player aux = (Player)GameManager.instance.characters[turno];
                for (int i = 0; i < GameManager.instance.Suposicion.Count; i++)
                {
                    for (int j = 0; j < cards_.Count; j++)
                    {
                        if (GameManager.instance.Suposicion[i] == cards_[j]) coincidentes.Add(cards_[j]);
                    }
                }

                int card = -1;
                if (coincidentes.Count > 0)
                {
                    card = rnd.Next(0, coincidentes.Count);
                    aux.libreta_.receiveCard(coincidentes[card], index);
                    GameManager.instance.cartaRecibida = coincidentes[card];
                }
                else
                {
                    GameManager.instance.cartaRecibida = "Ninguna";
                    aux.libreta_.notCoincidentCardsFrom(index);
                }
                GameManager.instance.startCartaCoroutine(2.0f);

            }
        }
    }
}
