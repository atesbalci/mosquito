using Godot;

public partial class Mosquito : RigidBody3D
{
    private Camera3D _camera;
    
    private float _yaw;
    private float _pitch;

    public Mosquito()
    {
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
    }

    public override void _Ready()
    {
        base._Ready();
        _camera = GetNode<Camera3D>(GetMeta("Camera").AsString());
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        float deltaFloat = (float) delta;
        Vector3 velocityVector = Vector3.Zero;
        Quaternion rotation = _camera.GetQuaternion();
        Vector3 up = rotation * Vector3.Up;
        Vector3 forward = rotation * Vector3.Forward;
        Vector3 right = rotation * Vector3.Right;
        if (Input.IsKeyLabelPressed(Key.W))
            velocityVector += forward;
        if (Input.IsKeyLabelPressed(Key.S))
            velocityVector -= forward;
        if (Input.IsKeyLabelPressed(Key.A))
            velocityVector -= right;
        if (Input.IsKeyLabelPressed(Key.D))
            velocityVector += right;
        if (Input.IsKeyLabelPressed(Key.Space))
            velocityVector += up;
        if (Input.IsKeyLabelPressed(Key.Ctrl))
            velocityVector -= up;

        LinearVelocity += deltaFloat * 100f * velocityVector.Normalized();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseMotion inputMouseMotion)
        {
            OffsetYawAndPitch(-inputMouseMotion.Relative.X * 0.1f, -inputMouseMotion.Relative.Y * 0.1f);
        }
    }

    private void OffsetYawAndPitch(float yaw, float pitch)
    {
        _yaw = Mathf.PosMod(_yaw + yaw, 360f);
        _pitch = Mathf.Clamp(_pitch + pitch, -89.9f, 89.9f);
        _camera.RotationDegrees = new Vector3(_pitch, _yaw + 180, 0f);
    }
}
