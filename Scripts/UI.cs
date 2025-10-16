using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Range = Godot.Range;

public partial class UI : Node
{
    private GameData _gameData;
    private Range _bloodMeter;
    private Range _annoyanceMeter;
    private bool _gameOver;
    private Splash _splash;
    
    public void Initialize(GameData gameData)
    {
        _gameData = gameData;
    }

    public override void _Ready()
    {
        _bloodMeter = GetNode<Range>(GetMeta("BloodMeter").AsNodePath());
        _annoyanceMeter = GetNode<Range>(GetMeta("AnnoyanceMeter").AsNodePath());
        SpawnSplash(SplashImage.Start);
    }

    public override void _Process(double delta)
    {
        _bloodMeter.SetValue(_gameData.MosquitoSize);
        _annoyanceMeter.SetValue(_gameData.Annoyance);
        if (_gameData.IsGameOver)
        {
            if (!_gameOver)
            {
                _gameOver = true;
                SpawnSplash(SplashImage.End);
                AddChild(GetMeta("GameOver").As<PackedScene>().Instantiate());
            }
        }
        else if (Input.IsKeyLabelPressed(Key.Space))
        {
            DespawnSplash();
        }
    }

    private void SpawnSplash(SplashImage splashImage)
    {
        _splash = (Splash) GetMeta("Splash").As<PackedScene>().Instantiate();
        _splash.Show(splashImage);
        AddChild(_splash);
    }

    private void DespawnSplash()
    {
        if (_splash == null) return;
        RemoveChild(_splash);
    }
}