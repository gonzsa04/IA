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
    using BehaviorDesigner.Runtime.Tasks;
    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("CTF")]
    [TaskDescription("La bandera ha sido capturada. Se celebra rotando en círculo.")]
    public class Celebrate : Action {
        [Tooltip("La velocidad de rotación")]
        public float rotationSpeed;

        // Siempre devolverá 'ejecutándose' (running). El comportamiento será desactivado pronto y por eso no hace falta que devuelve éxito/fracaso
        public override TaskStatus OnUpdate() {
            transform.Rotate(transform.up, rotationSpeed);
            return TaskStatus.Running;
        }
    }
}