using SQLite4Unity3d;

public class TuplePitcherStats : TupleLivePlayer {
    public double Stamina { get; set; }
    public double Velocity { get; set; }
    public double Stuff { get; set; }
    public double KMov { get; set; }
    public double GMov { get; set; }
    public double Control { get; set; }
    public double Composure { get; set; }
    public double ROpp{ get; set; }
    public double LOpp{ get; set; }

    public double Stamina_MAX { get; set; }
    public double Velocity_MAX { get; set; }
    public double Stuff_MAX { get; set; }
    public double KMov_MAX { get; set; }
    public double GMov_MAX { get; set; }
    public double Control_MAX { get; set; }
    public double Composure_MAX { get; set; }
    public double ROpp_MAX { get; set; }
    public double LOpp_MAX { get; set; }

    public double StartingAbility {
        get {
            return Velocity + Stuff + KMov + GMov + Control + Composure + ROpp + LOpp;
        }
    }

    public double ReliefAbility {
        get {
            return 1.3 * Velocity + 1.3 * Stuff + 1.2 * KMov + 1.0 * GMov + 1.3 * Control + 1.2 * Composure;
        }
    }
}
