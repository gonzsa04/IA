using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// indica si el jugador tiene la pelota en este momento
public class IHaveTheBall : MonoBehaviour {
    private bool iHaveTheBall = false;

    public bool getBool() { return iHaveTheBall; }
    public void setBool(bool b) { iHaveTheBall = b; }
}
