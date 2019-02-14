namespace AIMA.Core.Agent.Impl
{
    using System;
    using System.Collections.Generic;
    public class NoOperator : DynamicOperator
    {

        public static readonly NoOperator NO_OP = new NoOperator();

        // WARNING POR ESTAR OCULTANDO MÉTODO DEL MISMO NOMBRE DE NODEEXPANDER
        //
        // START-Action
        public bool isNoOp()
        {
            return true;
        }

        // END-Action
        //

        private NoOperator() : base("NoOp")
        {

        }
    }
}