using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using QLearningOrleans.Interfaces;

namespace QAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QTrainerController : ControllerBase
    {
        private readonly IClusterClient _client;

        public QTrainerController(IClusterClient client)
        {
            _client = client;
        }

        // GET: api/values
        [HttpGet()]
        [Route("[action]/{startTrans:int}")]
        public async Task<IActionResult> Start(int startTrans)
        {
            try
            {
                var grain = _client.GetGrain<IQState>(Guid.NewGuid());

                await grain.StartTrainingAsync(startTrans);

                return Ok(startTrans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet()]
        [Route("[action]/{stateToken}")]
        public async Task<int> NextValue(int stateToken)
        {
            var grain = _client.GetGrain<IQTrainedState>(stateToken);

            var qs = await grain.GetChildrenQTrainedStatesAsync();

            return qs[new Random().Next(0, qs.Count)];
        }
    }
}
