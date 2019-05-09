/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.KB {

    using System.Collections.Generic;

    using NUnit.Framework;

    using UCM.IAV.AI.Util;
    using UCM.IAV.AI.Logic.Propositional.Parsing; 

    [TestFixture] // Esto significa que en esta clase hay pruebas unitarias
    public class ModelTest {

        private Model m;

        private PLParser parser;

        Sentence trueSentence, falseSentence, andSentence, simpleOrSentence, orSentence,
                impliedSentence, biConditionalSentence;

        [SetUp] // El @Before de Java
        public void SetUp() {
            parser = new PLParser();

            trueSentence = (Sentence)parser.Parse("true");
            falseSentence = (Sentence)parser.Parse("false");
            andSentence = (Sentence)parser.Parse("(P  &  Q)");
            simpleOrSentence = (Sentence)parser.Parse("A | B");
            orSentence = (Sentence)parser.Parse("(P  |  Q)");
            impliedSentence = (Sentence)parser.Parse("(P  =>  Q)");
            biConditionalSentence = (Sentence)parser.Parse("(P  <=>  Q)");

            m = new Model();
        }

        [Test]
        public void TestEmptyModel() {
            Assert.AreEqual(null, m.GetValue(new PropositionSymbol("P")));
            Assert.AreEqual(true, m.IsUnknown(new PropositionSymbol("P")));
        }

        [Test]
        public void TestExtendModel() {
            string p = "P";
            m = m.Union(new PropositionSymbol(p), true);
            Assert.AreEqual(true, m.GetValue(new PropositionSymbol("P")));
        }

        [Test]
        public void TestTrueFalseEvaluation() {
            Assert.AreEqual(true, m.IsTrue(trueSentence));
            Assert.AreEqual(false, m.IsFalse(trueSentence));
            Assert.AreEqual(false, m.IsTrue(falseSentence));
            Assert.AreEqual(true, m.IsFalse(falseSentence));
        }

        [Test]
        public void TestSentenceStatusWhenPTrueAndQTrue() {
            string p = "P";
            string q = "Q";
            m = m.Union(new PropositionSymbol(p), true);
            m = m.Union(new PropositionSymbol(q), true);
            Assert.AreEqual(true, m.IsTrue(andSentence));
            Assert.AreEqual(true, m.IsTrue(orSentence));
            Assert.AreEqual(true, m.IsTrue(impliedSentence));
            Assert.AreEqual(true, m.IsTrue(biConditionalSentence));

            m = m.Union(new PropositionSymbol("A"), true);
            m = m.Union(new PropositionSymbol("B"), true);
            Assert.AreEqual(true, m.IsTrue(simpleOrSentence));
        }

        [Test]
        public void TestSentenceStatusWhenPFalseAndQFalse() {
            string p = "P";
            string q = "Q";
            m = m.Union(new PropositionSymbol(p), false);
            m = m.Union(new PropositionSymbol(q), false);
            Assert.AreEqual(true, m.IsFalse(andSentence));
            Assert.AreEqual(true, m.IsFalse(orSentence));
            Assert.AreEqual(true, m.IsTrue(impliedSentence));
            Assert.AreEqual(true, m.IsTrue(biConditionalSentence));

            m = m.Union(new PropositionSymbol("A"), false);
            m = m.Union(new PropositionSymbol("B"), false);
            Assert.AreEqual(true, m.IsFalse(simpleOrSentence));
        }

        [Test]
        public void TestSentenceStatusWhenPTrueAndQFalse() {
            string p = "P";
            string q = "Q";
            m = m.Union(new PropositionSymbol(p), true);
            m = m.Union(new PropositionSymbol(q), false);
            Assert.AreEqual(true, m.IsFalse(andSentence));
            Assert.AreEqual(true, m.IsTrue(orSentence));
            Assert.AreEqual(true, m.IsFalse(impliedSentence));
            Assert.AreEqual(true, m.IsFalse(biConditionalSentence));

            m = m.Union(new PropositionSymbol("A"), true);
            m = m.Union(new PropositionSymbol("B"), false);
            Assert.AreEqual(true, m.IsTrue(simpleOrSentence));
        }

        [Test]
        public void TestSentenceStatusWhenPFalseAndQTrue() {
            string p = "P";
            string q = "Q";
            m = m.Union(new PropositionSymbol(p), false);
            m = m.Union(new PropositionSymbol(q), true);
            Assert.AreEqual(true, m.IsFalse(andSentence));
            Assert.AreEqual(true, m.IsTrue(orSentence));
            Assert.AreEqual(true, m.IsTrue(impliedSentence));
            Assert.AreEqual(true, m.IsFalse(biConditionalSentence));

            m = m.Union(new PropositionSymbol("A"), false);
            m = m.Union(new PropositionSymbol("B"), true);
            Assert.AreEqual(true, m.IsTrue(simpleOrSentence));
        }

        [Test]
        public void TestComplexSentence() {
            string p = "P";
            string q = "Q";
            m = m.Union(new PropositionSymbol(p), true);
            m = m.Union(new PropositionSymbol(q), false);
            Sentence sent = (Sentence)parser.Parse("((P | Q) &  (P => Q))");
            Assert.IsFalse(m.IsTrue(sent));
            Assert.IsTrue(m.IsFalse(sent));
            Sentence sent2 = (Sentence)parser.Parse("((P | Q) & (Q))");
            Assert.IsFalse(m.IsTrue(sent2));
            Assert.IsTrue(m.IsFalse(sent2));
        }
    }
}