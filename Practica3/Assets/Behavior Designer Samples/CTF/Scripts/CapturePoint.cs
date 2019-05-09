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

    // Notifica al gestor del juego (Game Manager) cuando la bandera entra en el disparador de este punto de captura.
    public class CapturePoint : MonoBehaviour {

        // El objeto de la bandera
        public GameObject flag;

        public void OnTriggerEnter(Collider other) {

            if (other.gameObject.Equals(flag)) {
                // Cuando la bandera alcanza el punto de captura el juego se ha terminado
                CTFGameManager.instance.resetGame();
            }
        }
    }
}