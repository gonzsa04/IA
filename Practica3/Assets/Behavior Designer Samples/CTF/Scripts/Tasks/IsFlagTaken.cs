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

    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("CTF")]
    [TaskDescription("Tarea condicional que devuelve si la bandera ha sido capturada")]
    public class IsFlagTaken : Conditional {

        private CTFGameManager gameManager;

        public override void OnAwake() {
            // Cachear para acceder más rápido
            gameManager = CTFGameManager.instance;
        }

        // Devolverá éxito si la bandera ha sido capturada o fracaso si no lo ha sido
        public override TaskStatus OnUpdate() {
            if (gameManager.IsFlagTaken)
                return TaskStatus.Success;
            return TaskStatus.Failure;
        }
    }
}