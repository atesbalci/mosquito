using Godot;

public enum SplashImage
{
    Start,
    End
}

public partial class Splash : TextureRect
{
    public void Show(SplashImage splashImage)
    {
        Texture = GetMeta(splashImage == SplashImage.Start ?  "Start" : "End").As<Texture2D>();
    }
}