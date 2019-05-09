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

    // Moverse hacia el objetivo especificado utilizando la malla de navegación de Unity
    [TaskCategory("Common")]
    public class Seek : Action {

        [Tooltip("La velocidad del agente navegador")]
        public SharedFloat moveSpeed;
        [Tooltip("La rotación del agente navegador")]
        public SharedFloat rotationSpeed;
        [Tooltip("Cierto si la rotación del agente navegador debe terminar con la misma rotación que el objetivo")]
        public bool rotateToTarget;
        [Tooltip("Evita correr hacia objetos que están en modo de defensa. Nota: Esto debería hacerse con etiquetas pero para evitar tener que actualizar los ficheros de proyecto " +
                 "en estas demos lo han decidido hacer así")]
        public bool avoidDefeneUnits;
        [Tooltip("La posición que el agente está buscando")]
        public SharedVector3 targetPosition;
        [Tooltip("El objetivo que el agente está buscando")]
        public SharedTransform target;

        // Recuerda la magnitud desde el fotograma anterior de manera que sabemos si el objetivo se ha regenerado y ya no tenemos que seguir buscando el objetivo
        private float prevMagnitude = Mathf.Infinity;
        // Cierto si el objetivo ha sido obtenido de la posición de los objetivos
        private bool staticPosition = false;
        // Cierto si el agente navegador está actualmente siguiendo una ruta alternativa para evitar el objeto defensor
        private bool alternatePath = false;

        private NavMeshAgent navMeshAgent;

        public override void OnAwake() {
            // Se cachea para tener un acceso rápido
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

            // Establece la velocidad lineal y angular
            navMeshAgent.speed = moveSpeed.Value;
            navMeshAgent.angularSpeed = rotationSpeed.Value;
        }

        public override void OnStart() {
            navMeshAgent.enabled = true;

            // Usa la posición si no es cero
            if (targetPosition.Value != Vector3.zero) {
                staticPosition = true;
                navMeshAgent.destination = targetPosition.Value;
            }

            // Establece el destino si no ha sido previamente establecido con una posición estática
            if (staticPosition == false) {
                navMeshAgent.destination = target.Value.position;
            }
        }

        // Se mueve hacia el destino. Devuelve éxito cuando hemos alcanzado el destino, fallo si el destino se ha regenerado y ya no debemos buscarlo.
        // Devolverá 'ejecutándose' (running) si todavía estamos buscando
        public override TaskStatus OnUpdate() {

            // Usa la posición de destino del agente navegador si estamos en una ruta alternativa o si el objetivo es nulo. 
            // Estamos usando una ruta alternativa si la ruta anterior habría colisionado con un objeto defensor.
            // El destino será nulo cuando estamos buscando una posición especificada por la variable position.
            var targetPosition = (alternatePath || target.Value == null ? navMeshAgent.destination : target.Value.position);
            targetPosition.y = navMeshAgent.destination.y; // Ignora la 'y'

            // Sólo podemos llegar si la ruta no está pendiente
            if (!navMeshAgent.pathPending) {
                var thisPosition = transform.position;
                thisPosition.y = targetPosition.y;
                // Si la magnitud es menor que la magnitud de llegada entonces hemos llegado al destino
                if (Vector3.SqrMagnitude(thisPosition - navMeshAgent.destination) < SampleConstants.ArriveMagnitude) {
                    // Si hemos llegado desde una ruta alternativa entonces hay que cambiar de nuevo a la ruta regular
                    if (alternatePath) {
                        alternatePath = false;
                        targetPosition = target.Value.position;
                    } else {
                        // Devuelve cierto si no necesitamos rotar el objetivo o si ya estamos rotados como dice la rotación del objetivo
                        if (!rotateToTarget || transform.rotation == target.Value.rotation) {
                            return TaskStatus.Success;
                        }
                        // No se ha hecho todavía... aún hace falta rotar
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.Value.rotation, rotationSpeed.Value * Time.deltaTime);
                    }
                }

                // Falla si el objetivo se mueve demasiado deprisa en un sólo fotograma. Esto ocurre cuando el objetivo ha sido capturado y regenerado en otra parte
                float distance;
                if (prevMagnitude * 2 < (distance = Vector3.SqrMagnitude(thisPosition - targetPosition))) {
                    return TaskStatus.Failure;
                }
                prevMagnitude = distance;
            }

            // Intenta no dirigirse directamente hacia un objeto defensor
            RaycastHit hit;
            Vector3 hitPoint;
            if (avoidDefeneUnits && (rayCollision(transform.position - transform.right, targetPosition, out hit) ||
                                                rayCollision(transform.position + transform.right, targetPosition, out hit))) {
                // Parece que un objeto está en medio de la ruta. Evita el objeto estableciendo un nuevo destino hacia la DERECHA del objeto con el que podríamos haber impactado
                hitPoint = hit.point + transform.right * 5;
                hitPoint.y = transform.position.y;
                navMeshAgent.destination = hitPoint;
                // El objeto a evitar podría estar todavía en nuestro camino incluso si nos hemos movido a la derecha. En este caso moverse a la IZQUIERDA y esperemos que funcione...
                if (rayCollision(transform.position, navMeshAgent.destination, out hit)) {
                    hitPoint = hit.point - transform.right * 5;
                    hitPoint.y = transform.position.y;
                    navMeshAgent.destination = hitPoint;
                }

                // Recuerda el hecho de que estamos tomando una ruta alternativa para así prevenir que el agente comience a ir adelante y atrás de manera intermitente, como loco (jittering back and forth)
                alternatePath = true;
                var thisPosition = transform.position;
                thisPosition.y = hitPoint.y;
                prevMagnitude = Vector3.SqrMagnitude(thisPosition - hitPoint);
            } else if (navMeshAgent.destination != targetPosition) {
                // La posición de destino ha cambiado desde que establecimos el destino por última vez. Actualiza el destino
                navMeshAgent.destination = targetPosition; // PARECE QUE FALLA AQUÍ... (A LO MEJOR TIENE QUE VER CON EL PROBLEMA DE COGER LA BANDERA)
            }

            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            // Reinicia las variables y desactiva el agente navegador cuando la tarea termina
            alternatePath = false;
            prevMagnitude = Mathf.Infinity;

            navMeshAgent.enabled = false; // Curioso que opten por desactivarlo, directamente
        }

        // Lanza un rayo entre la posición inicial y la de destino. Devuelve cierto si el rayo colisiona con un objeto defensor
        private bool rayCollision(Vector3 startPosition, Vector3 targetPosition, out RaycastHit hit)
        {
            if (avoidDefeneUnits && Physics.Raycast(startPosition, targetPosition - startPosition, out hit, Mathf.Infinity)) {
                NPC npc = null;
                if ((npc = hit.collider.GetComponent<NPC>()) != null) {
                    return !npc.IsOffense;
                }
            }
            hit = new RaycastHit();
            return false;
        }
    }
}