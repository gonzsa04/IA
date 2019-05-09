/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Agency {

    public class NoOpAction : DynamicAction {

        public static readonly NoOpAction NO_OP = new NoOpAction();

        //
        // START-Action
        public override bool IsNoOp() {
            return true;
        }

        // END-Action
        //

        private NoOpAction() : base("NoOp") { 
        }
    }
}