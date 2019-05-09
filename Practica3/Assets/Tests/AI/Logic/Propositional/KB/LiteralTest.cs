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

    using UCM.IAV.AI.Logic.Propositional.Parsing;

    [TestFixture] // Esto significa que en esta clase hay pruebas unitarias
    public class LiteralTest {

        private readonly PropositionSymbol SYMBOL_P = new PropositionSymbol("P");
        private readonly PropositionSymbol SYMBOL_Q = new PropositionSymbol("Q");

        [Test]
        public void TestIsPositiveLiteral() {
            Literal literal = new Literal(SYMBOL_P);
            Assert.IsTrue(literal.IsPositiveLiteral());

            literal = new Literal(SYMBOL_P, true);
            Assert.IsTrue(literal.IsPositiveLiteral());

            literal = new Literal(SYMBOL_P, false);
            Assert.IsFalse(literal.IsPositiveLiteral());
        }

        [Test]
        public void TestIsNegativeLiteral() {
            Literal literal = new Literal(SYMBOL_P);
            Assert.IsFalse(literal.IsNegativeLiteral());

            literal = new Literal(SYMBOL_P, true);
            Assert.IsFalse(literal.IsNegativeLiteral());

            literal = new Literal(SYMBOL_P, false);
            Assert.IsTrue(literal.IsNegativeLiteral());
        }

        [Test]
        public void TestGetAtomicSentence() {
            Literal literal = new Literal(SYMBOL_P);
            Assert.AreSame(literal.GetAtomicSentence(), SYMBOL_P);
        }

        [Test]
        public void TestIsAlwaysTrue() {
            Literal literal = new Literal(SYMBOL_P);
            Assert.IsFalse(literal.IsAlwaysTrue());

            literal = new Literal(PropositionSymbol.TRUE);
            Assert.IsTrue(literal.IsAlwaysTrue());

            literal = new Literal(PropositionSymbol.TRUE, false);
            Assert.IsFalse(literal.IsAlwaysTrue());

            literal = new Literal(PropositionSymbol.FALSE);
            Assert.IsFalse(literal.IsAlwaysTrue());

            literal = new Literal(PropositionSymbol.FALSE, false);
            Assert.IsTrue(literal.IsAlwaysTrue());
        }

        [Test]
        public void TestIsAlwaysFalse() {
            Literal literal = new Literal(SYMBOL_P);
            Assert.IsFalse(literal.IsAlwaysFalse());

            literal = new Literal(PropositionSymbol.TRUE);
            Assert.IsFalse(literal.IsAlwaysFalse());

            literal = new Literal(PropositionSymbol.TRUE, false);
            Assert.IsTrue(literal.IsAlwaysFalse());

            literal = new Literal(PropositionSymbol.FALSE);
            Assert.IsTrue(literal.IsAlwaysFalse());

            literal = new Literal(PropositionSymbol.FALSE, false);
            Assert.IsFalse(literal.IsAlwaysFalse());
        }

        [Test]
        public void TestToString() {
            Literal literal = new Literal(SYMBOL_P);
            Assert.AreEqual("P", literal.ToString());

            literal = new Literal(SYMBOL_P, false);
            Assert.AreEqual("~P", literal.ToString());
        }

        [Test]
        public void TestEquals() {
            Literal literal1 = new Literal(SYMBOL_P);
            Literal literal2 = new Literal(SYMBOL_P);
            Assert.IsTrue(literal1.Equals(literal2));

            literal1 = new Literal(SYMBOL_P, false);
            literal2 = new Literal(SYMBOL_P, false);
            Assert.IsTrue(literal1.Equals(literal2));

            literal1 = new Literal(SYMBOL_P);
            literal2 = new Literal(SYMBOL_P, false);
            Assert.IsFalse(literal1.Equals(literal2));

            literal1 = new Literal(SYMBOL_P);
            literal2 = new Literal(SYMBOL_Q);
            Assert.IsFalse(literal1.Equals(literal2));

            literal1 = new Literal(SYMBOL_P);
            Assert.IsFalse(literal1.Equals(SYMBOL_P));
        }

        [Test]
        public void TestHashCode() {
            Literal literal1 = new Literal(SYMBOL_P);
            Literal literal2 = new Literal(SYMBOL_P);
            Assert.IsTrue(literal1.GetHashCode() == literal2.GetHashCode());

            literal1 = new Literal(SYMBOL_P, false);
            literal2 = new Literal(SYMBOL_P, false);
            Assert.IsTrue(literal1.GetHashCode() == literal2.GetHashCode());

            literal1 = new Literal(SYMBOL_P);
            literal2 = new Literal(SYMBOL_P, false);
            Assert.IsFalse(literal1.GetHashCode() == literal2.GetHashCode());
        }
    }
} 
