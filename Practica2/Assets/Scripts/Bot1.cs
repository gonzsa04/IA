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

            int random = rnd.Next(3);

            if (random == 0)
            {
                //acusar (elegir est aleatoria: si ya estas en ella elegir sospechoso y arma aleatorias, y si no, moverte a esa estancia)
                int otherEstance = rnd.Next(gm.estancias.Length);
                int otherSuspect = rnd.Next(gm.characters.Count - gm.numPlayers) + gm.numPlayers;
                int otherWeapon = rnd.Next(gm.weapons.Length);

                if (otherEstance != (int)libreta_.estanciaActual)
                {
                    int r = (otherEstance / gm.roomLength) * gm.roomLength;
                    int c = (otherEstance - r) * gm.roomLength;
                    Position freePos = gm.getFreeCasInEs(new Position(r, c));
                    gm.clickTab(freePos);
                    int susEstance = (int)gm.getTipoEstancia(gm.characters[otherSuspect].ficha_.getLogicPosition().GetRow(), 
                        gm.characters[otherSuspect].ficha_.getLogicPosition().GetColumn());
                    if (susEstance != otherEstance)
                    {
                        if (gm.getTurn() == index) gm.nextTurn();
                    }
                    else
                    {
                        acusar(otherSuspect, otherWeapon);
                    }
                }
                else
                {
                    acusar(otherSuspect, otherWeapon);
                }       
            }
            else
            {
                int otherPlayer;
                do
                {
                    otherPlayer = rnd.Next(gm.numPlayers);
                } while (otherPlayer == gm.getTurn() || gm.turnos[otherPlayer] == "");

                Player aux = (Player)gm.characters[otherPlayer];
                gm.playerPreguntado = aux.ficha_.name_;

                if (aux.libreta_.estanciaActual != libreta_.estanciaActual)
                {
                    Position freePos = gm.getFreeCasInEs(gm.getPosEstancia(aux.ficha_.getLogicPosition().GetRow(), aux.ficha_.getLogicPosition().GetColumn()));
                    gm.clickTab(freePos);
                }
                createSugestion();
            }
        }

        private void acusar(int otherSuspect, int otherWeapon)
        {
            GameManager gm = GameManager.instance;
            Suspect aux = (Suspect)gm.characters[otherSuspect];
            gm.moveSuspect(aux);
            gm.moveSuspect(aux);
            gm.Acusar(otherWeapon);
        }

        private void createSugestion()
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