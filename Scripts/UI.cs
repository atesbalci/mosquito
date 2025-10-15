using Godot;

public partial class UI : Node
{
    private GameData _gameData;
    private Range _bloodMeter;
    private Range _annoyanceMeter;
    
    public void Initialize(GameData gameData)
    {
        _gameData = gameData;
    }

    public override void _Ready()
    {
        _bloodMeter = GetNode<Range>(GetMeta("BloodMeter").AsNodePath());
        _annoyanceMeter = GetNode<Range>(GetMeta("AnnoyanceMeter").AsNodePath());
    }

    public override void _Process(double delta)
    {
        _bloodMeter.SetValue(_gameData.MosquitoSize);
        _annoyanceMeter.SetValue(_gameData.Annoyance);
    }
}