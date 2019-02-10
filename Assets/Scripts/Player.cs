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
            GameManager.instance.swap(new Vector2(GameManager.instance.hueco.x - 1, GameManager.instance.hueco.y));
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            GameManager.instance.swap(new Vector2(GameManager.instance.hueco.x, GameManager.instance.hueco.y - 1));
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            GameManager.instance.swap(new Vector2(GameManager.instance.hueco.x + 1, GameManager.instance.hueco.y));
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
           GameManager.instance.swap(new Vector2(GameManager.instance.hueco.x, GameManager.instance.hueco.y + 1));
        }
    }
}
