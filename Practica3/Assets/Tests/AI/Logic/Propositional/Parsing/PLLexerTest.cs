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
    public class PLLexerTest {

        private PLLexer pllexer;

        [SetUp] // El @Before de Java
        public void SetUp() {
            pllexer = new PLLexer();
        }

        [Test]
        public void TestLexBasicExpression() {
            pllexer.SetInput("(P)");
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 0),
                    pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.SYMBOL, "P", 1),
                    pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.RPAREN, ")", 2),
                    pllexer.NextToken());

            Assert.AreEqual(new Token(LogicTokenTypes.EOI, "EOI", 3),
                    pllexer.NextToken());
        }

        [Test]
        public void TestLexNotExpression() {
            pllexer.SetInput("(~ P)");
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 0),
                    pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "~", 1),
                    pllexer.NextToken());

            Assert.AreEqual(new Token(LogicTokenTypes.SYMBOL, "P", 3),
                    pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.RPAREN, ")", 4),
                    pllexer.NextToken());

            Assert.AreEqual(new Token(LogicTokenTypes.EOI, "EOI", 5),
                    pllexer.NextToken());
        }

        [Test]
        public void TestLexImpliesExpression() {
            pllexer.SetInput("(P => Q)");
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 0),
                    pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.SYMBOL, "P", 1),
                    pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "=>", 3),
                    pllexer.NextToken());
        }

        [Test]
        public void TestLexBiCOnditionalExpression() {
            pllexer.SetInput("(B11 <=> (P12 | P21))");
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 0), pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.SYMBOL, "B11", 1), pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "<=>", 5),
                    pllexer.NextToken());
        }

        [Test]
        public void TestChainedConnectiveExpression() {
            pllexer.SetInput("~~&&||=>=><=><=>");
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "~", 0), pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "~", 1), pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "&", 2), pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "&", 3), pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "|", 4), pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "|", 5), pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "=>", 6), pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "=>", 8), pllexer.NextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "<=>", 10), pllexer.NextToken());
        }

        [Test]
        public void TestLexerException() {
            try {
                pllexer.SetInput("A & B.1 & C");
                pllexer.NextToken();
                pllexer.NextToken();
                pllexer.NextToken();
                // La coma después de 'B' no es un carácter legal
                pllexer.NextToken();
                Assert.Fail("A LexerException should have been thrown here");
            } catch (LexerException le) {
                // Una manera curiosa de comprobar la excepción... poniendo asserts en el catch!
                // Asegura que la posición correcta de la entrada ha sido identificada
                Assert.AreEqual(5, le.GetCurrentPositionInInputExceptionThrown());
            }
        }
    }
}
