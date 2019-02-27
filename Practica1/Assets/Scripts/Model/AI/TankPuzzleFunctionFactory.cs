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

    public class TankPuzzleFunctionFactory
    {
        private static OperatorsFunction _operatorsFunction = null;
        private static ResultFunction _resultFunction = null;

        // 
        public static OperatorsFunction getOperatorsFunction()
        {
            if (_operatorsFunction == null) {
                _operatorsFunction = new TankPuzzleOperatorsFunction();
            }
            return _operatorsFunction;
        }

        public static ResultFunction getResultFunction()
        {
            if (_resultFunction == null) {
                _resultFunction = new TankPuzzleResultFunction();
            }
            return _resultFunction;
        }

        // HE QUITADO LO DE STATIC CLASS
        private class TankPuzzleOperatorsFunction : OperatorsFunction 
            {
            // SUSTITUÍ EL LINKEDHASHSET DE JAVA POR HASHSET SOLAMENTE... SE ME HACE RARO QUE SE DEVUELVA UN HASHSET, LO NORMAL SERÍA UNA LIST, Y POR DENTRO IMPLEMENTAR CON ARRAYLIST O ALGO
            public HashSet<Operator> Operators(object setup) {
                // Lo que entra es un problema, un TankPuzzle
                TankPuzzle puzzle = (TankPuzzle)setup;  

                HashSet<Operator> operators = new HashSet<Operator>();

                // Esto se lee: Puedo mover el hueco hacia arriba, o puedo mover la pieza de arriba del hueco
                if (puzzle.CanMoveUp(puzzle.TankPosition)) 
                    operators.Add(TankPuzzleSolver.UP);
                if (puzzle.CanMoveDown(puzzle.TankPosition))
                    operators.Add(TankPuzzleSolver.DOWN);
                if (puzzle.CanMoveLeft(puzzle.TankPosition))
                    operators.Add(TankPuzzleSolver.LEFT);
                if (puzzle.CanMoveRight(puzzle.TankPosition))
                    operators.Add(TankPuzzleSolver.RIGHT);

                return operators;
            }
        }

        // QUITO LO DE STATIC CLASSS
        private class TankPuzzleResultFunction : ResultFunction {

            public object GetResult(object setup, Operator op) {

                // Lo recibido es un puzle deslizante
                TankPuzzle puzzle = (TankPuzzle)setup;
                // Un puzle deslizante se puede clonar a nivel profundo
                TankPuzzle puzzleClone = puzzle.DeepClone();


                if (TankPuzzleSolver.UP.Equals(op))
                    if (puzzleClone.CanMoveUp(puzzleClone.TankPosition))
                        puzzleClone.MoveUp(puzzleClone.TankPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (TankPuzzleSolver.DOWN.Equals(op))
                    if (puzzleClone.CanMoveDown(puzzleClone.TankPosition))
                        puzzleClone.MoveDown(puzzleClone.TankPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (TankPuzzleSolver.LEFT.Equals(op))
                    if (puzzleClone.CanMoveLeft(puzzleClone.TankPosition))
                        puzzleClone.MoveLeft(puzzleClone.TankPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (TankPuzzleSolver.RIGHT.Equals(op))
                    if (puzzleClone.CanMoveRight(puzzleClone.TankPosition))
                        puzzleClone.MoveRight(puzzleClone.TankPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                // Si el operador no se reconoce o es un NoOp, se devolverá la configuración actual (que sería idéntica a la original, no ha habido cambios)
                return puzzleClone; 
            }
        }
    }
}