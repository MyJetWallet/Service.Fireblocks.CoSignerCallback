namespace Service.Fireblocks.Signer.NoSql
{
    public class CoSignerApiKeysNoSql : MyNoSqlServer.Abstractions.MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-fireblocks-cosigner-keys";

        public static string GeneratePartitionKey() => "Key";

        public static string GenerateRowKey() => "CoSignerCallbackHandler";

        public string CoSignerPubKey { get; set; }

        public string PrivateKey { get; set; }

        public static CoSignerApiKeysNoSql Create(string coSignerPubKey, string privateKey)
        {
            return new CoSignerApiKeysNoSql()
            {
                CoSignerPubKey = coSignerPubKey,
                PrivateKey = privateKey,
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(),
            };
        }
    }
}
