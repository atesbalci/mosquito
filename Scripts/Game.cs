using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Game : Node
{
    private GameRules _gameRules;
    private Mosquito _mosquito;
    private IList<Area3D> _suckableAreas;
    private IList<Area3D> _annoyanceAreas;
    private int _enteredSuckableAreaCount;
    private int _enteredAnnoyanceAreaCount;
    
    public GameData GameData { get; private set; } = new();

    public override void _Ready()
    {
        _suckableAreas = GetChildren().OfType<Area3D>().Where(area => area.GetCollisionLayerValue(3)).ToArray();
        _annoyanceAreas = GetChildren().OfType<Area3D>().Where(area => area.GetCollisionLayerValue(4)).ToArray();
        _gameRules = GetMeta("GameRules").Obj as GameRules;
        _mosquito = GetNode<Mosquito>(GetMeta("Mosquito").AsNodePath());
        SetSize(0);
        
        foreach (var suckableArea in _suckableAreas)
        {
            suckableArea.BodyEntered += SuckableAreaOnBodyEntered;
            suckableArea.BodyExited += SuckableAreaOnBodyExited;
        }
        
        foreach (var annoyanceArea in _annoyanceAreas)
        {
            annoyanceArea.BodyEntered += AnnoyanceAreaOnBodyEntered;
            annoyanceArea.BodyExited += AnnoyanceAreaOnBodyExited;
        }
    }

    public override void _Process(double delta)
    {
        float deltaF = (float) delta;
        if (_enteredSuckableAreaCount > 0)
        {
            SetSize(GameData.MosquitoSize + _gameRules.SuckPerSecond * deltaF);
        }

        float annoyanceDiff;
        if (_enteredAnnoyanceAreaCount > 0)
        {
            annoyanceDiff = _gameRules.AnnoyancePerSecond * deltaF;
        }
        else
        {
            annoyanceDiff = -_gameRules.AnnoyanceRecoveryPerSecond * deltaF;
        }
        
        GameData.Annoyance = Mathf.Clamp(GameData.Annoyance + annoyanceDiff, 0f, 1f);
    }

    private void SuckableAreaOnBodyEntered(Node3D body)
    {
        if (body != _mosquito) return;
        _enteredSuckableAreaCount++;
    }

    private void SuckableAreaOnBodyExited(Node3D body)
    {
        if (body != _mosquito) return;
        _enteredSuckableAreaCount--;
    }

    private void AnnoyanceAreaOnBodyEntered(Node3D body)
    {
        if (body != _mosquito) return;
        _enteredAnnoyanceAreaCount++;
    }

    private void AnnoyanceAreaOnBodyExited(Node3D body)
    {
        if (body != _mosquito) return;
        _enteredAnnoyanceAreaCount--;
    }

    private void SetSize(float size)
    {
        size = Mathf.Clamp(size, 0f, 1f);
        GameData.MosquitoSize = size;
        _mosquito.Acceleration = _gameRules.AccelerationCurve.Sample(size);
        _mosquito.MaxVelocity = _gameRules.MaxVelocityCurve.Sample(size);
        _mosquito.LinearDamp = _gameRules.DampCurve.Sample(size);
    }
}