using System;

namespace Base.Common.Encryption
{
    public class EncryptionService
    {
        private readonly KeyInfo _keyInfo;

        public EncryptionService(KeyInfo keyInfo = null)
        {
            _keyInfo = keyInfo;
        }

        public KeyInfo GetKeyInfo()
        { return _keyInfo; }

        public string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var enc = Encryption.EncryptStringToBytes_Aes(input, _keyInfo.Key, _keyInfo.Iv);
            return Convert.ToBase64String(enc);
        }
        public byte[] EncryptToByteArray(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            return Encryption.EncryptStringToBytes_Aes(input, _keyInfo.Key, _keyInfo.Iv);
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return string.Empty;
            var cipherBytes = Convert.FromBase64String(cipherText);
            return Encryption.DecryptStringFromBytes_Aes(cipherBytes, _keyInfo.Key, _keyInfo.Iv);
        }

        //public string Decrypt(string cipherText)
        //{
        //    if (string.IsNullOrEmpty(cipherText)) return string.Empty;
        //    var cipherBytes = Convert.FromBase64String(cipherText);
        //    return Encryption.DecryptStringFromBytes_Aes(cipherBytes, _keyInfo.Key, _keyInfo.Iv);
        //}
    }
}
