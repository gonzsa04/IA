namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    //Bot de toma de decisiones totalmente aleatorias. Hereda de la clase Player dado que tambien es un jugador
    public class Bot1 : Player
    {
        private System.Random rnd = new System.Random(); // decisiones

        public Bot1(Ficha fichaPrefab, Libreta lib, int i) : base(fichaPrefab, lib, i)
        {
        }
        
        //Lo que hace en cada turno. Llamado en el update de gameManager con un retardo elegible desde el editor
        public override void myTurn()
        {
            int random = rnd.Next(3); //Decision aleatoria

            //Hay una probabilidad del 33% de que acuse
            if (random == 0)        //DEBUG: poner random = -1 para que el bot2 siempre gane por acusacion
            {
                int otherEstance = rnd.Next(gm.estancias.Length);
                int otherSuspect = rnd.Next(gm.characters.Count - gm.numPlayers) + gm.numPlayers;
                int otherWeapon = rnd.Next(gm.weapons.Length);

                if (otherEstance != (int)libreta_.estanciaActual)       //Si la estancia elegida aleatoriamente no es la tuya
                {
                    Position freePos = gm.getFreeCasInEs(gm.getPosFromEstance((TipoEstancia)otherEstance));
                    gm.clickTab(freePos);   //Nos movemos a la suya

                    int susEstance = (int)gm.getTipoEstancia(                             
                        gm.characters[otherSuspect].ficha_.getLogicPosition().GetRow(),     //estancia del sospechoso a acusar
                        gm.characters[otherSuspect].ficha_.getLogicPosition().GetColumn());

                    if (susEstance != otherEstance)    //Si no esta en la estancia en la que queriamos acusar
                    {
                        if (gm.getTurn() == index) gm.nextTurn();   //Pasamos turno
                    }
                    else
                    {
                        acusar(otherSuspect, otherWeapon);          //Si no, acusamos
                    }
                }
                else      //Si ya estas en esa estancia aleatoria, acusas
                {
                    acusar(otherSuspect, otherWeapon);
                }       
            }
            //Y un 66% de que pregunte
            else
            {
                int otherPlayer;        //Jugador aleatorio al que vas a preguntar
                do
                {
                    otherPlayer = rnd.Next(gm.numPlayers);
                } while (otherPlayer == gm.getTurn() || gm.turnos[otherPlayer] == "");

                Player aux = (Player)gm.characters[otherPlayer];
                gm.playerPreguntado = aux.ficha_.name_;

                if (aux.libreta_.estanciaActual != libreta_.estanciaActual)     //Si ese jugador esta en otra estancia diferente a la tuya
                {
                    Position freePos = gm.getFreeCasInEs(gm.getPosEstancia(aux.ficha_.getLogicPosition().GetRow(), 
                        aux.ficha_.getLogicPosition().GetColumn()));
                    gm.clickTab(freePos);       //Nos movemos a la suya
                }
                createSugestion();        //Le preguntamos
            }
        }

        //Realiza una acusacion al sospechoso elegido con el arma elegida en la estancia actual
        private void acusar(int otherSuspect, int otherWeapon)
        {
            Suspect aux = (Suspect)gm.characters[otherSuspect];
            gm.moveSuspect(aux);                                //Si no esta en tu estancia, le mueves a ella
            gm.moveSuspect(aux);                                //Le acuso            
            gm.Acusar(otherWeapon);
        }

        //Elige una estancia sospechoso y arma aleatorias, y realiza una suposicion
        private void createSugestion()
        {
            string estancia = gm.estancias[rnd.Next(gm.estancias.Length)];
            gm.seleccionarSuposicion(estancia);
            string sospechoso = gm.names[(rnd.Next(gm.names.Length - gm.numPlayers)) + gm.numPlayers];
            gm.seleccionarSuposicion(sospechoso);
            string arma = gm.weapons[rnd.Next(gm.weapons.Length)];
            gm.seleccionarSuposicion(arma);
        }
    }
}