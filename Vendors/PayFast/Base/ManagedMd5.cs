namespace PayFast.Base
{
    /// <summary>
    /// Provides a managed implementation of the MD5 hashing algorithm for computing 128-bit hash values from input
    /// data.
    /// </summary>
    /// <remarks>This class is intended for internal use and is not intended to be used directly from your
    /// code. It offers a fully managed alternative to platform-specific MD5 implementations, which may be useful in
    /// environments where native cryptographic libraries are unavailable or undesirable. The MD5 algorithm is
    /// considered cryptographically broken and unsuitable for further use in security-sensitive applications.</remarks>
    internal static class ManagedMd5
    {
        /// <summary>
        /// Provides the per-round shift amounts used in the MD5 hash algorithm transformation steps.
        /// </summary>
        /// <remarks>These constants define the left rotation amounts applied to the intermediate hash
        /// values during each round of the MD5 computation. The values are specified by the MD5 standard (RFC 1321) and
        /// should not be modified.</remarks>
        private static readonly int[] S =
        {
            7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
            5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
            4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
            6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
        };

        /// <summary>
        /// Provides the set of constant values used as part of the MD5 hash algorithm's transformation rounds.
        /// </summary>
        /// <remarks>These constants are defined by the MD5 specification (RFC 1321) and are used
        /// internally during the computation of MD5 message digests. They are not intended for direct use outside of
        /// the hash algorithm implementation.</remarks>
        private static readonly uint[] K =
        {
            0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
            0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
            0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
            0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
            0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
            0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
            0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
            0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
            0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
            0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
            0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
            0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
            0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
            0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
            0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
            0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
        };

        /// <summary>
        /// Computes the MD5 hash value for the specified input data.
        /// </summary>
        /// <remarks>The returned hash is suitable for use in cryptographic and data integrity scenarios
        /// where a fixed-size hash value is required. This method does not provide cryptographic security against
        /// intentional tampering; for security-sensitive applications, consider using a more secure hash
        /// algorithm.</remarks>
        /// <param name="input">The input data to compute the hash for.</param>
        /// <returns>A 16-byte array containing the MD5 hash of the input data.</returns>
        public static byte[] ComputeHash(ReadOnlySpan<byte> input)
        {
            var paddedLength = CalculatePaddedLength(input.Length);
            var padded = new byte[paddedLength];

            input.CopyTo(padded);
            padded[input.Length] = 0x80;

            var bitLength = (ulong)input.Length * 8UL;
            for (var i = 0; i < 8; i++)
            {
                padded[paddedLength - 8 + i] = (byte)(bitLength >> (8 * i));
            }

            uint a0 = 0x67452301;
            uint b0 = 0xefcdab89;
            uint c0 = 0x98badcfe;
            uint d0 = 0x10325476;

            var chunk = new uint[16];

            for (var offset = 0; offset < padded.Length; offset += 64)
            {
                for (var i = 0; i < 16; i++)
                {
                    var index = offset + i * 4;
                    chunk[i] = (uint)(padded[index] | (padded[index + 1] << 8) | (padded[index + 2] << 16) | (padded[index + 3] << 24));
                }

                uint a = a0;
                uint b = b0;
                uint c = c0;
                uint d = d0;

                for (var i = 0; i < 64; i++)
                {
                    uint f;
                    int g;

                    if (i < 16)
                    {
                        f = (b & c) | (~b & d);
                        g = i;
                    }
                    else if (i < 32)
                    {
                        f = (d & b) | (~d & c);
                        g = (5 * i + 1) % 16;
                    }
                    else if (i < 48)
                    {
                        f = b ^ c ^ d;
                        g = (3 * i + 5) % 16;
                    }
                    else
                    {
                        f = c ^ (b | ~d);
                        g = (7 * i) % 16;
                    }

                    var temp = d;
                    d = c;
                    c = b;
                    b = b + LeftRotate(a + f + K[i] + chunk[g], S[i]);
                    a = temp;
                }

                a0 += a;
                b0 += b;
                c0 += c;
                d0 += d;
            }

            var result = new byte[16];
            WriteUInt32LittleEndian(result, 0, a0);
            WriteUInt32LittleEndian(result, 4, b0);
            WriteUInt32LittleEndian(result, 8, c0);
            WriteUInt32LittleEndian(result, 12, d0);

            return result;
        }

        /// <summary>
        /// Calculates the total length, in bytes, required to pad a message of the specified original length according
        /// to the standard padding scheme used in cryptographic hash functions.
        /// </summary>
        /// <remarks>This method is typically used when preparing data for block-based cryptographic
        /// algorithms, such as SHA-1 or SHA-256, which require messages to be padded to a specific block size. The
        /// calculation includes a mandatory padding byte and an 8-byte length field, ensuring the result is a multiple
        /// of 64 bytes.</remarks>
        /// <param name="originalLength">The length of the original message, in bytes. Must be non-negative.</param>
        /// <returns>The total length, in bytes, of the padded message, including the original data, padding, and length
        /// encoding.</returns>
        private static int CalculatePaddedLength(int originalLength)
        {
            var padding = (56 - ((originalLength + 1) % 64) + 64) % 64;
            return originalLength + 1 + padding + 8;
        }

        /// <summary>
        /// Performs a circular left rotation on a 32-bit unsigned integer by the specified number of bits.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to rotate.</param>
        /// <param name="bits">The number of bits to rotate to the left. Must be between 0 and 31, inclusive.</param>
        /// <returns>A 32-bit unsigned integer that is the result of rotating the input value to the left by the specified number
        /// of bits.</returns>
        private static uint LeftRotate(uint value, int bits)
        {
            return (value << bits) | (value >> (32 - bits));
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer to the specified buffer at the given offset in little-endian byte order.
        /// </summary>
        /// <remarks>This method writes the value in little-endian format, with the least significant byte
        /// at the lowest address. No bounds checking is performed; callers must ensure the buffer is large enough to
        /// accommodate the value at the specified offset.</remarks>
        /// <param name="buffer">The byte array to which the value will be written. Must not be null and must have sufficient space starting
        /// at the specified offset.</param>
        /// <param name="offset">The zero-based index in the buffer at which to begin writing the value. There must be at least four bytes
        /// available from this position.</param>
        /// <param name="value">The 32-bit unsigned integer value to write to the buffer.</param>
        private static void WriteUInt32LittleEndian(byte[] buffer, int offset, uint value)
        {
            buffer[offset] = (byte)(value & 0xff);
            buffer[offset + 1] = (byte)((value >> 8) & 0xff);
            buffer[offset + 2] = (byte)((value >> 16) & 0xff);
            buffer[offset + 3] = (byte)((value >> 24) & 0xff);
        }
    }
}
