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

    using System;
    using System.Collections.Generic;

    /* 
     * El operador dinámico 'no operador.
     * Es una clase de implementación del núcleo de la infraestructura de la búsqueda.
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