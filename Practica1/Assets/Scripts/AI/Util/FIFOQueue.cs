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

    public class FIFOQueue<E> : Queue<E>, IQueue<E> {
        
        public FIFOQueue() : base() {
        }

        public FIFOQueue(IQueue<E> c) : base(c) { // He puesto que pueda recibir una IQueue

        }

        /*
         * Podría ser interesante ofrecer estos métodos para que la clase esté completa
         * 
         
        public bool IsEmpty() {
            return 0 == Count;
        }

        public E Pop() {
            return this.Dequeue();
        }

        public void Push(E element) {
            this.Enqueue(element);
        }

        // Las colas no deberían tener un Remove
          public void Remove(E e) {
            base.Remove
        }

        public Queue<E> Insert(E element) {
            if (offer(element)) {
                return this;
            }
            return null;
        }

        */


    }
}