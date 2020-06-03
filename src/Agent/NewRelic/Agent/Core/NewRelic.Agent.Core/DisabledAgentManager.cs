using System;
using NewRelic.Agent.Core.Tracer;

namespace NewRelic.Agent.Core
{
    public sealed class DisabledAgentManager : IAgentManager
    {
        public ITracer GetTracerImpl(string tracerFactoryName, uint tracerArguments, string metricName, string assemblyName, Type type, string typename, string methodName, string argumentSignature, object invocationTarget, object[] arguments, ulong functionId)
        {
            return null;
        }
    }
}
