/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Util {

    using System.Collections.Generic;

    // Mi propio interfaz para aglutinar las "colas" en un sentido general (tanto colas propiamente dichas -FIFO, como pilas -LIFO- como colas de prioridad)
    public interface IQueue<T> : IEnumerable<T> {

        // No estoy usando este propiedad, pero sería interesante ofrecerla
        //T First { get; }

        // Propiedad para contar
        int Count { get; }

        
        void Enqueue(T item);

        T Dequeue();

        // No estoy usando este método, pero sería interesante ofrecerlo
        //void Clear();

        // Indica si la cola contiene un elemento
        bool Contains(T item);

        // Elimina un elemento de la cola (en realidad las colas no ofrecen este método... eso sería más una Lista)
        // void Remove(T item);
    }
}
