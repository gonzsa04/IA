namespace UCM.IAV.Puzzles {
    
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Model; 
    using Model.AI;

    // Esto es para usar la IA
    using UCM.IAV.IA;
    using UCM.IAV.IA.Search;
    using UCM.IAV.IA.Search.Uninformed; 

    public class SlidingPuzzleManager : MonoBehaviour {

        // El tablero de casillas
        public BlockBoard board;
                     
        public GameObject infoPanel;
        public Text timeNumber;
        public Text stepsNumber;
        public InputField rowsInput;
        public InputField columnsInput;

        // Dimensiones iniciales del juego
        public uint rows = 10;
        public uint columns = 10;
        
        private SlidingPuzzle puzzle;
        // El resolutor del puzle (que puede admitir varias estrategias)
        private SlidingPuzzleSolver solver;
        private double time = 0.0d; // in seconds
        private uint steps = 0;
        
        private System.Random random;
        
        void Start() {
            random = new System.Random();

            // Mientras que no requiera de las dimensiones del puzle ni nada especial, se puede crear en el Start
            solver = new SlidingPuzzleSolver();

            Initialize(rows, columns);
        }

        // Inicializa o reinicia el gestor
        private void Initialize(uint rows, uint columns) {
            if (board == null) throw new InvalidOperationException("The board reference is null");
            if (infoPanel == null) throw new InvalidOperationException("The infoPanel reference is null");
            if (timeNumber == null) throw new InvalidOperationException("The timeNumber reference is null");
            if (stepsNumber == null) throw new InvalidOperationException("The stepsNumber reference is null");
            if (rowsInput == null) throw new InvalidOperationException("The rowsInputText reference is null");
            if (columnsInput == null) throw new InvalidOperationException("The columnsInputText reference is null");

            this.rows = rows;
            this.columns = columns;
            rowsInput.text = rows.ToString();
            columnsInput.text = columns.ToString();

            // Se crea el puzle internamente 
            puzzle = new SlidingPuzzle(rows, columns);
            // Se crea el resolutor (que puede admitir varias estrategias)

            // Inicializar todo el tablero de bloques
            board.Initialize(this, puzzle);

            CleanInfo();
            
            //UpdateInfo();
        }

        // Pone los contadores de información a cero
        public void CleanInfo() {
            time = 0.0d;
            steps = 0;
        }

        // Devuelve cierto si un bloque se puede mover, si se lo permite el gestor
        public bool CanMove(Casilla block) {
            if (block == null) throw new ArgumentNullException(nameof(block));

            return puzzle.CanMoveByDefault(block.position);
        }

        // Mueve un bloque, según las normas que diga el gestor
        public Casilla Move(Casilla block) {
            if (block == null) throw new ArgumentNullException(nameof(block));
            if (!CanMove(block)) throw new InvalidOperationException("The required movement is not possible");

            Position originPosition = block.position;

            Debug.Log(ToString() + " moves " + block.ToString() + "."); 
            var targetPosition = puzzle.MoveByDefault(block.position);
            // Si hemos tenido éxito ha cambiado la matrix lógica del puzle... pero no ha cambiado la posición (lógica), ni la mía ni la del hueco. Toca hacerlo ahora
            block.position = targetPosition;
            Casilla targetBlock = board.GetCasilla(targetPosition);
            targetBlock.position = originPosition;

            //UpdateInfo();

            return targetBlock;
        }

        // Actualiza la información del panel, mostrándolo si corresponde
        /*private void UpdateInfo() {

            // Según el puzle esté en orden o no, enseño el panel de información o no
            if (puzzle.IsInDefaultOrder()) {
                timeNumber.text = (time * 1000).ToString("0.0"); // Lo enseñamos en milisegundos y sólo con un decimal
                stepsNumber.text = steps.ToString();
                infoPanel.gameObject.SetActive(true);
            } else
                infoPanel.gameObject.SetActive(false);

        }*/
        
        // restablece la config inicial del juego si no se han cambiado sus dimensiones
        // si se han cambiado, se crea un juego nuevo
        public void ResetPuzzle() {
            
            if (rowsInput.text != null && columnsInput.text != null) {
                uint newRows = Convert.ToUInt32(rowsInput.text);
                uint newColumns = Convert.ToUInt32(columnsInput.text);

                if (newRows != rows || newColumns != columns)
                    Initialize(newRows, newColumns);
                else
                    board.ResetBlockBoard(puzzle);
            }
        }
        
        // se crea un juego nuevo con casillas aleatorias
        public void RandomPuzzle() {

            if (rowsInput.text != null && columnsInput.text != null) {
                uint newRows = Convert.ToUInt32(rowsInput.text);
                uint newColumns = Convert.ToUInt32(columnsInput.text);

                Initialize(newRows, newColumns);
            }
        }

        /*// Muestra la solución obtenida paso a paso y con una animación
        private void ShowSolution(List<Operator> operators, Metrics metrics) {
            // el resultado lo tenemos que mostrar de manera animada, haciendo acción tras acción 
            foreach (Operator op in operators) {
                Position position = solver.GetOperatedPosition(puzzle, op);
                board.Move(board.GetCasilla(position), BlockBoard.AI_DELAY);
                Debug.Log("Applying " + op.ToString() + " operator");
            }

            steps = Convert.ToUInt32(operators.Count);

            UpdateInfo();

        // Mostrar tanto el resultado como las métricas... lo mismo incluso paso a paso y en otro color, para diferenciar cómo quedan al terminar :-)
        //...las métricas pintarlas pintar por la pantalla o en fichero
        Debug.Log("Métrics: " + metrics.ToString()); // hacer bucle para mostrarlas todas
        }*/

        public void SolvePuzzleByBFS() {

            // Si está correcto, no lo resuelvo (o lo puedo llamar igualmente y que me devuelva una solución vacía, ok?
            //if (!puzzle.IsInDefaultOrder()) {       }
            // El resolutor ya está construido porque no requiere nada
            time = Time.realtimeSinceStartup;
            List<Operator> operators = solver.Solve(puzzle, SlidingPuzzleSolver.Strategy.BFS);
            time = Time.realtimeSinceStartup - time;
            // Crear una estructura para las metrics, algo menos libre... con campos
            Metrics metrics = solver.GetMetrics();
            //ShowSolution(operators, metrics);
        }

        public void SolvePuzzleByDFS() {
            // El resolutor ya está construido porque no requiere nada
            time = Time.realtimeSinceStartup;
            List<Operator> operators = solver.Solve(puzzle, SlidingPuzzleSolver.Strategy.DFS);
            time = Time.realtimeSinceStartup - time;
            // Crear una estructura para las metrics, algo menos libre... con campos
            Metrics metrics = solver.GetMetrics();
            //ShowSolution(operators, metrics);
        }


        // Salir de la aplicación
        public void Quit() {
            Application.Quit();
        }

        // Cadena de texto representativa
        public override string ToString() {
            return "Manager of " + board.ToString() + " over " + puzzle.ToString();
        }
    }
}

    
