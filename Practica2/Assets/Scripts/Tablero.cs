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

        public void Initialize(CluedoPuzzle puzzle) {
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

        // genera las casillas del tipo que le indique puzzle (matriz logica de tipos de estancia)
        private void GenerateCasillas(CluedoPuzzle puzzle)
        {
            var rows = casillas.GetLength(0);
            var columns = casillas.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Casilla casilla = casillas[r, c];
                    if (casilla == null)
                    {
                        casilla = Instantiate(casillaPrefab, new Vector3(-((casillas.GetLength(1) / 2.0f) * POSITION_FACTOR_C - (POSITION_FACTOR_C / 2.0f)) + c * POSITION_FACTOR_C, 0,
                            (casillas.GetLength(0) / 2.0f) * POSITION_FACTOR_R - (POSITION_FACTOR_R / 2.0f) - r * POSITION_FACTOR_R),
                            Quaternion.identity);

                        casillas[r, c] = casilla;
                    }
                    
                    casilla.position = new Position(r, c);
                    casilla.Initialize(this, puzzle.GetType(r, c));
                }
            }

            // para cada ficha del gm le establecemos la posicion fisica que corresponda
            for (int i = 0; i < GameManager.instance.characters.Count; i++)
            {
                Vector3 position = casillas[GameManager.instance.characters[i].ficha_.position.GetRow(), GameManager.instance.characters[i].ficha_.position.GetColumn()].transform.position;
                GameManager.instance.characters[i].ficha_.setPosition(position);
                if (i < GameManager.instance.numPlayers) casillas[GameManager.instance.characters[i].ficha_.position.GetRow(), GameManager.instance.characters[i].ficha_.position.GetColumn()].tienePlayer = true;
                else casillas[GameManager.instance.characters[i].ficha_.position.GetRow(), GameManager.instance.characters[i].ficha_.position.GetColumn()].tieneSuspect = true;
            }
        }

        private void DestroyCasillas() {
            if (casillas == null) throw new InvalidOperationException("This object has not been initialized");

            int rows = casillas.GetLength(0);
            int columns = casillas.GetLength(1);

            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < columns; c++) {
                    if (casillas[r, c] != null)
                        Destroy(casillas[r, c].gameObject);
                }
            }
        }

        public void changeTieneSuspect(int r, int c)
        {
            casillas[r, c].tieneSuspect = !casillas[r, c].tieneSuspect;
        }
        public void changeTienePlayer(int r, int c)
        {
            casillas[r, c].tienePlayer = !casillas[r, c].tienePlayer;
        }

        public bool tieneSuspect(int r, int c)
        {
            return casillas[r, c].tieneSuspect;
        }

        public bool tienePlayer(int r, int c)
        {
            return casillas[r, c].tienePlayer;
        }

        public void setTienePlayer(int r, int c, bool b)
        {
            casillas[r, c].tienePlayer = b;
        }

        public TipoEstancia getCasEstancia(int r, int c)
        {
            return casillas[r, c].getTypeEstancia();
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
