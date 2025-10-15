using Godot;

public partial class SuckableArea : Area3D
{
    public float RemainingBlood { get; set; }
    public bool IsBeingSucked { get; private set; }

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (!IsBeingSucked && body is Mosquito)
        {
            IsBeingSucked = true;
        }
    }

    private void OnBodyExited(Node3D body)
    {
        if (IsBeingSucked && body is Mosquito)
        {
            IsBeingSucked = false;
        }
    }
}