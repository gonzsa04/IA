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
    using AIMA.Core.Search.Framework;

    /**
     * @author Ravi Mohan
     * 
     */
    public class SlidingPuzzleGoalTest : GoalTest {

        SlidingPuzzle goal = new SlidingPuzzle(); // El sliding puzle por defecto es el inicial

        // Yo haría que preguntase por el SlidingPuzle... no por el SlidingPuzleSolver
        public bool isGoalState(Object setup)
        {
            SlidingPuzzle puzzle = (SlidingPuzzle)setup;
            return puzzle.Equals(goal);
        }
    }
}