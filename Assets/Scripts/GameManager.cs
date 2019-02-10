using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	//GM
	public static GameManager instance;
	public int distancia;
	public int n;
	[HideInInspector] public Casilla[,] tablero;
    [HideInInspector] public int hueco;
    int in_hueco;
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
                    hueco = i;
                    in_hueco = i;
                }
                posibles.Remove(posibles[rnd]);
            }
		}

	}

	public void reset(){
        print("la puta de jorge");
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                tablero[i, j].getCube().transform.position = new Vector3(tablero[i,j].getInPos().x * distancia, tablero[i, j].getInPos().y * distancia, 0);
            }
        }
        hueco = in_hueco;
    }

    public void swap(int toChange)
    {
        if (toChange <= n * n && toChange >= 0)
        {
            bool found = false;
            int i = 0;

            /*while (!found && i < tablero.Count)
            {
                found = tablero[i].getactualCas() == toChange;
                i++;
            }

            print(i - 1);
            if (found)
            {
                tablero[i - 1].getCube().transform.position = new Vector3(hueco / n * distancia, -hueco % n * distancia, 0);
                hueco = toChange;
            }*/
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
