public class HitBallSimulator : SimulatorUnitBase {
    public HitBallSituation Get(Pitcher pitcher, Batter batter) {
        BaseballResultTypes result = BaseballResultTypes.HIT1;

        if(Utils.NotNull(pitcher, pitcher.Stats, batter, batter.Stats)) {
            double homerun = GetHomerun(pitcher.Stats, batter.Stats);
            double longHit = GetLong(pitcher.Stats, batter.Stats);
            
            double randomValue = random.NextDouble();
            if(randomValue <= homerun) {
                result = BaseballResultTypes.HOMERUN;
            }
            else if(randomValue <= homerun + longHit) {
                double longHit3 = GetLong3(batter.Stats);
                randomValue = random.NextDouble();

                if(randomValue <= longHit3) {
                    result = BaseballResultTypes.HIT3;
                }
                else {
                    result = BaseballResultTypes.HIT2;
                }
            }
        }

        return new HitBallSituation(result:result);
    }
    
    double GetHomerun(TuplePitcherStats pitcher, TupleBatterStats batter) {
        double pitcherStat = pitcher.Stuff * 0.5 + pitcher.Control * 0.3 + pitcher.Velocity * 0.2;
        double pitcherProb = MathUtils.Linear(x:pitcherStat, a:-0.0023076923076923075, b: 0.26076923076923075);
        double batterStat  = batter.Power * 0.85 + batter.GapPower * 0.15;
        double batterProb  = MathUtils.Linear(x:batterStat, a:0.0035384615384615385, b: -0.12384615384615386);
        // return MathUtils.Square(x1:pitcherProb, x2:batterProb);
        return (pitcherProb + batterProb) / 2.0;
    }
    
    double GetLong(TuplePitcherStats pitcher, TupleBatterStats batter) {
        double pitcherStat = pitcher.Stuff * 0.7 + pitcher.Velocity * 0.2 + pitcher.Control * 0.1;
        double pitcherProb = MathUtils.Linear(x:pitcherStat, a:-0.0038461538461538464, b: 0.39461538461538465);
        double batterStat  = batter.GapPower * 0.7 + batter.Speed * 0.15 + batter.Power * 0.15;
        double batterProb  = MathUtils.Linear(x:batterStat, a:0.003538461538461538, b:-0.05384615384615382);
        // return MathUtils.Square(x1:pitcherProb, x2:batterProb);
        return (pitcherProb + batterProb) / 2.0;
    }

    double GetLong3(TupleBatterStats batter) {
        int batterStat  = (int)(batter.Speed * 0.9 + batter.GapPower * 0.1);
        return MathUtils.Exponential(x:batterStat, a:0.0000013, b:1.13) - 0.0001;
    }

    public double GetDirection(Batter batter) {
        double result = random.NextDouble() / 2.0;
        if(Utils.NotNull(batter, batter.Stats)) {
            double pushRate = 0.35 + 0.05 * batter.Stats.Push;
            if(pushRate < random.NextDouble()) {
                result += 0.5;
            }

            if(batter.Base.HitHand == Hands.RIGHT) {
                result = 1.0 - result;
            }
        }
        
        return result;
    }
}