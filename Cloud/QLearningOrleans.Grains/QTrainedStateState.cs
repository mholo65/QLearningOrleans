using System.Collections.Generic;
using System.Runtime.Serialization;

namespace QLearningOrleans.Grains
{
    [DataContract]
    public class QTrainedStateState
    {
        [DataMember]
        public double MaximumReward { get; set; }

        [DataMember]
        public ISet<int> ChildrenQTrainedStates { get; } = new HashSet<int>();
    }
}