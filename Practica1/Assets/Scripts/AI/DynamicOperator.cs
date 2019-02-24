/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA {

    // Es una clase de implementación

    using System.Collections.Generic;

// ES UN POCO RARO EL CONCEPTO DE DYNAMIC OPERATOR...
public class DynamicOperator : ObjectWithDynamicAttributes, Operator {

    public const string ATTRIBUTE_NAME = "name";

	//

    public DynamicOperator(string name)
    {
		this.setAttribute(ATTRIBUTE_NAME, name);
	}

    public string getName()
    {
        return (string)getAttribute(ATTRIBUTE_NAME);
	}

	// Esta es la parte de operadores que hay aquí, la he hecho virtual para poder sobreescribirla
	public virtual bool isNoOp() {
		return false;
	}

}
}