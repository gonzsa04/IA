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

    using UCM.IAV.AI.Util;
    using UCM.IAV.AI.Logic.Propositional.Parsing;
    using System.Collections.Generic;

    [TestFixture] // Esto significa que en esta clase hay pruebas unitarias
    public class ClauseTest {

        private readonly Literal LITERAL_P = new Literal(new PropositionSymbol("P"));
        private readonly Literal LITERAL_NOT_P = new Literal(new PropositionSymbol("P"), false);
        private readonly Literal LITERAL_Q = new Literal(new PropositionSymbol("Q"));
        private readonly Literal LITERAL_NOT_Q = new Literal(new PropositionSymbol("Q"), false);
        private readonly Literal LITERAL_R = new Literal(new PropositionSymbol("R"));

        [Test]
        public void TestAlwaysFalseLiteralsExcludedOnConstruction() {
            Clause clause = new Clause();
            Assert.AreEqual(0, clause.GetNumberLiterals());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual(1, clause.GetNumberLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE));
            Assert.AreEqual(1, clause.GetNumberLiterals());
            Assert.AreEqual(Util.CreateSet(LITERAL_P), clause.GetLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE, false));
            Assert.AreEqual(1, clause.GetNumberLiterals());
            Assert.AreEqual(Util.CreateSet(LITERAL_P), clause.GetLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE, false));
            Assert.AreEqual(2, clause.GetNumberLiterals());
            Assert.AreEqual(Util.CreateSet(LITERAL_P, new Literal(PropositionSymbol.FALSE, false)), clause.GetLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE));
            Assert.AreEqual(2, clause.GetNumberLiterals());
            Assert.AreEqual(Util.CreateSet(LITERAL_P, new Literal(PropositionSymbol.TRUE)), clause.GetLiterals());
        }

        [Test]
        public void TestIsFalse() {
            Clause clause = new Clause();
            Assert.IsTrue(clause.IsFalse());

            clause = new Clause(LITERAL_P);
            Assert.IsFalse(clause.IsFalse());
        }

        [Test]
        public void TestIsEmpty() {
            Clause clause = new Clause();
            Assert.IsTrue(clause.IsEmpty());

            clause = new Clause(LITERAL_P);
            Assert.IsFalse(clause.IsEmpty());
        }

        [Test]
        public void TestIsUnitClause() {
            Clause clause = new Clause();
            Assert.IsFalse(clause.IsUnitClause());

            clause = new Clause(LITERAL_P);
            Assert.IsTrue(clause.IsUnitClause());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsFalse(clause.IsUnitClause());
        }

        [Test]
        public void TestIsDefiniteClause() {
            Clause clause = new Clause();
            Assert.IsFalse(clause.IsDefiniteClause());

            clause = new Clause(LITERAL_P);
            Assert.IsTrue(clause.IsDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.IsDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsFalse(clause.IsDefiniteClause());
        }

        [Test]
        public void TestIsImplicationDefiniteClause() {
            Clause clause = new Clause();
            Assert.IsFalse(clause.IsImplicationDefiniteClause());

            clause = new Clause(LITERAL_P);
            Assert.IsFalse(clause.IsImplicationDefiniteClause());

            clause = new Clause(LITERAL_NOT_P);
            Assert.IsFalse(clause.IsImplicationDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsFalse(clause.IsImplicationDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.IsImplicationDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.IsImplicationDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_Q, LITERAL_NOT_Q);
            Assert.IsFalse(clause.IsImplicationDefiniteClause());
        }

        [Test]
        public void TestIsHornClause() {
            Clause clause = new Clause();
            Assert.IsFalse(clause.IsHornClause());

            clause = new Clause(LITERAL_P);
            Assert.IsTrue(clause.IsHornClause());

            clause = new Clause(LITERAL_NOT_P);
            Assert.IsTrue(clause.IsHornClause());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsFalse(clause.IsHornClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.IsHornClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.IsHornClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_Q, LITERAL_NOT_Q);
            Assert.IsFalse(clause.IsHornClause());
        }

        [Test]
        public void TestIsGoalClause() {
            Clause clause = new Clause();
            Assert.IsFalse(clause.IsGoalClause());

            clause = new Clause(LITERAL_P);
            Assert.IsFalse(clause.IsGoalClause());

            clause = new Clause(LITERAL_NOT_P);
            Assert.IsTrue(clause.IsGoalClause());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsFalse(clause.IsGoalClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q);
            Assert.IsFalse(clause.IsGoalClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_NOT_Q);
            Assert.IsFalse(clause.IsGoalClause());

            clause = new Clause(LITERAL_NOT_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.IsGoalClause());
        }

        [Test]
        public void TestIsTautology() {
            Clause clause = new Clause();
            Assert.IsFalse(clause.IsTautology());

            clause = new Clause(LITERAL_P);
            Assert.IsFalse(clause.IsTautology());

            clause = new Clause(LITERAL_NOT_P);
            Assert.IsFalse(clause.IsTautology());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE), LITERAL_R);
            Assert.IsTrue(clause.IsTautology());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE, false), LITERAL_R);
            Assert.IsTrue(clause.IsTautology());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE, false), LITERAL_R);
            Assert.IsFalse(clause.IsTautology());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE), LITERAL_R);
            Assert.IsFalse(clause.IsTautology());

            clause = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R, LITERAL_NOT_Q);
            Assert.IsTrue(clause.IsTautology());

            clause = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R);
            Assert.IsFalse(clause.IsTautology());
        }

        [Test]
        public void TestGetNumberLiterals() {
            Clause clause = new Clause();
            Assert.AreEqual(0, clause.GetNumberLiterals());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual(1, clause.GetNumberLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE));
            Assert.AreEqual(1, clause.GetNumberLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE, false));
            Assert.AreEqual(1, clause.GetNumberLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE, false));
            Assert.AreEqual(2, clause.GetNumberLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE));
            Assert.AreEqual(2, clause.GetNumberLiterals());

            clause = new Clause(LITERAL_P, LITERAL_P);
            Assert.AreEqual(1, clause.GetNumberLiterals());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.AreEqual(2, clause.GetNumberLiterals());

            clause = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R);
            Assert.AreEqual(3, clause.GetNumberLiterals());
        }

        [Test]
        public void TestGetNumberPositiveLiterals() {
            Clause clause = new Clause();
            Assert.AreEqual(0, clause.GetNumberPositiveLiterals());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual(1, clause.GetNumberPositiveLiterals());

            clause = new Clause(LITERAL_NOT_P);
            Assert.AreEqual(0, clause.GetNumberPositiveLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_Q);
            Assert.AreEqual(2, clause.GetNumberPositiveLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q, LITERAL_R);
            Assert.AreEqual(2, clause.GetNumberPositiveLiterals());
        }

        [Test]
        public void TestGetNumberNegativeLiterals() {
            Clause clause = new Clause();
            Assert.AreEqual(0, clause.GetNumberNegativeLiterals());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual(0, clause.GetNumberNegativeLiterals());

            clause = new Clause(LITERAL_NOT_P);
            Assert.AreEqual(1, clause.GetNumberNegativeLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_Q);
            Assert.AreEqual(1, clause.GetNumberNegativeLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q, LITERAL_R);
            Assert.AreEqual(1, clause.GetNumberNegativeLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_NOT_Q);
            Assert.AreEqual(2, clause.GetNumberNegativeLiterals());
        }

        [Test]
        public void TestGetLiterals() {
            Clause clause = new Clause();
            Assert.AreEqual(new HashSet<Literal>(), clause.GetLiterals());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual(Util.CreateSet(LITERAL_P), clause.GetLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q, LITERAL_R);
            Assert.AreEqual(Util.CreateSet(LITERAL_P, LITERAL_NOT_Q, LITERAL_R), clause.GetLiterals());
        }

        [Test]
        public void TestGetPositiveSymbols() {
            Clause clause = new Clause();
            Assert.AreEqual(new HashSet<PropositionSymbol>(), clause.GetPositiveSymbols());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual(Util.CreateSet(new PropositionSymbol("P")), clause.GetPositiveSymbols());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q, LITERAL_R);
            Assert.AreEqual(Util.CreateSet(new PropositionSymbol("P"), new PropositionSymbol("R")), clause.GetPositiveSymbols());
        }

        [Test]
        public void TestGetNegativeSymbols() {
            Clause clause = new Clause();
            Assert.AreEqual(new HashSet<PropositionSymbol>(), clause.GetNegativeSymbols());

            clause = new Clause(LITERAL_NOT_P);
            Assert.AreEqual(Util.CreateSet(new PropositionSymbol("P")), clause.GetNegativeSymbols());

            clause = new Clause(LITERAL_NOT_P, LITERAL_NOT_Q, LITERAL_R);
            Assert.AreEqual(Util.CreateSet(new PropositionSymbol("P"), new PropositionSymbol("Q")), clause.GetNegativeSymbols());
        }

        [Test]
        public void TestToString() {
            Clause clause = new Clause();
            Assert.AreEqual("{}", clause.ToString());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual("{P}", clause.ToString());

            clause = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R);
            Assert.AreEqual("{P, Q, R}", clause.ToString());

            clause = new Clause(LITERAL_NOT_P, LITERAL_NOT_Q, LITERAL_R);
            Assert.AreEqual("{~P, ~Q, R}", clause.ToString());
        }

        [Test]
        public void TestEquals() {
            Clause clause1 = new Clause();
            Clause clause2 = new Clause();
            Assert.IsTrue(clause1.Equals(clause2)); // Dos cláusulas vacías son iguales

            clause1 = new Clause(LITERAL_P);
            clause2 = new Clause(LITERAL_P);
            Assert.IsTrue(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsTrue(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_R, LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R);
            Assert.IsTrue(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_P);
            clause2 = new Clause(LITERAL_Q);
            Assert.IsFalse(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_R);
            Assert.IsFalse(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_P);
            Assert.IsFalse(clause1.Equals(LITERAL_P));
        }

        [Test]
        public void TestHashCode() {
            Clause clause1 = new Clause();
            Clause clause2 = new Clause();
            Assert.IsTrue(clause1.GetHashCode() == clause2.GetHashCode()); // Dos cláusulas vacías deberían tener el mismo código hash

            clause1 = new Clause(LITERAL_P);
            clause2 = new Clause(LITERAL_P);
            Assert.IsTrue(clause1.GetHashCode() == clause2.GetHashCode());

            clause1 = new Clause(LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsTrue(clause1.GetHashCode() == clause2.GetHashCode());

            clause1 = new Clause(LITERAL_R, LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R);
            Assert.IsTrue(clause1.GetHashCode() == clause2.GetHashCode());

            clause1 = new Clause(LITERAL_P);
            clause2 = new Clause(LITERAL_Q);
            Assert.IsFalse(clause1.GetHashCode() == clause2.GetHashCode());

            clause1 = new Clause(LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_R);
            Assert.IsFalse(clause1.GetHashCode() == clause2.GetHashCode());
        }

        [Test]  //(expected= UnsupportedOperationException.class)
	public void TestLiteralsImmutable() {
            Clause clause = new Clause(LITERAL_P);
            clause.GetLiterals().Add(LITERAL_Q);

            // Dentro del assert hay que meter un TestDelegate y por eso he puesto una expresión lambda
            //Assert.Throws<ArgumentException>(() => new ComplexSentence(Connective.BICONDITIONAL, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C") }));
        }

        [Test] //(expected= UnsupportedOperationException.class)
	public void TestPostivieSymbolsImmutable() {
            Clause clause = new Clause(LITERAL_P);
            clause.GetPositiveSymbols().Add(new PropositionSymbol("Q"));
        }

        [Test] //(expected= UnsupportedOperationException.class)
	public void TestNegativeSymbolsImmutable() {
            Clause clause = new Clause(LITERAL_P);
            clause.GetNegativeSymbols().Add(new PropositionSymbol("Q"));
        }
    }
}
