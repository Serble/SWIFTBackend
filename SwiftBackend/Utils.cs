using System.Security.Cryptography;

namespace SwiftBackend;

public static class Utils {
    public static long DateTimeToUnixMillis(this DateTime dateTime) {
        DateTime unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan timeSpan = dateTime - unixEpoch;
        return (long) timeSpan.TotalMilliseconds;
    }

    public static DateTime UnixMillisToDateTime(this long unixMillis) {
        DateTime unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return unixEpoch.AddMilliseconds(unixMillis);
    }

    /// <summary>
    /// Decrypts bytes to a string using AES encryption
    /// </summary>
    /// <param name="cipherText">The encrypted bytes</param>
    /// <param name="key">AES Key</param>
    /// <param name="IV">Initialisation Vector</param>
    /// <returns>Decrypted text</returns>
    public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] key, byte[] IV) {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = IV;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream msDecrypt = new(cipherText);
        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
        csDecrypt.FlushFinalBlock();
        msDecrypt.ToArray();
        using StreamReader srDecrypt = new(csDecrypt);
        string plaintext = srDecrypt.ReadToEnd();
        return plaintext;
    }

    // public static byte[] DecryptBytesFromBytes_Aes(byte[] cipherText, byte[] key, byte[] IV) {
    //     using Aes aes = Aes.Create();
    //     aes.Key = key;
    //     aes.IV = IV;
    //
    //     ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
    //
    //     using MemoryStream msDecrypt = new(cipherText);
    //     using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
    //     csDecrypt.FlushFinalBlock();
    //     return msDecrypt.ToArray();
    // }
    
    public static byte[] DecryptBytesFromBytes_Aes(byte[] cipherText, byte[] key, byte[] IV) {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = IV;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream msDecrypt = new(cipherText);
        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
        using MemoryStream output = new();
        csDecrypt.CopyTo(output);
        return output.ToArray();
    }

    /// <summary>
    /// Encrypts a string to bytes using AES encryption
    /// </summary>
    /// <param name="plainText">The text to encrypt</param>
    /// <param name="key">AES Key</param>
    /// <param name="IV">Initialisation vector</param>
    /// <returns>The encrypted bytes</returns>
    public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] IV) {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = IV;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using MemoryStream msEncrypt = new();
        using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (StreamWriter swEncrypt = new(csEncrypt)) {
            swEncrypt.Write(plainText);
        }

        byte[] encrypted = msEncrypt.ToArray();
        return encrypted;
    }
    
    // public static byte[] EncryptBytesToBytes_Aes(byte[] input, byte[] key, byte[] IV) {
    //     using Aes aesAlg = Aes.Create();
    //     aesAlg.Key = key;
    //     aesAlg.IV = IV;
    //
    //     ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
    //     using MemoryStream ms = new();
    //     using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write)) {
    //         cs.Write(input, 0, input.Length);
    //     }
    //     byte[] encrypted = ms.ToArray();
    //
    //     return encrypted;
    // }
    
    public static byte[] EncryptBytesToBytes_Aes(byte[] input, byte[] key, byte[] IV) {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = IV;

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        using MemoryStream ms = new MemoryStream();
        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
            cs.Write(input, 0, input.Length);
            cs.FlushFinalBlock();
        }
        byte[] encrypted = ms.ToArray();

        return encrypted;
    }

    public static RSAParameters GenerateRsaKeyPair() {
        using RSACryptoServiceProvider rsa = new();
        return rsa.ExportParameters(includePrivateParameters: true);
    }

    public static byte[] EncryptWithRsa(byte[] data, RSAParameters publicKey) {
        using RSACryptoServiceProvider rsa = new();
        rsa.ImportParameters(publicKey);
        return rsa.Encrypt(data, false);
    }

    public static byte[] DecryptWithRsa(byte[] data, RSAParameters privateKey) {
        using RSACryptoServiceProvider rsa = new();
        rsa.ImportParameters(privateKey);
        return rsa.Decrypt(data, false);
    }

    public static ReadOnlySpan<T> ToReadOnlySpan<T>(this T[] array) {
        return new ReadOnlySpan<T>(array);
    }
    
    public static bool AreByteArraysEqual(this byte[] a1, byte[] a2) {
        if (a1.Length != a2.Length) {
            return false;
        }
        return !a1.Where((t, i) => t != a2[i]).Any();
    }

    public static string ByteArrayToBase64(this byte[] bytes) {
        return Convert.ToBase64String(bytes);
    }

}