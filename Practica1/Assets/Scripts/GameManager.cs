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

    public enum TipoCasilla { Libre, Agua, Barro, Rocas };

    // gestiona el juego del tanque
    public class GameManager : MonoBehaviour {
        
        // GameObjects
        public static GameManager instance; // para poder ser llamado desde los demas .cs (static)
        public Tablero tablero;             // tablero de casillas (representacion visual)
        public Tanque tank;                 // tanque que se ira moviendo por el tablero
        public GameObject flag;             // bandera que se posicionara en la casilla destino
        public GameObject markerPrefab;     // marcador que va indicando al tanque por donde ir
                     
        // Interfaz
        public GameObject infoPanel;
        public Text timeNumber;
        public Text stepsNumber;
        public InputField rowsInput;
        public InputField columnsInput;
        public Text cantMove;

        // Dimensiones iniciales del juego
        public uint rows = 10;
        public uint columns = 10;
        public double[] values = { 1, 2, 4, 1000 }; // costes de cruzar cada tipo de casilla

        private TankPuzzle puzzle;          // contiene la matriz logica que sera representada por el tablero de forma visual
        
        private double time = 0.0d;
        private uint steps = 0;

        private bool tankSelected = false;  // indica si el tanque esta seleccionado actualmente
        private bool tankMoving = false;

        private EdgeWeightedDigraph graph;  // grafo dirigido y valorado hecho a partir de la matriz logica de puzzle

        private Vector3 IniPosFlag;
        private System.Random random;

        void Awake()
        {
            instance = this;
        }

        void Start() {
            random = new System.Random();
            IniPosFlag = flag.transform.position;
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
            tablero.Initialize(puzzle);

            // Se crea el tanque
            tank.Initialize();

            // se crea el grafo a partir de la matriz logica
            CreateGraph();

            flag.transform.position = IniPosFlag;

            CleanInfo();   
            UpdateInfo();
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
                    if (puzzle.GetType(i, j) != (int)TipoCasilla.Rocas)
                    {
                        foreach (Vector2 v in directions)
                        {
                            int ni = i + (int)v.y;
                            int nj = j + (int)v.x;

                            if (ni >= 0 && ni < rows && nj >= 0 && nj < columns)
                            {
                                if (puzzle.GetType(ni, nj) != (int)TipoCasilla.Rocas)
                                {
                                    DirectedEdge a = new DirectedEdge((int)(j + columns * i), (int)(nj + columns * ni), values[puzzle.GetType(i, j)]);
                                    graph.AddEdge(a);
                                }
                            }
                        }
                    }
                }
            }
        }

        // actualiza el grafo tras haber clickado sobre una casilla (cambia aristas de alrededor, etc)
        public void updateGraph(TipoCasilla prevType, double prevValue, int r, int c)
        {
            Vector2[] directions = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
            foreach (Vector2 v in directions)
            {
                int ni = r + (int)v.y;
                int nj = c + (int)v.x;

                if (ni >= 0 && ni < rows && nj >= 0 && nj < columns)
                {
                    if (puzzle.GetType(r, c) == (int)TipoCasilla.Rocas)
                    {
                        graph.deleteEdge(new DirectedEdge((int)(c + r * columns), (int)(nj + ni * columns), values[puzzle.GetType(ni, nj)]));
                        graph.deleteEdge(new DirectedEdge((int)(nj + ni * columns), (int)(c + r * columns), prevValue));
                    }
                    else if (prevType != TipoCasilla.Rocas)
                    {
                        graph.modifyEdge(new DirectedEdge((int)(nj + ni * columns), (int)(c + r * columns), prevValue), values[puzzle.GetType(r, c)]);
                    }
                    else
                    {
                        graph.AddEdge(new DirectedEdge((int)(c + r * columns), (int)(nj + ni * columns), values[puzzle.GetType(ni, nj)]));
                        graph.AddEdge(new DirectedEdge((int)(nj + ni * columns), (int)(c + r * columns), values[puzzle.GetType(r, c)]));
                    }
                }
            }
        }

        // actualiza la matriz logica
        public void updatePuzzle(TipoCasilla type, int r, int c)
        {
            puzzle.SetType(r, c, (uint)type);
        }

        // teniendo el grafo construido, determina el camino a seguir con menor coste desde la posicion del tanque
        // hasta la posicion p. Es llamado al seleccionar la casilla a la que queremos que vaya el tanque
        public IEnumerator createPath(int x, int y)
        {
            CleanInfo();
            UpdateInfo();

            flag.transform.position = tablero.getCasPos(x, y);

            IndexedPriorityQueue<int> pq = new IndexedPriorityQueue<int>((int)(rows*columns));
            Astar astar = new Astar(); // la posicion de origen sera la posicion logica del tanque
            time = Time.realtimeSinceStartup;
            astar.Init(graph, ref pq, (int)(puzzle.TankPosition.GetColumn() + columns * puzzle.TankPosition.GetRow()));
            time = Time.realtimeSinceStartup - time;
            UpdateInfo();

            // una vez tenemos el camino, lo recorremos posicion a posicion hasta llegar al destino
            // moviendo al tanque por cada una de las casillas intermedias
            List<int> path = astar.GetPathTo((int)(y + x * columns));

            List<GameObject> markers = new List<GameObject>();
            setPathMarkers(path, ref markers);

            int r = 0, c = 0;
            int i = path.Count - 1;

            i = path.Count - 1;
            tankMoving = true;

            while (i >= 0 && path[i] >= 0)
            {
                r = (int)(path[i] / columns);
                c = (int)(path[i] - r * columns);
                setTankPosition(tablero.getCasPos(r, c));      // posicion fisica
                if(i > 0)Destroy(markers[markers.Count - i].gameObject);
                i--;
                steps++;
                UpdateInfo();
                yield return new WaitForSecondsRealtime(0.3f); // delay 
            }
            markers.Clear();

            if (i < 0)
                puzzle.TankPosition = new Position((uint)r, (uint)c);  // posicion logica
            else StartCoroutine(changeCanMove());

            tankMoving = false;
            changeTankSelected();
        }

        private void setPathMarkers(List<int> path, ref List<GameObject> markers)
        {
            int r = 0, c = 0;
            int i = path.Count - 1;

            Quaternion rot = Quaternion.Euler(0, 0, 0);
            while (i > 0 && path[i] > 0)
            {
                r = (int)(path[i] / columns);
                c = (int)(path[i] - r * columns);
                markers.Add(Instantiate(markerPrefab));

                Vector3 pos = new Vector3(tablero.getCasPos(r, c).x, 0.5f, tablero.getCasPos(r, c).z);
                markers[path.Count - i - 1].transform.position = pos;

                if (i < path.Count - 1)
                {
                    if (markers[path.Count - i - 1].transform.position.x > markers[path.Count - i - 2].transform.position.x)
                        rot = Quaternion.Euler(-90, 90, 0);
                    else if (markers[path.Count - i - 1].transform.position.x < markers[path.Count - i - 2].transform.position.x)
                        rot = Quaternion.Euler(-90, 270, 0);
                    else if (markers[path.Count - i - 1].transform.position.z > markers[path.Count - i - 2].transform.position.z)
                        rot = Quaternion.Euler(-90, 0, 0);
                    else
                        rot = Quaternion.Euler(-90, 180, 0);
                    markers[path.Count - i - 2].transform.rotation = rot;
                }
                i--;
            }
        }

        public IEnumerator changeCanMove()
        {
            cantMove.enabled = true;
            yield return new WaitForSecondsRealtime(2.0f);
            cantMove.enabled = false;
        }

        // Pone los contadores de información a cero
        private void CleanInfo() {
            time = 0.0d;
            steps = 0;
        }

        // Actualiza la información del panel, mostrándolo si corresponde
        private void UpdateInfo()
        {
            timeNumber.text = (time * 1000).ToString("0.0"); // Lo enseñamos en milisegundos y sólo con un decimal
            stepsNumber.text = steps.ToString();
        }

        // restablece la config inicial del juego (tanto en la matriz fisica de casillas como en la logica)
        // si no se han cambiado sus dimensiones si se han cambiado, se crea un juego nuevo
        public void ResetPuzzle() {
            CleanInfo();
            UpdateInfo();
            flag.transform.position = IniPosFlag;

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

                CleanInfo();
                UpdateInfo();

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

    
