/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA {

    using System;
    using System.Collections.Generic;

    /* 
     * El operador din�mico 'no operador.
     * Es una clase de implementaci�n del n�cleo de la infraestructura de la b�squeda.
     */
    public class NoOperator : DynamicOperator
    {

        public static readonly NoOperator NO_OP = new NoOperator();
         
        // Comienzo de los operadores
        public override bool isNoOp()
        {
            return true;
        } 

        private NoOperator() : base("NoOp")
        {

        }
    }
}