using Godot;

public partial class Main : Node
{
    [Export] private Game _game;
    [Export] private UI _ui;
    
    public override void _Ready()
    {
        var gameData = _game.GameData;
        _ui.Initialize(gameData);
    }

    public override void _Input(InputEvent @event)
    {
        _game.Mosquito._Input(@event);
    }
}