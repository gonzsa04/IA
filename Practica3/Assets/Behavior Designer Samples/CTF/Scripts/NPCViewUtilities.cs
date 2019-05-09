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

    // Una clase estática que contiene funciones comunes utilizadas por múltiples otras clases
    public static class NPCViewUtilities {

        // Devuelve cierto si targetTransform está a la vista de transform
        public static bool WithinSight(Transform transform, Transform targetTransform, float fieldOfViewAngle, float sqrViewMagnitude) {

            Vector3 direction = targetTransform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            // Un objeto está a la vista si está dentro del campo de vista (field of view) y tiene una magnitud menor de la que el objeto es capaz de ver
            if (angle < fieldOfViewAngle && Vector3.SqrMagnitude(direction) < sqrViewMagnitude) {
                RaycastHit hit;
                // Para estar a la vista no puede haber objetos obstruyendo la vista
                if (Physics.Raycast(transform.position, direction.normalized, out hit)) {
                    // Si el objeto con el que colisiona es el objeto objetivo entonces no hay otros objetos obstruyendo la vista
                    if (hit.transform.Equals(targetTransform)) {
                        return true;
                    }
                }
            }

            return false;
        }

        // Devuelve cierto si targetTransform está en la línea de visión de transform
        public static bool LineOfSight(Transform transform, Transform targetTransform, Vector3 direction)
        {
            RaycastHit hit;
            // Lanza un rayo. Si este colisiona con targetTransform entonces no hay otros objetos obstruyendo la vista
            if (Physics.Raycast(transform.position, direction.normalized, out hit)) {
                if (hit.transform.Equals(targetTransform)) {
                    return true;
                }
            }
            return false;
        }

        // Ayuda a visualizar la línea de visión desde el editor
        public static void DrawLineOfSight(Transform transform, float fieldOfViewAngle, float viewMagnitude) {
// Sólo desde el editor, claro
#if UNITY_EDITOR
            float radius = viewMagnitude * Mathf.Sin(fieldOfViewAngle * Mathf.Deg2Rad);
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            // Dibuja un disco al final de la distancia de visión
            UnityEditor.Handles.DrawWireDisc(transform.position + transform.forward * viewMagnitude, transform.forward, radius);
            // Dibuja líneas para representar el lado izquierdo y derecho de la línea de visión
            UnityEditor.Handles.DrawLine(transform.position, transform.TransformPoint(new Vector3(radius, 0, viewMagnitude)));
            UnityEditor.Handles.DrawLine(transform.position, transform.TransformPoint(new Vector3(-radius, 0, viewMagnitude)));
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}