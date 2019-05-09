/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.AI.Util {

    /*
    import java.util.*;
    import java.util.stream.Collectors;
    */
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /*
     * Clase con utilidades varias. Seguramente en C# sea posible ahorrarse esta clase usando extension methods y cosas por el estilo.
     */
    public class Util {

        public static readonly string NO = "No";
	    public static readonly string YES = "Yes";

	    private static Random random = new Random();

        private static readonly double EPSILON = 0.000000000001;

        /**
         * Devuelve el primer elemento de una lista.
         * 
         * @param l
         *            la lista de la que se extrae el primer elemento.
         * @return el primer elemento de la lista que se ha pasado.
         */
         // Para qué hace falta este método?
        public static T First<T>(IList<T> l) {
            return l[0];
        }

        /**
         * Devuelve una sublista con todos los elementos de una lista excepto el primero.
         * 
         * @param l
         *            la lista de donde hay que extraer los elementos.
         * @return    una sublista con todos los elementos de una lista excepto el primero.
         */
        public static IList<T> Rest<T>(IList<T> list) {
            IList<T> newList = new List<T>(list); //ArrayList Hay que crear una copia para no modificar la lista que nos pasan
            newList.RemoveAt(0); //En Java es subList(1, l.size()) ... aquí lo que hemos hecho ha sido lo opuesto: quitar el primer elemento
            return newList;
        }

        /**
         * Crea un diccionario<K, V> con todas las claves que se pasan inicializadas con el valor que se pasa.
         * 
         * @param keys
         *            las claves para el nuevo diccionario
         * @param value
         *            el valor que debe ser asociado con cada una de las claves.
         * @return un diccionario con todas las claves que se pasan inicializadas con el valor que se pasa.
         */
        public static IDictionary<K, V> Create<K, V>(ICollection<K> keys, V value) {
            IDictionary<K, V> map = new Dictionary<K, V>(); //LinkedHashMap<>

            foreach (K k in keys) 
                map.Add(k, value);

            return map;
        }

        /**
         * Crea un conjunto con los valores proporcionados.
         * @param values
         *        el conjunto inicial de valores
         * @return un conjunto con los valores proporcionados.
         */
        // Entiendo que @SafeVarargs en Java se necesita para evitar la polución en la pila al usar varargs... aquí en C# creo que no hace falta 
        public static ISet<V> CreateSet<V>(params V[] values) {
            ISet<V> set = new HashSet<V>(); //LinkedHashSet
            //Collections.addAll(set, values);
            foreach (V v in values)
                set.Add(v);
            return set;
        }

        /**
         * Selecciona aleatoriamente un elemento de una lista.
         * 
         * @param <T>
         *            el tipo de elemento de la lista l que será devuelto.
         * @param l
         *            una lista de tipo T de donde se seleccionará aleatoriamente un elemento.
         * @return un elemento seleccionado aleatoriamente de la lista l.
         */
        public static T SelectRandomlyFromList<T>(IList<T> l) {
            return l[random.Next(l.Count)]; //nextInt
        }

        public static T SelectRandomlyFromSet<T>(ISet<T> set) { 
            //Iterator<T> iterator = set.iterator();
            IEnumerator<T> enumerator = set.GetEnumerator();
            enumerator.MoveNext(); // Este hay que hacerle siempre para ponerlo en la primera posición
            for (int i = random.Next(set.Count); i > 0; i--) 
                enumerator.MoveNext();
            return enumerator.Current;
        }

        public static int RandomInt(int bound) {
            return random.Next(bound); // Ojo! The maxValue for the upper-bound in the Next() method is exclusive—the range includes minValue, maxValue-1, and all numbers in between.
        }

        public static bool RandomBool() { 
            return random.Next(2) == 1; // Efectivamente Next devolverá o 0 o 1
        }

        public static double[] Normalize(double[] probDist) {
            int len = probDist.Length;
            double total = 0.0;
            foreach (double d in probDist)  
                total = total + d;

            double[] normalized = new double[len];
            if (total != 0) 
                for (int i = 0; i < len; i++) 
                    normalized[i] = probDist[i] / total;

            return normalized;
        }

        public static IList<double> Normalize(IList<double> values) { // Double
            double[] valuesAsArray = new double[values.Count];
            for (int i = 0; i < valuesAsArray.Length; i++)
                valuesAsArray[i] = values[i];
            double[] normalized = Normalize(valuesAsArray);
            IList<double> results = new List<double>(); //ArrayList
            foreach (double aNormalized in normalized)
                results.Add(aNormalized);
            return results;
        }

        public static int Min(int i, int j) {
            return (i > j ? j : i);
        }

        public static int Max(int i, int j) {
            return (i < j ? j : i);
        }

        public static int Max(int i, int j, int k) {
            return Max(Max(i, j), k);
        }

        public static int min(int i, int j, int k) {
            return Min(Min(i, j), k);
        }

        public static T Mode<T>(IList<T> l) {
            IDictionary<T, int> hash = new Dictionary<T, int>(); //Hashtable, lo voy a transformar en un Dictionary, porque en C# el Hashtable no es un tipo genérico
            foreach (T obj in l) {
                int value;
                if (hash.TryGetValue(obj, out value)) {
                    hash.Add(obj, value + 1);
                } else {
                    hash.Add(obj, 1);
                }
            }

            IEnumerator<T> enumerator = hash.Keys.GetEnumerator(); //Keys (esto del movenext hace falta para pasar al primero
            enumerator.MoveNext();
            T maxkey = enumerator.Current; 
            foreach (T key in hash.Keys) {
                int valueKey;
                hash.TryGetValue(key, out valueKey);
                int valueMaxKey;
                hash.TryGetValue(maxkey, out valueMaxKey);
                if (valueKey > valueMaxKey) {
                    maxkey = key;
                }
            }
            return maxkey;
        }

        public static string[] YesNo() {
            return new string[] { YES, NO };
        }

        public static double Log2(double d) {
            return Math.Log(d) / Math.Log(2);
        }

        public static double Information(double[] probabilities) {
            double total = 0.0d;
            foreach (double d in probabilities) 
                total += (-1.0d * Log2(d) * d);

            return total;
        }

        public static IList<T> RemoveFrom<T>(IList<T> list, T member) {
            IList<T> newList = new List<T>(list); //ArrayList Hay que crear una copia para no modificar la lista que nos pasan
            newList.Remove(member);
            return newList;
        }

        // En C# no hay una clase Number, con lo que si esto falla habría que crear otro método que acepte un float o lo que sea, en vez de un doble
        public static double SumOfSquares(IList<double> list) { //<T extends Number> y luego el argumento era IList<Number> list. Creo que en C# esto no hace falta
            double accum = 0;
            foreach (double item in list) // double en vez de T o de Number
                accum = accum + (item * item); // Aquí ya no hace falta item.doubleValue()

            return accum;
        }

        public static string NTimes(string s, int n) {
            StringBuilder builder = new StringBuilder(); //StringBuilder
            for (int i = 0; i < n; i++) 
                builder.Append(s);

            return builder.ToString();
        }

        public static void CheckForNanOrInfinity(double d) {
            if (double.IsNaN(d)) { //isNaN
                throw new ArgumentException("Not a Number"); //RuntimeException
            }
            if (double.IsInfinity(d)) { //isInfinite
                throw new ArgumentException("Infinite Number");
            }
        }

        public static int RandomNumberBetween(int i, int j) {
            /* i,j bothinclusive */
            return random.Next(j - i + 1) + i;
        }

        public static double CalculateMean(IList<double> lst) {
            double sum = 0.0;
            foreach (double d in lst) 
                sum = sum + d;

            return sum / lst.Count;
        }

        public static double CalculateStDev(IList<double> values, double mean) {

            int listSize = values.Count;

            double sumOfDiffSquared = 0.0;
            foreach (double value in values) {
                double diffFromMean = value - mean;
                sumOfDiffSquared += ((diffFromMean * diffFromMean) / (listSize - 1));
                // La división se ha movido aquí para evitar que la suma se haga demasiado grande, si esto no funciona usar formulación incremental 

            }
            double variance = sumOfDiffSquared;
            // (listSize - 1);
            // asume al menos 2 miembros en la lista
            return Math.Sqrt(variance);
        }

        public static IList<double> NormalizeFromMeanAndStdev(IList<double> values, double mean, double stdev) {
            return values.Select<double, double>(d => (d - mean) / stdev).ToList(); // Del original en Java: values.stream().map(d-> (d - mean) / stdev).collect(Collectors.toList());
        }

        /**
         * Genera un real (de precisión doble) aleatorio entre dos límites, ambos inclusivos.
         * @param lowerLimit el límite inferior.
         * @param upperLimit el límite superior.
         * @return un real aleatorio más grande o igual que lowerLimit y más pequeño o igual que upperLimit.
         */
        public static double GenerateRandomDoubleBetween(double lowerLimit, double upperLimit) {
            return lowerLimit + ((upperLimit - lowerLimit) * random.NextDouble());
        }

        /**
         * Genera un real aleatorio entre dos límites, ambos inclusivos.
         * @param lowerLimit el límite inferior.
         * @param upperLimit el límite superior.
         * @return un real aleatorio más grande o igual que lowerLimit y más pequeño o igual que upperLimit.
         */
         // En que casos se necesita un Float y no un Double?? No podemos deshacernos de este método??
        public static float GenerateRandomFloatBetween(float lowerLimit, float upperLimit) {

            // Técnicamente así se calcula un número aleatorio tipo Float en C#:
            double mantissa = (random.NextDouble() * 2.0d) - 1.0d;
            // elegir -149 en lugar de -126 para generar también valores float por debajo de lo normal
            double exponent = Math.Pow(2.0d, random.Next(-126, 128));
            float result = (float)(mantissa * exponent);

            return lowerLimit + ((upperLimit - lowerLimit) * result);
        }

        /**
         * Compara dos reales (de precisión doble) para ver si son iguales.
         * @param a el primer real
         * @param b el segundo real
         * @return cierto si ambos reales contienen el mismo valor o la desviación abosluta entre ellos está por debajo de EPSILON.
         */
        public static bool CompareDoubles(double a, double b) {
            if (double.IsNaN(a) && double.IsNaN(b)) return true;
            if (!double.IsInfinity(a) && !double.IsInfinity(b)) return Math.Abs(a - b) <= EPSILON;
            return a == b;
        }

        /**
         * Compara dos reales para ver si son iguales.
         * @param a el primer real
         * @param b el segundo real
         * @return cierto si ambos reales contienen el mismo valor o la desviación abosluta entre ellos está por debajo de EPSILON.
         */
        public static bool CompareFloats(float a, float b) {
            if (float.IsNaN(a) && float.IsNaN(b)) return true;
            if (!float.IsInfinity(a) && !float.IsInfinity(b)) return Math.Abs(a - b) <= EPSILON;
            return a == b;
        }
    }
}


