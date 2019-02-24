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

    using System;
    using System.Collections.Generic;
    using System.Linq;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): pg 80.<br>
     * 
     * Last-in, first-out or LIFO queue (also known as a stack), which pops the newest element of the queue;
     */

    public class LIFOQueue<E> : Stack<E>, IQueue<E> //en el original heredaba de Stack<E> pero me petaba (la clase la habían dejado vacía... yo he hecho que herede de Stack pero implemente IQueue
    {

        public LIFOQueue()
            : base()
        {

        }

        public LIFOQueue(IQueue<E> c)
            : base(c)
        {

        }

        public E Dequeue() // en vez de pop
        {
            return Pop(); //POP DE LA STACK

            
        }

        public void Enqueue(E element) // en vez de push
        {
            Push(element); // PUSH DE LA STACK

        }

        /*
 * Podría ser interesante ofrecer estos métodos para que la clase esté completa
 * 

public bool isEmpty()
{
    return 0 == Count;
}


        public Queue<E> insert(E element)
        {
            if (offer(element))
            {
                return this;
            }
            //return default(E); // Esto si quisieras devolver null... por si es un tipo non-nullable
        }



        public void Remove (E e) {
            // remove
        }


        // START-Override LinkedList methods in order for it to behave in LIFO order.
        public bool Add(E e)
        {
            //Push(e); TAL VEZ HACER UN PUSH EN LA STACK QUE TENGAMOS DENTRO
            return true;
        }

        public bool AddAll(IEnumerable<E> c)
        {
            foreach(E e in c)
            {
                //Push(e); HACER PUSH A LA STACK QUE HAYA DENTRO
            }
            return true;
        }

        public bool Offer(E e)
        {
            //Push(e); HACER PUSH A LA STACK QUE HAYA DENTRO
            return true;
        }

                */
    }
}
 