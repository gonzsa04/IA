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

        private bool pause, reinit;     // juego pausado/reiniciado
        private IHaveTheBall ballOwner; // quien tiene la pelota actualmente
        private AudioSource kickSound;  // efecto de sonido -> feedback de chut

        // equipos
        private GameObject[] teamA;
        private GameObject[] teamB;

        void Awake()
        {
            instance = this;
            kickSound = GetComponent<AudioSource>();
        }

        // Use this for initialization
        void Start()
        {
            teamA = GameObject.FindGameObjectsWithTag("TeamA");
            teamB = GameObject.FindGameObjectsWithTag("TeamB");
            restart(); // inicia el juego
        }

        void LateUpdate()
        {
            reinit = false;
        }

        // manda un mensaje a todos los GameObjects de la escena
        // utilizado para decirles que se reseteen
        void sendMessageAll(string Message)
        {
            Object[] objects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
            {
                go.SendMessage(Message, SendMessageOptions.DontRequireReceiver);
            }
        }

        // manda un mensaje de resetear todas las posiciones
        void restartPositions()
        {
            sendMessageAll("resetTrans");
        }

        // muestra el texto de feedback cuando se marca un goal durante time segundos
        IEnumerator showGoalText(float time)
        {
            goalTextGo.SetActive(true);
            yield return new WaitForSecondsRealtime(time);
            goalTextGo.SetActive(false);
        }


        //------------------------------PUBLIC--------------------------------

        public static GameManager instance;

        // numeros de las metricas del partido
        [HideInInspector] public int scoreA, scoreB, numChutsA, numChutsB, numSavesA, numSavesB;

        public TEAM hasBall = TEAM.NONE; // equipo actual que tiene la pelota
        public float timeForGoal;        // tiempo durante el cual estara el texto de feedback

        // textos de las metricas del juego
        public Text score;
        public Text chutsA;
        public Text chutsB;
        public Text savesA;
        public Text savesB;

        // interfaz
        public GameObject goalTextGo;
        public GameObject pauseTextGO;

        // actualiza la interfaz
        public void updateUI()
        {
            score.text = scoreA.ToString() + " - " + scoreB.ToString();
            chutsA.text = "chutsA: " + numChutsA.ToString();
            chutsB.text = "chutsB: " + numChutsB.ToString();
            savesA.text = "savesA: " + numSavesA.ToString();
            savesB.text = "savesB: " + numSavesB.ToString();
        }

        // reinicia el juego -> interfaz + posiciones de los GameObjects
        public void restart()
        {
            reinit = true;
            scoreA = scoreB = numChutsA = numChutsB = numSavesA = numSavesB = 0;
            updateUI();
            restartPositions();
        }

        // suma un gol al equipo team
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
        public bool getReinit()
        {
            return reinit;
        }

        public void togglePause()
        {
            pause = !pause;
            pauseTextGO.SetActive(pause);
            sendMessageAll("toggleRB");
        }

        public void playKickSound()
        {
            kickSound.Play();
        }

        public void exit()
        {
            Application.Quit();
        }


        //-----------METODOS AUXILIARES PARA LAS CONDICIONES-------------

        // establece que jugador tiene la pelota
        public void setIHaveTheBall(IHaveTheBall ballOwner_)
        {
            if(ballOwner != null)ballOwner.setBool(false);
            ballOwner = ballOwner_;
            ballOwner.setBool(true);
        }

        // establece que nadie tiene la pelota
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

        // devuelve el jugador del equipo team que este mas cerca del target
        public GameObject getMinPlayerToTarget(TEAM team, Transform target)
        {
            float minDistance = 1000000000000;
            int min = 0;

            GameObject[] teamGOs = { };
            if (team == TEAM.A) teamGOs = teamA;
            else if (team == TEAM.B) teamGOs = teamB;

            for (int i = 0; i < teamGOs.Length; i++)
            {
                if (Vector3.Distance(teamGOs[i].gameObject.transform.position, target.position) < minDistance)
                {
                    minDistance = Vector3.Distance(teamGOs[i].gameObject.transform.position, target.position);
                    min = i;
                }
            }

            return teamGOs[min];
        }
    }
}
