namespace AIMA.Core.Agent.Impl
{
    using System.Collections.Generic;
    using AIMA.Core.Agent;

/**
 * @author Ciaran O'Reilly
 */
 // ES UN POCO RARO EL CONCEPTO DE DYNAMIC OPERATOR...
public class DynamicOperator : ObjectWithDynamicAttributes ,
		Operator {
    public const System.String ATTRIBUTE_NAME = "name";

	//

    public DynamicOperator(System.String name)
    {
		this.setAttribute(ATTRIBUTE_NAME, name);
	}

    public System.String getName()
    {
        return (System.String)getAttribute(ATTRIBUTE_NAME);
	}

	//
	// START-Action
	public bool isNoOp() {
		return false;
	}

        // END-Action
        //

        // WARNING POR ESTAR OCULTANDO MÉTODO DEL MISMO NOMBRE DE NODEEXPANDER
        public System.String describeType() {
		return this.GetType().Name;
	}
}
}