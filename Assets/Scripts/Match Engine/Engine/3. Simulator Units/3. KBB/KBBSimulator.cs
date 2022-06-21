using UnityEngine;

public class KBBSimulator : SimulatorUnitBase {
    double additionalBB = 0.0f;

    public KBBSituation Get(Pitcher pitcher, Batter batter) {
        BaseballResultTypes result = BaseballResultTypes.NONE;
        if(Utils.NotNull(pitcher, pitcher.Stats, batter, batter.Stats)) {
            double k = GetK(pitcher.Stats, batter.Stats);
            double bb = GetBB(pitcher.Stats, batter.Stats);

            double randomValue = random.NextDouble();

            if(randomValue <= k) {
                result = random.Next(0, 2) == 0 ? BaseballResultTypes.STRIKE_OUT : BaseballResultTypes.STRIKE_OUT_SWING;
            }
            else if(randomValue <= k + bb) {
                result = BaseballResultTypes.BB;
            }
        }
        return new KBBSituation(result:result);
    }

    double GetK(TuplePitcherStats pitcher, TupleBatterStats batter) {
        double pitcherStat = pitcher.KMov * 0.7f + pitcher.Velocity * 0.15f + pitcher.Stuff * 0.1f + pitcher.Control * 0.05f;
        double pitcherProb = MathUtils.Linear(x:pitcherStat, a:0.003076923076923077, b: -1.0 * 0.05769230769230771);
        double batterStat  = batter.AvoidK * 0.85 + batter.Eye * 0.15;
        double batterProb  = MathUtils.Linear(x:pitcherStat, a:-1.0 * 0.004, b: 0.46);
        // return MathUtils.Square(x1:pitcherProb, x2:batterProb);
        return (pitcherProb + batterProb) / 2.0;
    }

    double GetBB(TuplePitcherStats pitcher, TupleBatterStats batter) {
        double pitcherStat = pitcher.Control;
        double pitcherProb = MathUtils.Linear(x:pitcherStat, a:-1.0 * 0.0021538461538461538, b: 0.2553846153846154);
        double batterStat  = batter.Eye;
        double batterProb  = MathUtils.Linear(x:batterStat,  a: 0.002153846153846154,  b:-1.0 * 0.04538461538461541);
        // return MathUtils.Square(x1:pitcherProb, x2:batterProb);
        return ((pitcherProb + batterProb) / 2.0) + additionalBB;
    }
}
