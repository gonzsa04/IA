using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase que representa cada casilla deslizable del puzle
public class Casilla
{
    private int numCas;      // numero de la casilla que es
    private int actualCas;   // posicion atual en la que se encuentra
    private GameObject cube; // representa la casilla
    private GameObject text; // escribe el numero de la casilla sobre el cubo

    private void addText()
    {
        text = new GameObject();
        text.transform.SetParent(cube.transform);
        text.AddComponent<TextMesh>();

        TextMesh textMeshComp = text.GetComponent<TextMesh>();
        textMeshComp.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        textMeshComp.fontSize = 50;
        textMeshComp.characterSize = 0.1f;
        text.transform.position -= new Vector3(0.13f, -0.3f, 0f);
    }

    public Casilla(int num)
    {
        actualCas = num; // crea una casilla con texto, estableciendo su posicion actual
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        addText();
    }

    public int getNum() { return numCas; }
    public void setNum(int n) { numCas = n; text.GetComponent<TextMesh>().text = numCas.ToString(); }
    public GameObject getCube() { return cube; }
    public int getactualCas() { return actualCas; }
    public void setActualNum(int n) { actualCas = n; }
}

