using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Maple.NetCore
{
    public static class CCommonCode
    {
        private static JsonSerializerSettings JsonSettings { get; }

        static CCommonCode()
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            JsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                DateFormatString = "yyyy.MM.dd HH:mm:ss",
            };
        }

        public static string ToJson(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj, JsonSettings);
            return json;
        }

        public static T FromJson<T>(this string json)
        {
            var obj = JsonConvert.DeserializeObject<T>(json, JsonSettings);
            return obj;
        }

        public static string ToBase64(this byte[] buffer)
        {
            return Convert.ToBase64String(buffer);
        }

        public static string ToBase64Url(this byte[] buffer)
        {
            return Base64UrlTextEncoder.Encode(buffer);
        }

        public static byte[] FromBase64Url(this string str)
        {
            return Base64UrlTextEncoder.Decode(str);
        }

        public static string AESEncrypt(this string input, string key)
        {
            var buffer = Encoding.UTF8.GetBytes(key);
            var encode = EncryptStringToBytes_Aes(input, buffer);
            return encode.ToBase64Url();
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] keyBuffer)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (keyBuffer == null || keyBuffer.Length <= 0)
                throw new ArgumentNullException("Key");

            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(keyBuffer, null);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        public static string AESDecrypt(this string input, string key)
        {
            var encode = input.FromBase64Url();
            var buffer = Encoding.UTF8.GetBytes(key);
            return DecryptStringFromBytes_Aes(encode, buffer);
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] keyBuffer)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (keyBuffer == null || keyBuffer.Length <= 0)
                throw new ArgumentNullException("Key");


            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;


                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(keyBuffer, null);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        public static string ToXml<T>(this T obj)
        {
            using (var ms = new MemoryStream(1024 * 4))
            {
                var settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    OmitXmlDeclaration = false,
                    Indent = true,
                    NewLineHandling = NewLineHandling.Entitize,
                    NewLineOnAttributes = true
                };
                using (var xmlWriter = XmlWriter.Create(ms, settings))
                {
                    var xmlNameSpace = new XmlSerializerNamespaces();
                    xmlNameSpace.Add(string.Empty, string.Empty);

                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(xmlWriter, obj, xmlNameSpace);
                }
                var xml = Encoding.UTF8.GetString(ms.ToArray());
                return xml;
            }
        }


        public static T FromXml<T>(this string xml)
        {
            var buffer = Encoding.UTF8.GetBytes(xml);
            using (var ms = new MemoryStream(buffer))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                if (serializer.Deserialize(ms) is T xmlData)
                {
                    return xmlData;
                }
            }
            return default;
        }



        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// </summary>
        public static long ToUnixTicks(this DateTime dateTime)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1);
            return (dateTime.Ticks - dt1970.Ticks) / 10000;
        }

        public static long ToUnixSecond(this DateTime dateTime)
        {
            return dateTime.ToUnixTicks() / 1000;
        }


        /// <summary>
        /// 根据时间戳timestamp（单位毫秒）计算日期
        /// </summary>
        public static DateTime FromUnixTicks(this long timestamp)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1);
            long t = dt1970.Ticks + timestamp * 10000;
            return new DateTime(t);
        }

        /// <summary>
        /// Get the Span from the MemoryStream buffer instance.
        /// </summary>
        /// <param name="mStream">MemoryStream instance</param>
        /// <returns>Span instance from the buffer</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> AsSpan(this MemoryStream mStream)
            => mStream != null ? new Span<byte>(mStream.GetBuffer(), 0, (int)mStream.Length) : Span<byte>.Empty;
        /// <summary>
        /// Get the ReadOnlySpan from the MemoryStream buffer instance.
        /// </summary>
        /// <param name="mStream">MemoryStream instance</param>
        /// <returns>ReadOnlySpan instance from the buffer</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> AsReadOnlySpan(this MemoryStream mStream)
            => mStream != null ? new ReadOnlySpan<byte>(mStream.GetBuffer(), 0, (int)mStream.Length) : ReadOnlySpan<byte>.Empty;
        /// <summary>
        /// Get the Memory from the MemoryStream buffer instance.
        /// </summary>
        /// <param name="mStream">MemoryStream instance</param>
        /// <returns>Memory instance from the buffer</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Memory<byte> AsMemory(this MemoryStream mStream)
            => mStream != null ? new Memory<byte>(mStream.GetBuffer(), 0, (int)mStream.Length) : Memory<byte>.Empty;
        /// <summary>
        /// Get the ReadOnlyMemory from the MemoryStream buffer instance.
        /// </summary>
        /// <param name="mStream">MemoryStream instance</param>
        /// <returns>ReadOnlyMemory instance from the buffer</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<byte> AsReadOnlyMemory(this MemoryStream mStream)
            => mStream != null ? new ReadOnlyMemory<byte>(mStream.GetBuffer(), 0, (int)mStream.Length) : ReadOnlyMemory<byte>.Empty;
        /// <summary>
        /// Split a char memory using a separator
        /// </summary>
        /// <param name="memory">Source memory</param>
        /// <param name="separator">Char separator</param>
        /// <param name="options">StringSplit options</param>
        /// <returns>List with the split result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<ReadOnlyMemory<char>> Split(this ReadOnlyMemory<char> memory, char separator, StringSplitOptions options = StringSplitOptions.None)
        {
            var result = new List<ReadOnlyMemory<char>>();
            while (memory.Length > 0)
            {
                var idx = memory.Span.IndexOf(separator);
                if (idx == -1)
                    break;
                var value = memory.Slice(0, idx);
                if (options == StringSplitOptions.None || value.Length > 0)
                    result.Add(value);
                memory = memory.Slice(idx + 1);
            }
            if (options == StringSplitOptions.None || memory.Length > 0)
                result.Add(memory);
            return result;
        }
        /// <summary>
        /// Split a char memory using a separator
        /// </summary>
        /// <param name="memory">Source memory</param>
        /// <param name="separator">Char separator</param>
        /// <param name="options">StringSplit options</param>
        /// <returns>List with the split result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<ReadOnlyMemory<char>> Split(this ReadOnlyMemory<char> memory, ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
        {
            var result = new List<ReadOnlyMemory<char>>();
            while (memory.Length > 0)
            {
                var idx = memory.Span.IndexOf(separator);
                if (idx == -1)
                    break;
                var value = memory.Slice(0, idx);
                if (options == StringSplitOptions.None || value.Length > 0)
                    result.Add(value);
                memory = memory.Slice(idx + separator.Length);
            }
            if (options == StringSplitOptions.None || memory.Length > 0)
                result.Add(memory);
            return result;
        }
        /// <summary>
        /// Split a char memory using a separator
        /// </summary>
        /// <param name="memory">Source memory</param>
        /// <param name="separator">Char separator</param>
        /// <param name="options">StringSplit options</param>
        /// <returns>List with the split result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<string> SplitAsString(this ReadOnlyMemory<char> memory, char separator, StringSplitOptions options = StringSplitOptions.None)
            => SplitAsString(memory.Span, separator, options);
        /// <summary>
        /// Split a char memory using a separator
        /// </summary>
        /// <param name="memory">Source memory</param>
        /// <param name="separator">Char separator</param>
        /// <param name="options">StringSplit options</param>
        /// <returns>List with the split result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<string> SplitAsString(this ReadOnlyMemory<char> memory, ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
            => SplitAsString(memory.Span, separator, options);
        /// <summary>
        /// Split a char span using a separator
        /// </summary>
        /// <param name="span">Source span</param>
        /// <param name="separator">Char separator</param>
        /// <param name="options">StringSplit options</param>
        /// <returns>List with the split result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<string> SplitAsString(this ReadOnlySpan<char> span, char separator, StringSplitOptions options = StringSplitOptions.None)
        {
            var result = new List<string>();
            while (span.Length > 0)
            {
                var idx = span.IndexOf(separator);
                if (idx == -1)
                    break;
                var value = span.Slice(0, idx);
                if (options == StringSplitOptions.None || value.Length > 0)
                    result.Add(value.ToString());
                span = span.Slice(idx + 1);
            }
            if (options == StringSplitOptions.None || span.Length > 0)
                result.Add(span.ToString());
            return result;
        }
        /// <summary>
        /// Split a char span using a separator
        /// </summary>
        /// <param name="span">Source span</param>
        /// <param name="separator">Char separator</param>
        /// <param name="options">StringSplit options</param>
        /// <returns>List with the split result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<string> SplitAsString(this ReadOnlySpan<char> span, ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
        {
            var result = new List<string>();
            while (span.Length > 0)
            {
                var idx = span.IndexOf(separator);
                if (idx == -1)
                    break;
                var value = span.Slice(0, idx);
                if (options == StringSplitOptions.None || value.Length > 0)
                    result.Add(value.ToString());
                span = span.Slice(idx + separator.Length);
            }
            if (options == StringSplitOptions.None || span.Length > 0)
                result.Add(span.ToString());
            return result;
        }


    }
}
