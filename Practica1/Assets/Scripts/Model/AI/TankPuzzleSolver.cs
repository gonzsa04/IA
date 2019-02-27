/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Puzzles.Model.AI {
    using System;
    using System.Collections.Generic;
    using UCM.IAV.IA.Search;
    using UCM.IAV.IA.Search.Uninformed; 
    using UCM.IAV.IA;


    /* 
     * El resolutor de IA especializado en puzles de bloques deslizantes.
     * Se basa en la clase TankPuzzle, el modelo l�gico de este tipo de puzles.
     * Estas clases podr�an ser abreviadas usando SP en vez de TankPuzzle, en plan SPSolver.
     */
    public class TankPuzzleSolver {

        // Estrategias posibles (podemos a�adir m�s)
        public enum Strategy { BFS, DFS }

        // A�ad� AIMA.Core.Agent. a los cuatro operadores
        // Tal vez ser�a mejor con enumerados
        // Esto se lee: Puedo mover el hueco hacia arriba, o puedo mover la pieza de arriba del hueco
        public static Operator UP = new DynamicOperator("Up");
        public static Operator DOWN = new DynamicOperator("Down");
        public static Operator LEFT = new DynamicOperator("Left");
        public static Operator RIGHT = new DynamicOperator("Right");
                
        //He a�adido yo aqu� las m�tricas
        private Metrics metrics;

        private OperatorsFunction oFunction;
        private ResultFunction rFunction;
        private GoalTest goalTest;

        //
        // PUBLIC METHODS
        //
        
        // Construye un resolutor (no necesita el puzle, se le pasar� despu�s)
        public TankPuzzleSolver() { 
            oFunction = TankPuzzleFunctionFactory.getOperatorsFunction();
            rFunction = TankPuzzleFunctionFactory.getResultFunction();
            goalTest = new TankPuzzleGoalTest();
        }

        /*
        // Para copiar el puzle con el que este trabajando otro resolutor... raro
        public TankPuzzleSolver(TankPuzzleSolver copyBoard) : this(copyBoard.getPuzle()) {

        }
        */


        // A ver si esto tiene que estar aqu� o puede ser otra cosa (en el propio TankPuzzleManager)
         public List<Operator> Solve(TankPuzzle setup, TankPuzzleSolver.Strategy strategy) {


            // Construimos el problema a partir del puzle. 
            //Pieza a pieza (el puzle tal cual ser� el initialSetup -lo mismo deber�a sacar el array-)



            //Aqu� construimos el problema en base al puzle actual (la CONFIGURACI�N del puzle actual), que no me gusta como es porque es el puzle con unas pocas cosas por encima!!! El dominio de problema no es un objeto
            Problem problem = new Problem(setup, oFunction, rFunction, goalTest); //Me molar�a m�s que el problema no sea el solver, sino el puzle... pero bueno, quiz� sea PROBLEM lo que deber�a llamarse el solver
                                                                                              //public Problem(Object initialSetup, OperatorsFunction operatorsFunction, ResultFunction resultFunction, GoalTest goalTest)

            // AQU� CREAR�AMOS LA ESTRATEGIA, EL TIPO DE B�SQUEDA, pidi�ndole que use b�squeda de coste uniforme por ejemplo. Y lo llamamos

            Search search = null;
            switch (strategy) {
                case Strategy.BFS: search = new BreadthFirstSearch(); break; //por defecto te crea un GraphSearch, no hay que meterle nada
                case Strategy.DFS: search = new DepthFirstSearch(new GraphSearch()); break;  // NO ENTIENDO PORQUE ESTE CONSTRUCTOR NO TE HACE NADA POR DEFECTO, Y SOY YO QUE LE ESTOY METIENDO UN GRAPHSEARCH
                    // ...
            }           
            List<Operator> operators = search.Search(problem);
            metrics = search.GetMetrics();

            return operators; // Deber�amos devolver tambi�n las m�tricas, seguramente
        }

        // Devuelve la posici�n que se ve afectada por este operador (la que se mover�a si lo aplic�semos)
        public Position GetOperatedPosition(TankPuzzle puzzle, Operator op) {
            // Fallar si no es aplicable con throw new InvalidOperationException("This operator is not working propertly");

            if (TankPuzzleSolver.UP.Equals(op))
                return puzzle.TankPosition.Up();
            if (TankPuzzleSolver.DOWN.Equals(op))
                return puzzle.TankPosition.Down();
            if (TankPuzzleSolver.LEFT.Equals(op))
                return puzzle.TankPosition.Left();
            if (TankPuzzleSolver.RIGHT.Equals(op))
                return puzzle.TankPosition.Right();

            // Si el operador no se entiendo o es NoOp vamos a devolver el propio hueco, por ejemplo
            return puzzle.TankPosition;
        }

        // Cuidado porque a esto s�lo tiene sentido llamar una vez se ha hecho la b�squeda y tal
        public Metrics GetMetrics() {
            // Si no est�n, peta ... o devuelve unas vac�as
            return metrics;
        }
        
    }
}