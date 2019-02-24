/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search {

    using System.Collections.Generic;
    using UCM.IAV.IA;
    
    /**
     * Una especializaci�n de la prueba objetivo que permite comprobar si la soluci�n es aceptable tal cual, una vez identificado el objetivo. 
     * Esto permite continuar la b�squeda de soluciones alternativas sin tener que reiniciar la b�squeda. 
     * No siempre tendr� sentido continuar con una b�squeda cuando ya se ha encontrado un objetivo.
     * Esto pertenece a la infraestructura (framework) de la b�squeda.
     */
    public interface SolutionChecker : GoalTest
    {
        // S�lo si la prueba objetivo da cierto se llama a este m�todo, para saber si la soluci�n es aceptable tal cual o si hay que seguir la b�squeda 
        bool IsAcceptableSolution(List<Operator> operators, object goal);
    }
}