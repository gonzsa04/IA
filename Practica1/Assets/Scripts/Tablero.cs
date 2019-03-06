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
        
        private Casilla[,] casillas;

        public void Initialize(TankPuzzle puzzle) {
            if (puzzle == null) throw new ArgumentNullException(nameof(puzzle));

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

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Casilla casilla = casillas[r, c];
                    if (casilla == null)
                    {
                        casilla = Instantiate(casillaPrefab, new Vector3(-((casillas.GetLength(1) / 2.0f) * POSITION_FACTOR_C - (POSITION_FACTOR_C / 2.0f)) + c * POSITION_FACTOR_C, 0,
                            (casillas.GetLength(0) / 2.0f) * POSITION_FACTOR_R - (POSITION_FACTOR_R / 2.0f) - r * POSITION_FACTOR_R),
                            Quaternion.identity); // En Y, que es la separación del tablero, estoy poniendo 0 pero la referencia la debería dar el tablero

                        casillas[r, c] = casilla;
                    }
                    
                    casilla.position = new Position((uint)r, (uint)c);
                    casilla.Initialize(this, (uint)puzzle.GetType(r, c));

                    if (!tankInitialized && puzzle.GetType(r, c) == 0)
                    {
                        GameManager.instance.setTankPosition(casilla.transform.position);
                        puzzle.InitialTankPosition = casilla.position;
                        puzzle.TankPosition = puzzle.InitialTankPosition;
                        tankInitialized = true;
                    }
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

        // restablece las casillas del tablero a sus parametros iniciales, asi como la matriz logica
        public void ResetTablero(TankPuzzle puzzle)
        {
            var rows = casillas.GetLength(0);
            var columns = casillas.GetLength(1);

            for (var r = 0u; r < rows; r++)
            {
                for (var c = 0u; c < columns; c++)
                {
                    puzzle.SetType((int)r, (int)c, casillas[r, c].getInitialType());
                    casillas[r, c].Reset();
                }
            }
            puzzle.TankPosition = puzzle.InitialTankPosition;
        }

        // devuelve la posicion fisica de la casilla situada en (r, c)
        public Vector3 getCasPos(int r, int c)
        {
            return casillas[r, c].transform.position;
        }

        // Cadena de texto representativa
        public override string ToString() {
            return "Board{" + casillas.ToString() + "}";
        }
    }
}
