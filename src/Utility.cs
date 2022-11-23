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
