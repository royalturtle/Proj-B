using System;
using UnityEngine;

public class HBPSimulator : SimulatorUnitBase {
    public bool Get(Pitcher pitcher) {
        bool result = false;
        if(pitcher != null && pitcher.Stats != null) {
            double x = pitcher.Stats.Control;
            result = random.NextDouble() < MathUtils.Exponential(x:x, a:0.09909, b:0.9696);
        }
        return result;
    }
}
