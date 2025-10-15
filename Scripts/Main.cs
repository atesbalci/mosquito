using Godot;

public partial class Main : Node
{
    public override void _Ready()
    {
        var gameData = GetNode<Game>(GetMeta("Game").AsNodePath()).GameData;
        GetNode<UI>(GetMeta("UI").AsNodePath()).Initialize(gameData);
    }
}