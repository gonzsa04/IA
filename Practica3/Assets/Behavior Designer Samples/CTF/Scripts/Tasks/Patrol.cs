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
    [TaskDescription("Patrulla el agente navegador a lo largo de los puntos de ruta. Devolverá éxito si un objetivo se pone a la vista.")]
    public class Patrol : Action {
        [Tooltip("La velocidad del agente navegador")]
        public SharedFloat moveSpeed;
        [Tooltip("La velocidad de rotación del agente navegador")] 
        public SharedFloat rotationSpeed;
        [Tooltip("El ángulo del campo de vista del agente navegador (en grados)")] 
        public float fieldOfViewAngle;
        [Tooltip("Cómo de lejos puede ver el agente")] 
        public float viewMagnitude;
        [Tooltip("Los puntos de ruta de la patrulla")] 
        public Transform[] waypoints;
        [Tooltip("Devuelve éxito si uno de estos objetivos se pone a la vista")] 
        public Transform[] targets;
        [Tooltip("La transformada del objeto que hemos encontrado mientras buscábamos")]
        public SharedTransform target;

        // El índice actual del punto de ruta al que nos estamos dirigiendo actualmente dentro del array de puntos de ruta
        private int waypointIndex;
        // magnitude * magnitude, calcular la raíz cuadrada es muy caro cuando en realidad no hace falta
        private float sqrViewMagnitude;

        private NavMeshAgent navMeshAgent;

        public override void OnAwake() {
            // Cachear para acceder más rápido
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

            // Establece la velocidad linear y angular
            navMeshAgent.speed = moveSpeed.Value;
            navMeshAgent.angularSpeed = rotationSpeed.Value;

            // Inicialmente moverse hacia el punto de ruta más cercano
            float distance = Mathf.Infinity;
            float localDistance;
            for (int i = 0; i < waypoints.Length; ++i) {
                if ((localDistance = Vector3.Magnitude(transform.position - waypoints[i].position)) < distance) {
                    distance = localDistance;
                    waypointIndex = i;
                }
            }

            sqrViewMagnitude = viewMagnitude * viewMagnitude;
        }

        public override void OnStart() {
            navMeshAgent.enabled = true;
            navMeshAgent.destination = waypoints[waypointIndex].position;
        }

        public override TaskStatus OnUpdate() {

            // Éxito si el objetivo está a la vista
            for (int i = 0; i < targets.Length; ++i) {
                if (NPCViewUtilities.WithinSight(transform, targets[i], fieldOfViewAngle, sqrViewMagnitude)) {
                    // Establece el objetivo de modo que la siguiente tarea sepa que transformada debe poner como objetivo
                    target.Value = targets[i];
                    return TaskStatus.Success;
                }
            }

            // Sólo podemos llevar al siguiente punto de ruta si la ruta no está pendiente
            if (!navMeshAgent.pathPending) {
                var thisPosition = transform.position;
                thisPosition.y = navMeshAgent.destination.y; // Ignora 'y'
                if (Vector3.SqrMagnitude(thisPosition - navMeshAgent.destination) < SampleConstants.ArriveMagnitude) {
                    // Cicla a través de todos los puntos de ruta
                    waypointIndex = (waypointIndex + 1) % waypoints.Length;
                    navMeshAgent.destination = waypoints[waypointIndex].position;
                }
            }

            // Si no hay objetivo a la vista entonces sigue patrullando
            return TaskStatus.Running;
        }

        public override void OnEnd() {
            // Desactiva al agente navegador (curioso que se haga esto de desactivarlo)
            navMeshAgent.enabled = false;
        }

        public override void OnReset() {
            moveSpeed.Value = 0;
            rotationSpeed.Value = 0;
            fieldOfViewAngle = 0;
            viewMagnitude = 0;
            waypoints = null;
            targets = null;
        }

        // Dibuja la representación de la línea de visión dentro de la ventana de escena
        // ESTE CÓDIGO DEBERÍA QUIZÁ SER SÓLO PARA CUANDO SE ESTÁ USANDO EL EDITOR DE UNITY
        public override void OnDrawGizmos()
        {
            NPCViewUtilities.DrawLineOfSight(Owner.transform, fieldOfViewAngle, viewMagnitude);
        }
    }
}