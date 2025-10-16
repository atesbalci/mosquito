using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Game : Node
{
    private GameRules _gameRules;
    private Mosquito _mosquito;
    private IList<SuckableArea> _suckableAreas;
    private IList<Area3D> _annoyanceAreas;
    private int _enteredSuckableAreaCount;
    private Node3D _currentAnnoyanceArea;
    
    public GameData GameData { get; } = new();

    public override void _Ready()
    {
        _suckableAreas = GetChildren().OfType<SuckableArea>().ToArray();
        _annoyanceAreas = GetChildren().OfType<Area3D>().Where(area => area.GetCollisionLayerValue(4)).ToArray();
        _gameRules = GetMeta("GameRules").As<GameRules>();
        _mosquito = GetNode<Mosquito>(GetMeta("Mosquito").AsNodePath());
        _mosquito.Initialize(GameData);
        SetSize(0);

        SetStage(0);
        
        foreach (var annoyanceArea in _annoyanceAreas)
        {
            annoyanceArea.BodyEntered += _ => _currentAnnoyanceArea = annoyanceArea;
            annoyanceArea.BodyExited += _ => _currentAnnoyanceArea = null;
        }
    }

    private void SetStage(int stage)
    {
        GameData.Stage = stage;
        foreach (var suckableArea in _suckableAreas)
        {
            int suckableAreaStage = suckableArea.GetMeta("SuckStage").AsInt32();
            bool active = suckableAreaStage == stage;
            suckableArea.Visible = active;
            suckableArea.RemainingBlood = active ? (1f / _suckableAreas.Count) : 0f;
        }
    }

    public override void _Process(double delta)
    {
        float deltaF = (float) delta;
        if (_enteredSuckableAreaCount > 0)
        {
            SetSize(GameData.MosquitoSize + _gameRules.SuckPerSecond * deltaF);
        }

        GameData.IsSucking = false;
        foreach (var suckableArea in _suckableAreas)
        {
            if (suckableArea.IsBeingSucked && suckableArea.RemainingBlood > 0.001f)
            {
                float suckAmount = Mathf.Min(suckableArea.RemainingBlood, _gameRules.SuckPerSecond * deltaF);
                suckableArea.RemainingBlood -= suckAmount;
                GameData.MosquitoSize += suckAmount;
                if (suckableArea.RemainingBlood < 0.001f)
                {
                    suckableArea.Visible = false;
                }
                
                GameData.IsSucking = true;
            }
        }

        if (_suckableAreas.All(area => area.RemainingBlood < 0.001f))
        {
            SetStage(GameData.Stage + 1);
        }

        ApplyAnnoyance(deltaF);

        if (GameData.IsGameOver)
        {
            _mosquito.SetLocked(true);
            GameData.IsSucking = false;
        }
    }

    private void ApplyAnnoyance(float deltaF)
    {
        float annoyanceDiff;
        if (_currentAnnoyanceArea != null)
        {
            float distNormalized = (_mosquito.GlobalPosition - _currentAnnoyanceArea.GlobalPosition).Length() /
                                   (_currentAnnoyanceArea.Scale.X * 0.5f);
            annoyanceDiff = _gameRules.AnnoyancePerSecondCurve.Sample(1f - distNormalized);
        }
        else
        {
            annoyanceDiff = -_gameRules.AnnoyanceRecoveryPerSecond;
        }

        GameData.AnnoyanceGenerationRate = Mathf.Max(0f, annoyanceDiff);
        GameData.Annoyance = Mathf.Clamp(GameData.Annoyance + annoyanceDiff * deltaF, 0f, 1f);
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