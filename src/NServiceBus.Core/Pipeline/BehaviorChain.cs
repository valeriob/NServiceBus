﻿namespace NServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Janitor;
    using Pipeline;

    class BehaviorChain : IDisposable
    {
        public BehaviorChain(IEnumerable<BehaviorInstance> behaviorList)
        {
            itemDescriptors = behaviorList.ToArray();
        }

        public void Dispose()
        {
        }

        public Task Invoke(IBehaviorContext context)
        {
            Guard.AgainstNull(nameof(context), context);

            return InvokeNext(context, 0);
        }

        Task InvokeNext(IBehaviorContext context, int currentIndex)
        {
            if (currentIndex == itemDescriptors.Length)
            {
                return TaskEx.CompletedTask;
            }

            var behavior = itemDescriptors[currentIndex];

            return behavior.Invoke(context, newContext => InvokeNext(newContext, currentIndex + 1));
        }

        [SkipWeaving] BehaviorInstance[] itemDescriptors;
    }
}