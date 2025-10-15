using Godot;

public partial class UI : Node
{
    private GameData _gameData;
    private Range _bloodMeter;
    private Range _annoyanceMeter;
    private PackedScene _gameOverPanel;
    private bool _gameOver;
    
    public void Initialize(GameData gameData)
    {
        _gameData = gameData;
    }

    public override void _Ready()
    {
        _bloodMeter = GetNode<Range>(GetMeta("BloodMeter").AsNodePath());
        _annoyanceMeter = GetNode<Range>(GetMeta("AnnoyanceMeter").AsNodePath());
        _gameOverPanel = GetMeta("GameOver").Obj as PackedScene;
    }

    public override void _Process(double delta)
    {
        _bloodMeter.SetValue(_gameData.MosquitoSize);
        _annoyanceMeter.SetValue(_gameData.Annoyance);
        if (_gameData.IsGameOver && !_gameOver)
        {
            _gameOver = true;
            AddChild(_gameOverPanel.Instantiate());
        }
    }
}