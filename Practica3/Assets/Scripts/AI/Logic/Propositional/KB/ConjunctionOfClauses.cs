/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.KB {

    using System.Collections.Generic;
    using System.Text;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 253. 
     * Una conjunción de cláusulas, donde cada cláusula es una disyunción de literales.
     * Aquí se representa una conjunción de cláusulas como un conjunto de cláusulas, donde cada cláusula es a su vez un conjunto de literales. 
     * Además, una conjunción de cláusulas es implementada como inmutable.  
     */
    public class ConjunctionOfClauses {

        private ISet<Clause> clauses = new HashSet<Clause>();
        private string cachedStringRep = null;
        private int cachedHashCode = -1;

        /**
	     * Constructor.
	     * @param conjunctionOfClauses
	     *            una colección de cláusulas que representa una conjunción.
	     */
        public ConjunctionOfClauses(ICollection<Clause> conjunctionOfClauses) { // Collection de Java se queda igual
            this.clauses.UnionWith(conjunctionOfClauses);  // UnionWith de ISet en vez del método addAll de Set 

            // Hacer inmutable (AHORA MISMO NO LO ESTOY HACIENDO, OJO)
            // using System.Collections.Immutable; y usar así ImmutableHashSet
            // Las clases de colección inmutables están disponibles con.NET Core, pero no son parte de la biblioteca de clases básica distribuida con.NET Framework. Están disponibles a partir de.NET Framework 4.5 a través de NuGet.
            // Aquí sugieren crearnos nuestro propio tipo ReadOnlySet<T> (https://stackoverflow.com/questions/36815062/c-sharp-hashsett-read-only-workaround)
            // new ReadOnlyCollection<Literal> en vez de Collections.unmodifiableSet no funciona porque es sobre listas
            // this.clauses = new ReadOnlyCollection<Clause>(this.clauses);  
        }

        /**
	     * Devuelve el número de cláusulas contenidas en esta conjunción.
	     * @return el número de cláusulas contenidas en esta conjunción.
	     */
        public int CountClauses() { // En vez de getNumberOfClauses... debería ser una property
            return clauses.Count;
        }

        /**
	     * Devuelve el conjunto de cláusulas contenidas en esta conjunción.
	     * @return el conjunto de cláusulas contenidas en esta conjunción.
	     */
        public ISet<Clause> GetClauses() { // La property...
            return clauses;
        }

        /**
	     * Crea una nueva conjunción de cláusulas tomando las cláusulas de la actual conjunción y añadiendo algunas cláusulas adicionales. 
	     * 
	     * @param additionalClauses
	     *            las cláusulas adicionales que serán añadidas al conjunto existente de cláusulas para crear una nueva conjunción. 
	     * @return una nueva conjunción de cláusulas conteniendo las existentes y las cláusulas adicionales que se han pasado. 
	     */
        public ConjunctionOfClauses Extend(ICollection<Clause> additionalClauses) {
            ISet<Clause> extendedClauses = new HashSet<Clause>(); // LinkedHashSet
            extendedClauses.UnionWith(clauses);
            extendedClauses.UnionWith(additionalClauses);

            ConjunctionOfClauses result = new ConjunctionOfClauses(extendedClauses);

            return result;
        }

        // Cadena de texto representativa  
        public override string ToString() {
            if (cachedStringRep == null) {
                StringBuilder sb = new StringBuilder(); // Hay StringBuilder en C#
                bool first = true;
                sb.Append("{");
                foreach (Clause c in clauses) {
                    if (first) 
                        first = false;
                    else 
                        sb.Append(", ");
                    sb.Append(c);
                }
                sb.Append("}");
                cachedStringRep = sb.ToString();
            }
            return cachedStringRep;
        }

        // Devuelve código hash de la conjunción de cláusulas (para optimizar el acceso en colecciones y así)
        // No debe contener bucles, tiene que ser muy rápida
        public override int GetHashCode() {
            if (cachedHashCode == -1) {
                cachedHashCode = clauses.GetHashCode();
            }
            return cachedHashCode;
        }

        // Compara esta conjunción con otra y dice si son iguales
        public bool Equals(ConjunctionOfClauses c) {
            if (c == null) // Estando el otro método Equals no sé si van a llamar con un null a este...
                return false;
            if (this == c)
                return true;
            return c.clauses.Equals(this.clauses);
        }

        // Compara esta conjunción con otro objeto y dice si son iguales 
        public override bool Equals(object obj) {
            if (obj is ConjunctionOfClauses) // is en vez de instanceof (y mejor que typeof que obligaría a ser del tipo exacto)
                return this.Equals(obj as ConjunctionOfClauses); // as en vez del casting (ConjunctionOfClauses)
            return false;
        }
    }
}
