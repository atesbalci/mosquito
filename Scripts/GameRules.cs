using Godot;

public partial class GameRules : Resource
{
    [Export]
    public Curve MaxVelocityCurve { get; set; }
    
    [Export]
    public Curve AccelerationCurve { get; set; }
    
    [Export]
    public Curve DampCurve { get; set; }
    
    [Export]
    public float SuckPerSecond { get; set; }
    
    [Export]
    public float AnnoyancePerSecond { get; set; }
    
    [Export]
    public float AnnoyanceRecoveryPerSecond { get; set; }
}