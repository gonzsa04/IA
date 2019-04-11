namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    //Jugador humano, del que heredaran los bots
    public class Player : Character
    {
        private System.Random rnd = new System.Random();

        public Libreta libreta_;        //Base de conocimiento
        public List<string> cards_;     //Cartas que se le han repartido
        public bool moved = false;
        public bool asked = false;
        public bool supposed = false;

        public Player(Ficha fichaPrefab, Libreta lib, int i) : base(fichaPrefab, i)
        {
            libreta_ = lib;
            libreta_.Initialize();
            cards_ = new List<string>();
        }

        //El jugador humano no hara nada en su turno
        public virtual void myTurn() { }

        //Marca la posicion logica posL como que tiene un jugador, y se mueve a la posicion fisica posP (tambien actualiza la estancia actual)
        public new void move(Position posL, Vector3 posP)
        {
            gm.changeTienePlayer(ficha_.getLogicPosition().GetRow(), ficha_.getLogicPosition().GetColumn());
            libreta_.estanciaActual = gm.getTipoEstancia(posL.GetRow(), posL.GetColumn());
            base.move(posL, posP);
            moved = true;
        }

        //Muestra todas sus cartas a los demas jugadores
        public void showAllCards()
        {
            int turno = gm.getTurn();
            for(int i = 0; i < cards_.Count; i++)
            {
                for (int j = 0; j < gm.numPlayers; j++)
                {
                    if (j != turno)
                    {
                        Player aux = (Player)gm.characters[j];
                        aux.libreta_.receiveCard(cards_[i], index);
                        aux.libreta_.notReceivedCards(index);
                    }
                }
            }
        }

        //Simula un click y permite empezar a otro jugador una suposicion hacia ti
        public override void onClicked() {
            if (!gm.GameOver)
            {
                int turno = gm.getTurn();
                Player aux = (Player)gm.characters[turno];
                if(ficha_.name_ != aux.ficha_.name_){
                    if (libreta_.estanciaActual == aux.libreta_.estanciaActual)
                    {
                        gm.estanciasSupPanel.SetActive(true);
                        gm.playerPreguntado = ficha_.name_;
                        aux.supposed = true;
                    }
                    else
                    {
                        gm.startCanMoveRoutine();
                    }
                }
            }
        }

        //Envia las cartas coincidentes con la suposicion al jugador que ha preguntado. Si no hay coincidentes envia "ninguna"
        //Si hay varias coincidentes, envia una aleatoria
        public void makeSugestion()
        {
            if (!gm.GameOver)
            {
                List<string> coincidentes = new List<string>();
                int turno = gm.getTurn();
                Player aux = (Player)gm.characters[turno];
                for (int i = 0; i < gm.Suposicion.Count; i++)
                {
                    for (int j = 0; j < cards_.Count; j++)
                    {
                        if (gm.Suposicion[i] == cards_[j]) coincidentes.Add(cards_[j]);
                    }
                }

                int card = -1;
                if (coincidentes.Count > 0)
                {
                    card = rnd.Next(0, coincidentes.Count);
                    aux.libreta_.receiveCard(coincidentes[card], index);
                    gm.cartaRecibida = coincidentes[card];
                }
                else
                {
                    gm.cartaRecibida = "Ninguna";
                    aux.libreta_.notCoincidentCardsFrom(index);
                }
                gm.startCartaCoroutine();

            }
        }
    }
}
