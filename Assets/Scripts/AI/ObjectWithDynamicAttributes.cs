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

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics;

    /*
     * Objeto que cuenta con atributos dinámicos.
     * Es una clase de implementación del núcleo de la infraestructura de la búsqueda.
     */
public abstract class ObjectWithDynamicAttributes {
	private Dictionary<object, object> attributes = new Dictionary<object, object>();

	//
	// PUBLIC METHODS
	//
	public string describeType() {
		return this.GetType().Name;
	}

	public string describeAttributes() {
		StringBuilder sb = new StringBuilder();

		sb.Append("[");
		bool first = true;
		foreach (object key in attributes.Keys) {
			if (first) {
				first = false;
			} else {
                sb.Append(", ");
			}

            sb.Append(key);
            sb.Append("==");
            sb.Append(attributes[key]);
		}
        sb.Append("]");

		return sb.ToString();
	}

	public HashSet<object> getKeySet() {
            // HE TENIDO QUE PONER HASHSET EN VEZ DE SET
		return new HashSet<object>(attributes.Keys);
	}

	public void setAttribute(object key, object value) {
		attributes[key] = value;
	}

	public object getAttribute(object key) {
		return attributes[key];
	}

	public void removeAttribute(object key) {
		attributes.Remove(key);
	}

	public ObjectWithDynamicAttributes copy() {
		ObjectWithDynamicAttributes copy = null;

		try {
			copy = (ObjectWithDynamicAttributes)this.GetType().GetConstructor(System.Type.EmptyTypes).Invoke(null);
            foreach (object val in attributes)
            {
                copy.attributes.Add(val, attributes[val]);
            }
		} catch (Exception ex) {
			Debug.WriteLine(ex.ToString());
		}

		return copy;
	}

	public override bool Equals(object o) {
		if (o == null || this.GetType() != o.GetType()) {
			return base.Equals(o);
		}
		return attributes.Equals(((ObjectWithDynamicAttributes) o).attributes);
	}

	public override int GetHashCode() {
		return attributes.GetHashCode();
	}

	public override string ToString() {
		StringBuilder sb = new StringBuilder();

		sb.Append(describeType());
		sb.Append(describeAttributes());

		return sb.ToString();
	}
}
}
 