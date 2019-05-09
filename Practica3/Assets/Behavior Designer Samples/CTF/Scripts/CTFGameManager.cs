/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del ejemplo Capture the Flag (CTF) de la herramienta Behavior Designer
*/
namespace BehaviorDesigner.Samples {

    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using BehaviorDesigner.Runtime;

    // El gestor del juego (game manager) que se encarga de llevar el control de las normas del Captura la Bandera
    public class CTFGameManager : MonoBehaviour {

        // Sigue el patr�n Ejemplar �nico (Singleton)
        public static CTFGameManager instance;

        // El tiempo de celebraci�n de los NPCs ofensores tras haber capturado la bandera y antes de que el juego se reinicie
        public float celebrationTime = 2.0f;
        // La transformada que es padre de todos los NPCs
        public Transform NPCGroup;
        // La transformada de la bandera
        public Transform flag;

        // Cierto si el juego est� actualmente activo
        // ESTA CONSTRUCCI�N ES MUY RARA Y DEBER�A SIMPLIFICARSE COMO UNA SIMPLE PROPERTY
        private bool gameActive = false;
        public bool GameActive { get { return gameActive; } }

        // Los comportamiento se agrupan en dos conjuntos. 
        // El primero contiene todos los comportamientos que deben ejecutarse mientras la bandera no haya sido cogida.
        // Cuando se coge la bandera, se ejecuta el segundo conjunto.
        // Por ejemplo, cuando la bandera no ha sido cogida los NPCs ofensores deber�n buscarla y evitar cualquier enemigo.
        // Sin embargo cuando la bandera est� cogida deben llevarla al punto de captura. 
        // Esto podr�a haberse hecho comprobando esta condici�n DENTRO del propio comportamiento, y habr�a sido otra forma de resolverlo. 
        // ESTA CONSTRUCCI�N ES MUY RARA Y DEBER�A SIMPLIFICARSE COMO UNA SIMPLE PROPERTY
        private int activeGroup = -1;
        public int ActiveGroup { get { return activeGroup; } }

        // Cierto si la bandera la tiene una unidad ofensora
        // ESTA CONSTRUCCI�N ES MUY RARA Y DEBER�A SIMPLIFICARSE COMO UNA SIMPLE PROPERTY
        private bool isFlagTaken = false;
        public bool IsFlagTaken { get { return isFlagTaken; } }

        // La posici�n y rotaci�n inicial de la bandera, que se utiliza cada vez que el juego se reinicia
        private Vector3 startFlagPosition;
        private Quaternion startFlagRotation;

        // Una lista con todas las unidades (o NPCs)
        private List<NPC> NPCs = new List<NPC>();

        // Mantiene una lista de todos los comportamientos de cuando se tiene (o no se tiene) la bandera para poder activarlos/desactivarlos
        private List<Behavior> flagNotTakenBehaviors = new List<Behavior>();
        private List<Behavior> flagTakenBehaviors = new List<Behavior>();

        // El gestor de comportamientos de Behavior Designer
        private BehaviorManager behaviorManager;

        // Patr�n Singleton
        public void Awake() {
            instance = this;
        }

        public void Start() {
            // Se cachea este gestor para acceder a �l m�s r�pido luego
            behaviorManager = BehaviorManager.instance;

            // Establece la velocidad del juego (suele ser recomendado ponerla al doble o as�)
            Time.timeScale = 2;

            // Recuerda la posici�n y rotaci�n iniciales de la bandera
            startFlagPosition = flag.position;
            startFlagRotation = flag.rotation;

            // Encuentra todos los NPCs
            for (int i = 0; i < NPCGroup.childCount; ++i) {
                NPCs.Add(NPCGroup.GetChild(i).GetComponent<NPC>());
            }

            // Agrupa los comportamientos como se ha dicho anteriormente
            var allBehaviors = FindObjectsOfType(typeof(Behavior)) as Behavior[];
            for (int i = 0; i < allBehaviors.Length; ++i) {
                if (allBehaviors[i].Group == 0) { // 0 indica los comportamientos de cuando NO se tiene la bandera
                    flagNotTakenBehaviors.Add(allBehaviors[i]);
                } else { // 1 indica los comportamientos de cuando S� se tiene la bandera
                    flagTakenBehaviors.Add(allBehaviors[i]);
                }
            }

            // Inicialmente la bandera no ha sido cogida y el juego est� activo
            activeGroup = 0;
            gameActive = true;
        }

