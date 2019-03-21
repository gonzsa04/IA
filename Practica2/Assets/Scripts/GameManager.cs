namespace UCM.IAV.Puzzles {
    
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Model;
    
    // IA
    using UCM.IAV.IA;
    using UCM.IAV.IA.Search;
    using UCM.IAV.IA.Search.Informed; 
    using UCM.IAV.IA.Util;

    public enum TipoEstancia { Biblioteca, Cocina, Comedor, Estudio, Pasillo, Recibidor, Billar, Baile, Terraza };
    public enum Turn { H0, B1, B2 };
    public enum TipoHeuristicas { SINH, H1, H2, H3 };

    // gestiona el juego del tanque
    public class GameManager : MonoBehaviour {

        // GameObjects
        public static GameManager instance; // para poder ser llamado desde los demas .cs (static)
        public Tablero tablero;             // tablero de casillas (representacion visual)
        public Ficha fichaPrefab;           // prefab de player generico

        public List<Character> characters;          // jugadores y sospechosos
        public string[] names = { "h0", "b1", "b2", "A", "B", "C", "M", "P", "R" };
        public int numPlayers = 3;

        // Interfaz
        public Text timeNumber;
        public Text stepsNumber;
        public Text costNumber;
        public Text nodesNumber;
        public Text depthNumber;
        public Text memNumber;
        public Text cantMove;

        // Dimensiones iniciales del juego
        public int rows = 9;
        public int columns = 9;
        public int roomLength = 3;

        private CluedoPuzzle puzzle;    // contiene la matriz logica que sera representada por el tablero de forma visual
        private Turn turn = Turn.H0;

        // medidas de exito mostradas por pantalla
        private double time = 0.0d;
        private uint steps = 0;
        private double cost = 0;
        private int expandedNodes = 0;
        private int depth = 0;
        private int memSize = 0;

        void Awake()
        {
            instance = this;
        }

        void Start() {
            characters = new List<Character>();
            for (int i = 0; i < names.Length; i++) { 
                if (i < numPlayers) characters.Add(new Player(fichaPrefab));
                else characters.Add(new Suspect(fichaPrefab));
            }
            Initialize(rows, columns, roomLength);
        }

        // Inicializa o reinicia el gestor
        private void Initialize(int rows, int columns, int roomLength) {
            if (tablero == null) throw new InvalidOperationException("The board reference is null");
            if (timeNumber == null) throw new InvalidOperationException("The timeNumber reference is null");
            if (stepsNumber == null) throw new InvalidOperationException("The stepsNumber reference is null");

            this.rows = rows;
            this.columns = columns;
            this.roomLength = roomLength;

            // Se crea el puzle internamente (matriz logica)
            puzzle = new CluedoPuzzle(rows, columns, roomLength);

            // Se inicializa todo el tablero de casillas (representacion visual a partir de la matriz logica)
            tablero.Initialize(puzzle);

            // GUI
            CleanInfo();
            UpdateInfo();
        }

        // actualiza la matriz logica
        public void updatePuzzle(TipoEstancia type, int r, int c)
        {
            puzzle.SetType(r, c, (int)type);
        }

        // se escribira por pantalla que el tanque no puede moverse al destino solicitado durante un tiempo 
        private IEnumerator changeCanMove(float time)
        {
            cantMove.enabled = true;
            yield return new WaitForSecondsRealtime(time);
            cantMove.enabled = false;
        }

        public void startCanMoveRoutine(float time)
        {
            StartCoroutine(changeCanMove(time));
        }

        public void changeTieneSuspect(int r, int c)
        {
            tablero.changeTieneSuspect(r, c);
        }
        public void changeTienePlayer(int r, int c)
        {
            tablero.changeTienePlayer(r, c);
        }

        public Position getEstancia(int r, int c)
        {
            return new Position((r/ roomLength) *roomLength, (c / roomLength) *roomLength);
        }

        public void processClick(Position posL, Vector3 posP)
        {
            Player p = (Player)characters[(int)turn];
            p.move(posL, posP);

            int suspectNum = 0;

            for (int i = 0; i < roomLength; i++)
            {
                for (int j = 0; j < roomLength; j++)
                {
                    Position pos = getEstancia(posL.GetRow(), posL.GetColumn());
                    if (tablero.tieneSuspect(i + pos.GetRow(), j + pos.GetColumn())) suspectNum++;
                }
            }
            if (suspectNum == 1)
            {
                //DESPLEGABLE OBJECTOS
            }
            else if (suspectNum > 1)
            {
                //DESPLEGABLE SOSPECHOSOS
            }
        }

        public void moveSuspect(Suspect sus)
        {
            int r, c;
            Position suspectE, playerE;
            suspectE = getEstancia(sus.ficha_.position.GetRow(), sus.ficha_.position.GetColumn());
            playerE = getEstancia(characters[(int)turn].ficha_.getLogicPosition().GetRow(), characters[(int)turn].ficha_.getLogicPosition().GetColumn());
            if (suspectE.GetRow() == playerE.GetRow() && suspectE.GetColumn() == playerE.GetColumn()) {
                //DESPLEGABLE
            }
            else
            {
                r = playerE.GetRow();
                c = playerE.GetColumn();
                while (r < playerE.GetRow() + roomLength &&  (tablero.tienePlayer(r,c) || tablero.tieneSuspect(r, c)))
                {
                    while (c < playerE.GetColumn() + roomLength && (tablero.tienePlayer(r, c) || tablero.tieneSuspect(r, c)))
                    {
                        c++;
                    }
                    if (c >= playerE.GetColumn() + roomLength)
                    {
                        r++;
                        c = playerE.GetColumn();
                    }
                }
                sus.move(new Position(r, c), tablero.getCasPos(r, c));
                tablero.changeTieneSuspect(r, c);
            }
        }

        // Pone los contadores de información a cero
        private void CleanInfo() {
            time = 0.0d;
            steps = 0;
            cost = 0;
            expandedNodes = 0;
            memSize = 0;
            depth = 0;
        }

        // Actualiza la información del panel, mostrándolo si corresponde
        private void UpdateInfo()
        {
            timeNumber.text = (time * 1000).ToString("0.0"); // Lo enseñamos en milisegundos y sólo con un decimal
            stepsNumber.text = steps.ToString();
            costNumber.text = cost.ToString();
            nodesNumber.text = expandedNodes.ToString();
            memNumber.text = memSize.ToString();
            depthNumber.text = depth.ToString();
        }

        // se crea un juego nuevo con casillas aleatorias
        public void RandomPuzzle()
        {
            CleanInfo();
            UpdateInfo();

            Initialize(rows, columns, roomLength);
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

    
