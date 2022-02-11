using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.Fireblocks.CoSignerCallback.Settings
{
    public class SettingsModel
    {
        [YamlProperty("FireblocksCoSignerCallback.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("FireblocksCoSignerCallback.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("FireblocksCoSignerCallback.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("FireblocksCoSignerCallback.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; set; }
    }
}
