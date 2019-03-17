/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search {

    // Esto pertenece a la infraestructura (framework) de la búsqueda

    using System;
    using System.Collections.Generic;

    /**
     * Según lo que hemos visto sobre dominios de trabajo, un problema se puede definir mediante cinco componentes:
     * - Configuración inicial 
     * - Operadores posibles, con una función que dada una configuración sabe decir los operadores aplicables
     * - Modelo de transición, una función que dada una configuración y un operador sabe decir la configuración resultante
     * - Prueba de objetivo, función que determina si una determinada configuración es objetivo o no
     * - Coste de ruta, función que otorga un valor numérico (coste) a cada ruta
     * 
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public class Problem {

        protected object initialSetup;  

        protected OperatorsFunction operatorsFunction;  

        protected ResultFunction resultFunction; // o también transitionsFunction

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