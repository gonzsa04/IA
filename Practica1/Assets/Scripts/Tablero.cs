namespace UCM.IAV.Puzzles {

    using System;
    using System.Collections;
    using UnityEngine;
    using Model;
    using System.Collections.Generic;
    
    // tablero de casillas (representacion grafica del juego)
    public class Tablero : MonoBehaviour {

        // Constantes
        public static readonly float USER_DELAY = 0.0f;
        public static readonly float AI_DELAY = 0.2f;

        private static readonly float POSITION_FACTOR_R = 1.1f;
        private static readonly float POSITION_FACTOR_C = 1.1f;
        private static readonly float SCALE_FACTOR_R = 1.1f;
        private static readonly float SCALE_FACTOR_C = 1.1f;
        
        public Casilla casillaPrefab;
        private GameManager manager;
        
        private Casilla[,] casillas;

        public void Initialize(GameManager manager, TankPuzzle puzzle) {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            if (puzzle == null) throw new ArgumentNullException(nameof(puzzle));

            this.manager = manager;

            if (casillas == null) {
                casillas = new Casilla[puzzle.rows, puzzle.columns];
                
                transform.localScale = new Vector3(SCALE_FACTOR_C * casillas.GetLength(1), 
                    transform.localScale.y, SCALE_FACTOR_R * casillas.GetLength(0));

            } else if (casillas.GetLength(0) != puzzle.rows || casillas.GetLength(1) != puzzle.columns) {
                DestroyCasillas();
                casillas = new Casilla[puzzle.rows, puzzle.columns];

                transform.localScale = new Vector3(SCALE_FACTOR_C * casillas.GetLength(1), 
                    transform.localScale.y, SCALE_FACTOR_R * casillas.GetLength(0));
            }

            GenerateCasillas(puzzle);
        }

        // genera las casillas del tipo que le indique puzzle (matriz logica de tipos)
        private void GenerateCasillas(TankPuzzle puzzle)
        {
            if (puzzle == null) throw new ArgumentNullException(nameof(puzzle));

            var rows = casillas.GetLength(0);
            var columns = casillas.GetLength(1);

            bool tankInitialized = false;

            for (var r = 0u; r < rows; r++)
            {
                for (var c = 0u; c < columns; c++)
                {
                    Casilla casilla = casillas[r, c];
                    if (casilla == null)
                    {
                        casilla = Instantiate(casillaPrefab, new Vector3(-((casillas.GetLength(1) / 2.0f) * POSITION_FACTOR_C - (POSITION_FACTOR_C / 2.0f)) + c * POSITION_FACTOR_C, 0,
                            (casillas.GetLength(0) / 2.0f) * POSITION_FACTOR_R - (POSITION_FACTOR_R / 2.0f) - r * POSITION_FACTOR_R),
                            Quaternion.identity); // En Y, que es la separación del tablero, estoy poniendo 0 pero la referencia la debería dar el tablero

                        casillas[r, c] = casilla;
                    }

                    Position position = new Position(r, c);
                    casilla.position = position;
                    casilla.Initialize(this, puzzle.GetType(position));

                    if (!tankInitialized && puzzle.GetType(position) == 0)
                    {
                        GameManager.instance.setTankPosition(casilla.transform.position);
                        puzzle.TankPosition = new Position(r, c);
                        tankInitialized = true;
                    }
                    Debug.Log(ToString() + "generated " + casilla.ToString() + ".");
                }
            }

        }

        private void DestroyCasillas() {
            if (casillas == null) throw new InvalidOperationException("This object has not been initialized");

            var rows = casillas.GetLength(0);
            var columns = casillas.GetLength(1);

            for (var r = 0u; r < rows; r++) {
                for (var c = 0u; c < columns; c++) {
                    if (casillas[r, c] != null)
                        Destroy(casillas[r, c].gameObject);
                }
            }
        }

        // restablece las casillas del tablero a sus parametros iniciales
        public void ResetTablero(TankPuzzle puzzle)
        {
            var rows = casillas.GetLength(0);
            var columns = casillas.GetLength(1);

            for (var r = 0u; r < rows; r++)
            {
                for (var c = 0u; c < columns; c++)
                {
                    casillas[r, c].Reset();
                    puzzle.SetType(new Position(r, c), casillas[r, c].getInitialType());
                }
            }
        }

        public double getCasValue(int r, int c)
        {
            return casillas[r, c].getValue();
        }

        public TipoCasilla getCasType(int r, int c)
        {
            return casillas[r, c].getType();
        }

        public Vector3 getCasPos(int r, int c)
        {
            return casillas[r, c].transform.position;
        }

        // Pone los contadores de información a cero
        public void UserInteraction() {
            manager.CleanInfo();
        }

        // Cadena de texto representativa
        public override string ToString() {
            return "Board{" + casillas.ToString() + "}";
        }
    }
}
