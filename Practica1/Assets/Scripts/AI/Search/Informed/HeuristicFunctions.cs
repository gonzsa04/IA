namespace UCM.IAV.IA.Search.Informed
{
    using UCM.IAV.Puzzles;
    using System;

    // clase que engloba los tres tipos de heuristica
    public class HeuristicFunctions
    {
        private int f_; // destino al que queremos llegar

        // elige una de las heuristicas dado el tipo de heuristica que reciba
        public double chooseHeuristic(TipoHeuristicas t, NodeCool e, int fin)
        {
            f_ = fin;

            switch (t)
            {
                case TipoHeuristicas.H1:
                    return heuristic1(e);

                case TipoHeuristicas.H2:
                    return heuristic2(e);

                case TipoHeuristicas.H3:
                    return heuristic3(e);

                default:
                    return 0.0;
            }
        }

        // admisible e inconsistente
        private double heuristic1(NodeCool e)
        {
            return 0.0;
        }

        // admisible y consistente
        private double heuristic2(NodeCool e)
        {
            int inix = (int)(e.GetPos() / GameManager.instance.columns);
            int iniy = (int)(e.GetPos() - inix * GameManager.instance.columns);
            int destx = (int)(f_ / GameManager.instance.columns);
            int desty = (int)(f_ - destx * GameManager.instance.columns);

            double A = Math.Abs(destx - inix);
            double B = Math.Abs(desty - iniy);

            return A + B;
        }

        // admisible, consistente y dominante. Distancia euclidea
        private double heuristic3(NodeCool e)
        {
            int inix = (int)(e.GetPos() / GameManager.instance.columns);
            int iniy = (int)(e.GetPos() - inix * GameManager.instance.columns);
            int destx = (int)(f_ / GameManager.instance.columns);
            int desty = (int)(f_ - destx * GameManager.instance.columns);

            double catA = destx - inix;
            double catB = desty - iniy;

            return Math.Sqrt(catA * catA + catB * catB);
        }
    }
}
