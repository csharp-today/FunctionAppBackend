namespace CSharpToday.FunctionAppBackend.Connections
{
    public class AzureConfiguraitonConnectionProvider : EnvironmentConnectionProvider
    {
        public AzureConfiguraitonConnectionProvider(string connectionStringName)
            : base("CUSTOMCONNSTR_" + connectionStringName) { }
    }
}
