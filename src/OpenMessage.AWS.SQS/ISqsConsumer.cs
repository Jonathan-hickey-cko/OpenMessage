using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OpenMessage.AWS.SQS
{
    internal interface ISqsConsumer<T> where T : class
    {
        Task<List<SqsMessage<T>>> ConsumeAsync(CancellationToken cancellationToken);
        void Initialize(string consumerId, CancellationToken cancellationToken);
    }
}