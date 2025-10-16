using Godot;

public partial class MosquitoAudio : Node
{
    private AudioStreamPlayer3D _buzzing;
    private AudioStreamPlayer3D _sucking;
    private GameData _gameData;
    private AudioParameters _audioParameters;

    public void Initialize(GameData gameData)
    {
        _gameData = gameData;
    }

    public override void _Ready()
    {
        _buzzing = GetNode<AudioStreamPlayer3D>("Buzzing");
        _sucking = GetNode<AudioStreamPlayer3D>("Sucking");
        _audioParameters = GetMeta("AudioParameters").As<AudioParameters>();

        _buzzing.VolumeLinear = 0f;
    }

    public override void _Process(double delta)
    {
        _buzzing.VolumeLinear = _gameData.IsGameOver
            ? 0f
            : _audioParameters.BuzzingCurve.Sample(_gameData.AnnoyanceGenerationRate);
        if (_gameData.IsSucking && !_sucking.IsPlaying()) _sucking.Play();
        else if (!_gameData.IsSucking && _sucking.IsPlaying()) _sucking.Stop();
    }
}