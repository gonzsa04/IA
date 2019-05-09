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

    using NUnit.Framework;

    [TestFixture] // Esto significa que en esta clase hay pruebas unitarias
    public class KnowledgeBaseTest {

        private KnowledgeBase kb;

        [SetUp] // El @Before de Java
        public void SetUp() {
            kb = new KnowledgeBase();
        }

        [TearDown] // El @After de Java (ahora mismo no se está utilizando)
        public void TearDown() { 
        }

        /* Los métodos de test (dentro se utiliza la clase Assert para probar que se cumplen las condiciones) */

        [Test]  
        public void TestTellInsertsSentence() {
            kb.Tell("(A & B)");
            Assert.AreEqual(1, kb.Count());
        }

        [Test]
        public void TestTellDoesNotInsertSameSentenceTwice() {
            kb.Tell("(A & B)");
            Assert.AreEqual(1, kb.Count());
            kb.Tell("(A & B)");
            Assert.AreEqual(1, kb.Count());
        }

        [Test]
        public void TestEmptyKnowledgeBaseIsAnEmptyString() {
            Assert.AreEqual("", kb.ToString());
        }

        [Test]
        public void TestKnowledgeBaseWithOneSentenceToString() {
            kb.Tell("(A & B)");
            Assert.AreEqual("A & B", kb.ToString());
        }

        [Test]
        public void TestKnowledgeBaseWithTwoSentencesToString() {
            kb.Tell("(A & B)");
            kb.Tell("(C & D)");
            Assert.AreEqual("A & B & C & D", kb.ToString());
        }

        [Test]
        public void TestKnowledgeBaseWithThreeSentencesToString() {
            kb.Tell("(A & B)");
            kb.Tell("(C & D)");
            kb.Tell("(E & F)");
            Assert.AreEqual("A & B & C & D & E & F", kb.ToString());
        }
    }
}
