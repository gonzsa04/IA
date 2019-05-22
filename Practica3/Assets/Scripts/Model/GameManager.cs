using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Model
{
    public class GameManager : MonoBehaviour
    {
        //------------------------------PRIVATE------------------------------

        private bool pause;
        private int scoreA, scoreB, numChutsA, numChutsB, numSavesA, numSavesB;

        void Awake()
        {
            instance = this;    
        }

        // Use this for initialization
        void Start()
        {
            restart();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void updateUI()
        {
            score.text = scoreA.ToString() + " - " + scoreB.ToString();
            chutsA.text = "chutsA: " + numChutsA.ToString();
            chutsB.text = "chutsB: " + numChutsB.ToString();
            savesA.text = "savesA: " + numSavesA.ToString();
            savesB.text = "savesB: " + numSavesB.ToString();
        }

        void sendMessageAll(string Message)
        {
            Object[] objects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
            {
                go.SendMessage(Message, SendMessageOptions.DontRequireReceiver);
            }
        }

        void restartPositions()
        {
            sendMessageAll("resetTrans");
        }

        //------------------------------PUBLIC--------------------------------

        public static GameManager instance;

        public Text score;
        public Text chutsA;
        public Text chutsB;
        public Text savesA;
        public Text savesB;
        public GameObject pauseTextGO;

        public void restart()
        {
            scoreA = scoreB = numChutsA = numChutsB = numSavesA = numSavesB = 0;
            updateUI();
            restartPositions();
        }

        public void addScore(string team)
        {
            if (team == "A") scoreA++;
            else if (team == "B") scoreB++;
            restartPositions();
            updateUI();
        }

        public bool getPause()
        {
            return pause;
        }

        public void togglePause()
        {
            pause = !pause;
            pauseTextGO.SetActive(pause);
            sendMessageAll("toggleRB");
        }

        public void exit()
        {
            Application.Quit();
        }

    }
}
