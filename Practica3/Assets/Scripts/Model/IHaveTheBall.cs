using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHaveTheBall : MonoBehaviour {
    private bool iHaveTheBall = false;

    public bool getBool() { return iHaveTheBall; }
    public void setBool(bool b) { iHaveTheBall = b; }
}
