namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    //Bot de toma de decisiones inteligentes basadas en su libreta (BC). Hereda de la clase Player dado que tambien es un jugador
    public class Bot2 : Player
    {
        private int[] sobre = { -1, -1, -1 };       //Sobre que se ira rellenando con la estancia sospechoso y arma del crimen 
        private int turnos = 0;                     //DEBUG: Se utiliza para saber cuantos turnos tarda en ganar

        public Bot2(Ficha fichaPrefab, Libreta lib, int i) : base(fichaPrefab, lib, i)
        {
        }

        //Lo que hace en cada turno. Llamado en el update de gameManager con un retardo elegible desde el editor
        public override void myTurn()
        {
            turnos++;

            rellenaSobre();      //Actualiza el sobre si puede    

            if (SobreLleno())    //Si ya sabe el sobre completo, acusa
            {
                if (sobre[0] != (int)libreta_.estanciaActual)   //Si no esta en la estancia del crimen
                {
                    Position freePos = gm.getFreeCasInEs(gm.getPosFromEstance((TipoEstancia)sobre[0]));
                    gm.clickTab(freePos);       //Se mueve a ella

                    int susEstance = (int)gm.getTipoEstancia(gm.characters[sobre[1]].ficha_.getLogicPosition().GetRow(),
                      gm.characters[sobre[1]].ficha_.getLogicPosition().GetColumn());
                    if (susEstance != sobre[0])     //Si el sospechoso no esta en esa estancia
                    {
                        if (gm.getTurn() == index) gm.nextTurn();   //Pasa turno
                    }
                    else
                    {
                        acusar(sobre[1], sobre[2]);     //Si esta, acusa
                    }

                }
                else      //Si ya esta en la estancia del crimen, acusa
                {
                    acusar(sobre[1], sobre[2]);
                }
            }
            else          //Si le faltan cosas por saber, pregunta
            {
                int otherPlayer = libreta_.getMinPlayerInfo();      //Escoge al jugador del que menos informacion tiene
                string estance = libreta_.getFirstBlankEstanceFrom(otherPlayer);        //Pergunta por cosas que aun no tiene sobre ese jugador
                string suspect = libreta_.getFirstBlankSuspectFrom(otherPlayer);
                string weapon = libreta_.getFirstBlankWeaponFrom(otherPlayer);

                Player aux = (Player)gm.characters[otherPlayer];
                gm.playerPreguntado = aux.ficha_.name_;

                if (aux.libreta_.estanciaActual != libreta_.estanciaActual)         //Si no esta en la misma estancia de ese jugador
                {
                    Position freePos = gm.getFreeCasInEs(gm.getPosEstancia(aux.ficha_.getLogicPosition().GetRow(),
                        aux.ficha_.getLogicPosition().GetColumn()));
                    gm.clickTab(freePos);       //Se mueve
                }

                gm.seleccionarSuposicion(estance);      //Pregunta a ese jugador
                gm.seleccionarSuposicion(suspect);
                gm.seleccionarSuposicion(weapon);
            }
        }

        //Realiza una acusacion al sospechoso elegido con el arma elegida en la estancia actual
        private void acusar(int otherSuspect, int otherWeapon)
        {
            Suspect aux = (Suspect)gm.characters[otherSuspect + gm.numPlayers];
            gm.moveSuspect(aux);
            gm.moveSuspect(aux);
            gm.Acusar(otherWeapon);
            Debug.Log(turnos);
        }

        private bool SobreLleno()       //Devuelve si el sobre esta lleno
        {
            int i = 0;
            while (i < sobre.Length && sobre[i] != -1) i++;
            return (i == sobre.Length);
        }

        private void rellenaSobre()     //Actualiza el sobre
        {
            int finEstances = gm.estancias.Length;      //Numero de cartas por cada tipo
            int finSuspects = finEstances + gm.names.Length - gm.numPlayers;
            int finWeapons = finSuspects + gm.weapons.Length;

            int uncompletedEstance = -1, uncompletedSuspect = -1, uncompletedWeapon = -1;
            int numUncompletedEstance = 0, numUncompletedSuspect = 0, numUncompletedWeapon = 0;

            for (int i = 0; i < libreta_.DEFAULT_ROWS; i++)
            {
                if (libreta_.completedRow(i))       //Si esa fila esta completa
                {
                    if (libreta_.completedWithXRow(i))      //Si esta llena de X (No la tiene nadie), se añade al sobre
                    {
                        if (i < finEstances) sobre[0] = i;
                        else if (i < finSuspects) sobre[1] = i - finEstances;
                        else if (i < finWeapons) sobre[2] = i - finSuspects;
                    }
                }
                else                               //Si no esta completa, se guarda para una posible inferencia
                {
                    if (i < finEstances) { uncompletedEstance = i; numUncompletedEstance++; }
                    else if (i < finSuspects) { uncompletedSuspect = i - finEstances; numUncompletedSuspect++; }
                    else if (i < finWeapons) { uncompletedWeapon = i - finSuspects; numUncompletedWeapon++; }
                }
            }

            //INFERENCIA: Si solo una carta de un tipo esta incompleta, se añade al sobre sin necesidad de preguntar por esa carta
            if (numUncompletedEstance == 1 && sobre[0] == -1) sobre[0] = uncompletedEstance;
            if (numUncompletedSuspect == 1 && sobre[1] == -1) sobre[1] = uncompletedSuspect;
            if (numUncompletedWeapon == 1 && sobre[2] == -1) sobre[2] = uncompletedWeapon;
            //Debug.Log(sobre[0] + " " + sobre[1] + " " + sobre[2]);    //DEBUG: muestra en cada turno el sobre actual del bot
        }
    }
}