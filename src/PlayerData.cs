/**
 * Copyright (C) 2022 Alexander Rothman <gnomesort@megate.ch>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.Text;

/**
 * @brief In-Memory save file representation.
 */
public class PlayerData {
  protected const int REQUIRED_BUFFER_SIZE = 32;
  protected const ushort FILE_TAG = 0x8a8b;
  protected const String FILE_MAGIC = "SAVEME\x0d\x0a\x1a\x0a";
  protected const uint FILE_VERSION = 1;

  /**
   * @brief A bitmask representing no bits set in the Flags property.
   */
  public const ushort NONE_BIT = 0x00_00;

  /**
   * @brief A bitmask that indicates the player has cleared the tutorial level.
   */
  public const ushort TUTORIAL_COMPLETE_BIT = 0x00_01;

  /**
   * @brief The number of times the player has died.
   */
  public ushort Deaths { get; set; } = 0;

  /**
   * @brief The number of times the player has collided with damaging objects.
   */
  public ushort Collisions { get; set; } = 0;

  /**
   * @brief The player's progress (i.e., the index of the next level to play).
   */
  public ushort Progress { get; set; } = 0;

  /**
   * @brief A set of bit Flags representing general information about the player.
   */
  public ushort Flags { get; set; } = 0;

  /**
   * @brief The total time (in seconds) that the game has been in the Play state.
   */
  public ulong PlayTime { get; set; } = 0;

  /**
   * @brief Convert a byte array into a PlayerData object.
   *
   * @param buffer A buffer containing a valid PlayerData structure. This must be as-if it was returned by
   *               PlayerData.GetBytes()
   *
   * @return A PlayerData value equivalent to the data stored in the input buffer.
   *
   * @throw ArgumentException If the data in the input buffer is not as-if it was returned by PlayerData.GetBytes().
   */
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
    res.Flags = BitConverter.ToUInt16(buffer, 22);
    res.PlayTime = BitConverter.ToUInt64(buffer, 24);
    return res;
  }

  /**
   * @brief Checks whether or not a PlayerData value is in its initial state.
   *
   * This does not consider whether PlayerData.Flags has been set to 0.
   *
   * @return True if all properties are 0. False in all other cases.
   */
  public bool IsInitialized() {
    return Deaths == 0 && Collisions == 0 && Progress == 0 && PlayTime == 0;
  }

  /**
   * @brief Convert a PlayerData object into a byte array suitable for writing to storage.
   *
   * For more information on the structure of this array see docs/save_file_format_v1.md
   *
   * @return A byte array representing the PlayerData.
   */
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
    Buffer.BlockCopy(BitConverter.GetBytes(Flags), 0, res, 22, 2);
    Buffer.BlockCopy(BitConverter.GetBytes(PlayTime), 0, res, 24, 8);
    return res;
  }

  /**
   * @brief Convert a PlayerData object into a String.
   *
   * @return A String representation of the current PlayerData values.
   */
  public override string ToString() {
    return $"PlayerData{{ Deaths: {Deaths}, Collisions: {Collisions}, Progress: {Progress}, Flags: {Flags}, " +
           $"PlayTime: {PlayTime} }}";
  }
}
