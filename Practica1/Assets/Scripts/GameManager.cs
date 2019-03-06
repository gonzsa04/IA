namespace UCM.IAV.Puzzles {
    
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Model; 
    using Model.AI;
    
    // IA
    using UCM.IAV.IA;
    using UCM.IAV.IA.Search;
    using UCM.IAV.IA.Search.Informed; 
    using UCM.IAV.IA.Util;

    // gestiona el juego del tanque
    public class GameManager : MonoBehaviour {
        
        public static GameManager instance; // para poder ser llamado desde los demas .cs (static)
        public Tablero tablero;             // tablero de casillas (representacion visual)
        public Tanque tank;                 // tanque que se ira moviendo por el tablero
                     
        // Interfaz
        public GameObject infoPanel;
        public Text timeNumber;
        public Text stepsNumber;
        public InputField rowsInput;
        public InputField columnsInput;

        // Dimensiones iniciales del juego
        public uint rows = 10;
        public uint columns = 10;
        
        private TankPuzzle puzzle;          // contiene la matriz logica que sera representada por el tablero de forma visual
        
        private double time = 0.0d;
        private uint steps = 0;

        private bool tankSelected = false;  // indica si el tanque esta seleccionado actualmente
        private bool tankMoving = false;

        private EdgeWeightedDigraph graph;  // grafo dirigido y valorado hecho a partir de la matriz logica de puzzle
        
        private System.Random random;

        void Awake()
        {
            instance = this;
        }

        void Start() {
            random = new System.Random();

            Initialize(rows, columns);
        }

        // Inicializa o reinicia el gestor
        private void Initialize(uint rows, uint columns) {
            if (tablero == null) throw new InvalidOperationException("The board reference is null");
            if (tank == null) throw new InvalidOperationException("The board reference is null");
            if (infoPanel == null) throw new InvalidOperationException("The infoPanel reference is null");
            if (timeNumber == null) throw new InvalidOperationException("The timeNumber reference is null");
            if (stepsNumber == null) throw new InvalidOperationException("The stepsNumber reference is null");
            if (rowsInput == null) throw new InvalidOperationException("The rowsInputText reference is null");
            if (columnsInput == null) throw new InvalidOperationException("The columnsInputText reference is null");

            this.rows = rows;
            this.columns = columns;
            rowsInput.text = rows.ToString();
            columnsInput.text = columns.ToString();

            // Se crea el puzle internamente (matriz logica)
            puzzle = new TankPuzzle(rows, columns);

            // Se inicializa todo el tablero de casillas (representacion visual de la matriz logica) a partir de puzzle
            tablero.Initialize(this, puzzle);

            // Se crea el tanque
            tank.Initialize();

            // se crea el grafo a partir de la matriz logica
            CreateGraph();

            CleanInfo();
            
            //UpdateInfo();
        }

        // se crea el grafo a partir de la matriz logica
        private void CreateGraph()
        {
            graph = new EdgeWeightedDigraph((int)(rows * columns)); // adyacencias de cada casilla (derecha, abajo, izquierda, arriba)
            Vector2[] directions = { new Vector2( 0, 1 ), new Vector2( 1, 0 ), new Vector2( 0, -1 ), new Vector2( -1, 0 ) };

            // si la casilla no es de tipo Roca, le establecemos union 
            // con todas sus adyacentes que tampoco sean de tipo Roca
            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (tablero.getCasType(i, j) != TipoCasilla.Rocas)
                    {
                        foreach (Vector2 v in directions)
                        {
                            int ni = i + (int)v.y;
                            int nj = j + (int)v.x;

                            if (ni >= 0 && ni < rows && nj >= 0 && nj < columns)
                            {
                                if (tablero.getCasType(ni, nj) != TipoCasilla.Rocas)
                                {
                                    DirectedEdge a = new DirectedEdge((int)(j + columns * i), (int)(nj + columns * ni), tablero.getCasValue(ni, nj));
                                    graph.AddEdge(a);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void updateGraph(TipoCasilla type, TipoCasilla prevType, double value, double prevValue, int r, int c)
        {
            Vector2[] directions = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
            foreach (Vector2 v in directions)
            {
                int ni = r + (int)v.y;
                int nj = c + (int)v.x;

                if (ni >= 0 && ni < rows && nj >= 0 && nj < columns)
                {
                    if (type == TipoCasilla.Rocas)
                    {
                        graph.deleteEdge(new DirectedEdge((int)(c + r * columns), (int)(nj + ni * columns), tablero.getCasValue(ni, nj)));
                        graph.deleteEdge(new DirectedEdge((int)(nj + ni * columns), (int)(c + r * columns), prevValue));
                    }
                    else if (prevType != TipoCasilla.Rocas)
                    {
                        graph.modifyEdge(new DirectedEdge((int)(nj + ni * columns), (int)(c + r * columns), prevValue), value);
                    }
                    else
                    {
                        graph.AddEdge(new DirectedEdge((int)(c + r * columns), (int)(nj + ni * columns), tablero.getCasValue(ni, nj)));
                        graph.AddEdge(new DirectedEdge((int)(nj + ni * columns), (int)(c + r * columns), value));
                    }
                }
            }
        }

        // teniendo el grafo construido, determina el camino a seguir con menor coste desde la posicion del tanque
        // hasta la posicion p. Es llamado al seleccionar la casilla a la que queremos que vaya el tanque
        public IEnumerator createPath(Position p)
        {
            IndexedPriorityQueue<int> pq = new IndexedPriorityQueue<int>((int)(rows*columns));
            Astar astar = new Astar(); // la posicion de origen sera la posicion logica del tanque
            astar.Init(graph, ref pq, (int)(puzzle.TankPosition.GetColumn() + columns * puzzle.TankPosition.GetRow()));

            // una vez tenemos el camino, lo recorremos posicion a posicion hasta llegar al destino
            // moviendo al tanque por cada una de las casillas intermedias
            List<int> path = astar.GetPathTo((int)(p.GetColumn()+p.GetRow()*columns));
            int r = 0, c = 0;
            int i = path.Count - 1;

            tankMoving = true;

            while (i >= 0 && path[i] >= 0)
            {
                r = (int)(path[i] / columns);
                c = (int)(path[i] - r * columns);
                setTankPosition(tablero.getCasPos(r, c));      // posicion fisica
                yield return new WaitForSecondsRealtime(0.3f); // delay 
                i--;
            }

            if (i < 0)
                puzzle.TankPosition = new Position((uint)r, (uint)c);  // posicion logica
            else Debug.Log("No se pué");
            tankMoving = false;
            changeTankSelected();
        }

        // Pone los contadores de información a cero
        public void CleanInfo() {
            time = 0.0d;
            steps = 0;
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
                {
                    tablero.ResetTablero(puzzle);
                    tank.Reset();
                }
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

        // el tanque pasa de seleccionado a no seleccionado
        public void changeTankSelected()
        {
            tankSelected = !tankSelected;
            tank.UpdateColor();
        }

        public bool isTankSelected()
        {
            return tankSelected;
        }

        public bool isTankMoving()
        {
            return tankMoving;
        }

        // establece la posicion fisica del tanque
        public void setTankPosition(Vector3 pos)
        {
            tank.setPosition(pos);
        }

        // Salir de la aplicación
        public void Quit() {
            Application.Quit();
        }

        // Cadena de texto representativa
        public override string ToString() {
            return "Manager of " + tablero.ToString() + " over " + puzzle.ToString();
        }
    }
}

    
