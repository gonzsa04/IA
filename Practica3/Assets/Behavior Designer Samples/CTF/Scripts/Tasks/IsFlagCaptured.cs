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

    using BehaviorDesigner.Runtime.Tasks;

    [TaskCategory("CTF")]
    [TaskDescription("Tarea condicional que devuelve si la bandera ha sido capturada")]
    public class IsFlagCaptured : Conditional {

        private CTFGameManager gameManager;

        public override void OnAwake() {
            // Cachear para acceder m�s r�pido
            gameManager = CTFGameManager.instance;
        }

        // Devuelve �xito si la bandera ha sido capturada, o fracaso si no lo ha sido
        public override TaskStatus OnUpdate() {
            // El juego est� activo cuando la bandera no ha sido capturada
            if (gameManager.GameActive)
                return TaskStatus.Failure;
            return TaskStatus.Success;
        }
    }
}