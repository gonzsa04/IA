using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Model
{
    public enum TEAM { NONE, A, B } // existing teams

    public class GameManager : MonoBehaviour
    {
        //------------------------------PRIVATE------------------------------

        private bool pause;
        private IHaveTheBall ballOwner;
        private AudioSource kickSound;

        void Awake()
        {
            instance = this;    
        }

        // Use this for initialization
        void Start()
        {
            restart();
            kickSound = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

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

        IEnumerator showGoalText(float time)
        {
            goalTextGo.SetActive(true);
            yield return new WaitForSecondsRealtime(time);
            goalTextGo.SetActive(false);
        }

        //------------------------------PUBLIC--------------------------------

        public static GameManager instance;

        [HideInInspector] public int scoreA, scoreB, numChutsA, numChutsB, numSavesA, numSavesB;

        public TEAM hasBall = TEAM.NONE;
        public float timeForGoal;
        public Text score;
        public Text chutsA;
        public Text chutsB;
        public Text savesA;
        public Text savesB;
        public GameObject goalTextGo;
        public GameObject pauseTextGO;

        public void updateUI()
        {
            score.text = scoreA.ToString() + " - " + scoreB.ToString();
            chutsA.text = "chutsA: " + numChutsA.ToString();
            chutsB.text = "chutsB: " + numChutsB.ToString();
            savesA.text = "savesA: " + numSavesA.ToString();
            savesB.text = "savesB: " + numSavesB.ToString();
        }

        public void restart()
        {
            scoreA = scoreB = numChutsA = numChutsB = numSavesA = numSavesB = 0;
            updateUI();
            restartPositions();
        }

        public void addScore(string team)
        {
            StartCoroutine(showGoalText(timeForGoal));

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

        public void setIHaveTheBall(IHaveTheBall ballOwner_)
        {
            if(ballOwner != null)ballOwner.setBool(false);
            ballOwner = ballOwner_;
            ballOwner.setBool(true);
        }

        public void clearBall()
        {
            if (ballOwner != null)
            {
                if (hasBall == TEAM.A) numChutsA++;
                else if (hasBall == TEAM.B) numChutsB++;
                updateUI();
                ballOwner.setBool(false);
                hasBall = TEAM.NONE;
            }
        }

        public void playKickSound()
        {
            kickSound.Play();
        }

        public void exit()
        {
            Application.Quit();
        }

    }
}
