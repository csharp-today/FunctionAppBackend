using System;

namespace CSharpToday.FunctionAppBackend.Connections
{
    public class EnvironmentConnectionProvider : SimpleConnectionProvider
    {
        public EnvironmentConnectionProvider(string environmentVariableName)
            : base(Environment.GetEnvironmentVariable(environmentVariableName)) { }
    }
}
