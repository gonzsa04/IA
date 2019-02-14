namespace AIMA.Core.Util.DataStructure
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel; // Aquí está Collection
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): pg 80.<br>
     * 
     * The priority queue, which pops the element of the queue with the highest
     * priority according to some ordering function.
     */

        // ESTA CLASE ES QUE LA TENÍAN EN JAVA SIN TRADUCIR


    /**
     * @author Ciaran O'Reilly
     */
    public class PriorityQueue<E> : Queue<E> //He quitado la parte de java.util.PriorityQueue<E>,
    {
        private const long serialVersionUID = 1;

        public PriorityQueue()
            : base()
        {

        }

        public PriorityQueue(Collection<E> c)
            : base(c)
        {

        }

        // EN VEZ DE SUPER, LO PONGO COMO BASE
        public PriorityQueue(int initialCapacity) : base(initialCapacity)
        {
            
        }

        // Comparer en lugar de Comparator
        public PriorityQueue(int initialCapacity, Comparer<E> comparator)
            //: base(initialCapacity, comparator)
        {
            // NO SE ENTIENDE QUE LLAME A UN CONSTRUCTOR CON DOS PARÁMETROS SI ESTE ES EL ÚNICO :-(
        }

        public PriorityQueue(PriorityQueue<E> c)
            : base(c)
        {
        }

        public PriorityQueue(SortedSet<E> c)
            : base(c)
        {

        }

        //
        // START-Queue
        public bool isEmpty()
        {
            return 0 == this.Count; // en vez de size();
        }

        public E pop()
        {
            return this.pop(); //en vez de poll();
        }

        public Queue<E> insert(E element)
        {
            if (this.Contains(element)) // En vez de offer(element)
            {
                return this;
            }
            return null;
        }
        // END-Queue
        //
    }
}