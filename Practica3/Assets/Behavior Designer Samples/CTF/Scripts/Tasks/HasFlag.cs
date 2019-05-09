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
    
    //using UnityEngine; No se usa aqu�
    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("CTF")]
    [TaskDescription("Tarea condicional que devuelve si el componente NPC tiene la bandera")]
    public class HasFlag : Conditional {
        private NPC npc;

        public override void OnAwake() {
            // Cachear para acceder m�s r�pido
            npc = gameObject.GetComponent<NPC>();
        }

        // Devuelve cierto si el NPC tiene la bandera, fracaso si no la tiene
        public override TaskStatus OnUpdate() {
            if (npc.HasFlag)
                return TaskStatus.Success;
            return TaskStatus.Failure;
        }
    }
}