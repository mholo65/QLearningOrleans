namespace QLearningOrleans.Interfaces
{
    public interface IPastState
    {
        int StateToken { get; set; }
        int NextStateToken { get; set; }
    }
}