/** Branch-free control processing structure.
 *
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
using Godot;
using System;

public class Controller : Reference {
  /**
   * Controls understood by the Controller.
   */
  public enum Control : int {
    // Not an actual control.
    NONE = 0,
    UP,
    LEFT,
    DOWN,
    RIGHT,
    // Not an actual control.
    MAX
  }

  private readonly ulong[] _controls = new ulong[(int) Control.MAX];

  /**
   * Return the given Control if condition is true.
   *
   * @param condition The condition upon which to return the control.
   * @param control The Control to return.
   * @return control if condition was true. Otherwise Control.NONE.
   */
  public static Control ToControlIf(bool condition, Control control) {
    return (Control) ((~Convert.ToInt32(condition) + 1) & (int) control);
  }

  /**
   * Set the given Control unconditionally.
   *
   * @param control The Control to be set.
   */
  public void SetControl(Control control) {
    _controls[(int) control] = OS.GetTicksMsec();
  }

  /**
   * Set the given Control conditionally.
   *
   * @param condition The condition upon which to set the Control.
   * @param control The Control to set.
   */
  public void SetControlIf(bool condition, Control control) {
    // Explicit two's complement to make C# happy.
    // Bit fiddling is too "error prone" apparently.
    _controls[(int) control] = (~Convert.ToUInt64(condition) + 1) & OS.GetTicksMsec();
  }

  /**
   * Clear the given Control unconditionally.
   *
   * @param control The Control to clear.
   */
  public void ClearControl(Control control) {
    _controls[(int) control] = 0;
  }

  /**
   * Clear the given Control conditionally.
   *
   * @param condition The condition upon which to clear the Control.
   * @param control The Control to clear.
   */
  public void ClearControlIf(bool condition, Control control) {
    // Same explicit two's complement as above.
    _controls[(int) control] = (~Convert.ToUInt64(!condition) + 1) & _controls[(int) control];
  }


  /**
   * Get the monotonic time (in milliseconds) at which the given Control was set.
   *
   * Time 0 is program start up.
   *
   * @param control The Control to retrieve the timestamp of.
   *
   * @return The time in milliseconds when control was first set or 0 if control is clear.
   */
  public ulong GetControlTimestamp(Control control) {
    return _controls[(int) control];
  }


  /**
   * Check if a Control is set.
   *
   * @param control The Control to check.
   *
   * @return true if the Control is set. Otherwise false.
   */
  public bool IsSet(Control control) {
    return _controls[(int) control] != 0;
  }

  /**
   * Check if a Control is clear.
   *
   * @param control The Control to check.
   *
   * @return true if the Control is clear. Otherwise false.
   */
  public bool IsClear(Control control) {
    return _controls[(int) control] == 0;
  }

  /**
   * Get the string representation of a Controller.
   *
   * @return a string displaying the current Control timestamps.
   */
  public override string ToString() {
    var res = "[ ";
    for (int i = (int) Control.NONE + 1; i < _controls.Length; ++i)
    {
      res += $"{_controls[i]}, ";
    }
    return res + "]";
  }
}
