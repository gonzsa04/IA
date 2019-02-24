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


    using System.Collections.Generic;

    /**
     * Describe un operador que puede ser aplicado por el resolutor
     */
    public interface Operator
    {

         // Indica que este operador es un 'No Operación', una instrucción que no hace nada, y se usa como assembly language instruction
        bool isNoOp();

        // Podría tener un toString para depurar o algo...
    }
}