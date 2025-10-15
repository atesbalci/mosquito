using Godot;

public partial class Mosquito : RigidBody3D
{
    private Camera3D _camera;
    
    private float _yaw;
    private float _pitch;
    private bool _locked;
    
    public float Acceleration { get; set; }
    public float MaxVelocity { get; set; }

    public void SetLocked(bool locked)
    {
        _locked = locked;
        Input.SetMouseMode(locked ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured);
    }

    public override void _Ready()
    {
        SetLocked(false);
        _camera = GetNode<Camera3D>(GetMeta("Camera").AsString());
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_locked)
        {
            LinearVelocity = Vector3.Zero;
            return;
        }
        float deltaFloat = (float) delta;
        Vector3 velocityVector = Vector3.Zero;
        Quaternion rotation = _camera.GetQuaternion();
        Vector3 up = Vector3.Up;
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

        var newVelocity = LinearVelocity + deltaFloat * Acceleration * velocityVector.Normalized();
        LinearVelocity = newVelocity.Normalized() * Mathf.Min(MaxVelocity, newVelocity.Length());
    }

    public override void _Input(InputEvent @event)
    {
        if (_locked) return;
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
