using System;

/**
 * @brief Static utility method class.
 */
public static class Utility {
  /**
   * @brief Perform an integer clamp operation.
   *
   * @param x The input value.
   * @param min The minimum value.
   * @param max The maximum value.
   *
   * @return x if x is less than max and greater than min. min if x is less than min. max if x is greater than max.
   */
  public static uint Clamp(uint x, uint min, uint max) {
    return Math.Min(Math.Max(x, min), max);
  }

  /**
   * @brief Perform an integer clamp operation.
   *
   * @param x The input value.
   * @param min The minimum value.
   * @param max The maximum value.
   *
   * @return x if x is less than max and greater than min. min if x is less than min. max if x is greater than max.
   */
  public static int Clamp(int x, int min, int max) {
    return Math.Min(Math.Max(x, min), max);
  }

  /**
   * @brief Perform an integer clamp operation.
   *
   * @param x The input value.
   * @param min The minimum value.
   * @param max The maximum value.
   *
   * @return x if x is less than max and greater than min. min if x is less than min. max if x is greater than max.
   */
  public static ushort Clamp(ushort x, ushort min, ushort max) {
    return Math.Min(Math.Max(x, min), max);
  }
}
