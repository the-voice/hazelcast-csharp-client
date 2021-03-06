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
using Hazelcast.Core;

namespace Hazelcast.IO.Serialization
{
    /// <summary>This is an exception thrown when an exception occurs while serializing/deserializing objects.</summary>
    public class HazelcastSerializationException : HazelcastException
    {
        public HazelcastSerializationException(string message)
            : base(message)
        {
        }

        public HazelcastSerializationException(string message, Exception cause)
            : base(message, cause)
        {
        }

        public HazelcastSerializationException(Exception e)
            : base(e)
        {
        }
    }
}