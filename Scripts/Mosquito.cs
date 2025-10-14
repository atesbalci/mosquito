using Godot;

public partial class Mosquito : Node3D
{
    private float _yaw;
    private float _pitch;
    
    public Mosquito()
    {
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
    }

    public override void _Process(double delta)
    {
        
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion inputMouseMotion)
        {
            OffsetYawAndPitch(inputMouseMotion.Relative.X * 0.01f, -inputMouseMotion.Relative.Y * 0.01f);
        }
    }

    private void OffsetYawAndPitch(float yaw, float pitch)
    {
        _yaw = Mathf.PosMod(_yaw + yaw, 360f);
        _pitch = Mathf.Clamp(_pitch + pitch, -89.9f, 89.9f);
        Rotation = new Vector3(_pitch, _yaw, 0f);
    }
}
