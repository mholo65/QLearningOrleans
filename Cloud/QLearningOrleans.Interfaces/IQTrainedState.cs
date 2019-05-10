using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace QLearningOrleans.Interfaces
{
    public interface IQTrainedState : IGrainWithIntegerKey
    {
        Task AddChildQTrainedStateAsync(int stateToken, double reward);

        Task<List<int>> GetChildrenQTrainedStatesAsync();
        
    }
}