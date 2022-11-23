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
using Godot;

/**
 * @brief A Control that emits a signal when any key, mouse button, or gamepad button is pressed.
 */
public class PressAnyKey : Label {
  /**
   * @brief Whether or not the text of the control should blink.
   */
  [Export]
  public bool Blinking { get; set; } = true;

  /**
   * @brief A Signal emitted when any key or button is pressed.
   */
  [Signal]
  public delegate void Pressed();

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    var animations = GetNode<AnimationPlayer>("Animations");
    if (Blinking)
    {
      animations.Play("Blink");
    }
  }

  /**
   * @brief Handling for otherwise unhandled InputEvents.
   *
   * @param ev The event that triggered handling.
   */
  public override void _UnhandledInput(InputEvent ev) {
    if ((ev is InputEventKey || ev is InputEventMouseButton || ev is InputEventJoypadButton) && ev.IsPressed()
        && !ev.IsEcho())
    {
      GetTree().SetInputAsHandled();
      EmitSignal("Pressed");
    }
  }
}
