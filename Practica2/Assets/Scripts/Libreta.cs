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
                    libreta[getRowFromCard(card), i].GetComponent<Text>().text = textoLibreta[(int)TipoLibreta.N];
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
            textPrefab = new GameObject();
            textPrefab.AddComponent<Text>();
            textPrefab.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");

            float width = panel.transform.localScale.x / GameManager.instance.numPlayers;
            float height = panel.transform.localScale.y / cardNames.Length;

            for(int i = 0; i < DEFAULT_ROWS; i++)
            {
                for (int j = 0; j < DEFAULT_COLUMNS; j++)
                {
                    libreta[i, j] = Instantiate(textPrefab, panel.transform);
                    //libreta[i, j].transform.localScale = new Vector3(width, height, 0);
                    libreta[i, j].transform.position = new Vector3(j*(libreta[i, j].transform.localScale.x), 
                        i * (libreta[i, j].transform.localScale.y), 0) + panel.transform.position;
                }
            }
        }
    }
}
