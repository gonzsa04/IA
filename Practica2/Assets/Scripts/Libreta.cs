namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    //BASE DE CONOCIMIENTO DE LA INTELIGENCIA ARTIFICIAL
    public class Libreta : MonoBehaviour {
        //todos los jugadores tendran una libreta

        public int DEFAULT_ROWS = 21;
        public int DEFAULT_COLUMNS = 3;

        public enum TipoLibreta { N, X, O };                //N siginifica que no lo tiene nadie, O que lo tiene alguien y X seguro que no lo tiene
        public string[] textoLibreta = { " ", "X", "O" };   //Representacion en texto de TipoLibreta
        public GameObject[,] libreta;                       //Matriz de los textos donde se escribiran que cartas tiene cada uno de los jugadores            

        public Text playerName;                                         
        public GameObject panel;                            //Columna del jugador 1                    
        public GameObject panel1;                           //Columna del jugador 2            
        public GameObject panel2;                           //Columna del jugador 3

        private GameObject textPrefab;                      //Prefab del que se instancian copias en la matriz libreta
        private GameManager gm;                             

        //Arrat con todos los nombres de las cartas
        public string[] cardNames = {
            "Biblioteca", "Cocina", "Comedor", "Estudio", "Pasillo", "Recibidor", "Billar", "Baile", "Terraza",
            "A", "B", "C", "M", "P", "R",
            "Candelabro", "Cuerda", "Herramienta", "Pistola", "Puñal", "Tuberia"
        };

        [HideInInspector] public TipoEstancia estanciaActual = TipoEstancia.Biblioteca;
        [HideInInspector] public int sospechosoActual = 0;

        public void Initialize()
        {
            gm = GameManager.instance;

            for (int i = 0; i < DEFAULT_ROWS; i++)
            {
                for(int j = 0; j < DEFAULT_COLUMNS; j++)
                {
                    libreta[i, j].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.N];     //Inicializacion de todas las casillas a vacio
                }
            }
        }

        //Recibes una carta card del jugador turno
        public void receiveCard(string card, int turno)
        {
            for (int i = 0; i < DEFAULT_COLUMNS; i++)
            {
                if(i == turno)
                    libreta[getRowFromCard(card), i].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.O];  //si esa columna es la del jugador, marcas que la tiene
                else
                    libreta[getRowFromCard(card), i].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.X];  //si no la marcas como que no la tiene
            }
        }

        //Llena todas las cartas de una columna (turno) que esten vacias a X. Se llama cuando un jugador revela todas sus cartas
        public void notReceivedCards(int turno)
        {
            for(int i = 0; i< DEFAULT_ROWS; i++)
            {
                if(libreta[i, turno].GetComponent<Text>().text != textoLibreta[(int)TipoLibreta.O])
                    libreta[i, turno].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.X];
            }
        }

        //Dada la suposicion del game manager, si ninguna coincide con las del jugador preguntado, se marcan esas tres cartas en su columna como X
        public void notCoincidentCardsFrom(int i)
        {
            for(int j = 0; j < gm.Suposicion.Count; j++)
            {
                libreta[getRowFromCard(gm.Suposicion[j]), i].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.X];
            }
        }

        public void setPlayerName(string name) { playerName.text = name; }

        //Dada una carta card, te devuelve en que fila esta
        public int getRowFromCard(string card)
        {
            int i = 0;
            while (i < DEFAULT_ROWS && cardNames[i] != card) i++;
            return i;
        }

        //Devuelve al jugador del que menos informacion sepas, siempre y cuando no sepas todas sus cartas
        public int getMinPlayerInfo()
        {
            int index = -1, maxBlanks = -1;
            for(int i = 0; i < DEFAULT_COLUMNS; i++)
            {
                if(i != gm.getTurn())
                {
                    int blanks = 0, cards = 0;
                    for (int j = 0; j < DEFAULT_ROWS; j++)
                    {
                        if (libreta[j, i].GetComponent<Text>().text == textoLibreta[(int)TipoLibreta.N])
                            blanks++;
                        else if (libreta[j, i].GetComponent<Text>().text == textoLibreta[(int)TipoLibreta.O])
                            cards++;
                    }
                    if (blanks > maxBlanks && cards < gm.numCards/gm.numPlayers)
                    {
                        maxBlanks = blanks;
                        index = i;
                    }
                }
            }

            return index;
        }

        //Dada una columna (player), devuelve la primera estancia de la que no sabe
        public string getFirstBlankEstanceFrom(int player)
        {
            int i = 0; int backup = -1;
            while (i < gm.estancias.Length &&
                libreta[i, player].GetComponent<Text>().text != textoLibreta[(int)TipoLibreta.N])
            {
                if (backup == -1 && libreta[i, player].GetComponent<Text>().text == textoLibreta[(int)TipoLibreta.X])
                    backup = i;
                i++;
            }

            if (i < gm.estancias.Length) return gm.estancias[i];
            else return gm.estancias[backup];
        }

        //Dada una columna (player), devuelve el primer sospechoso del que no sabe
        public string getFirstBlankSuspectFrom(int player)
        {
            int i = gm.estancias.Length; int backup = -1;
            while (i < DEFAULT_ROWS - gm.weapons.Length &&
                libreta[i, player].GetComponent<Text>().text != textoLibreta[(int)TipoLibreta.N])
            {
                if (backup == -1 && libreta[i, player].GetComponent<Text>().text == textoLibreta[(int)TipoLibreta.X])
                    backup = i;
                i++;
            }

            if (i < DEFAULT_ROWS - gm.weapons.Length) return gm.names[i - gm.estancias.Length + gm.numPlayers];
            else return gm.names[backup - gm.estancias.Length + gm.numPlayers];
        }

        //Dada una columna (player), devuelve el primer arma de la que no sabe
        public string getFirstBlankWeaponFrom(int player)
        {
            int i = gm.estancias.Length + gm.names.Length - gm.numPlayers;
            int backup = -1;
            while (i < DEFAULT_ROWS && libreta[i, player].GetComponent<Text>().text != textoLibreta[(int)TipoLibreta.N])
            {
                if (backup == -1 && libreta[i, player].GetComponent<Text>().text == textoLibreta[(int)TipoLibreta.X])
                    backup = i;
                i++;
            }

            if (i < DEFAULT_ROWS)
                return gm.weapons[i - gm.estancias.Length - gm.names.Length +
                    gm.numPlayers];
            else return gm.weapons[backup - gm.estancias.Length - gm.names.Length +
                    gm.numPlayers];
        }

        //Devuelve si la fila r tiene X en todas sus columnas
        public bool completedWithXRow(int r)
        {
            int i = 0;
            while (i < DEFAULT_COLUMNS && libreta[r, i].GetComponent<Text>().text == textoLibreta[(int)TipoLibreta.X]) i++;
            return (i == DEFAULT_COLUMNS);
        }

        //Devuelve si la fila r esta completa con O/X
        public bool completedRow(int r)
        {
            int i = 0;
            while (i < DEFAULT_COLUMNS && libreta[r, i].GetComponent<Text>().text != textoLibreta[(int)TipoLibreta.N]) i++;
            return (i == DEFAULT_COLUMNS);
        }

        //Llena la fila r con el tipo determinado type
        public void setRowTo(int r, TipoLibreta type)
        {
            for(int i = 0; i < DEFAULT_COLUMNS; i++) libreta[r, i].GetComponent<Text>().text = textoLibreta[(int)type];
        }

        //Inicializa toda la matriz de textos instanciando y colocando cada uno en su posicion
        public void initMatrix()
        {
            libreta = new GameObject[DEFAULT_ROWS, DEFAULT_COLUMNS];
            float width = panel.GetComponent<RectTransform>().rect.width;
            float height = panel.GetComponent<RectTransform>().rect.height / cardNames.Length;

            textPrefab = new GameObject();
            textPrefab.AddComponent<Text>();
            textPrefab.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textPrefab.GetComponent<Text>().color = Color.black;
            textPrefab.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            textPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            textPrefab.GetComponent<RectTransform>().localScale = new Vector3(15, 1, 1);

            for (int i = 0; i < DEFAULT_ROWS; i++)
            {
                for (int j = 0; j < DEFAULT_COLUMNS; j++)
                {
                    if(j == 0) libreta[i, j] = Instantiate(textPrefab, panel.transform);
                    if(j == 1) libreta[i, j] = Instantiate(textPrefab, panel1.transform);
                    if(j == 2) libreta[i, j] = Instantiate(textPrefab, panel2.transform);
                }
            }
        }
    }
}
