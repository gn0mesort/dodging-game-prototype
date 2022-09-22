/** Game over node.
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
using System.Diagnostics;
using Godot;

public class GameOver : Control, IDependsOnMain {
  private Main _main = null;

  /**
   * Set the main node.
   *
   * @param main The top level node of the scene.
   */
  public void SetMainNode(Main main) {
    _main = main;
    Debug.Assert(_main != null);
  }

  /**
   * Get the main node.
   *
   * @return The main node.
   */
  public Main GetMainNode() {
    return _main;
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    Debug.Assert(_main != null);
  }

  /**
   * Node-specific input handling.
   *
   * @param ev The input event to handle.
   */
  public override void _Input(InputEvent ev) {
    if (ev.IsActionPressed("ui_accept") || ev.IsActionPressed("ui_cancel"))
    {
      _main.Scenes.LoadScene("res://scenes/TitleMenu.tscn");
    }
  }
}
