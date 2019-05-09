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

    // CURIOSO PORQUE YA TENÍAMOS OTRO WITHINSIGHT GENÉRICO...

    [TaskCategory("CTF")]
    [TaskDescription("Comprueba para ver si algún objeto dentro del array de objetivos está a la vista")]
    public class WithinSight : Conditional
    {
        [Tooltip("El ángulo del campo de visión del agente navegador (en grados)")]
        public float fieldOfViewAngle;
        [Tooltip("Cómo de lejos puede ver el agente")]
        public float viewMagnitude;
        [Tooltip("Devuelve éxito si un objeto del array se coloca en la línea de visión")]
        public Transform[] targets;
        [Tooltip("Devuelve cierto si este objeto se vuelve en la línea de visión")]
        public SharedTransform target;

        // magnitude * magnitude, calcular la raíz cuadrada es caro cuando en realidad no hace falta
        private float sqrViewMagnitude;

        public override void OnAwake() {
            sqrViewMagnitude = viewMagnitude * viewMagnitude;
        }

        public override TaskStatus OnUpdate() {

            // Devuelve éxito si un objetivo está al alcance de la vista
            for (int i = 0; i < targets.Length; ++i) {
                if (NPCViewUtilities.WithinSight(transform, targets[i], fieldOfViewAngle, sqrViewMagnitude)) {
                    // Establece el objetivo de manera que otras tareas sepan la transformada que está a la vista
                    target.Value = targets[i];
                    return TaskStatus.Success;
                }
            }
            // Un objetivo no está a la vista de modo que se devuelve fracaso
            return TaskStatus.Failure;
        }

        // Dibuja la representación de la línea de visión dentro de la ventana de escena
        // ESTE CÓDIGO DEBERÍA QUIZÁ SER SÓLO PARA CUANDO SE ESTÁ USANDO EL EDITOR DE UNITY
        public override void OnDrawGizmos() {
            NPCViewUtilities.DrawLineOfSight(Owner.transform, fieldOfViewAngle, sqrViewMagnitude);
        }
    }
}