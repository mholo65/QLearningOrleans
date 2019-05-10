using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;

namespace QLearningOrleans.Interfaces
{
    public interface IQState : IGrainWithGuidKey
    {
        [OneWay]
        Task StartTrainingAsync(int initialTransitionValue);

        [OneWay]
        Task TransitionAsync(int? previousStateToken, int transitionValue);
    }
}
