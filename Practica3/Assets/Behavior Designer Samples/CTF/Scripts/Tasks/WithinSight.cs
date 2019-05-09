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
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    // CURIOSO PORQUE YA TEN�AMOS OTRO WITHINSIGHT GEN�RICO...

    [TaskCategory("CTF")]
    [TaskDescription("Comprueba para ver si alg�n objeto dentro del array de objetivos est� a la vista")]
    public class WithinSight : Conditional
    {
        [Tooltip("El �ngulo del campo de visi�n del agente navegador (en grados)")]
        public float fieldOfViewAngle;
        [Tooltip("C�mo de lejos puede ver el agente")]
        public float viewMagnitude;
        [Tooltip("Devuelve �xito si un objeto del array se coloca en la l�nea de visi�n")]
        public Transform[] targets;
        [Tooltip("Devuelve cierto si este objeto se vuelve en la l�nea de visi�n")]
        public SharedTransform target;

        // magnitude * magnitude, calcular la ra�z cuadrada es caro cuando en realidad no hace falta
        private float sqrViewMagnitude;

        public override void OnAwake() {
            sqrViewMagnitude = viewMagnitude * viewMagnitude;
        }

        public override TaskStatus OnUpdate() {

            // Devuelve �xito si un objetivo est� al alcance de la vista
            for (int i = 0; i < targets.Length; ++i) {
                if (NPCViewUtilities.WithinSight(transform, targets[i], fieldOfViewAngle, sqrViewMagnitude)) {
                    // Establece el objetivo de manera que otras tareas sepan la transformada que est� a la vista
                    target.Value = targets[i];
                    return TaskStatus.Success;
                }
            }
            // Un objetivo no est� a la vista de modo que se devuelve fracaso
            return TaskStatus.Failure;
        }

        // Dibuja la representaci�n de la l�nea de visi�n dentro de la ventana de escena
        // ESTE C�DIGO DEBER�A QUIZ� SER S�LO PARA CUANDO SE EST� USANDO EL EDITOR DE UNITY
        public override void OnDrawGizmos() {
            NPCViewUtilities.DrawLineOfSight(Owner.transform, fieldOfViewAngle, sqrViewMagnitude);
        }
    }
}