/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Agency {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class ObjectWithDynamicAttributes {

        private IDictionary<object, object> attributes = new Dictionary<object, object>(); // LinkedHashMap... he puesto Dictionary y además de <object, object>

        //
        // PUBLIC METHODS
        //

        /**
         * Por defecto, devuelve el nombre simple de la clase subyacente tal y como figure en el código fuente.
         * @return el nombre simple de la clase subyacente
         */
         // Es virtual para que pueda ser sobreescrito
        public virtual string DescribeType() {
            return GetType().Name;
        }

        /**
         * Devuelve una representación en forma de cadena de los atributos actuales del objeto.
         * @return una representación en forma de cadena de los atributos actuales del objeto.
         */
         // A lo mejor sería suficiente con ToString
        public string DescribeAttributes() {
            StringBuilder sb = new StringBuilder(); //StringBuilder

            sb.Append("[");
            bool first = true;
            foreach (object key in attributes.Keys) {
                if (first) 
                    first = false;
                else 
                    sb.Append(", ");

                sb.Append(key);
                sb.Append("=");
                object value;
                if (attributes.TryGetValue(key, out value))
                    sb.Append(value); // en vez de get es un TryGetValue (no puede fallar porque key es una de las claves del diccionario
            }
            sb.Append("]");

            return sb.ToString();
        }

        /**
         * Devuelve una vista inmutable del conjunto de claves del objeto.
         * @return una vista inmutable del conjunto de claves del objeto.
         */
        public ISet<object> GetKeySet() {
            // Entiendo que esta conversión eliminará cualquier copia extra que haya porque en un conjunto no puede haber repeticiones
            return new HashSet<object>(attributes.Keys); // La parte de inmodificable Collections.unmodifiableSet es lo que no se puede hacer
        }

        /**
         * Asocia el valor especificado a la clave de atributo especificada.
         * Si el objeto con atributos dinámicos previamente contenía una relación por esa clave, se sustituye el valor antiguo. 
         * 
         * @param key
         *            la clave del atributo
         * @param value
         *            el valor del atributo
         */
        public void SetAttribute(object key, object value) {
            attributes.Add(key, value); //put
        }

        /**
         * Devuelve el valor de la clave de atributo especificada, o nulo si el atributo no se encuentra.
         * 
         * @param key
         *            la clave del atributo
         * 
         * @return el valor del nombre del atributo especificado, o nulo si no se encuentra.
         */
        public object GetAttribute(object key) {
            object value;
            attributes.TryGetValue(key, out value); // en vez de get es un TryGetValue (no puede fallar porque key es una de las claves del diccionario) 
            return value;
        }

        /**
         * Quita el atributo con la clave especificada de este objeto con atributos dinámicos.
         * 
         * @param key
         *            la clave del atributo
         */
        public void RemoveAttribute(object key) {
            attributes.Remove(key);
        }

        /**
         * Crea y devuelve una copia de este objeto con atributos dinámicos.
         */
        public ObjectWithDynamicAttributes Copy() {
            ObjectWithDynamicAttributes copy = null;

            try {
                // Como no se puede crear un ejemplar de esta clase abstracta, con esta línea de código se crea un ejemplar de la clase CONCRETA que seamos en realidad
                copy = (ObjectWithDynamicAttributes)Activator.CreateInstance(GetType()); // getClass().newInstance()
                //copy.attributes.putAll(attributes);
                foreach (var attr in attributes)
                    copy.attributes.Add(attr.Key, attr.Value);
            } catch (Exception ex) {
                Console.WriteLine(ex); // ex.printStackTrace(); aunque no me gusta la idea de mostrar cosas por consola directamente aquí
            }

            return copy;
        }


        // Compara este objeto con atributos dinámicos con otro y dice si son iguales
        public bool Equals(ObjectWithDynamicAttributes o) {
            if (o == null)
                return false;
            if (this == o)
                return true;
            return attributes.Equals(o.attributes);
        }

        // Compara este objeto con atributos dinámicos con otro objeto y dice si son iguales 
        public override bool Equals(object obj) {
            if (obj is ObjectWithDynamicAttributes) // is en vez de instanceof (y mejor que typeof que obligaría a ser del tipo exacto)
                return this.Equals(obj as ObjectWithDynamicAttributes);
            return false;
        }

        // Devuelve código hash del literal (para optimizar el acceso en colecciones y así)
        // No debe contener bucles, tiene que ser muy rápida
        public override int GetHashCode() {
            return attributes.GetHashCode();
        }

        // Cadena de texto representativa  
        public override string ToString() { 
            return DescribeType() + DescribeAttributes();
        }
    }
}
