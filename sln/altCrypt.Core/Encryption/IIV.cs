namespace altCrypt.Core.Encryption
{
    public interface IIV
    {
        byte[] GenerateIV(int blockSize);
    }
}
