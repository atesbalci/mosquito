using Godot;

public partial class GameStateVisualizer : Node
{
    private GameData _gameData;
    private float _currentSuckingMultiplier;

    public void Initialize(GameData gameData)
    {
        _gameData = gameData;
    }
    
    public override void _Process(double delta)
    {
        float deltaF = (float) delta;
        _currentSuckingMultiplier = Mathf.Lerp(_currentSuckingMultiplier, _gameData.IsSucking ? 1f : 0f, deltaF * 10f);
        RenderingServer.GlobalShaderParameterSet("Sucking", _currentSuckingMultiplier);
        RenderingServer.GlobalShaderParameterSet("Annoyance", _gameData.AnnoyanceGenerationRate);
    }
}