/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Visitors {

    using System.Collections.Generic;

    using NUnit.Framework;

    using UCM.IAV.AI.Logic.Propositional.Parsing;

    [TestFixture] // Esto significa que en esta clase hay pruebas unitarias
    public class SymbolCollectorTest {

        private PLParser parser;

        [SetUp] // El @Before de Java
        public void SetUp() {
            parser = new PLParser();
        }

        [Test]
        public void TestCollectSymbolsFromComplexSentence() {
            Sentence sentence = (Sentence)parser.Parse("(~B11 | P12 | P21) & (B11 | ~P12) & (B11 | ~P21)");
            ISet<Sentence> s = new HashSet<Sentence>(SymbolCollector.GetSymbolsFrom(sentence)); // En C# si usas ISet<PropositionSymbol> no te va a dejar mirar si Contains algo que son Sentences :-(
            Assert.AreEqual(3, s.Count);
            Sentence b11 = parser.Parse("B11");
            Sentence p21 = parser.Parse("P21");
            Sentence p12 = parser.Parse("P12");
            Assert.IsTrue(s.Contains(b11));
            Assert.IsTrue(s.Contains(p21));
            Assert.IsTrue(s.Contains(p12));
        }
    }
}