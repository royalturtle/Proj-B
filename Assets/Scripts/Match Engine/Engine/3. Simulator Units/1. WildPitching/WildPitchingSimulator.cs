using System;
using UnityEngine;

public class WildPitchingSimulator : SimulatorUnitBase{
    public bool Get(Pitcher pitcher) {
        bool result = false;
        if(pitcher != null && pitcher.Stats != null) {
            double x = pitcher.Stats.Control;
            result = random.NextDouble() < MathUtils.Exponential(x:x, a:0.1702, b:0.9558);
        }
        return result;
    }
}
