/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.AI.Util {

    using System.Collections.Generic;

    /**
     * Nota: Este código está basado en Java Tutorial: The Set Interface
     * http://download.oracle.com/javase/tutorial/collections/interfaces/set.html
     * 
     * En el código original de Java se usa LinkedHashSet, que aunque es algo más lento que HashSet, nos asegura que el orden siempre es respetado (por ejemplo, si se llama con implementaciones TreeSet o LinkedHashSet).
     * 
     * En C# lo que utilizamos aquí es HashSet
     */
     // Confirmar que realmente haga falta esta clase de utilidad
    public class SetOps {

        /**
         * Devuelve la unión entre los conjuntos s1 y s2.
         * @param <T>
         * @param s1
         * @param s2
         * @return la unión entre los conjuntos s1 y s2.
         */
        public static ISet<T> Union<T>(ISet<T> s1, ISet<T> s2) {
            if (s1 == s2) 
                return s1;

            ISet<T> union = new HashSet<T>(s1);
            foreach (T e in s2)
                union.Add(e);

            return union;
        }

        /**
         * Devuelve la intersección entre los conjuntos s1 y s2.
         * @param <T>
         * @param s1
         * @param s2
         * @return la intersección entre los conjuntos s1 y s2.
         */
        public static ISet<T> Intersection<T>(ISet<T> s1, ISet<T> s2) {
            if (s1 == s2) 
                return s1;
            
            ISet<T> intersection = new HashSet<T>(s1);
            foreach (T e in s1)
                if (!s2.Contains(e))
                    intersection.Remove(e); 

            return intersection;
        }

        /**
         * Devuelve la diferencia (asimmétrica) entre los conjuntos s1 y s2 (todos los elementos qué SÍ están en s1 pero NO están en s2).
         * @param <T>
         * @param s1
         * @param s2
         * @return la diferencia (asimmétrica) entre los conjuntos s1 y s2 (todos los elementos qué SÍ están en s1 pero NO están en s2).
         */
        public static ISet<T> Difference<T>(ISet<T> s1, ISet<T> s2) {
            if (s1 == s2) 
                return new HashSet<T>();

            ISet<T> difference = new HashSet<T>(s1);
            foreach (T e in s2)
                difference.Remove(e); 

            return difference;
        }
    }
}


