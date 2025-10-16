public class GameData
{
    public float MosquitoSize { get; set; }
    public float Annoyance { get; set; }
    public float Sleepiness { get; set; }
    public int Stage { get; set; }
    public bool IsGameOver => Annoyance > 0.999f;
    public bool IsSucking { get; set; }
    public float AnnoyanceGenerationRate { get; set; }
}