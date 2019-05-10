using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using QLearningOrleans.Interfaces;

namespace QLearningOrleans.Grains
{
    public abstract class QState : Grain, IQState
    {
        internal abstract IReward GetReward(int? previousStateToken, int transitionValue);
        internal abstract IEnumerable<int> GetTransitions(int stateToken);
        internal abstract IEnumerable<IPastState> GetRewardingQStates(int stateToken);

        public Task StartTrainingAsync(int initialTransitionValue)
        {
            return TransitionAsync(null, initialTransitionValue);
        }

        public Task TransitionAsync(int? previousStateToken, int transitionValue)
        {
            var rwd = GetReward(previousStateToken, transitionValue);

            var stateToken = transitionValue;
            if (previousStateToken != null)
            {
                stateToken = int.Parse(previousStateToken.Value + stateToken.ToString());
            }

            var ts = new List<Task>();

            if (rwd == null || !rwd.IsAbsorbing)
            {
                ts.AddRange(GetTransitions(stateToken).Select(p => GrainFactory.GetGrain<IQState>(Guid.NewGuid()).TransitionAsync(stateToken, p)));
            }

            if (rwd != null)
            {
                ts.Add(SetRewardAsync(rwd.StateToken, rwd.Value, rwd.Discount));
            }

            return Task.WhenAll(ts);
        }

        public Task SetRewardAsync(int stateToken, double stateReward, double discount)
        {
            var t = new List<Task>();

            foreach (var pastState in GetRewardingQStates(stateToken))
            {
                var reward = stateReward;
                t.Add(GrainFactory.GetGrain<IQTrainedState>(pastState.StateToken)
                    .AddChildQTrainedStateAsync(pastState.NextStateToken, reward));

                stateReward = stateReward * discount;
            }

            return Task.WhenAll(t);
        }
    }
}
