using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BerkeleyDB;

namespace WalletHash
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Usage: WalletHash wallet.dat");
                return;
            }

            try
            {
                Run(args[0]);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void Run(string filename)
        {
            MKey mkey = null;
            using (var db = Database.Open(filename, "main", new DatabaseConfig()))
            {
                foreach (var entry in db.Cursor())
                {
                    var keyStream = new BCDataStream(entry.Key.Data);

                    var type = keyStream.ReadString();

                    if (type == "mkey")
                    {
                        var valueStream = new BCDataStream(entry.Value.Data);

                        // var nID = keyStream.ReadUInt32();
                        var encryptedKey = valueStream.ReadBytes(valueStream.ReadCompactSize());
                        var salt = valueStream.ReadBytes(valueStream.ReadCompactSize());
                        var derivationMethod = valueStream.ReadInt32();
                        var derivationIterations = valueStream.ReadInt32();
                        // var otherParams = valueStream.ReadString();

                        if (mkey != null) throw new NotSupportedException("Multiple mkeys!");
                        mkey = new MKey(encryptedKey, salt, derivationMethod, derivationIterations);
                    }
                }
            }

            if (mkey == null) throw new NotSupportedException("No mkey present");
            if (mkey.DerivationMethod != 0) throw new NotSupportedException("Unsupported derivation method");

            int expectedKeyLength;
            switch (mkey.Salt.Length)
            {
                case 0:
                    throw new NotSupportedException("Not encrypted!");

                case 8:
                    expectedKeyLength = 48; // 32 bytes padded to 3 AES blocks (last block is padding-only)
                    break;

                case 16:
                    expectedKeyLength = 80; // 72 bytes padded to whole AES blocks
                    break;

                default:
                    throw new NotSupportedException("Unsupported salt length");
            }

            if (mkey.EncryptedKey.Length != expectedKeyLength) throw new NotSupportedException("Unexpected key length");
            var keyPart = new ArraySegment<byte>(mkey.EncryptedKey, expectedKeyLength - 32, 32);

            Console.WriteLine("$bitcoin${0}${1}${2}${3}${4}$2$00$2$00", 64, BytesToHex(keyPart), mkey.Salt.Length * 2, BytesToHex(mkey.Salt), mkey.DerivationIterations);
        }

        private static string BytesToHex(IEnumerable<byte> seg) => String.Join("", seg.Select(b => b.ToString("x2")));
    }

    struct BCDataStream
    {
        private readonly byte[] data;
        private int position;

        public BCDataStream(byte[] data) : this()
        {
            this.data = data;
        }

        public string ReadString()
        {
            var length = ReadCompactSize();
            var result = Encoding.ASCII.GetString(data, position, length);
            position += length;
            return result;
        }

        public int ReadCompactSize()
        {
            var b = data[position++];
            switch (b)
            {
                case 253:
                    return data[position++] | (data[position++] << 8);

                case 254:
                    return ReadInt32();

                case 255:
                    throw new NotSupportedException("Long strings not supported");

                default:
                    return b;
            }
        }

        public byte[] ReadBytes(int length)
        {
            var result = new byte[length];
            Array.ConstrainedCopy(data, position, result, 0, length);
            position += length;
            return result;
        }

        public int ReadInt32()
        {
            return data[position++] | (data[position++] << 8) | (data[position++] << 16) | (data[position++] << 24);
        }
    }

    class MKey
    {
        public readonly byte[] EncryptedKey;
        public readonly byte[] Salt;
        public readonly int DerivationMethod;
        public readonly int DerivationIterations;

        public MKey(byte[] encryptedKey, byte[] salt, int derivationMethod, int derivationIterations)
        {
            this.EncryptedKey = encryptedKey;
            this.Salt = salt;
            this.DerivationMethod = derivationMethod;
            this.DerivationIterations = derivationIterations;
        }
    }
}