namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class Libreta : MonoBehaviour {

        public int DEFAULT_ROWS = 21;
        public int DEFAULT_COLUMNS = 3;

        public enum TipoLibreta { N, X, O };
        public string[] textoLibreta = { " ", "X", "O" };
        public GameObject[,] libreta;

        public Text playerName;
        public GameObject panel;
        public GameObject panel1;
        public GameObject panel2;

        private GameObject textPrefab;
        private GameManager gm;

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
                    libreta[i, j].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.N];
                }
            }
        }

        public void receiveCard(string card, int turno)
        {
            for (int i = 0; i < DEFAULT_COLUMNS; i++)
            {
                if(i == turno)
                    libreta[getRowFromCard(card), i].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.O];
                else
                    libreta[getRowFromCard(card), i].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.X];
            }
        }

        public void notReceivedCards(int turno)
        {
            for(int i = 0; i< DEFAULT_ROWS; i++)
            {
                if(libreta[i, turno].GetComponent<Text>().text != textoLibreta[(int)TipoLibreta.O])
                    libreta[i, turno].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.X];
            }
        }

        public void notCoincidentCardsFrom(int i)
        {
            for(int j = 0; j < gm.Suposicion.Count; j++)
            {
                libreta[getRowFromCard(gm.Suposicion[j]), i].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.X];
            }
        }

        public void setPlayerName(string name) { playerName.text = name; }

        public int getRowFromCard(string card)
        {
            int i = 0;
            while (i < DEFAULT_ROWS && cardNames[i] != card) i++;
            return i;
        }

        public int getMinPlayerInfo()
        {
            int index = -1, maxBlanks = -1;
            for(int i = 0; i < DEFAULT_COLUMNS; i++)
            {
                if(i != gm.getTurn())
                {
                    int blanks = 0;
                    for (int j = 0; j < DEFAULT_ROWS; j++)
                    {
                        if (libreta[j, i].GetComponent<Text>().text == textoLibreta[(int)TipoLibreta.N])
                            blanks++;
                    }
                    if (blanks > maxBlanks)
                    {
                        maxBlanks = blanks;
                        index = i;
                    }
                }
            }

            return index;
        }

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

        public bool completedRow(int r)
        {
            int i = 0;
            while (i < DEFAULT_COLUMNS && libreta[r, i].GetComponent<Text>().text == textoLibreta[(int)TipoLibreta.X]) i++;
            return (i == DEFAULT_COLUMNS);
        }

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
