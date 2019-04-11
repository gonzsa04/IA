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
        public string[] weapons = { "Candelabro", "Cuerda", "Herramienta", "Pistola", "Puñal", "Tuberia"};
        public List<int> weaponsAux; // nombres de los tipos de arma
        public string[] estancias = { "Biblioteca", "Cocina", "Comedor", "Estudio", "Pasillo", "Recibidor", "Billar", "Baile", "Terraza" };
        public List<int> estanciasAux; // nombres de los tipos de estancia
        public List<string> Suposicion;
        public List<string> turnos;
        public int numCards;                //Numero de cartas restantes
        public int numPlayers = 3;          //Numero de jugadores
        public int humanPlayers = 1;        //Numero de jugadores humanos
        public float timeBetweenTurn = 3f;  //Tiempo entre turnos (Usado para los bots)
        public float timeForPanels = 2f;    //Tiempo que tarda en quitarse cada panel

        // Interfaz
        public Text turnoNumber;
        public Text PlayerNumber;
        public Text PlayerReqNumber;
        public Text CartaNumber;
        public Text PlayerAcNumber;
        public Text ArmaAcNumber;
        public Text EstanciaAcNumber;
        public Text SuspectAcNumber;
        public Text cantMove;
        public Text ganar;
        public Text perder;
        public Canvas canvas;
        public GameObject armasPanel;
        public GameObject cartaPanel;
        public GameObject armasSupPanel;
        public GameObject sospechososSupPanel;
        public GameObject estanciasSupPanel;
        public GameObject AcusarPanel;

        [HideInInspector] public string cartaRecibida, playerPreguntado, playerRequester;

        // Dimensiones iniciales del juego
        public int rows = 9;
        public int columns = 9;
        public int roomLength = 3;

        private System.Random rnd = new System.Random();
        private Libreta[] libretas;
        private float actualTime = -1;
        private int supos = 0;

        private CluedoPuzzle puzzle;    // contiene la matriz logica que sera representada por el tablero de forma visual
        private int turn;

        // crimen
        private string armaCrimen;
        private string estanciaCrimen;
        private string sospechosoCrimen;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            characters = new List<Character>();
            turnos = new List<string>();
            libretas = new Libreta[numPlayers];
            Suposicion = new List<string>();

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


            // init de variables
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].ficha_ != null) Destroy(characters[i].ficha_.gameObject);
            }

            turnos.Clear();
            characters.Clear();

            //Añadimos en characters los jugadores (humanos y bots) en las primeras posiciones y los sospechosos al final
            for (int i = 0; i < names.Length; i++)
            {
                if (i < numPlayers)
                {
                    if(i < humanPlayers)characters.Add(new Player(fichaPrefab, libretas[i], i));      
                    else if(i == humanPlayers) characters.Add(new Bot1(fichaPrefab, libretas[i], i));
                    else characters.Add(new Bot2(fichaPrefab, libretas[i], i));
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
            armaCrimen = weapons[chooseCardFromType(ref weaponsAux)];
            estanciaCrimen = estancias[chooseCardFromType(ref estanciasAux)];

            // repartimos las cartas sobrantes entre los jugadores
            dealCards();

            turn = 0;

            // GUI
            disableGUI();
            AcusarPanel.SetActive(false);
            CleanInfo();
            UpdateInfo();
        }

        //-----------------------REPARTICION DE CARTAS----------------------------

        // inicializa los distintos tipos de cartas (nombres, armas y estancias)
        private void initCards()
        {
            namesAux = new List<int>();
            weaponsAux = new List<int>();
            estanciasAux = new List<int>();
            for (int i = 0; i < estancias.Length; i++)
            {
                if (i < names.Length - numPlayers) namesAux.Add(i);
                if (i < weapons.Length) weaponsAux.Add(i);
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
                        index = chooseCardFromType(ref weaponsAux);
                        if (index != -1) return weapons[index];
                        break;
                    default:
                        index = chooseCardFromType(ref estanciasAux);
                        if (index != -1) return estancias[index];
                        break;
                }
            } while (index == -1);

            return " ";
        }

        //Elige una carta aleatoria de la lista recibida
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
            numCards = namesAux.Count + weaponsAux.Count + estanciasAux.Count;
            for (int i = 0; i < numPlayers; i++)
            {
                Player aux = (Player)characters[i];
                for (int j = 0; j < numCards / numPlayers; j++)
                {
                    aux.cards_.Add(chooseCard());
                    aux.libreta_.receiveCard(aux.cards_[j], i);
                }
                aux.libreta_.notReceivedCards(i);
            }
        }

        //------------------------------------------------------------------------

        //Realizamos el turno de cada jugador con un delay(timeBetweenTurn) entre ellos 
        void Update()
        {
            if (!GameOver)
            {
                if ((Time.time - actualTime > timeBetweenTurn || actualTime == -1) && turn >= humanPlayers)
                {
                    Player aux = (Player)characters[turn];
                    if (!aux.asked && !aux.moved) aux.myTurn();
                }
            }
        }

        // actualiza el turno del jugador actual
        public void nextTurn()
        {
            if (!GameOver)
            {
                Suposicion.Clear();
                supos = 0;
                Player aux = (Player)characters[turn];
                aux.moved = false; aux.asked = false; aux.supposed = false;
                //disableLibretas();        //DEBUG: comenta esta linea para poder ver las libretas en todo momento
                armasPanel.SetActive(false);
                estanciasSupPanel.SetActive(false);
                sospechososSupPanel.SetActive(false);
                armasSupPanel.SetActive(false);
                if (turn == turnos.Count - 1) turn = 0;
                else turn++;
                if (turnos[turn] == "") nextTurn();
                UpdateInfo();
                actualTime = Time.time;
            }
        }

        public bool isPlayerTurn()
        {
            return turn == 0; 
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

        //Hace aparecer el panel de perder durante time segundos
        private IEnumerator changePerder(float time)
        {
            perder.enabled = true;
            yield return new WaitForSecondsRealtime(time);
            perder.enabled = false;
        }

        //Hace aparecer el panel de la carta mostrada durante time segundos
        private IEnumerator changeCartaPanel(float time)
        {
            cartaPanel.SetActive(true);
            yield return new WaitForSecondsRealtime(time);
            cartaPanel.SetActive(false);
        }

        //Hace aparecer el panel de acusacion durante time segundos
        private IEnumerator changeAcusarPanel(float time)
        {
            AcusarPanel.SetActive(true);
            yield return new WaitForSecondsRealtime(time);
            AcusarPanel.SetActive(false);
        }

        public void startCanMoveRoutine()
        {
            StartCoroutine(changeCanMove(timeForPanels));
        }

        public void changeTieneSuspect(int r, int c)
        {
            tablero.changeTieneSuspect(r, c);
        }
        public void changeTienePlayer(int r, int c)
        {
            tablero.changeTienePlayer(r, c);
        }

        //Devuelve la primera posicion de esa estancia dada otra que tambien pertenezca a ella
        public Position getPosEstancia(int r, int c)
        {
            return new Position((r / roomLength) * roomLength, (c / roomLength) * roomLength);
        }

        public TipoEstancia getTipoEstancia(int r, int c)
        {
            return tablero.getCasEstancia(r, c);
        }

        //Devuelve la posicion de la primera casilla con el tipo de estancia "estancia"
        public Position getPosFromEstance(TipoEstancia estancia)
        {
            return tablero.getEstancePos(estancia);
        }

        // mueve al jugador a esa posicion (fisica y logicamente)
        public void movePlayer(Position posL, Vector3 posP)
        {
            Player p = (Player)characters[(int)turn];

            if (!p.asked && !p.moved && !p.supposed)
            {
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
                tablero.setTienePlayer(posL.GetRow(), posL.GetColumn(), true);
            }
        }

        //Si el jugador ya esta en la estancia del sospechoso sus se activa el panel de armas para
        //acusar, si no se mueve a ese sospechoso a tu estancia
        public void moveSuspect(Suspect sus)
        {
            Position suspectE, playerE;
            suspectE = getPosEstancia(sus.ficha_.position.GetRow(), sus.ficha_.position.GetColumn());
            playerE = getPosEstancia(characters[(int)turn].ficha_.getLogicPosition().GetRow(), characters[(int)turn].ficha_.getLogicPosition().GetColumn());
            Player aux = (Player)characters[(int)turn];

            if (suspectE.GetRow() == playerE.GetRow() && suspectE.GetColumn() == playerE.GetColumn())
            {
                armasPanel.SetActive(true);
                aux.libreta_.sospechosoActual = sus.index;
            }
            else if (!aux.moved && !aux.asked && !aux.supposed)
            {
                Position freePos = getFreeCasInEs(playerE);
                sus.move(freePos, tablero.getCasPos(freePos.GetRow(), freePos.GetColumn()));
                tablero.changeTieneSuspect(freePos.GetRow(), freePos.GetColumn());
            }
        }

        //Devuelve la primera posicion libre en la estancia a la que pertenezca estancePos
        public Position getFreeCasInEs(Position estancePos)
        {
            int r, c;
            r = estancePos.GetRow();
            c = estancePos.GetColumn();
            while (r < estancePos.GetRow() + roomLength && (tablero.tienePlayer(r, c) || tablero.tieneSuspect(r, c)))
            {
                while (c < estancePos.GetColumn() + roomLength && (tablero.tienePlayer(r, c) || tablero.tieneSuspect(r, c)))
                {
                    c++;
                }
                if (c >= estancePos.GetColumn() + roomLength)
                {
                    r++;
                    c = estancePos.GetColumn();
                }
            }
            return new Position(r, c);
        }

        //Simula un click en la casilla (r,c)
        public void clickTab(Position pos)
        {
            tablero.clickCas(pos.GetRow(), pos.GetColumn());
        }

        //Realiza una acusacion con el sospechoso y estancia actual y el arma escogida(i)
        public void Acusar(int i)
        {
            Player aux = (Player)characters[turn];

            PlayerAcNumber.text = names[turn];
            SuspectAcNumber.text = names[aux.libreta_.sospechosoActual];
            EstanciaAcNumber.text = aux.libreta_.estanciaActual.ToString();
            ArmaAcNumber.text = weapons[i];

            StartCoroutine(changeAcusarPanel(timeForPanels));

            armasPanel.SetActive(false);
            //Si la acusacion corresponde con las cartas del sobre, gana el jugador que ha acusado
            if (names[aux.libreta_.sospechosoActual] == sospechosoCrimen && 
                aux.libreta_.estanciaActual.ToString() == estanciaCrimen && weapons[i] == armaCrimen)
            {
                Ganar();
            }
            else  //Si no coincide, pierde
            {
                Perder(ref aux);
            }

            UpdateInfo();
        }

        //Añade una carta a la suposicion global, si la suposicion ya esta completa, pregunta
        public void seleccionarSuposicion(string carta)
        {
            Suposicion.Add(carta);
            supos++;
            if (supos == 1)
            {
                estanciasSupPanel.SetActive(false);
                sospechososSupPanel.SetActive(true);
            }
            else if (supos == 2)
            {
                sospechososSupPanel.SetActive(false);
                armasSupPanel.SetActive(true);
            }
            else
            {
                int i = 0;
                while (i < numPlayers && characters[i].ficha_.name_ != playerPreguntado) i++;
                Player aux = (Player)characters[i];
                aux.makeSugestion();
                armasSupPanel.SetActive(false);
                playerRequester = names[turn];
                nextTurn();
            }
        }

        //Actualiza la libreta de un jugador dado
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
            estanciasSupPanel.SetActive(false);
            sospechososSupPanel.SetActive(false);
            armasSupPanel.SetActive(false);
            cartaPanel.SetActive(false);
            ganar.enabled = false;
            perder.enabled = false;
            cantMove.enabled = false;
        }

        public int getTurn()
        {
            return turn;
        }

        public void startCartaCoroutine()
        {
            UpdateInfo();
            StartCoroutine(changeCartaPanel(timeForPanels));
        }

        private void startPerderCoroutine()
        {
            perder.text = "El jugador " + names[turn] + " ha sido eliminado";
            StartCoroutine(changePerder(timeForPanels));
        }

        //Revela las cartas del perdedor aux a los demas jugadores, lo elimina del juego (a el y a su turno) y pasa turno
        private void Perder(ref Player aux)
        {
            startPerderCoroutine();
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
            if (remainingPlayers <= 1) Ganar();     //Si al eliminarse este jugador solo queda uno en juego, este gana
        }

        //Señaliza el final de la partida y muestra al ganador
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
        }

        // Actualiza la información del panel, mostrándolo si corresponde
        private void UpdateInfo()
        {
            turnoNumber.text = turnos[turn];
            CartaNumber.text = cartaRecibida;
            PlayerNumber.text = playerPreguntado;
            PlayerReqNumber.text = playerRequester;
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


