/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Visitors {

    using UCM.IAV.AI.Logic.Propositional.Parsing;

    /**
     * Convierte una sentencia su sentencia equivalente en forma normal conjuntiva (CNF).
     * Una sentencia está en CNF si es una conjunción de disyunciones de literales. 
     */
    public class ConvertToCNF {

        /**
         * Devuelve la sentencia especificada en su sentencia equivalente en forma normal conjuntiva.
         * 
         * @param s
         *            una sentencia de lógica proposicional
         * 
         * @return la sentencia de entrada convertida en su equivalente lógica de forma normal conjuntiva.
         */
        public static Sentence Convert(Sentence s) {
            Sentence result = null;

            Sentence nnfSentence = ConvertToNNF.Convert(s);
            Sentence cnfSentence = DistributeOrOverAnd.Distribute(nnfSentence);

            result = cnfSentence;

            return result;
        }
    }
}