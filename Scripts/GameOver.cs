using Godot;

public partial class GameOver : Control
{
    public void OnRestart()
    {
        GetTree().ReloadCurrentScene();
    }
}