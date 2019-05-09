/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del ejemplo Capture the Flag (CTF) de la herramienta Behavior Designer
*/
namespace BehaviorDesigner.Samples {

    using UnityEngine;
    using BehaviorDesigner.Runtime;

    public class NPC : MonoBehaviour {

        // Cierto si el NPC está en la ofensiva
        // ESTA CONSTRUCCIÓN ES MUY RARA Y DEBERÍA SIMPLIFICARSE COMO UNA SIMPLE PROPERTY
        [SerializeField]
        private bool isOffense;
        public bool IsOffense { get { return isOffense; } }

        // Cierto si el NPC actualmente tiene la bandera
        // ESTA CONSTRUCCIÓN ES MUY RARA Y DEBERÍA SIMPLIFICARSE COMO UNA SIMPLE PROPERTY
        private bool hasFlag = false;
        public bool HasFlag { get { return hasFlag; } set { hasFlag = value; } }

        // La posición y rotación inicial, que se recuerda para los reinicios
        private Vector3 startPosition;
        private Quaternion startRotation;

        private CTFGameManager gameManager;
        private Behavior[] behaviors;

        // Recuerda la posición y rotación inicial
        public void Awake() {
            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        public void Start() {
            // Se cachea el gestor del juego par acceder más rápido
            gameManager = CTFGameManager.instance;
            behaviors = GetComponents<Behavior>();
        }

        // Reinicia el NPC. Puede ocurrir por chocarse contra un enemigo, o por reiniciar el juego debido a que se ha capturado la bandera
        public void reset(bool fromCollision) {
            // Suelta la bandera si la lleva actualmente
            if (hasFlag) {
                // La bandera será el primer y único hijo que tenga, en este caso
                if (transform.childCount > 0) {
                    transform.GetChild(0).parent = null;
                    gameManager.flagDropped();
                }
            }

            // Reinicia las variables
            hasFlag = false;
            transform.position = startPosition;
            transform.rotation = startRotation;

            // Reinicia los comportamientos si el reinicio viene de un comportamiento.
            // Si el reinicio no viene de un comportamiento entonces no se reinicia, porque ya será todo reiniciado desde el propio gestor del juego. 
            if (fromCollision) {
                for (int i = 0; i < behaviors.Length; ++i) {
                    var enemy = behaviors[i].GetVariable("Enemy") as SharedTransform;
                    if (enemy != null)
                        enemy.Value = null;
                    if (behaviors[i].Group == gameManager.ActiveGroup) {
                        BehaviorManager.instance.RestartBehavior(behaviors[i]);
                        break;
                    }
                }
            }
        }

        // Reinicia el NPC si es ofensor y colisiona con un objeto defensivo
        public void OnCollisionEnter(Collision collision) {
            if (isOffense && gameManager.GameActive && (hasFlag || !gameManager.IsFlagTaken)) {
                NPC npc = null;
                if ((npc = collision.gameObject.GetComponent<NPC>()) != null) {
                    if (!npc.IsOffense) {
                        reset(true);
                    }
                }
            }
        }
    }
}