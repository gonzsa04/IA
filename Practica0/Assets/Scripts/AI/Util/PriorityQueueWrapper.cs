/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Util {

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel; // Aqu� est� Collection


    using UCM.IAV.IA.Search;

    using PriorityQueue;
    using System.Collections;



    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): pg 80.<br>
     * 
     * The priority queue, which pops the element of the queue with the highest
     * priority according to some ordering function.
     */

    // ESTA CLASE ES QUE LA TEN�AN EN JAVA SIN TRADUCIR


    // La idea es que extienda Queue... pero tiene tambi�n que cumplir nuestro interfaz IQueue, claro
    // HE PUESTO SIMPLEPRIORITYQUEUE, PERO PODR�A HABER PUESTO LA FAST
    // Podr�a ser un interfaz en plan IPriorityQueue...
    public class PriorityQueueWrapper : IQueue<Node> //He quitado la parte de java.util.PriorityQueue<E>,
         //where TPriority : IComparable<TPriority> AL FINAL EL TIPO NO LE HICE GEN�RICO
         {

        private IPriorityQueue<Node, double> priorityQueue;

        int IQueue<Node>.Count {
            get {
                return priorityQueue.Count;
            }
        }

        //private Comparer<E> comparer;

        // Constructor por defecto (se inventa un comparer por defecto, parece ser)
        public PriorityQueueWrapper() {
            priorityQueue = new SimplePriorityQueue<Node, double>();
        }

        // SimplePriorityQueue exige que sea un IComparer de double
        public PriorityQueueWrapper(IComparer<double> comparer) {
            priorityQueue = new SimplePriorityQueue<Node, double>(comparer);
        }

        /*
        public PriorityQueue(IQueue<E> c) // Hab�a un Collection... pero creo que ser�a mejor el IQueue
            : base(c)
        {

        }
        */

        /* Voy a intentar no usar este constructor donde le metes la capacidad...
        // EN VEZ DE SUPER, LO PONGO COMO BASE
        public PriorityQueue(int initialCapacity) : base(initialCapacity)
        {
            
        }

        // Comparer en lugar de Comparator
        public PriorityQueue(int initialCapacity, Comparer<E> comparator)
            : base(initialCapacity)
        {
            // NO SE ENTIENDE QUE LLAME A UN CONSTRUCTOR CON DOS PAR�METROS SI ESTE ES EL �NICO... NO HAY CONSTRUCTORES CON DOS PAR�METROS EN STACK :-(
            // ME VOY A GUARDAR EL COMPARATOR, PORQUE ENTIENDO QUE ES LO QUE HABR�A QUE USAR
            this.comparator = comparator;
        }
        */

        public void Enqueue(Node item) {
            // NO S� SI TENDR�A QUE CALCULAR AQU� LA PRIORIDAD O QUE ME LA METAN DESDE FUERA (Lo que pasa es que entonces no respeto el interfaz, no?)
            priorityQueue.Enqueue(item, 1d); // ME HE INVENTADO PRIORIDAD 1... DEBER�A MIRAR EL COSTE DE LA RUTA O ALGO AS�
            // EN VEZ DE NULL TENGO QUE SER CAPAZ DE GENERAR UN VALOR DE TPRIORITY... HABR� QUE PENSAR C�MO HACER ESTO DE OTRA MANERA
        }

        public Node Dequeue() {
            return priorityQueue.Dequeue();
        }

        int Count() {
            return priorityQueue.Count;
        }

        
        public bool Contains(Node item) {
            return priorityQueue.Contains(item);
        }

        public IEnumerator<Node> GetEnumerator() {
            return priorityQueue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return priorityQueue.GetEnumerator();
        }


        /*
        public PriorityQueue(PriorityQueue<E> c)
            : base(c)
        {
        }

        public PriorityQueue(SortedSet<E> c)
            : base(c)
        {

        }


        public void Remove(E e) {
            // remove
        }


        public bool IsEmpty()
        {
            return 0 == this.Count; // en vez de size();
        }

        public E Pop()
        {
            return this.pop(); //en vez de poll();
        }

        public Queue<E> Insert(E element)
        {
            if (this.Contains(element)) // En vez de offer(element)
            {
                return this;
            }
            return null;
        }
                */

    }
}