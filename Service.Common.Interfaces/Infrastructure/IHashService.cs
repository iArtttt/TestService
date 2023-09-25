namespace Service.Common.Interfaces.Infrastructure
{
    internal interface IHashService
    {
        (byte[] hash, byte[] key) GetHash(string value, byte[]? key = null);
    }
}
