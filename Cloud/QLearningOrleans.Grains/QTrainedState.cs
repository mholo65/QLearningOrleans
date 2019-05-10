﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using QLearningOrleans.Interfaces;

namespace QLearningOrleans.Grains
{
    public class QTrainedState : Grain<QTrainedStateState>, IQTrainedState
    {
        public Task AddChildQTrainedStateAsync(int stateToken, double reward)
        {
            if (reward < State.MaximumReward)
            {
                return Task.FromResult(true);
            }

            if (Math.Abs(reward - State.MaximumReward) < 0.10)
            {
                State.ChildrenQTrainedStates.Add(stateToken);
                return Task.FromResult(true);
            }

            State.MaximumReward = reward;
            State.ChildrenQTrainedStates.Clear();
            State.ChildrenQTrainedStates.Add(stateToken);

            return Task.FromResult(true);
        }

        public Task<List<int>> GetChildrenQTrainedStatesAsync()
        {
            return Task.FromResult(State.ChildrenQTrainedStates.ToList());
        }
    }
}