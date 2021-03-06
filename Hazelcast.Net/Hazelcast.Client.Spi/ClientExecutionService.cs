// Copyright (c) 2008-2015, Hazelcast, Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Hazelcast.Core;

namespace Hazelcast.Client.Spi
{
    internal sealed class ClientExecutionService : IClientExecutionService
    {
        private readonly TaskFactory _taskFactory = Task.Factory;

        public ClientExecutionService(string name, int poolSize)
        {
        }

        public Task Submit(Action action)
        {
            return _taskFactory.StartNew(action);
        }

        public Task<T> Submit<T>(Func<T> function)
        {
            return _taskFactory.StartNew(function);
        }

        public void ScheduleWithFixedDelay(Action command, long initialDelay, long period, TimeUnit unit, CancellationToken token)
        {
            ScheduleWithCancellation(command, initialDelay, unit, token).ContinueWith(task =>
            {
                if (!task.IsCanceled)
                {
                    ScheduleWithFixedDelay(command, period, period, unit, token);
                }
            }, token);
        }

        public Task ScheduleWithCancellation(Action command, long delay, TimeUnit unit, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<object>();
            var continueTask = tcs.Task.ContinueWith(t =>
            {
                token.ThrowIfCancellationRequested();
                if (t.IsCompleted)
                {
                    command();
                }
            });
            new Timer(o => tcs.SetResult(null)).Change(unit.ToMillis(delay), Timeout.Infinite);
            return continueTask;
        }

        public Task Schedule(Action command, long delay, TimeUnit unit)
        {
            var tcs = new TaskCompletionSource<object>();
            var continueTask = tcs.Task.ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    command();
                }
            });
            new Timer(o => tcs.SetResult(null)).Change(unit.ToMillis(delay), Timeout.Infinite);
            return continueTask;
        }

        public void Shutdown()
        {
        }

        public Task<object> ScheduleAtFixedRate(Action command, long initialDelay, long period, TimeUnit unit)
        {
            throw new NotImplementedException();
        }

        internal Task SubmitInternal(Action action)
        {
            //TODO: should use different thread pool
            return _taskFactory.StartNew(action);
        }
    }
}