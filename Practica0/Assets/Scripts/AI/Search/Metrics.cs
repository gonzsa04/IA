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

    using System;
    using System.Collections.Generic;

    /* 
     * Almacena y gestiona las distintas m�tricas que pueden tener los distintos algoritmos de b�squeda.
     * Esto pertenece a la infraestructura (framework) de la b�squeda.
     */
    public class Metrics
    {
        private Dictionary<string, string> hash;

        public Metrics()
        {
            this.hash = new Dictionary<string, string>();
        }

        public void set(string name, int i)
        {
            hash[name] = i.ToString();
        }

        public void set(string name, double d)
        {
            hash[name] = d.ToString();
        }

        public int getInt(string name)
        {
            return int.Parse(hash[name]);
        }

        public double getDouble(string name)
        {
            return double.Parse(hash[name]);
        }

        public string get(string name)
        {
            return hash[name];
        }

        public HashSet<string> keySet()
        {
            return new HashSet<string>(hash.Keys);
        }
    }
}
 