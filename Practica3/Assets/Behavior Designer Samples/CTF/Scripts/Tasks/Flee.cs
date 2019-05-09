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
    // En versiones antiguas de Unity al parece hacía falta usar este otro espacio de nombres
#if !(UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)
    using UnityEngine.AI;
#endif
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("CTF")]
    [TaskDescription("Huya en la dirección opuesta al enemigo")]
    public class Flee : Action {

        [Tooltip("La velocidad del agente navegador")]
        public SharedFloat moveSpeed;
        [Tooltip("La rotación del agente navegador")]
        public SharedFloat rotationSpeed;
        [Tooltip("La huída tiene éxito cuando esta distancia con el enemigo ha sido alcanzada")]
        public float fleedDistance;
        [Tooltip("Cómo de lejos debemos mirar en la dirección opuesta")]
        public float lookAheadDistance;
        [Tooltip("Coje la transformada del enemigo del que estamos huyendo")]
        public SharedTransform fleeFromTransform;

        // La posición a la que huir
        private Vector3 targetPosition;

        // fleedDistance * fleedDistance, calcular la raíz cuadrada es caro cuando en realidad no hace falta
        private float sqrFleedDistance;

        private NavMeshAgent navMeshAgent;

        public override void OnAwake() {
            // Cachear para acceder más rápido
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

            // Establece la velocidad linear y angular
            navMeshAgent.speed = moveSpeed.Value;
            navMeshAgent.angularSpeed = rotationSpeed.Value;
            sqrFleedDistance = fleedDistance * fleedDistance;
        }

        public override void OnStart() {

            // Si la transformada es nula entonces no tenemos a nadie de quien huir
            if (fleeFromTransform.Value == null)
                return;

            // Huye en la dirección opuesta
            targetPosition = OppositeDirection();
            navMeshAgent.enabled = true;
            navMeshAgent.destination = targetPosition;
        }

        public override TaskStatus OnUpdate() {

            // La huida falla si la transformada ya no existe
            if (fleeFromTransform.Value == null) {
                return TaskStatus.Failure;
            }

            // La huida sólo tiene una posibilidad de tener éxito si la ruta no está pendiente
            if (!navMeshAgent.pathPending) {
                // Consigue nuestra posición, ignorando a la 'y'
                var thisPosition = transform.position;
                thisPosition.y = navMeshAgent.destination.y;

                // La huida fue un éxito si estamos ya lejos del enemigo
                if (Vector3.SqrMagnitude(thisPosition - fleeFromTransform.Value.position) > sqrFleedDistance) {
                    return TaskStatus.Success;
                    // Huye a una nueva posición en la dirección opuesta si hemos llegado a nuestro destino de huida
                } else if (Vector3.SqrMagnitude(thisPosition - navMeshAgent.destination) < SampleConstants.ArriveMagnitude) {
                    targetPosition = OppositeDirection();
                    navMeshAgent.destination = targetPosition;
                }
            }

            return TaskStatus.Running;
        }

        public override void OnEnd() {
            navMeshAgent.enabled = false;
        }

        private Vector3 OppositeDirection() {
            // Calcula un valor en la dirección opuesta al enemigo
            return transform.position + (transform.position - fleeFromTransform.Value.position).normalized * lookAheadDistance;
        }
    }
}