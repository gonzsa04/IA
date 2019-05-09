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

    // La bandera se reasignará el padre a sí misma si llega a estar en contacto con un objeto que esté marcado con la etiqueta (tag) de 'ofensor/ofensa'
    public class Flag : MonoBehaviour {

        // La etiqueta de los que pueden capturar la bandera
        public string offenseTag;

        // Cuando la bandera entra en contacto con el ofensor, es cogida por este
        public void OnTriggerEnter(Collider other) {
            NPC npc = null;
            if ((npc = other.GetComponent<NPC>()) != null) {
                // El objeto con el que se colisiona tiene que ser un NPC 'ofensor'
                if (npc.IsOffense) {
                    // Se notifica al gestor del juego
                    if (CTFGameManager.instance.flagTaken()) {
                        // Si al gestor del juego le parece ok que se capture la bandera...
                        transform.parent = other.transform;
                        // El objeto que captura la bandera tendrá un componente NPC, y es a ese componente al que notificamos que tiene la bandera
                        other.GetComponent<NPC>().HasFlag = true;
                    }
                }
            }
        }
    }
}