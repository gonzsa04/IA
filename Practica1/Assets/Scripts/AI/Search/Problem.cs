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

    // Esto pertenece a la infraestructura (framework) de la b�squeda

    using System;
    using System.Collections.Generic;

    /**
     * Seg�n lo que hemos visto sobre dominios de trabajo, un problema se puede definir mediante cinco componentes:
     * - Configuraci�n inicial 
     * - Operadores posibles, con una funci�n que dada una configuraci�n sabe decir los operadores aplicables
     * - Modelo de transici�n, una funci�n que dada una configuraci�n y un operador sabe decir la configuraci�n resultante
     * - Prueba de objetivo, funci�n que determina si una determinada configuraci�n es objetivo o no
     * - Coste de ruta, funci�n que otorga un valor num�rico (coste) a cada ruta
     * 
     * Esto pertenece a la infraestructura (framework) de la b�squeda.
     */
    public class Problem {

        protected object initialSetup;  

        protected OperatorsFunction operatorsFunction;  

        protected ResultFunction resultFunction; // o tambi�n transitionsFunction

        protected GoalTest goalTest;

        protected StepCostFunction stepCostFunction; // path cost

        public Problem(object initialSetup, OperatorsFunction operatorsFunction,
                ResultFunction resultFunction, GoalTest goalTest)
            : this(initialSetup, operatorsFunction, resultFunction, goalTest,
                new DefaultStepCostFunction()) {
            
        }

        public Problem(object initialSetup, OperatorsFunction operatorsFunction,
                ResultFunction resultFunction, GoalTest goalTest,
                StepCostFunction stepCostFunction) {

            this.initialSetup = initialSetup;
            this.operatorsFunction = operatorsFunction;
            this.resultFunction = resultFunction;
            this.goalTest = goalTest;
            this.stepCostFunction = stepCostFunction;
        }

        public object GetInitialSetup()
        {
            return initialSetup;
        }

        public bool IsGoalSetup(object setup)
        {
            return goalTest.IsGoalSetup(setup);
        }

        public GoalTest GetGoalTest()
        {
            return goalTest;
        }

        public OperatorsFunction GetOperatorsFunction()
        {
            return operatorsFunction;
        }

        public ResultFunction GetResultFunction()
        {
            return resultFunction;
        }

        public StepCostFunction GetStepCostFunction()
        {
            return stepCostFunction;
        }

        //
        // PROTECTED METHODS
        //
        protected Problem()
        {
        }
    }
}