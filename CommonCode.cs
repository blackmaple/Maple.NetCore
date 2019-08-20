using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.IO;

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
        public static  DateTime FromUnixTicks(this long timestamp)
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
