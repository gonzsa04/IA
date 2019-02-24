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
    using UCM.IAV.IA;

    public class SlidingPuzzleFunctionFactory
    {
        private static OperatorsFunction _operatorsFunction = null;
        private static ResultFunction _resultFunction = null;

        // 
        public static OperatorsFunction getOperatorsFunction()
        {
            if (_operatorsFunction == null) {
                _operatorsFunction = new SlidingPuzzleOperatorsFunction();
            }
            return _operatorsFunction;
        }

        public static ResultFunction getResultFunction()
        {
            if (_resultFunction == null) {
                _resultFunction = new SlidingPuzzleResultFunction();
            }
            return _resultFunction;
        }

        // HE QUITADO LO DE STATIC CLASS
        private class SlidingPuzzleOperatorsFunction : OperatorsFunction 
            {
            // SUSTITUÍ EL LINKEDHASHSET DE JAVA POR HASHSET SOLAMENTE... SE ME HACE RARO QUE SE DEVUELVA UN HASHSET, LO NORMAL SERÍA UNA LIST, Y POR DENTRO IMPLEMENTAR CON ARRAYLIST O ALGO
            public HashSet<Operator> Operators(object setup) {
                // Lo que entra es un problema, un SlidingPuzzle
                SlidingPuzzle puzzle = (SlidingPuzzle)setup;  

                HashSet<Operator> operators = new HashSet<Operator>();

                // Esto se lee: Puedo mover el hueco hacia arriba, o puedo mover la pieza de arriba del hueco
                if (puzzle.CanMoveUp(puzzle.GapPosition)) 
                    operators.Add(SlidingPuzzleSolver.UP);
                if (puzzle.CanMoveDown(puzzle.GapPosition))
                    operators.Add(SlidingPuzzleSolver.DOWN);
                if (puzzle.CanMoveLeft(puzzle.GapPosition))
                    operators.Add(SlidingPuzzleSolver.LEFT);
                if (puzzle.CanMoveRight(puzzle.GapPosition))
                    operators.Add(SlidingPuzzleSolver.RIGHT);

                return operators;
            }
        }

        // QUITO LO DE STATIC CLASSS
        private class SlidingPuzzleResultFunction : ResultFunction {

            public object GetResult(object setup, Operator op) {

                // Lo recibido es un puzle deslizante
                SlidingPuzzle puzzle = (SlidingPuzzle)setup;
                // Un puzle deslizante se puede clonar a nivel profundo
                SlidingPuzzle puzzleClone = puzzle.DeepClone();


                if (SlidingPuzzleSolver.UP.Equals(op))
                    if (puzzleClone.CanMoveUp(puzzleClone.GapPosition))
                        puzzleClone.MoveUp(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (SlidingPuzzleSolver.DOWN.Equals(op))
                    if (puzzleClone.CanMoveDown(puzzleClone.GapPosition))
                        puzzleClone.MoveDown(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (SlidingPuzzleSolver.LEFT.Equals(op))
                    if (puzzleClone.CanMoveLeft(puzzleClone.GapPosition))
                        puzzleClone.MoveLeft(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (SlidingPuzzleSolver.RIGHT.Equals(op))
                    if (puzzleClone.CanMoveRight(puzzleClone.GapPosition))
                        puzzleClone.MoveRight(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                // Si el operador no se reconoce o es un NoOp, se devolverá la configuración actual (que sería idéntica a la original, no ha habido cambios)
                return puzzleClone; 
            }
        }
    }
}