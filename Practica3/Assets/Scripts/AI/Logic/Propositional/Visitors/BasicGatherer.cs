/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Visitors {

    /*
    import aima.core.logic.propositional.parsing.PLVisitor;
    import aima.core.logic.propositional.parsing.ast.ComplexSentence;
    import aima.core.logic.propositional.parsing.ast.PropositionSymbol;
    import aima.core.util.SetOps;
    */
    using System;
    using System.Collections.Generic;

    using UCM.IAV.AI.Util;
    using UCM.IAV.AI.Logic.Propositional.Parsing;

    /**
     * Superclase de los visitantes que son de "sólo lectura" y recopilan información de un árbol de análisis sintáctico existente.
     * 
     * @param <T>
     *            el tipo de elementos a recopilar.
     */
    public abstract class BasicGatherer<T> : PLVisitor<ISet<T>, ISet<T>> {
            
        // Es virtual por si hace falta sobreescribirlo
        public virtual ISet<T> VisitPropositionSymbol(PropositionSymbol s, ISet<T> arg) {
            return arg;
        }

        // Es virtual por si hace falta sobreescribirlo
        public virtual ISet<T> VisitUnarySentence(ComplexSentence s, ISet<T> arg) {
            return SetOps.Union(arg, s.GetSimplerSentence(0).Accept(this, arg));
        }

        // Es virtual por si hace falta sobreescribirlo
        public virtual ISet<T> VisitBinarySentence(ComplexSentence s, ISet<T> arg) {
            ISet<T> termunion = SetOps.Union(s.GetSimplerSentence(0).Accept(this, arg), s.GetSimplerSentence(1).Accept(this, arg));
            return SetOps.Union(arg, termunion);
         }
    }
}
