using System;

public static class Utility {
  public static uint Clamp(uint x, uint min, uint max) {
    return Math.Min(Math.Max(x, min), max);
  }
}
