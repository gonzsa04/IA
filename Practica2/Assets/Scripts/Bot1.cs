namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    public class Bot1 : Player
    {
        private System.Random rnd = new System.Random(); // decisiones

        public Bot1(Ficha fichaPrefab, Libreta lib, int i) : base(fichaPrefab, lib, i)
        {
        }

        // Update is called once per frame
        public override void update()
        {
            GameManager gm = GameManager.instance;

            int random = rnd.Next(4);

            if (random < 3)
            {
                //acusar (elegir est aleatoria: si ya estas en ella elegir sospechoso y arma aleatorias, y si no, moverte a esa estancia)
            }
            else
            {
                int otherPlayer;
                do
                {
                    otherPlayer = rnd.Next(gm.numPlayers);
                } while (otherPlayer == gm.getTurn());

                Player aux = (Player)gm.characters[otherPlayer];
                gm.playerPreguntado = aux.ficha_.name_;

                if (gm.getTipoEstancia(aux.ficha_.getLogicPosition().GetRow(), aux.ficha_.getLogicPosition().GetColumn()) !=
                    gm.getTipoEstancia(ficha_.getLogicPosition().GetRow(), ficha_.getLogicPosition().GetColumn()))
                {
                    Position freePos = gm.getFreeCasInEs(gm.getPosEstancia(aux.ficha_.getLogicPosition().GetRow(), aux.ficha_.getLogicPosition().GetColumn()));
                    gm.clickTab(freePos);
                }
                createSugestion(2.0f);
            }
        }

        private void createSugestion(float time)
        {
            GameManager gm = GameManager.instance;
            string estancia = gm.estancias[rnd.Next(gm.estancias.Length)];
            gm.seleccionarSuposicion(estancia);
            string sospechoso = gm.names[(rnd.Next(gm.names.Length - gm.numPlayers)) + gm.numPlayers];
            gm.seleccionarSuposicion(sospechoso);
            string arma = gm.weapons[rnd.Next(gm.weapons.Length)];
            gm.seleccionarSuposicion(arma);
            //yield return new WaitForSecondsRealtime(time);
        }
    }
}