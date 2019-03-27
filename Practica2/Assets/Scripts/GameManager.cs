namespace UCM.IAV.Puzzles
{

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

    // enums
    public enum TipoEstancia { Biblioteca, Cocina, Comedor, Estudio, Pasillo, Recibidor, Billar, Baile, Terraza };
    public enum TipoArmas { Candelabro, Cuerda, Herramienta, Pistola, Puñal, Tuberia };
    public enum TipoHeuristicas { SINH, H1, H2, H3 };

    // gestiona el juego del tanque
    public class GameManager : MonoBehaviour
    {

        // GameObjects
        public static GameManager instance; // para poder ser llamado desde los demas .cs (static)
        public Tablero tablero;             // tablero de casillas (representacion visual)
        public Ficha fichaPrefab;           // prefab de player generico
        public Libreta libPrefab;          // libretas para los tres jugadores
        public bool GameOver = false;

        // jugadores, estancias y armas
        public List<Character> characters;  // jugadores y sospechosos
        public string[] names = { "h0", "b1", "b2", "A", "B", "C", "M", "P", "R" };
        public List<int> namesAux; // nombres de los jugadores y sospechosos
        public string[] armas = { "Candelabro", "Cuerda", "Herramienta", "Pistola", "Puñal", "Tuberia"};
        public List<int> armasAux; // nombres de los tipos de arma
        public string[] estancias = { "Biblioteca", "Cocina", "Comedor", "Estudio", "Pasillo", "Recibidor", "Billar", "Baile", "Terraza" };
        public List<int> estanciasAux; // nombres de los tipos de estancia
        public int numPlayers = 3;

        // Interfaz
        public Text timeNumber;
        public Text stepsNumber;
        public Text costNumber;
        public Text nodesNumber;
        public Text depthNumber;
        public Text memNumber;
        public Text turnoNumber;
        public Text cantMove;
        public Text ganar;
        public Text perder;
        public Canvas canvas;
        public GameObject armasPanel;

        // Dimensiones iniciales del juego
        public int rows = 9;
        public int columns = 9;
        public int roomLength = 3;

        private System.Random rnd = new System.Random();
        private Libreta[] libretas;
        private List<string> turnos;

        private CluedoPuzzle puzzle;    // contiene la matriz logica que sera representada por el tablero de forma visual
        private int turn;

        // crimen
        private string armaCrimen;
        private string estanciaCrimen;
        private string sospechosoCrimen;

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

        void Start()
        {
            characters = new List<Character>();
            turnos = new List<string>();
            libretas = new Libreta[numPlayers];

            for (int i = 0; i < numPlayers; i++)
            {
                libretas[i] = Instantiate(libPrefab, canvas.transform);
                libretas[i].setPlayerName(names[i]);
                libretas[i].initMatrix();
            }

            Initialize(rows, columns, roomLength);
        }

        // Inicializa o reinicia el gestor
        private void Initialize(int rows, int columns, int roomLength)
        {
            if (tablero == null) throw new InvalidOperationException("The board reference is null");
            if (timeNumber == null) throw new InvalidOperationException("The timeNumber reference is null");
            if (stepsNumber == null) throw new InvalidOperationException("The stepsNumber reference is null");


            // init de variables
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].ficha_ != null) Destroy(characters[i].ficha_.gameObject);
            }

            turnos.Clear();
            characters.Clear();

            for (int i = 0; i < names.Length; i++)
            {
                if (i < numPlayers)
                {
                    characters.Add(new Player(fichaPrefab, libretas[i], i));
                    turnos.Add(names[i]);
                }
                else characters.Add(new Suspect(fichaPrefab, i));
            }

            this.rows = rows;
            this.columns = columns;
            this.roomLength = roomLength;

            // Se crea el puzle internamente (matriz logica)
            puzzle = new CluedoPuzzle(rows, columns, roomLength);

            // Se inicializa todo el tablero de casillas (representacion visual a partir de la matriz logica)
            tablero.Initialize(puzzle);

            //Se reinicia el juego
            GameOver = false;

            initCards(); // inicializa los distintos tipos de cartas

            // se elige un arma, un sospechoso y una estancia
            sospechosoCrimen = names[chooseCardFromType(ref namesAux) + numPlayers];
            armaCrimen = armas[chooseCardFromType(ref armasAux)];
            estanciaCrimen = estancias[chooseCardFromType(ref estanciasAux)];

            // repartimos las cartas sobrantes entre los jugadores
            dealCards();

            turn = 0;

            // GUI
            disableGUI();
            CleanInfo();
            UpdateInfo();
        }

        //-----------------------REPARTICION DE CARTAS----------------------------

        // inicializa los distintos tipos de cartas (nombres, armas y estancias)
        private void initCards()
        {
            namesAux = new List<int>();
            armasAux = new List<int>();
            estanciasAux = new List<int>();
            for (int i = 0; i < estancias.Length; i++)
            {
                if (i < names.Length - numPlayers) namesAux.Add(i);
                if (i < armas.Length) armasAux.Add(i);
                estanciasAux.Add(i);
            }
        }

        // devuelve una carta de los tres tipos que hay (nombres, armas y estancias)
        // y que no haya sido elegida ya
        private string chooseCard()
        {
            int index = -1;

            do
            {
                int random = rnd.Next(0, 3);
                switch (random)
                {
                    case 0:
                        index = chooseCardFromType(ref namesAux);
                        if (index != -1) return names[index + numPlayers];
                        break;
                    case 1:
                        index = chooseCardFromType(ref armasAux);
                        if (index != -1) return armas[index];
                        break;
                    default:
                        index = chooseCardFromType(ref estanciasAux);
                        if (index != -1) return estancias[index];
                        break;
                }
            } while (index == -1);

            return " ";
        }
        private int chooseCardFromType(ref List<int> cards)
        {
            int random = rnd.Next(0, cards.Count);
            int value = -1;
            if (cards.Count > 0)
            {
                value = cards[random];
                cards.Remove(value);
            }
            return value;
        }

        // reparte las cartas sobrantes entre el resto de jugadores
        private void dealCards()
        {
            int numCards = namesAux.Count + armasAux.Count + estanciasAux.Count;
            for (int i = 0; i < numPlayers; i++)
            {
                Player aux = (Player)characters[i];
                string card = "";
                for (int j = 0; j < numCards / numPlayers; j++)
                {
                    card = chooseCard();
                    aux.cards_.Add(card);
                    aux.libreta_.receiveCard(aux.cards_[j], i);
                }
            }
        }

        //------------------------------------------------------------------------

        // actualiza el turno del jugador actual
        public void nextTurn()
        {
            if (!GameOver)
            {
                disableLibretas();
                armasPanel.SetActive(false);
                cantMove.enabled = false;
                if (turn == turnos.Count - 1) turn = 0;
                else turn++;
                if (turnos[turn] == "") nextTurn();
                UpdateInfo();
            }
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

        private IEnumerator changePerder(float time)
        {
            perder.enabled = true;
            yield return new WaitForSecondsRealtime(time);
            perder.enabled = false;
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

        public Position getPosEstancia(int r, int c)
        {
            return new Position((r / roomLength) * roomLength, (c / roomLength) * roomLength);
        }

        public TipoEstancia getTipoEstancia(int r, int c)
        {
            return tablero.getCasEstancia(r, c);
        }

        // mueve al jugador a esa posicion (fisica y logicamente)
        public void movePlayer(Position posL, Vector3 posP)
        {
            Player p = (Player)characters[(int)turn];
            p.move(posL, posP);

            int characNum = 0, i = 0, j;

            while (i < roomLength && characNum <= 0)
            {
                j = 0;
                while (j < roomLength && characNum <= 0)
                {
                    Position pos = getPosEstancia(posL.GetRow(), posL.GetColumn());
                    if (tablero.tieneSuspect(i + pos.GetRow(), j + pos.GetColumn()) ||
                        tablero.tienePlayer(i + pos.GetRow(), j + pos.GetColumn()))
                        characNum++;
                    j++;
                }
                i++;
            }
            if (characNum == 0) nextTurn();                 //Si no hay sospechosos pasamos turno
        }

        public void moveSuspect(Suspect sus)
        {
            int r, c;
            Position suspectE, playerE;
            suspectE = getPosEstancia(sus.ficha_.position.GetRow(), sus.ficha_.position.GetColumn());
            playerE = getPosEstancia(characters[(int)turn].ficha_.getLogicPosition().GetRow(), characters[(int)turn].ficha_.getLogicPosition().GetColumn());
            if (suspectE.GetRow() == playerE.GetRow() && suspectE.GetColumn() == playerE.GetColumn())
            {
                armasPanel.SetActive(true);
                Player aux = (Player)characters[(int)turn];
                aux.libreta_.sospechosoActual = sus.index;
            }
            else
            {
                r = playerE.GetRow();
                c = playerE.GetColumn();
                while (r < playerE.GetRow() + roomLength && (tablero.tienePlayer(r, c) || tablero.tieneSuspect(r, c)))
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

        public void Acusar(int i)
        {
            //FALTA ACCEDER A LA LIBRETA DEL JUGADOR ACTUAL(SOSPECHOSO, ARMA(i), ESTANCIA)
            Player aux = (Player)characters[(int)turn];
            Debug.Log("El jugador " + names[(int)turn] + " ha acusado al sospechoso " + names[aux.libreta_.sospechosoActual] + " en la estancia " +
            aux.libreta_.estanciaActual.ToString() + " con el arma " + ((TipoArmas)i).ToString());
            armasPanel.SetActive(false);
            if (names[aux.libreta_.sospechosoActual] == sospechosoCrimen && aux.libreta_.estanciaActual.ToString() == estanciaCrimen && armas[i] == armaCrimen)
            {
                Ganar();
            }
            else
            {
                Perder(ref aux);
            }

            UpdateInfo();
        }

        public void changeLibreta(GameObject go)
        {
            Pair parComp = go.GetComponent<Pair>();
            int r = parComp.R, c = parComp.C;
            Player aux = (Player)characters[(int)turn];
            if (aux.libreta_.libreta[r, c].GetComponent<Text>().text == aux.libreta_.textoLibreta[(int)Libreta.TipoLibreta.O])
            {
                aux.libreta_.libreta[r, c].GetComponent<Text>().text = aux.libreta_.textoLibreta[(int)Libreta.TipoLibreta.N];
            }
            else if (aux.libreta_.libreta[r, c].GetComponent<Text>().text == aux.libreta_.textoLibreta[(int)Libreta.TipoLibreta.N])
            {
                aux.libreta_.libreta[r, c].GetComponent<Text>().text = aux.libreta_.textoLibreta[(int)Libreta.TipoLibreta.X];
            }
            else if (aux.libreta_.libreta[r, c].GetComponent<Text>().text == aux.libreta_.textoLibreta[(int)Libreta.TipoLibreta.X])
            {
                aux.libreta_.libreta[r, c].GetComponent<Text>().text = aux.libreta_.textoLibreta[(int)Libreta.TipoLibreta.O];
            }

            go.GetComponentInChildren<Text>().text = aux.libreta_.libreta[r, c].ToString();
        }

        public void enableLibretas(int index)
        {
            for (int i = 0; i < numPlayers; i++)
            {
                if (i == index)
                    libretas[i].gameObject.SetActive(!libretas[i].gameObject.activeSelf);
                else
                    libretas[i].gameObject.SetActive(false);
            }
        }
        public void disableLibretas()
        {
            for (int i = 0; i < numPlayers; i++)
            {
                libretas[i].gameObject.SetActive(false);
            }
        }

        public void disableGUI()
        {
            disableLibretas();
            armasPanel.SetActive(false);
            perder.enabled = false;
            ganar.enabled = false;
            cantMove.enabled = false;
        }

        public int getTurn()
        {
            return turn;
        }

        private void startPerderCoroutine(float time)
        {
            perder.text = "El jugador " + names[turn] + " ha sido eliminado";
            StartCoroutine(changePerder(time));
        }

        private void Perder(ref Player aux)
        {
            startPerderCoroutine(2.0f);
            aux.showAllCards();
            tablero.setTienePlayer(aux.ficha_.getLogicPosition().GetRow(), aux.ficha_.getLogicPosition().GetColumn(), false);
            Destroy(aux.ficha_.gameObject);
            turnos[turn] = "";
            nextTurn();

            int remainingPlayers = numPlayers;
            for (int i = 0; i < numPlayers; i++)
            {
                if (turnos[i] == "") remainingPlayers--;
            }
            if (remainingPlayers <= 1) Ganar();
        }

        private void Ganar()
        {
            disableGUI();
            ganar.enabled = true;
            ganar.text = "EL JUGADOR " + names[turn] + " HA GANADO";
            GameOver = true;
        }

        // Pone los contadores de información a cero
        private void CleanInfo()
        {
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
            turnoNumber.text = turnos[turn];
        }

        // se crea un juego nuevo con casillas aleatorias
        public void RandomPuzzle()
        {
            CleanInfo();
            UpdateInfo();

            Initialize(rows, columns, roomLength);
        }

        // Salir de la aplicación
        public void Quit()
        {
            Application.Quit();
        }

        // Cadena de texto representativa
        public override string ToString()
        {
            return "Manager of " + tablero.ToString() + " over " + puzzle.ToString();
        }
    }
}


