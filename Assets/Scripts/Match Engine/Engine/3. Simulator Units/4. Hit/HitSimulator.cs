using System;
using UnityEngine;

public class HitSimulator : SimulatorUnitBase { 
    const double additional = 0.0f;

    public bool Get(Pitcher pitcher, Batter batter) {
        bool result = false;
        if(Utils.NotNull(pitcher, pitcher.Stats, batter, batter.Stats)) {
            double prob = GetHit(pitcher.Stats, batter.Stats);
            double randomValue = random.NextDouble();

            result = randomValue <= prob;
        }
        return result;
    }

    double GetHit(TuplePitcherStats pitcher, TupleBatterStats batter) {
        double pitcherStat = pitcher.Stuff * 0.7 + pitcher.Velocity * 0.15 + pitcher.Control * 0.1 + pitcher.KMov * 0.05;
        double pitcherProb = MathUtils.Linear(x:pitcherStat, a:0.003076923076923077 * -1.0, b: 0.5576923076923077);
        double batterStat  = batter.Hit;
        double batterProb  = MathUtils.Linear(x:batterStat, a:0.0033846153846153844, b: 0.08153846153846156);
        return ((pitcherProb + batterProb) / 2.0) + additional;
    }
}