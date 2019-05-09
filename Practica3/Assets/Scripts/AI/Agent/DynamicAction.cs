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

    public class DynamicAction : ObjectWithDynamicAttributes, Action {

	    public static readonly string ATTRIBUTE_NAME = "name";
        
	    public DynamicAction(string name) {
		    this.SetAttribute(ATTRIBUTE_NAME, name);
	    }

        /**
	     * Devuelve el valor del atributo nombre.
	     * 
	     * @return el valor del atributo nombre.
	     */
        public string GetName() {
		    return (string) GetAttribute(ATTRIBUTE_NAME); // Conversi�n rara a string
	    }

	    //
	    // START-Action
        // Es virtual para que se pueda sobreescribir
	    public virtual bool IsNoOp() {
		    return false;
	    }

        // END-Action
        //

        // Por ahora se puede evitar tener que sobreescribir este m�todo y tener un ToString
        /* public override string describeType() {
		    return nameof(Action); //Action.class.getSimpleName(); 
        }*/
    }
}