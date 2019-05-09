/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.KB {

    using System.Collections.Generic;
    using System.Text;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 253. 
     * Una conjunci�n de cl�usulas, donde cada cl�usula es una disyunci�n de literales.
     * Aqu� se representa una conjunci�n de cl�usulas como un conjunto de cl�usulas, donde cada cl�usula es a su vez un conjunto de literales. 
     * Adem�s, una conjunci�n de cl�usulas es implementada como inmutable.  
     */
    public class ConjunctionOfClauses {

        private ISet<Clause> clauses = new HashSet<Clause>();
        private string cachedStringRep = null;
        private int cachedHashCode = -1;

        /**
	     * Constructor.
	     * @param conjunctionOfClauses
	     *            una colecci�n de cl�usulas que representa una conjunci�n.
	     */
        public ConjunctionOfClauses(ICollection<Clause> conjunctionOfClauses) { // Collection de Java se queda igual
            this.clauses.UnionWith(conjunctionOfClauses);  // UnionWith de ISet en vez del m�todo addAll de Set 

            // Hacer inmutable (AHORA MISMO NO LO ESTOY HACIENDO, OJO)
            // using System.Collections.Immutable; y usar as� ImmutableHashSet
            // Las clases de colecci�n inmutables est�n disponibles con.NET Core, pero no son parte de la biblioteca de clases b�sica distribuida con.NET Framework. Est�n disponibles a partir de.NET Framework 4.5 a trav�s de NuGet.
            // Aqu� sugieren crearnos nuestro propio tipo ReadOnlySet<T> (https://stackoverflow.com/questions/36815062/c-sharp-hashsett-read-only-workaround)
            // new ReadOnlyCollection<Literal> en vez de Collections.unmodifiableSet no funciona porque es sobre listas
            // this.clauses = new ReadOnlyCollection<Clause>(this.clauses);  
        }

        /**
	     * Devuelve el n�mero de cl�usulas contenidas en esta conjunci�n.
	     * @return el n�mero de cl�usulas contenidas en esta conjunci�n.
	     */
        public int CountClauses() { // En vez de getNumberOfClauses... deber�a ser una property
            return clauses.Count;
        }

        /**
	     * Devuelve el conjunto de cl�usulas contenidas en esta conjunci�n.
	     * @return el conjunto de cl�usulas contenidas en esta conjunci�n.
	     */
        public ISet<Clause> GetClauses() { // La property...
            return clauses;
        }

        /**
	     * Crea una nueva conjunci�n de cl�usulas tomando las cl�usulas de la actual conjunci�n y a�adiendo algunas cl�usulas adicionales. 
	     * 
	     * @param additionalClauses
	     *            las cl�usulas adicionales que ser�n a�adidas al conjunto existente de cl�usulas para crear una nueva conjunci�n. 
	     * @return una nueva conjunci�n de cl�usulas conteniendo las existentes y las cl�usulas adicionales que se han pasado. 
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

        // Devuelve c�digo hash de la conjunci�n de cl�usulas (para optimizar el acceso en colecciones y as�)
        // No debe contener bucles, tiene que ser muy r�pida
        public override int GetHashCode() {
            if (cachedHashCode == -1) {
                cachedHashCode = clauses.GetHashCode();
            }
            return cachedHashCode;
        }

        // Compara esta conjunci�n con otra y dice si son iguales
        public bool Equals(ConjunctionOfClauses c) {
            if (c == null) // Estando el otro m�todo Equals no s� si van a llamar con un null a este...
                return false;
            if (this == c)
                return true;
            return c.clauses.Equals(this.clauses);
        }

        // Compara esta conjunci�n con otro objeto y dice si son iguales 
        public override bool Equals(object obj) {
            if (obj is ConjunctionOfClauses) // is en vez de instanceof (y mejor que typeof que obligar�a a ser del tipo exacto)
                return this.Equals(obj as ConjunctionOfClauses); // as en vez del casting (ConjunctionOfClauses)
            return false;
        }
    }
}
