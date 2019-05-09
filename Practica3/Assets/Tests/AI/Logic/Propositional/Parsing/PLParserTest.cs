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

    using UCM.IAV.AI.Logic;

    [TestFixture] // Esto significa que en esta clase hay pruebas unitarias
    public class PLParserTest {

        private PLParser parser = null;
        private Sentence sentence = null;
        private string expected = null;

        [SetUp] // El @Before de Java
        public void SetUp() {
            parser = new PLParser();
        }

        [Test]
        public void TestAtomicSentenceTrueParse() {
            sentence = parser.Parse("true");
            expected = prettyPrintF("True");
            Assert.IsTrue(sentence.IsPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.Parse("(true)");
            expected = prettyPrintF("True");
            Assert.IsTrue(sentence.IsPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.Parse("((true))");
            expected = prettyPrintF("True");
            Assert.IsTrue(sentence.IsPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [Test]
        public void TestAtomicSentenceFalseParse() {
            sentence = parser.Parse("faLse");
            expected = prettyPrintF("False");
            Assert.IsTrue(sentence.IsPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [Test]
        public void TestAtomicSentenceSymbolParse() {
            sentence = parser.Parse("AIMA");
            expected = prettyPrintF("AIMA");
            Assert.IsTrue(sentence.IsPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [Test]
        public void TestNotSentenceParse() {
            sentence = parser.Parse("~ AIMA");
            expected = prettyPrintF("~AIMA");
            Assert.IsTrue(sentence.IsNotSentence());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [Test]
        public void TestDoubleNegation() {
            sentence = parser.Parse("~~AIMA");
            expected = prettyPrintF("~~AIMA");
            Assert.IsTrue(sentence.IsNotSentence());
            Assert.IsTrue(sentence.GetSimplerSentence(0).IsNotSentence());
            Assert.IsTrue(sentence.GetSimplerSentence(0).GetSimplerSentence(0).IsPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [Test]
        public void TestBinarySentenceParse() {
            sentence = parser.Parse("PETER  &  NORVIG");
            expected = prettyPrintF("PETER & NORVIG");
            Assert.IsTrue(sentence.IsAndSentence());
            Assert.IsTrue(sentence.GetSimplerSentence(0).IsPropositionSymbol());
            Assert.IsTrue(sentence.GetSimplerSentence(1).IsPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [Test]
        public void TestComplexSentenceParse() {
            sentence = parser.Parse("(NORVIG | AIMA | LISP) & TRUE");
            expected = prettyPrintF("(NORVIG | AIMA | LISP) & True");
            Assert.IsTrue(sentence.IsAndSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.Parse("((NORVIG | AIMA | LISP) & (((LISP => COOL))))");
            expected = prettyPrintF("(NORVIG | AIMA | LISP) & (LISP => COOL)");
            Assert.IsTrue(sentence.IsAndSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.Parse("((~ (P & Q ))  & ((~ (R & S))))");
            expected = prettyPrintF("~(P & Q) & ~(R & S)");
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.Parse("((P & Q) | (S & T))");
            expected = prettyPrintF("P & Q | S & T");
            Assert.IsTrue(sentence.IsOrSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.Parse("(~ ((P & Q) => (S & T)))");
            expected = prettyPrintF("~(P & Q => S & T)");
            Assert.IsTrue(sentence.IsNotSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.Parse("(~ (P <=> (S & T)))");
            expected = prettyPrintF("~(P <=> S & T)");
            Assert.IsTrue(sentence.IsNotSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.Parse("(P <=> (S & T))");
            expected = prettyPrintF("P <=> S & T");
            Assert.IsTrue(sentence.IsBiconditionalSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.Parse("(P => Q)");
            expected = prettyPrintF("P => Q");
            Assert.IsTrue(sentence.IsImplicationSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.Parse("((P & Q) => R)");
            expected = prettyPrintF("P & Q => R");
            Assert.IsTrue(sentence.IsImplicationSentence());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [Test]
        public void TestSquareBracketsParse() {
            // En lugar de corchetes, paréntesis
            sentence = parser.Parse("[NORVIG | AIMA | LISP] & TRUE");
            expected = prettyPrintF("(NORVIG | AIMA | LISP) & True");
            Assert.AreEqual(expected, sentence.ToString());

            // Alternando paréntesis y corchetes
            sentence = parser.Parse("[A | B | C] & D & [C | D & (F | G | H & [I | J])]");
            expected = prettyPrintF("(A | B | C) & D & (C | D & (F | G | H & (I | J)))");
            Assert.AreEqual(expected, sentence.ToString());
        }

        [Test]
        public void TestParserException() {
            try {
                sentence = parser.Parse("");
                Assert.Fail("A Parser Exception should have been thrown.");
            } catch (ParserException pex) {
                Assert.AreEqual(0, pex.GetProblematicTokens().Count);
            }

            try {
                sentence = parser.Parse("A A1.2");
                Assert.Fail("A Parser Exception should have been thrown.");
            } catch (ParserException pex) {
                Assert.AreEqual(0, pex.GetProblematicTokens().Count);
                Assert.IsTrue(pex.InnerException is LexerException); // No hay pex.getCause()... miro la InnerException (no creo que haga faltar ir al raíz, al GetBaseException)
                Assert.AreEqual(4, ((LexerException)pex.InnerException).GetCurrentPositionInInputExceptionThrown());
            }

            try {
                sentence = parser.Parse("A & & B");
                Assert.Fail("A Parser Exception should have been thrown.");
            } catch (ParserException pex) {
                Assert.AreEqual(1, pex.GetProblematicTokens().Count);
                Assert.IsTrue(pex.GetProblematicTokens()[0].GetTokenType() == LogicTokenTypes.CONNECTIVE);
                Assert.AreEqual(4, pex.GetProblematicTokens()[0].GetStartCharPositionInInput());
            }

            try {
                sentence = parser.Parse("A & (B & C &)");
                Assert.Fail("A Parser Exception should have been thrown.");
            } catch (ParserException pex) {
                Assert.AreEqual(1, pex.GetProblematicTokens().Count);
                Assert.IsTrue(pex.GetProblematicTokens()[0].GetTokenType() == LogicTokenTypes.CONNECTIVE);
                Assert.AreEqual(11, pex.GetProblematicTokens()[0].GetStartCharPositionInInput());
            }
        }

        [Test]
        public void TestIssue72() {
            // filter1 AND filter2 AND filter3 AND filter4
            sentence = parser.Parse("filter1 & filter2 & filter3 & filter4");
            expected = prettyPrintF("filter1 & filter2 & filter3 & filter4");
            Assert.AreEqual(expected, sentence.ToString());

            // (filter1 AND filter2) AND (filter3 AND filter4)
            sentence = parser.Parse("(filter1 & filter2) & (filter3 & filter4)");
            expected = prettyPrintF("filter1 & filter2 & filter3 & filter4");
            Assert.AreEqual(expected, sentence.ToString());

            // ((filter1 AND filter2) AND (filter3 AND filter4))
            sentence = parser.Parse("((filter1 & filter2) & (filter3 & filter4))");
            expected = prettyPrintF("filter1 & filter2 & filter3 & filter4");
            Assert.AreEqual(expected, sentence.ToString());
        }

        private string prettyPrintF(string prettyPrintedFormula) {
            Sentence s = parser.Parse(prettyPrintedFormula);
             
            // No sé por qué ponía "" + s... supongo que para forzar la conversión a cadena, yo uso ToString
            // Se podrían meter opciones al final para que la comparación sea case-sensitive y cosas así... pero no lo estoy haciendo por ahora
            Assert.AreEqual(prettyPrintedFormula, s.ToString(), "The pretty print formula should parse and print the same.");

            return prettyPrintedFormula;
        }
    }
}
