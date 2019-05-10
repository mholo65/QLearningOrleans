using System.Runtime.Serialization;
using QLearningOrleans.Interfaces;

namespace QLearningOrleans.Grains
{
    [DataContract]
    public class TicTacToeReward : IReward
    {
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public double Discount { get; set; }
        [DataMember]
        public bool IsAbsorbing { get; set; }
        [DataMember]
        public int StateToken { get; set; }
    }
}