namespace Service.Common.Interfaces.Infrastructure
{
    public interface IHashService
    {
        (byte[] hash, byte[] key) GetHash(string value, byte[]? key = null);
    }
}
