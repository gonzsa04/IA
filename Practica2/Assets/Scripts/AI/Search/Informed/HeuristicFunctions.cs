namespace UCM.IAV.IA.Search.Informed
{
    using UCM.IAV.Puzzles;
    using System;

    // clase que engloba los tipos de heuristica que tenemos implementados
    public class HeuristicFunctions
    {
        private int f_; // posicion del destino al que queremos llegar
        private GameManager gm = GameManager.instance;

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

        // admisible e inconsistente: devolvera una h aleatoria, nunca mayor que el
        // coste fisico minimo de ir desde el nodo actual al destino (1 x numCasillas hasta el destino)
        private double heuristic1(NodeCool e)
        {
            int inix = (int)(e.GetPos() / gm.columns);
            int iniy = (int)(e.GetPos() - inix * gm.columns);
            int destx = (int)(f_ / gm.columns);
            int desty = (int)(f_ - destx * gm.columns);

            double A = Math.Abs(destx - inix);
            double B = Math.Abs(desty - iniy);

            System.Random rnd = new System.Random();

            return rnd.Next(0, (int)(A + B + 1)); ;
        }

        // admisible y consistente: devolvera la suma del numero de casillas horizontales
        // y verticales que separan al nodo e del destino
        private double heuristic2(NodeCool e)
        {
            int inix = (int)(e.GetPos() / gm.columns);
            int iniy = (int)(e.GetPos() - inix * gm.columns);
            int destx = (int)(f_ / gm.columns);
            int desty = (int)(f_ - destx * gm.columns);

            double A = Math.Abs(destx - inix);
            double B = Math.Abs(desty - iniy);

            return A + B;
        }

        // admisible, consistente y dominante: devolvera la distancia euclidea entre el
        // nodo e y el destino
        private double heuristic3(NodeCool e)
        {
            int inix = (int)(e.GetPos() / gm.columns);
            int iniy = (int)(e.GetPos() - inix * gm.columns);
            int destx = (int)(f_ / gm.columns);
            int desty = (int)(f_ - destx * gm.columns);

            double catA = destx - inix;
            double catB = desty - iniy;

            return Math.Sqrt(catA * catA + catB * catB); // hipotenusa
        }
    }
}
