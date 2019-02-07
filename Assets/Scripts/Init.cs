using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour {
    List<Casilla> tablero = new List<Casilla>();
    public int distancia;
    public int n;
    
    void Start()
    {
        List<int> posibles = new List<int>();
        for (int i = 0; i < n * n; i++) posibles.Add(i);

        for (int i = 0; i < n * n; i++)
        {
            int rnd = Random.Range(0, posibles.Count);
            if (posibles[rnd] != 0)
            {
                Casilla c = new Casilla(i);
                tablero.Add(c);
                c.getCube().transform.position = new Vector3(i / n * distancia, -i % n * distancia, 0);
                c.setNum(posibles[rnd]);
            }
            posibles.Remove(posibles[rnd]);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