        // La bandera ha sido cogida, con lo que se desactivan los comportamientos de cuando NO est� cogida y se activan los otros
        public bool flagTaken() {

            // �Devuelve falso r�pidamente si comprobamos que en realidad la bandera ya estaba cogida de antes!
            if (isFlagTaken) {
                return false;
            }

            // Desactiva los comportamientos de cuando la bandera NO est� cogida
            for (int i = 0; i < flagNotTakenBehaviors.Count; ++i) {
                if (behaviorManager.IsBehaviorEnabled(flagNotTakenBehaviors[i])) {
                    flagNotTakenBehaviors[i].DisableBehavior();
                }
            }

            // Activa los comportamientos de cuando la bandera est� cogida
            for (int i = 0; i < flagTakenBehaviors.Count; ++i) {
                flagTakenBehaviors[i].EnableBehavior();
            }

            // La bandera se marca como cogida
            isFlagTaken = true;
            return true;
        }

        // La bandera ha sido soltada, con lo que se desactivan los comportamientos de cuando S� est� cogida y se activan los otros 
        public void flagDropped() {

            // �Devuelve falso r�pidamente si comprobamos que en realidad la bandera NO estaba cogida antes!
            if (!isFlagTaken) {
                return;
            }

            // Desactiva los comportamientos de cuando la bandera S� est� cogida
            for (int i = 0; i < flagTakenBehaviors.Count; ++i) {
                if (behaviorManager.IsBehaviorEnabled(flagTakenBehaviors[i])) {
                    flagTakenBehaviors[i].DisableBehavior();
                }
            }

            // Activa los comportamientos de cuando la bandera NO est� cogida
            for (int i = 0; i < flagNotTakenBehaviors.Count; ++i) {
                flagNotTakenBehaviors[i].EnableBehavior();
            }

            // La bandera se marca como NO cogida
            isFlagTaken = false;
        }

        // La bandera ha sido capturada, se reinicia el juego
        public void resetGame() {
            if (gameActive) {
                StartCoroutine(doReset());
                gameActive = false;
            }
        }

        public IEnumerator doReset() {
            yield return new WaitForSeconds(celebrationTime);

            // Detiene todos los comportamientos
            for (int i = 0; i < flagTakenBehaviors.Count; ++i) {
                if (behaviorManager.IsBehaviorEnabled(flagTakenBehaviors[i])) {
                    flagTakenBehaviors[i].DisableBehavior();
                }
            }
            for (int i = 0; i < flagNotTakenBehaviors.Count; ++i) {
                if (behaviorManager.IsBehaviorEnabled(flagNotTakenBehaviors[i])) {
                    flagNotTakenBehaviors[i].DisableBehavior();
                }
            }

            // Reinicia la posici�n y rotaci�n de la bandera
            flag.parent = null;
            flag.position = startFlagPosition;
            flag.rotation = startFlagRotation;

            // Reinicia la posici�n y rotaci�n de los NPCs
            for (int i = 0; i < NPCs.Count; ++i) {
                NPCs[i].reset(false);
            }

            // Arranca los comportamientos
            for (int i = 0; i < flagNotTakenBehaviors.Count; ++i) {
                flagNotTakenBehaviors[i].EnableBehavior();
            }

            // Reinicia las variables
            isFlagTaken = false;
            gameActive = true;
        }
    }
}