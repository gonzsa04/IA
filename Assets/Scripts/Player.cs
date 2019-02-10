using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        handleEvent();
	}

    void handleEvent()
    {
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            print(GameManager.instance.hueco - GameManager.instance.n);
            GameManager.instance.swap(GameManager.instance.hueco - GameManager.instance.n);
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            if ((GameManager.instance.hueco - 1) % GameManager.instance.n != 0)
                GameManager.instance.swap(GameManager.instance.hueco - 1);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            print(GameManager.instance.hueco + GameManager.instance.n);
            GameManager.instance.swap(GameManager.instance.hueco + GameManager.instance.n);
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            if((GameManager.instance.hueco + 1) % GameManager.instance.n != 2)
                GameManager.instance.swap(GameManager.instance.hueco + 1);
        }
    }
}
