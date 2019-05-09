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
// En versiones antiguas de Unity al parece hac�a falta usar este otro espacio de nombres
#if !(UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)
    using UnityEngine.AI;
#endif
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("CTF")]
    [TaskDescription("Defiende el objeto especificado desde el objetivo.")]
    public class Defend : Action {
        [Tooltip("La velocidad del agente navegador")]
        public SharedFloat moveSpeed;
        [Tooltip("La rotaci�n del agente navegador")]
        public SharedFloat rotationSpeed;

        // La transformada del objeto a defender
        public Transform defendObject;
        // Defender dentro de este radio especificado por defendObject
        public float defendRadius;

        // Defender del objeto especificado
        public SharedTransform target;

        // Recuerda la magnitud del fotograma anterior de manera que sabremos si el objetivo se ha regenerado y ya no nos hace falta buscar el objetivo
        private float prevMagnitude = Mathf.Infinity;
        // defendRadius * defendRadius, calcular la ra�z cuadrada es cara cuando en realidad no hace falta hacerla
        private float sqrDefendRadius;

        private NavMeshAgent navMeshAgent;

        public override void OnAwake() {
            // Cachear para acceder m�s r�pido
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

            // Establece la velocidad linear y angular
            navMeshAgent.speed = moveSpeed.Value;
            navMeshAgent.angularSpeed = rotationSpeed.Value;

            sqrDefendRadius = defendRadius * defendRadius;
        }

        public override void OnStart() {
            // Establece el destino de la posici�n del objetivo
            navMeshAgent.enabled = true;
            var targetPosition = target.Value.position;
            targetPosition.y = navMeshAgent.destination.y; // Ignora 'y'
            if (targetPosition != navMeshAgent.destination) {
                navMeshAgent.destination = targetPosition;
            }
        }

        public override TaskStatus OnUpdate() {
            // S�lo podemos alcanzar el objetivo si la ruta no est� pendiente
            if (!navMeshAgent.pathPending) {
                var thisPosition = transform.position;
                thisPosition.y = navMeshAgent.destination.y; // Ignora 'y'
                float sqrMgnitude = Vector3.SqrMagnitude(thisPosition - navMeshAgent.destination);
                // Devuelve fracaso si estamoa fuera de nuestro �rea para defender
                if (sqrMgnitude > sqrDefendRadius) {
                    return TaskStatus.Failure;
                } else if (sqrMgnitude < SampleConstants.ArriveMagnitude) { // Devuelve �xito si hemos alcanzado nuestro objetivo
                    return TaskStatus.Success;
                }

                // Fracaso si el objetivo se movi� demasiado deprisa en un s�lo fotograma. Esto ocurre cuando el objetivo ha sido atrapado y se regenera en alg�n otro lugar
                if (prevMagnitude * 2 < sqrMgnitude) {
                    return TaskStatus.Failure;
                }
                prevMagnitude = sqrMgnitude;
            }

            // Establece un nuevo destino si el objetivo se ha movido
            var targetPosition = target.Value.position;
            targetPosition.y = navMeshAgent.destination.y; // Ignora 'y'
            if (targetPosition != navMeshAgent.destination) {
                navMeshAgent.destination = targetPosition;
            }

            // Se mantiene 'ejecut�ndose' (running) mientras que permanezcamos en el radio de defensa y no hayamos cogido al objetivo todav�a
            return TaskStatus.Running;
        }

        public override void OnEnd() {
            // Reinicia las variables
            prevMagnitude = Mathf.Infinity;
            navMeshAgent.enabled = false;
        }

        // Dibuja el �rea que estamos defendiendo dentro de la ventana del editor de escenas
        public override void OnDrawGizmos()
        {
// S�lo en el editor, claro
#if UNITY_EDITOR
            if (defendObject != null) {
                var oldColor = UnityEditor.Handles.color;
                UnityEditor.Handles.color = new Color(1, 1, 0, 0.3f);
                UnityEditor.Handles.DrawWireDisc(defendObject.position, defendObject.up, defendRadius);
                UnityEditor.Handles.color = oldColor;
            }
#endif
        }
    }
}