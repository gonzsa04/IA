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
    using UCM.IAV.AI.Logic.Propositional.Visitors;

    [TestFixture] // Esto significa que en esta clase hay pruebas unitarias
    public class ConvertToConjunctionOfClausesTest {

        private PLParser parser = new PLParser();

        [SetUp] // El @Before de Java (lo tienen pero no lo utilizan)
        public void SetUp() {
        }

        [Test]
        public void TestSymbolTransform() {
            Sentence symbol = parser.Parse("A");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(symbol);
            Assert.AreEqual("{{A}}", transformed.ToString());
        }

        [Test]
        public void TestBasicSentenceTransformation() {
            Sentence and = parser.Parse("A & B");
            ConjunctionOfClauses transformedAnd = ConvertToConjunctionOfClauses.Convert(and);
            Assert.AreEqual("{{A}, {B}}", transformedAnd.ToString());

            Sentence or = parser.Parse("A | B");
            ConjunctionOfClauses transformedOr = ConvertToConjunctionOfClauses.Convert(or);
            Assert.AreEqual("{{A, B}}", transformedOr.ToString());

            Sentence not = parser.Parse("~C");
            ConjunctionOfClauses transformedNot = ConvertToConjunctionOfClauses.Convert(not);
            Assert.AreEqual("{{~C}}", transformedNot.ToString());
        }

        [Test]
        public void TestImplicationTransformation() {
            Sentence impl = parser.Parse("A => B");
            ConjunctionOfClauses transformedImpl = ConvertToConjunctionOfClauses.Convert(impl);
            Assert.AreEqual("{{~A, B}}", transformedImpl.ToString());
        }

        [Test]
        public void TestBiConditionalTransformation() {
            Sentence bic = parser.Parse("A <=> B");
            ConjunctionOfClauses transformedBic = ConvertToConjunctionOfClauses.Convert(bic);
            Assert.AreEqual("{{~A, B}, {~B, A}}", transformedBic.ToString());
        }

        [Test]
        public void TestTwoSuccessiveNotsTransformation() {
            Sentence twoNots = parser.Parse("~~A");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(twoNots);
            Assert.AreEqual("{{A}}", transformed.ToString());
        }

        [Test]
        public void TestThreeSuccessiveNotsTransformation() {
            Sentence threeNots = parser.Parse("~~~A");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(threeNots);
            Assert.AreEqual("{{~A}}", transformed.ToString());
        }

        [Test]
        public void TestFourSuccessiveNotsTransformation() {
            Sentence fourNots = parser.Parse("~~~~A");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(fourNots);
            Assert.AreEqual("{{A}}", transformed.ToString());
        }

        [Test]
        public void TestDeMorgan1() {
            Sentence dm = parser.Parse("~(A & B)");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(dm);
            Assert.AreEqual("{{~A, ~B}}", transformed.ToString());
        }

        [Test]
        public void TestDeMorgan2() {
            Sentence dm = parser.Parse("~(A | B)");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(dm);
            Assert.AreEqual("{{~A}, {~B}}", transformed.ToString());
        }

        [Test]
        public void TestOrDistribution1() {
            Sentence or = parser.Parse("A & B | C)");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(or);
            Assert.AreEqual("{{A, C}, {B, C}}", transformed.ToString());
        }

        [Test]
        public void TestOrDistribution2() {
            Sentence or = parser.Parse("A | B & C");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(or);
            Assert.AreEqual("{{A, B}, {A, C}}", transformed.ToString());
        }

        [Test]
        public void TestAimaExample() {
            Sentence aimaEg = parser.Parse("B11 <=> P12 | P21");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(aimaEg);
            Assert.AreEqual("{{~B11, P12, P21}, {~P12, B11}, {~P21, B11}}", transformed.ToString());
        }

        [Test]
        public void TestNested() {
            Sentence nested = parser.Parse("A | (B | (C | (D & E)))");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(nested);
            Assert.AreEqual("{{A, B, C, D}, {A, B, C, E}}", transformed.ToString());

            nested = parser.Parse("A | (B | (C & (D & E)))");
            transformed = ConvertToConjunctionOfClauses.Convert(nested);
            Assert.AreEqual("{{A, B, C}, {A, B, D}, {A, B, E}}", transformed.ToString());

            nested = parser.Parse("A | (B | (C & (D & (E | F))))");
            transformed = ConvertToConjunctionOfClauses.Convert(nested);
            Assert.AreEqual("{{A, B, C}, {A, B, D}, {A, B, E, F}}", transformed.ToString());

            nested = parser.Parse("(A | (B | (C & D))) | E | (F | (G | (H & I)))");
            transformed = ConvertToConjunctionOfClauses.Convert(nested);
            Assert.AreEqual("{{A, B, C, E, F, G, H}, {A, B, D, E, F, G, H}, {A, B, C, E, F, G, I}, {A, B, D, E, F, G, I}}", transformed.ToString());

            nested = parser.Parse("(((~P | ~Q) => ~(P | Q)) => R)");
            transformed = ConvertToConjunctionOfClauses.Convert(nested);
            Assert.AreEqual("{{~P, ~Q, R}, {P, Q, R}}", transformed.ToString());

            nested = parser.Parse("~(((~P | ~Q) => ~(P | Q)) => R)");
            transformed = ConvertToConjunctionOfClauses.Convert(nested);
            Assert.AreEqual("{{P, ~P}, {Q, ~P}, {P, ~Q}, {Q, ~Q}, {~R}}", transformed.ToString());
        }

        [Test]
        public void TestIssue78() {
            // (  ( NOT J1007 )  OR  ( NOT ( OR J1008 J1009 J1010 J1011 J1012 J1013 J1014 J1015  )  )  )
            Sentence issue78Eg = parser.Parse("(  ( ~ J1007 )  |  ( ~ ( J1008 | J1009 | J1010 | J1011 | J1012 | J1013 | J1014 | J1015  )  ) )");
            ConjunctionOfClauses transformed = ConvertToConjunctionOfClauses.Convert(issue78Eg);
            Assert.AreEqual("{{~J1007, ~J1008}, {~J1007, ~J1009}, {~J1007, ~J1010}, {~J1007, ~J1011}, {~J1007, ~J1012}, {~J1007, ~J1013}, {~J1007, ~J1014}, {~J1007, ~J1015}}", transformed.ToString());
        }
    }
}