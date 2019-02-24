/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

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
     * Se basa en la clase SlidingPuzzle, el modelo lógico de este tipo de puzles.
     * Estas clases podrían ser abreviadas usando SP en vez de SlidingPuzzle, en plan SPSolver.
     */
    public class SlidingPuzzleSolver {

        // Estrategias posibles (podemos añadir más)
        public enum Strategy { BFS, DFS }

        // Añadí AIMA.Core.Agent. a los cuatro operadores
        // Tal vez sería mejor con enumerados
        // Esto se lee: Puedo mover el hueco hacia arriba, o puedo mover la pieza de arriba del hueco
        public static Operator UP = new DynamicOperator("Up");
        public static Operator DOWN = new DynamicOperator("Down");
        public static Operator LEFT = new DynamicOperator("Left");
        public static Operator RIGHT = new DynamicOperator("Right");
                
        //He añadido yo aquí las métricas
        private Metrics metrics;

        private OperatorsFunction oFunction;
        private ResultFunction rFunction;
        private GoalTest goalTest;

        //
        // PUBLIC METHODS
        //
        
        // Construye un resolutor (no necesita el puzle, se le pasará después)
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


        // A ver si esto tiene que estar aquí o puede ser otra cosa (en el propio SlidingPuzzleManager)
         public List<Operator> Solve(SlidingPuzzle setup, SlidingPuzzleSolver.Strategy strategy) {


            // Construimos el problema a partir del puzle. 
            //Pieza a pieza (el puzle tal cual será el initialSetup -lo mismo debería sacar el array-)



            //Aquí construimos el problema en base al puzle actual (la CONFIGURACIÓN del puzle actual), que no me gusta como es porque es el puzle con unas pocas cosas por encima!!! El dominio de problema no es un objeto
            Problem problem = new Problem(setup, oFunction, rFunction, goalTest); //Me molaría más que el problema no sea el solver, sino el puzle... pero bueno, quizá sea PROBLEM lo que debería llamarse el solver
                                                                                              //public Problem(Object initialSetup, OperatorsFunction operatorsFunction, ResultFunction resultFunction, GoalTest goalTest)

            // AQUÍ CREARÍAMOS LA ESTRATEGIA, EL TIPO DE BÚSQUEDA, pidiéndole que use búsqueda de coste uniforme por ejemplo. Y lo llamamos

            Search search = null;
            switch (strategy) {
                case Strategy.BFS: search = new BreadthFirstSearch(); break; //por defecto te crea un GraphSearch, no hay que meterle nada
                case Strategy.DFS: search = new DepthFirstSearch(new GraphSearch()); break;  // NO ENTIENDO PORQUE ESTE CONSTRUCTOR NO TE HACE NADA POR DEFECTO, Y SOY YO QUE LE ESTOY METIENDO UN GRAPHSEARCH
                    // ...
            }           
            List<Operator> operators = search.Search(problem);
            metrics = search.GetMetrics();

            return operators; // Deberíamos devolver también las métricas, seguramente
        }

        // Devuelve la posición que se ve afectada por este operador (la que se movería si lo aplicásemos)
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

        // Cuidado porque a esto sólo tiene sentido llamar una vez se ha hecho la búsqueda y tal
        public Metrics GetMetrics() {
            // Si no están, peta ... o devuelve unas vacías
            return metrics;
        }
        
    }
}