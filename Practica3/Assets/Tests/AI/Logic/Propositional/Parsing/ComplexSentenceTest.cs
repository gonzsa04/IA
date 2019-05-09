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
    using NUnit.Framework;

    [TestFixture] // Esto significa que en esta clase hay pruebas unitarias
    public class ComplexSentenceTest {

        [Test]
        public void Test_ArgumentNullExceptionOnConstruction_1() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentNullException>(() => new ComplexSentence(null, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B") }));
        }

        [Test]
        public void Test_ArgumentNullExceptionOnConstruction_2() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentNullException>(() => new ComplexSentence(Connective.NOT, (Sentence[])null));
        }

        [Test]
        public void Test_ArgumentExceptionOnConstruction_1() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.NOT, new Sentence[] { }));
        }

        [Test]
        public void Test_ArgumentExceptionOnConstruction_2() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.NOT, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B") }));
        }

        [Test]
        public void Test_ArgumentExceptionOnConstruction_3() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.AND, new Sentence[] { new PropositionSymbol("A") }));
        }

        [Test]
        public void Test_ArgumentExceptionOnConstruction_4() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.AND, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C") }));
        }

        [Test]
        public void Test_ArgumentExceptionOnConstruction_5() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.OR, new Sentence[] { new PropositionSymbol("A") }));
        }

        [Test]
        public void Test_ArgumentExceptionOnConstruction_6() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.OR, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C") }));
        }

        [Test]
        public void Test_ArgumentExceptionOnConstruction_7() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.IMPLICATION, new Sentence[] { new PropositionSymbol("A") }));
        }

        [Test]
        public void Test_ArgumentExceptionOnConstruction_8() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.IMPLICATION, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C") }));
        }

        [Test]
        public void Test_ArgumentExceptionOnConstruction_9() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.BICONDITIONAL, new Sentence[] { new PropositionSymbol("A") }));
        }

        [Test]
        public void Test_ArgumentExceptionOnConstruction_10() {
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.BICONDITIONAL, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C") }));
        }
    }
}