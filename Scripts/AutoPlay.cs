using Godot;

public partial class AutoPlay : AnimationPlayer
{
    public override void _Ready()
    {
        Play(GetAnimationList()[0]);
    }
}