using System.Security.Cryptography;

namespace IRISChatServer.Extensions
{
    public static class ByteArrayExtension
    {
        public static byte[] Randomize(this byte[] Src)
        {
            using (RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider())
            {
                RNG.GetNonZeroBytes(Src);
                return Src;
            }
        }
    }
}