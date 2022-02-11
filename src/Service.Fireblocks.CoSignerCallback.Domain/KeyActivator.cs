namespace Service.Fireblocks.CoSignerCallback.Domain
{
    public class KeyActivator
    {
        public KeyActivator()
        {
            IsActivated = false;
        }

        public KeyActivator(string coSignerPubKey, string privateKey)
        {
            Activate(coSignerPubKey, privateKey);
        }

        public string CoSignerPubKey { get; private set; }
        public string PrivateKey { get; private set; }
        public bool IsActivated { get; private set; }

        public void Activate(string coSignerPubKey, string privateKey)
        {
            CoSignerPubKey = coSignerPubKey;
            PrivateKey = privateKey;
            IsActivated = true;
        }
    }
}
