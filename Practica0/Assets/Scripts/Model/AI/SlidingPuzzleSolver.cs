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
    using AIMA.Core.Agent;
    using AIMA.Core.Agent.Impl;
    using AIMA.Core.Util.DataStructure;
    using AIMA.Core.Search;
    using AIMA.Core.Search.Framework;
    using AIMA.Core.Search.Uninformed;

    /**
     * @author Ravi Mohan
     * @author R. Lunde
     */
    // Se basa en la clase EightPuzzleBoard
    // LO MISMO NO ES SLIDING, EH? SINO UN RESOLUTOR DE B�SQUEDA GEN�RICO CHAVALES!!!! Bueno, ll�malo SearchBasedSolver
    public class SlidingPuzzleSolver {

        // Estrategias posibles (podemos a�adir m�s)
        public enum Strategy { BFS, DFS }

        // A�ad� AIMA.Core.Agent. a los cuatro operadores
        // Tal vez ser�a mejor con enumerados
        // Esto se lee: Puedo mover el hueco hacia arriba, o puedo mover la pieza de arriba del hueco
        public static AIMA.Core.Agent.Operator UP = new DynamicOperator("Up");
        public static AIMA.Core.Agent.Operator DOWN = new DynamicOperator("Down");
        public static AIMA.Core.Agent.Operator LEFT = new DynamicOperator("Left");
        public static AIMA.Core.Agent.Operator RIGHT = new DynamicOperator("Right");
                
        //He a�adido yo aqu� las m�tricas
        private Metrics metrics;

        private OperatorsFunction oFunction;
        private ResultFunction rFunction;
        private GoalTest goalTest;

        //
        // PUBLIC METHODS
        //
        
        // Construye un resolutor (no necesita el puzle, se le pasar� despu�s)
        public SlidingPuzzleSolver() { 
            oFunction = SlidingPuzzleFunctionFactory.getOperatorsFunction();
            rFunction = SlidingPuzzleFunctionFactory.getResultFunction();
            goalTest = new SlidingPuzzleGoalTest();
        }

        /*
        // Para copiar el puzle con el que este trabajando otro resolutor... raro
        public SlidingPuzzleSolver(SlidingPuzzleSolver copyBoard) : this(copyBoard.getPuzle()) {

        }
        */


        // A ver si esto tiene que estar aqu� o puede ser otra cosa (en el propio SlidingPuzzleManager)
        public List<Operator> Solve(SlidingPuzzle config, SlidingPuzzleSolver.Strategy strategy) {


            // Construimos el problema a partir del puzle. 
            //Pieza a pieza (el puzle tal cual ser� el initialState -lo mismo deber�a sacar el array-)



            //Aqu� construimos el problema en base al puzle actual (la CONFIGURACI�N del puzle actual), que no me gusta como es porque es el puzle con unas pocas cosas por encima!!! El dominio de problema no es un objeto
            Problem problem = new Problem(config, oFunction, rFunction, goalTest); //Me molar�a m�s que el problema no sea el solver, sino el puzle... pero bueno, quiz� sea PROBLEM lo que deber�a llamarse el solver
                                                                                                                                                                                       //public Problem(Object initialState, OperatorsFunction actionsFunction, ResultFunction resultFunction, GoalTest goalTest)

            // AQU� CREAR�AMOS LA ESTRATEGIA, EL TIPO DE B�SQUEDA, pidi�ndole que use b�squeda de coste uniforme por ejemplo. Y lo llamamos

            Search search = null;
            switch (strategy) {
                case Strategy.BFS: search = new BreadthFirstSearch(); break; //por defecto te crea un GraphSearch, no hay que meterle nada
                case Strategy.DFS: search = new DepthFirstSearch(new GraphSearch()); break;  // NO ENTIENDO PORQUE ESTE CONSTRUCTOR NO TE HACE NADA POR DEFECTO, Y SOY YO QUE LE ESTOY METIENDO UN GRAPHSEARCH
                    // ...
            }           
            List<Operator> operators = search.Search(problem);
            metrics = search.getMetrics();

            return operators; // Deber�amos devolver tambi�n las m�tricas, seguramente
        }

        // Devuelve la posici�n que se ve afectada por este operador (la que se mover�a si lo aplic�semos)
        public Position GetOperatedPosition(SlidingPuzzle puzzle, Operator op) {
            // Fallar si no es aplicable con throw new InvalidOperationException("This operator is not working propertly");

            if (SlidingPuzzleSolver.UP.Equals(op))
                return puzzle.GapPosition.Up();
            if (SlidingPuzzleSolver.DOWN.Equals(op))
                return puzzle.GapPosition.Down();
            if (SlidingPuzzleSolver.LEFT.Equals(op))
                return puzzle.GapPosition.Left();
            if (SlidingPuzzleSolver.RIGHT.Equals(op))
                return puzzle.GapPosition.Right();

            // Si el operador no se entiendo o es NoOp vamos a devolver el propio hueco, por ejemplo
            return puzzle.GapPosition;
        }

        // Cuidado porque a esto s�lo tiene sentido llamar una vez se ha hecho la b�squeda y tal
        public Metrics GetMetrics() {
            // Si no est�n, peta ... o devuelve unas vac�as
            return metrics;
        }
        
    }
}