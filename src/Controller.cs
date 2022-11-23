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
using System.Diagnostics;
using Godot;
/**
 * @brief Branch-free control processing structure.
 */
public class Controller {
  /**
   * @brief Constant indicating no control is pressed.
   */
  public const int NONE = -1;

  private readonly ulong[] _controls;

  /**
   * @brief Constructs a new controller with controlCount controls.
   *
   * @param controlCount The number of controls to allocate.
   */
  public Controller(int controlCount) {
    Debug.Assert(controlCount > 0);
    // Additonal space is allocated for the null/none/empty control.
    _controls = new ulong[controlCount + 1];
  }

  /**
   * @brief Set the given control to the desired state unconditionally.
   *
   * @param control The control to be set.
   * @param state The state (on or off) to set the control to.
   */
  public void SetControl(int control, bool state) {
    Debug.Assert(control > NONE);
    // Explicit two's complement to make C# happy.
    // Bit fiddling is too "error prone" apparently.
    _controls[control + 1] = (~Convert.ToUInt64(state) + 1) & OS.GetTicksUsec();
  }

  /**
   * @brief Set the given control conditionally.
   *
   * @param condition The condition upon which to set the control.
   * @param control The control to set.
   * @param state The state (on or off) to set the control to.
   */
  public void SetControlIf(bool condition, int control, bool state) {
    Debug.Assert(control > NONE);
    _controls[(~Convert.ToInt32(condition) + 1) & (control + 1)] = (~Convert.ToUInt64(state) + 1) & OS.GetTicksUsec();
  }

  /**
   * @brief Release the given control unconditionally.
   *
   * @param control The control to release.
   */
  public void ReleaseControl(int control) {
    Debug.Assert(control > NONE);
    _controls[control] = 0;
  }

  /**
   * @brief Release the given control conditionally.
   *
   * @param condition The condition upon which to release the control.
   * @param control The control to release.
   */
  public void ReleaseControlIf(bool condition, int control) {
    Debug.Assert(control > NONE);
    _controls[((~Convert.ToInt32(!condition) + 1) & (control + 1))] =  0;
  }

  /**
   * @brief Get the monotonic time (in microseconds) at which the given control was pressed.
   *
   * Time 0 is program start up.
   *
   * @param control The control to retrieve the timestamp of.
   *
   * @return The time in microseconds when control was first pressed or 0 if control is released.
   */
  public ulong GetControlTimestamp(int control) {
    Debug.Assert(control > NONE);
    return _controls[control + 1];
  }

  /**
   * @brief Check if a control is pressed.
   *
   * @param control The control to check.
   *
   * @return true if the control is pressed. Otherwise false.
   */
  public bool IsPressed(int control) {
    Debug.Assert(control > NONE);
    return _controls[control + 1] != 0;
  }

  /**
   * @brief Check if a control is released.
   *
   * @param control The control to check.
   *
   * @return true if the control is released. Otherwise false.
   */
  public bool IsReleased(int control) {
    Debug.Assert(control > NONE);
    return _controls[control + 1] == 0;
  }

  /**
   * @brief Return which of two controls was pressed first.
   *
   * If one control is pressed and the other is released, the pressed control will be returned.
   *
   * @param a The first control to check.
   * @param b The second control to check.
   *
   * @return a if a was pressed before b. b if b was pressed before a. Otherwise Controller.NONE.
   */
  public int FirstPressed(int a, int b) {
    Debug.Assert(a > NONE && b > NONE);
    var timeA = GetControlTimestamp(a);
    var timeB = GetControlTimestamp(b);
    var cmp = (long) timeA - (long) timeB;
    if (timeA != 0 && timeB == 0)
    {
      return a;
    }
    if (timeA == 0 && timeB != 0)
    {
      return b;
    }
    if (cmp < 0)
    {
      return a;
    }
    if (cmp > 0 && timeB != 0)
    {
      return b;
    }
    return NONE;
  }

  /**
   * @brief Get the string representation of a Controller.
   *
   * @return a string displaying the current Control timestamps.
   */
  public override string ToString() {
    var res = "[ ";
    for (int i = 1; i < _controls.Length; ++i)
    {
      res += $"{_controls[i]}, ";
    }
    return res + "]";
  }
}
