namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    public class Bot2 : Player
    {
        private System.Random rnd = new System.Random(); // decisiones
        private int[] sobre = { -1, -1, -1 };

        public Bot2(Ficha fichaPrefab, Libreta lib, int i) : base(fichaPrefab, lib, i)
        {
        }
        
        public override void myTurn()
        {
            rellenaSobre();

            if (SobreLleno())
            {
                if (sobre[0] != (int)libreta_.estanciaActual)
                {
                    Position freePos = gm.getFreeCasInEs(gm.getPosFromEstance((TipoEstancia)sobre[0]));
                    gm.clickTab(freePos);

                    int susEstance = (int)gm.getTipoEstancia(gm.characters[sobre[1]].ficha_.getLogicPosition().GetRow(),
                      gm.characters[sobre[1]].ficha_.getLogicPosition().GetColumn());
                    if (susEstance != sobre[0])
                    {
                        if (gm.getTurn() == index) gm.nextTurn();
                    }
                    else
                    {
                        acusar(sobre[1], sobre[2]);
                    }

                }
                else
                {
                    acusar(sobre[1], sobre[2]);
                }
            }
            else
            {
                int otherPlayer = libreta_.getMinPlayerInfo();
                string estance = libreta_.getFirstBlankEstanceFrom(otherPlayer);
                string suspect = libreta_.getFirstBlankSuspectFrom(otherPlayer);
                string weapon = libreta_.getFirstBlankWeaponFrom(otherPlayer);

                Player aux = (Player)gm.characters[otherPlayer];
                gm.playerPreguntado = aux.ficha_.name_;

                if (aux.libreta_.estanciaActual != libreta_.estanciaActual)
                {
                    Position freePos = gm.getFreeCasInEs(gm.getPosEstancia(aux.ficha_.getLogicPosition().GetRow(),
                        aux.ficha_.getLogicPosition().GetColumn()));
                    gm.clickTab(freePos);
                }

                gm.seleccionarSuposicion(estance);
                gm.seleccionarSuposicion(suspect);
                gm.seleccionarSuposicion(weapon);
            }
        }

        private void acusar(int otherSuspect, int otherWeapon)
        {
            Suspect aux = (Suspect)gm.characters[otherSuspect + gm.numPlayers];
            gm.moveSuspect(aux);
            gm.moveSuspect(aux);
            gm.Acusar(otherWeapon);
        }

        private bool SobreLleno()
        {
            int i = 0;
            while (i < sobre.Length && sobre[i] != -1) i++;
            return (i == sobre.Length);
        }

        private void rellenaSobre()
        {
            int finEstances = gm.estancias.Length;
            int finSuspects = finEstances + gm.names.Length - gm.numPlayers;
            int finWeapons = finSuspects + gm.weapons.Length;

            for (int i = 0; i < libreta_.DEFAULT_ROWS; i++)
            {
                if (libreta_.completedRow(i))
                {
                    if (i < finEstances) sobre[0] = i;
                    else if (i < finSuspects) sobre[1] = i - finEstances;
                    else if (i < finWeapons) sobre[2] = i - finSuspects;
                }
            }
        }
    }
}