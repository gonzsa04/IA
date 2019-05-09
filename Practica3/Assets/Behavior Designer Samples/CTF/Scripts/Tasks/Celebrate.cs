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
    using BehaviorDesigner.Runtime.Tasks;
    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("CTF")]
    [TaskDescription("La bandera ha sido capturada. Se celebra rotando en c�rculo.")]
    public class Celebrate : Action {
        [Tooltip("La velocidad de rotaci�n")]
        public float rotationSpeed;

        // Siempre devolver� 'ejecut�ndose' (running). El comportamiento ser� desactivado pronto y por eso no hace falta que devuelve �xito/fracaso
        public override TaskStatus OnUpdate() {
            transform.Rotate(transform.up, rotationSpeed);
            return TaskStatus.Running;
        }
    }
}