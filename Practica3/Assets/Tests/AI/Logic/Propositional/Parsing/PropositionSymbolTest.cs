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
    public class PropositionSymbolTest {

        [Test]
        public void Test_isAlwaysTrueSymbol() {
            Assert.IsTrue(PropositionSymbol.IsAlwaysTrueSymbol("True"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysTrueSymbol("tRue"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysTrueSymbol("trUe"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysTrueSymbol("truE"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysTrueSymbol("TRUE"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysTrueSymbol("true"));
            //
            Assert.IsFalse(PropositionSymbol.IsAlwaysTrueSymbol("Tru3"));
            Assert.IsFalse(PropositionSymbol.IsAlwaysTrueSymbol("True "));
            Assert.IsFalse(PropositionSymbol.IsAlwaysTrueSymbol(" True"));
        }

        [Test]
        public void Test_isAlwaysFalseSymbol() {
            Assert.IsTrue(PropositionSymbol.IsAlwaysFalseSymbol("False"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysFalseSymbol("fAlse"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysFalseSymbol("faLse"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysFalseSymbol("falSe"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysFalseSymbol("falsE"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysFalseSymbol("FALSE"));
            Assert.IsTrue(PropositionSymbol.IsAlwaysFalseSymbol("false"));
            //
            Assert.IsFalse(PropositionSymbol.IsAlwaysFalseSymbol("Fals3"));
            Assert.IsFalse(PropositionSymbol.IsAlwaysFalseSymbol("False "));
            Assert.IsFalse(PropositionSymbol.IsAlwaysFalseSymbol(" False"));
        }

        [Test]
        public void Test_isPropositionSymbol() {

            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("True"));
            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("False"));
            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("A"));
            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("A1"));
            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("A_1"));
            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("a"));
            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("a1"));
            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("A_1"));
            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("_"));
            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("_1"));
            Assert.IsTrue(PropositionSymbol.IsPropositionSymbol("_1_2"));

            // Las comas no se permiten (sólo caracteres que puedan estar en identificadores legales de Java... ahora C#)
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("A1,2"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol(" A"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("A "));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("A B"));
            // En Java sí se permiten identificadores con el símbolo del dólar... pero en C# no
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("$"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("$1"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("$1_1"));
        }

        [Test]
        public void Test_isPropositionSymbolDoesNotContainConnectiveChars() {
            // '~', '&', '|', '=', '<', '>'
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("~"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("&"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("|"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("="));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("<"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol(">"));

            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("A~"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("A&"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("A|"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("A="));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("A<"));
            Assert.IsFalse(PropositionSymbol.IsPropositionSymbol("A>"));
        }

        //@Test(expected = IllegalArgumentException.class) El constructor lanza una ArgumentException...
        [Test]
        public void Test_ArgumentExceptionOnConstruction() { 
            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            Assert.Throws<ArgumentException>(() => new PropositionSymbol("A_1,2"));
            // Esto no me parece ir bien: Assert.That(new PropositionSymbol("A_1,2"), Throws.ArgumentException);
        }
    }
}
