namespace altCrypt.Core.Encryption
{
    public interface IKey
    {
        byte[] GenerateBlock(BlockSize keySize);
    }
}
