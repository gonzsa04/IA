/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Parsing {

    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture] // Esto significa que en esta clase hay pruebas unitarias
    public class ListTest {

        [Test]
        public void TestListOfSymbolsClone() {
            IList<PropositionSymbol> l = new List<PropositionSymbol>();
            l.Add(new PropositionSymbol("A"));
            l.Add(new PropositionSymbol("B"));
            l.Add(new PropositionSymbol("C"));
            IList<PropositionSymbol> l2 = new List<PropositionSymbol>(l);
            l2.Remove(new PropositionSymbol("B"));
            // Ojo, no cometas el error de confundirte y usar el método Equals (de hecho en la clase Assert lo han ocultado a propósito)
            Assert.AreEqual(l.Count, 3);
            Assert.AreEqual(l2.Count, 2);
        }

        [Test]
        public void TestListRemove() {
            IList<int> one = new List<int>(); //Integer
            one.Add(1); // new Integer(1)
            // Ojo, no cometas el error de confundirte y usar el método Equals (de hecho en la clase Assert lo han ocultado a propósito)
            Assert.AreEqual(one.Count, 1);
            one.Remove(1); // Aquí había un one.Remove(0) porque en Java se indicaba la posición de lo que había que borrar... pero no, se trata de borrar lo mismo que hemos metido
            Assert.AreEqual(0, one.Count);
        }
    }
}
