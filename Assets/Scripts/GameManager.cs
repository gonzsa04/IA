using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	//GM
	public static GameManager instance;
	public int distancia;
	public int n;
	[HideInInspector] public Casilla[,] tablero;
    [HideInInspector] public Vector2 hueco;
    Casilla[,] in_tablero;
    Vector2 in_hueco;
    bool done = false;

	// Use this for initialization
	void Awake () {
		instance = this;
    }
		
	void Start()
	{
		List<int> posibles = new List<int>();
        tablero = new Casilla[n, n];

        for (int i = 0; i < n * n; i++) posibles.Add(i);

		for (int i = 0; i < n; i++)
		{
            for (int j = 0; j < n; j++)
            {
                int rnd = Random.Range(0, posibles.Count);
                if (posibles[rnd] != 0)
                {
                    Casilla c = new Casilla(new Vector2(i,j));
                    tablero[i, j] = c;
                    c.getCube().transform.position = new Vector3(i * distancia, j * distancia, 0);
                    c.setNum(posibles[rnd]);
                }
                else
                {
                    hueco = new Vector2(i, j);
                    in_hueco = hueco;
                }
                posibles.Remove(posibles[rnd]);
            }
		}

        in_tablero = tablero;
    }

	public void reset(){
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (tablero[i, j] != null)
                {
                    Casilla temp = tablero[(int)tablero[i, j].getInPos().x, (int)tablero[i, j].getInPos().y];
                    tablero[(int)tablero[i, j].getInPos().x, (int)tablero[i, j].getInPos().y] = tablero[i, j];
                    tablero[i, j] = temp;

                    
                    tablero[i, j].getCube().transform.position = new Vector3(tablero[i, j].getInPos().x * distancia, tablero[i, j].getInPos().y * distancia, 0);
                }
            }
        }

        hueco = in_hueco;
    }

    public void swap(Vector2 toChange)
    {
        if (toChange.x < n && toChange.x >= 0 && toChange.y < n && toChange.y >= 0)
        {

            Casilla temp = tablero[(int)toChange.x, (int)toChange.y];
            tablero[(int)toChange.x, (int)toChange.y] = tablero[(int)hueco.x, (int)hueco.y];
            tablero[(int)hueco.x, (int)hueco.y] = temp;

            tablero[(int)hueco.x, (int)hueco.y].getCube().transform.position = new Vector3(hueco.x * distancia, hueco.y * distancia, 0);
            hueco = toChange;
        }
    }
    
    public bool win()
    {
        bool win = true;
        int i = 0;

        while (win && i < n)
        {
            int j = 0;
            while (win && j < n)
            {
                win = ((tablero[i, j].getactualCas().x*n+ tablero[i, j].getactualCas().y) == tablero[i, j].getNum());
                j++;
            }
            i++;
        }

        return win;
    }
}
