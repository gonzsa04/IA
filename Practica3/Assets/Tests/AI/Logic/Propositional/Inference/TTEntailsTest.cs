/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Inference {

    using System.Collections;

    using UnityEngine.TestTools;
    using NUnit.Framework;

    using UCM.IAV.AI.Logic.Propositional.Parsing;
    using UCM.IAV.AI.Logic.Propositional.KB;
    
    [TestFixture] // Esto significa que en esta clase hay pruebas unitarias
    public class TTEntailsTest {

        //TTEntails tte;

        KnowledgeBase kb;

        [SetUp] // El @Before de Java
        public void SetUp() {
            //tte = new TTEntails();
            kb = new KnowledgeBase();
        }

        [TearDown] // El @After de Java (ahora mismo no lo estoy usando)
        public void TearDown() { 
        }

        // Un UnityTest se comporta como una corrutina en PlayMode y permite hacer yield null para saltarse un frame en EditMode 
        [UnityTest]
        public IEnumerator TestScriptWithEnumeratorPasses() {
            // Aquí se usaría la clase Assert para probar que se dan las condiciones del test 
            // yield sirve para saltarse un frame
            yield return null;
        } 

        /* Los métodos de test (dentro se utiliza la clase Assert para probar que se cumplen las condiciones) */

        [Test]
        public void TestSimpleSentence1() {
            kb.Tell("A & B");
            Assert.AreEqual(true, kb.AskWithTTEntails("A"));
        }

        [Test] // Me está fallando este test ahora mismo... está dando true
        public void TestSimpleSentence2() {
            kb.Tell("A | B");
            Assert.AreEqual(false, kb.AskWithTTEntails("A"));
        }

        [Test]  
        public void TestSimpleSentence3() {
            kb.Tell("A | B");
            Assert.AreEqual(false, kb.AskWithTTEntails("B"));
        }

        [Test]
        public void TestSimpleSentence4() {
            kb.Tell("(A => B) & A");
            Assert.AreEqual(true, kb.AskWithTTEntails("B"));
        }

        [Test]
        public void TestSimpleSentence5() {
            kb.Tell("(A => B) & B");
            Assert.AreEqual(false, kb.AskWithTTEntails("A"));
        }

        [Test]
        public void TestSimpleSentence6() {
            kb.Tell("A");
            Assert.AreEqual(false, kb.AskWithTTEntails("~A"));
        }

        [Test]
        public void TestSUnkownSymbol() {
            kb.Tell("(A => B) & B");
            Assert.AreEqual(false, kb.AskWithTTEntails("X"));
        }

        [Test]
        public void TestSimpleSentence7() {
            kb.Tell("~A");
            Assert.AreEqual(false, kb.AskWithTTEntails("A"));
        }

        [Test]
        public void TestNewAIMAExample() {
            kb.Tell("~P11");
            kb.Tell("B11 <=> P12 | P21");
            kb.Tell("B21 <=> P11 | P22 | P31");
            kb.Tell("~B11");
            kb.Tell("B21");

            Assert.AreEqual(true, kb.AskWithTTEntails("~P12"));
            Assert.AreEqual(false, kb.AskWithTTEntails("P22"));
        }

        [Test]
        public void TestTTEntailsSucceedsWithChadCarffsBugReport() {
            KnowledgeBase kb = new KnowledgeBase();
            kb.Tell("B12 <=> P11 | P13 | P22 | P02");
            kb.Tell("B21 <=> P20 | P22 | P31 | P11");
            kb.Tell("B01 <=> P00 | P02 | P11");
            kb.Tell("B10 <=> P11 | P20 | P00");
            kb.Tell("~B21");
            kb.Tell("~B12");
            kb.Tell("B10");
            kb.Tell("B01");

            Assert.IsTrue(kb.AskWithTTEntails("P00"));
            Assert.IsFalse(kb.AskWithTTEntails("~P00"));
        }

        [Test]
        public void TestDoesNotKnow() {
            KnowledgeBase kb = new KnowledgeBase();
            kb.Tell("A");
            Assert.IsFalse(kb.AskWithTTEntails("B"));
            Assert.IsFalse(kb.AskWithTTEntails("~B"));
        }

        public void TestTTEntailsSucceedsWithCStackOverFlowBugReport() {
            KnowledgeBase kb = new KnowledgeBase();

            Assert.IsTrue(kb.AskWithTTEntails("((A | (~ A)) & (A | B))"));
        }

        [Test]
        public void TestModelEvaluation() {
            kb.Tell("~P11");
            kb.Tell("B11 <=> P12 | P21");
            kb.Tell("B21 <=> P11 | P22 | P31");
            kb.Tell("~B11");
            kb.Tell("B21");

            Model model = new Model();
            model = model.Union(new PropositionSymbol("B11"), false);
            model = model.Union(new PropositionSymbol("B21"), true);
            model = model.Union(new PropositionSymbol("P11"), false);
            model = model.Union(new PropositionSymbol("P12"), false);
            model = model.Union(new PropositionSymbol("P21"), false);
            model = model.Union(new PropositionSymbol("P22"), false);
            model = model.Union(new PropositionSymbol("P31"), true);

            Sentence kbs = kb.AsSentence();
            Assert.AreEqual(true, model.IsTrue(kbs));
        }
        
        [Test]
        // El ejemplo que vimos en clase
        public void TestTTEntailsCluedoExample() {
            //KnowledgeBase kb = new KnowledgeBase();
            kb.Tell("PradoSOBRE <=> ~PradoH0 & ~PradoB1 & ~PradoB2");
            kb.Tell("PradoH0 <=> ~PradoB1 & ~PradoB2 & ~PradoSOBRE");
            kb.Tell("PradoB1 <=> ~PradoB2 & ~PradoSOBRE & ~PradoH0");
            kb.Tell("PradoB2 <=> ~PradoSOBRE & ~PradoH0 & ~PradoB1"); 
            kb.Tell("~PradoH0");
            kb.Tell("~PradoB1");
            kb.Tell("~PradoB2"); 

            Assert.IsTrue(kb.AskWithTTEntails("PradoSOBRE")); 
        }

    }
}
