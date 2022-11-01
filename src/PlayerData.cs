using System;
using System.Text;

public class PlayerData {
  protected const int REQUIRED_BUFFER_SIZE = 32;
  protected const ushort FILE_TAG = 0x8a8b;
  protected const String FILE_MAGIC = "SAVEME\x0d\x0a\x1a\x0a";
  protected const uint FILE_VERSION = 1;

  public ushort Deaths { get; set; } = 0;
  public ushort Collisions { get; set; } = 0;
  public ushort Progress { get; set; } = 0;
  public ulong PlayTime { get; set; } = 0;

  public static PlayerData FromBytes(byte[] buffer) {
    // Verify Buffer Size
    if (buffer.Length < REQUIRED_BUFFER_SIZE)
    {
      throw new ArgumentException("Invalid input buffer.");
    }
    // Verify Structure
    var tag = BitConverter.ToUInt16(buffer, 0);
    var magic = (new ASCIIEncoding()).GetString(buffer, 2, 10);
    var version = BitConverter.ToUInt32(buffer, 12);
    if (tag != FILE_TAG || magic != FILE_MAGIC || version != FILE_VERSION)
    {
      throw new ArgumentException("Invalid input buffer.");
    }
    // Read Data
    var res = new PlayerData();
    res.Deaths = BitConverter.ToUInt16(buffer, 16);
    res.Collisions = BitConverter.ToUInt16(buffer, 18);
    res.Progress = BitConverter.ToUInt16(buffer, 20);
    res.PlayTime = BitConverter.ToUInt64(buffer, 24);
    return res;
  }

  public byte[] GetBytes() {
    var res = new byte[REQUIRED_BUFFER_SIZE];
    // File Header
    // Detect endianness errors and 7-bit transmission errors.
    Buffer.BlockCopy(BitConverter.GetBytes(FILE_TAG), 0, res, 0, 2);
    // Detect line-ending conversion errors
    Buffer.BlockCopy((new ASCIIEncoding()).GetBytes(FILE_MAGIC), 0, res, 2, 10);
    Buffer.BlockCopy(BitConverter.GetBytes(FILE_VERSION), 0, res, 12, 4);
    // File Data
    Buffer.BlockCopy(BitConverter.GetBytes(Deaths), 0, res, 16, 2);
    Buffer.BlockCopy(BitConverter.GetBytes(Collisions), 0, res, 18, 2);
    Buffer.BlockCopy(BitConverter.GetBytes(Progress), 0, res, 20, 2);
    // Pad 2 bytes here
    Buffer.BlockCopy(BitConverter.GetBytes(PlayTime), 0, res, 24, 8);
    return res;
  }

  public override string ToString() {
    return $"PlayerData{{ Deaths: {Deaths}, Collisions: {Collisions}, Progress: {Progress}, PlayTime: {PlayTime} }}";
  }
}
