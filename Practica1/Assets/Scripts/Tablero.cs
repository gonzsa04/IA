namespace UCM.IAV.Puzzles {

    using System;
    using System.Collections;
    using UnityEngine;
    using Model;
    using System.Collections.Generic;
    
    // tablero de casillas
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
        
        /*private bool tankInMotion = false;
        // Lista de bloques para ir moviendo (ojo, van de dos en dos... porque en C# 6 no hay tuplas todavía)
        private Queue<Casilla> blocksInMotion = new Queue<Casilla>(); */

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

        /*// Devuelve si se puede mover un bloque en el tablero
        public bool CanMove(Casilla block) {
            if (block == null) throw new ArgumentNullException(nameof(block));

            return manager.CanMove(block);
        }

        // Mueve un bloque en el tablero, devolviendo el otro bloque que ahora pasa a ocupar el lugar de este
        // Si no se puede realizar el movimiento, da fallo
        public Casilla Move(Casilla block, float delay) {
            if (block == null) throw new ArgumentNullException(nameof(block));
            if (!CanMove(block)) throw new InvalidOperationException("The required movement is not possible");

            Debug.Log(ToString() + " moves " + block.ToString() + ".");

            // Intercambio de valores entre las dos posiciones de la matriz de bloques?
            Casilla otherBlock = manager.Move(block);
            // Ya ha cambiado el puzle, y mi posición lógica (y la del hueco -otherBlock-)... faltan las posiciones físicas en la escena y ubicaciones en la matriz de bloques de ambos

            // Cambio la ubicación en la matriz del bloque auxiliar para que sea la correcta
            casillas[otherBlock.position.GetRow(), otherBlock.position.GetColumn()] = otherBlock;
            // Cambio la ubicación en la matriz del bloque resultante para que sea la correcta
            casillas[block.position.GetRow(), block.position.GetColumn()] = block;

            // Los meto en la lista para que se muevan cuando corresponda...
            blocksInMotion.Enqueue(block);
            blocksInMotion.Enqueue(otherBlock);
            // Debería meter el delay o marcar de alguna manera si el movimiento es de humano o máquina

            return otherBlock;
        }
        
        private void Update() {
            if (!blockInMotion && blocksInMotion.Count > 0)  // Only start the coroutine if loading is false, meaning it hasn't already been started
                // Se podría mostrar una animación de movimiento incluso, ya que las únicas restricciones de movimiento son las del modelo
                StartCoroutine(BlockInMotion(USER_DELAY));
        }

        // Corrutina para pausar entre movimientos y poder verlos
        IEnumerator BlockInMotion(float delay) {
            blockInMotion = true;
            // Animar tal vez las dos piezas...
            yield return new WaitForSeconds(delay);

            Casilla block = blocksInMotion.Dequeue();
            Casilla otherBlock = blocksInMotion.Dequeue();
            // Ya ha cambiando el puzle, la posición lógica y ubicación en la matriz de bloques de ambos bloques
            // Sólo queda intercambiar la parte visual, la posición física de ambos en la escena 
            //block.ExchangeTransform(otherBlock);

            blockInMotion = false;
        }*/

        // Pone los contadores de información a cero
        public void UserInteraction() {
            manager.CleanInfo();
        }
        
        public Casilla GetCasilla(Position position) {
                if (position == null) throw new ArgumentNullException(nameof(position));

                return casillas[position.GetRow(), position.GetColumn()];
        }

        // Cadena de texto representativa
        public override string ToString() {
            return "Board{" + casillas.ToString() + "}";
        }
    }
}
