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
    using BehaviorDesigner.Runtime.Tasks;
    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    // Comprueba para ver si cualquier objeto entre el array de objetivos está dentro de la distancia especificada
    [TaskCategory("Common")]
    public class WithinDistance : Conditional {

        [Tooltip("La distancia a la que el objeto objetivo necesita encontrarse")]
        public float magnitude;
        [Tooltip("Cierto si el objetivo necesita estar A LA VISTA para poder estar a la distancia adecuada. Si esto es cierto entonces un objeto tras un muro nunca estará a la distancia adecuada aunque " +
                 "pueda estar físicamente cerca del otro objeto")]
        public bool lineOfSight;
        [Tooltip("Un array de objetivos para comprobar")]
        public Transform[] targets;
        [Tooltip("Si los objetivos son nulos entonces rellenar la variable dinámicamente usando la etiqueta de objetivo (targetTag)")]
        public string targetTag;
        [Tooltip("La variable de objetivo compartido que se establecerá de manera que las otras tareas sepan que objetivo es")]
        public SharedTransform target;

        // Cierto si obtenemos los objetivos mediante la etiqueta de objetivo (targetTag)
        private bool dynamicTargets;
        // distance * distance, una optimización para que así no tengamos que calcular la raíz cuadrada
        private float sqrMagnitude;

        public override void OnAwake() {
            // Inicializa las variables
            sqrMagnitude = magnitude * magnitude;
            dynamicTargets = targets != null && targets.Length == 0;
        }

        public override void OnStart() {

            // Si los objetivos son nulos entonces encontrar todos los objetivos usando la etiqueta de objetivo (targetTag)
            if (targets == null || targets.Length == 0) {
                var gameObjects = GameObject.FindGameObjectsWithTag(targetTag);
                targets = new Transform[gameObjects.Length];
                for (int i = 0; i < gameObjects.Length; ++i) {
                    targets[i] = gameObjects[i].transform;
                }
            }
        }

        // Devuelve cierto (éxito, creo) si hay algún objeto a la distancia adecuada del objeto actual, en otro caso devuelve fallo
        public override TaskStatus OnUpdate() {

            Vector3 direction;
            // Comprueba cada objetivo. Todo lo que hace falta es un objetivo para ser capaces de devolver éxito
            for (int i = 0; i < targets.Length; ++i) {
                direction = targets[i].position - transform.position;
                // Comprueba para ver si la magnitud al cuadrado es menor que la especificada
                if (Vector3.SqrMagnitude(direction) < sqrMagnitude) {
                    // La magnitud es menr. Si hay línea de visión (lineOfSight) hacer una comprobación más
                    if (lineOfSight) {
                        if (NPCViewUtilities.LineOfSight(transform, targets[i], direction)) {
                            // El objetivo tiene una magnitud menor que la magnitud especificada y está a la vista. Establece el objetivo y devuelve éxito
                            target.Value = targets[i];
                            return TaskStatus.Success;
                        }
                    } else {
                        // El objetivo tiene una magnitud menor que la magnitud especificada. Establece el objetivo y devuelve éxito
                        target.Value = targets[i];
                        return TaskStatus.Success;
                    }
                }
            }
            // No hay objetivos a la distancia apropiada. Devuelve fallo
            return TaskStatus.Failure;
        }

        public override void OnEnd() {

            // Establece el array de objetivos a nulo si objetivos dinámicos (dynamic targets) está a cierto de modo que los objetivos se encontrarán otra vez la próxima vez que comience la tarea
            if (dynamicTargets) {
                targets = null;
            }
        }

        // Dibuja la distancia
        public override void OnDrawGizmos()
        {
// Sólo cuando estamos en el editor, claro
#if UNITY_EDITOR
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(Owner.transform.position, Owner.transform.up, magnitude);
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}