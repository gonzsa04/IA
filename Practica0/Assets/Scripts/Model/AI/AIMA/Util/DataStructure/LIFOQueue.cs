namespace AIMA.Core.Util.DataStructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): pg 80.<br>
     * 
     * Last-in, first-out or LIFO queue (also known as a stack), which pops the newest element of the queue;
     */

    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class LIFOQueue<E> : Queue<E> //en el original heredaba de Stack<E> pero me petaba...
    {

        // CREO QUE INTERIORMENTE DEBERÍAMOS REPRESENTARLO CON UNA STACK. ME DA LA IMPRESIÓN DE QUE HAN DEJADO LA CLASE VACÍA

        private const long serialVersionUID = 1;

        public LIFOQueue()
            : base()
        {

        }

        public LIFOQueue(IEnumerable<E> c)
            : base(c)
        {

        }

        //
        // START-Queue
        public bool isEmpty()
        {
            return 0 == Count;
        }

        public E pop()
        {
            //return Pop(); HABRÍA QUE HACER POP DE LA STACK
            return default(E); // Por si es un tipo non-nullable
        }

        public void push(E element)
        {
            //Push(element); HABRÍA QUE HACER PUSH DE LA STACK

        }

        //public Queue<E> insert(E element)
        //{
        //    if (offer(element))
        //    {
        //        return this;
        //    }
        //    return null;
        //}

        // END-Queue
        //

        //
        // START-Override LinkedList methods in order for it to behave in LIFO
        // order.
        public bool add(E e)
        {
            //Push(e); TAL VEZ HACER UN PUSH EN LA STACK QUE TENGAMOS DENTRO
            return true;
        }

        public bool addAll(IEnumerable<E> c)
        {
            foreach(E e in c)
            {
                //Push(e); HACER PUSH A LA STACK QUE HAYA DENTRO
            }
            return true;
        }

        public bool offer(E e)
        {
            //Push(e); HACER PUSH A LA STACK QUE HAYA DENTRO
            return true;
        }
        // End-Override LinkedList methods in order for it to behave like a LIFO.
        //
    }
}