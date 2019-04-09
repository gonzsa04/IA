namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class Libreta : MonoBehaviour {

        private static readonly int DEFAULT_ROWS = 21;
        private static readonly int DEFAULT_COLUMNS = 3;
        private string[] cardNames = {
            "Biblioteca", "Cocina", "Comedor", "Estudio", "Pasillo", "Recibidor", "Billar", "Baile", "Terraza",
            "A", "B", "C", "M", "P", "R",
            "Candelabro", "Cuerda", "Herramienta", "Pistola", "Puñal", "Tuberia"
        };

        public enum TipoLibreta { N, X, O };
        public string[] textoLibreta = { " ", "X", "O" };
        public GameObject[,] libreta;

        public Text playerName;
        public GameObject panel;
        public GameObject panel1;
        public GameObject panel2;

        private GameObject textPrefab;

        [HideInInspector] public TipoEstancia estanciaActual = TipoEstancia.Biblioteca;
        [HideInInspector] public int sospechosoActual = 0;

        public void Initialize()
        {
            for(int i = 0; i < DEFAULT_ROWS; i++)
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
            for(int j = 0; j < GameManager.instance.Suposicion.Count; j++)
            {
                libreta[getRowFromCard(GameManager.instance.Suposicion[j]), i].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.X];
            }
        }

        public void setPlayerName(string name) { playerName.text = name; }

        private int getRowFromCard(string card)
        {
            int i = 0;
            while (i < DEFAULT_ROWS && cardNames[i] != card) i++;
            return i;
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
