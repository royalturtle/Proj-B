public class OutBallSimulator : SimulatorUnitBase {
    public OutBallSituation Get(Pitcher pitcher, Batter batter) {
        BaseballResultTypes result  = BaseballResultTypes.FLY_INNER;

        if(Utils.NotNull(pitcher, pitcher.Stats, batter, batter.Stats)) {
            double ground = GetGround(pitcher.Stats, batter.Stats);
            double flyOutside = GetGround(pitcher.Stats, batter.Stats);
            
            double randomValue = random.NextDouble();
            if(randomValue <= ground) {
                result = BaseballResultTypes.GROUND_BALL;
            }
            else if(randomValue <= ground + flyOutside) {
                result = BaseballResultTypes.FLY_OUTSIDE;
            }
        }

        return new OutBallSituation(result:result);
    }

    
    double GetGround(TuplePitcherStats pitcher, TupleBatterStats batter) {
        double pitcherStat = pitcher.GMov * 0.6 + pitcher.Stuff * 0.2 + pitcher.Velocity * 0.1 + pitcher.Control * 0.1;
        double pitcherProb = MathUtils.Linear(x:pitcherStat, a:0.004153846153846154, b: 0.20461538461538464);
        double batterStat  = batter.Hit * 0.8 + batter.Power * 0.1 + batter.Eye * 0.1;
        double batterProb  = MathUtils.Linear(x:batterStat, a:-0.0038461538461538464, b: 0.7546153846153847);
        return (pitcherProb + batterProb) / 2.0;
    }

    
    double GetFlyOutside(TuplePitcherStats pitcher, TupleBatterStats batter) {
        double pitcherStat = pitcher.GMov * 0.7 + pitcher.Stuff * 0.15 + pitcher.Velocity * 0.15;
        double pitcherProb = MathUtils.Linear(x:pitcherStat, a:-0.0033846153846153844, b: 0.5884615384615384);
        double batterStat  = batter.Power * 0.6 + batter.GapPower * 0.25 + batter.Hit * 0.1 + batter.Eye * 0.05;
        double batterProb  = MathUtils.Linear(x:batterStat, a:0.0032307692307692306, b: 0.10692307692307695);
        return (pitcherProb + batterProb) / 2.0;
    }
}