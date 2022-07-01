/** Targetable Entity controller.
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

public class EntityController : Node {
  private readonly Controller _controller = new Controller();

  private Player _target = null;

  /**
   * A NodePath indicating the entity to control.
   */
  [Export]
  public NodePath Target { get; set; } = "";

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _target = GetNode<Player>(Target);
  }

  /**
   * Input processing.
   *
   * @param ev An input event that triggered processing.
   */
  public override void _Input(InputEvent ev) {
    if (ev.IsEcho())
    {
      return;
    }
    if (ev is InputEventKey evKey)
    {
      _controller.SetControlIf(evKey.IsPressed(),
                               Controller.ToControlIf(evKey.IsAction("move_up"), Controller.Control.UP));
      _controller.SetControlIf(evKey.IsPressed(),
                               Controller.ToControlIf(evKey.IsAction("move_left"), Controller.Control.LEFT));
      _controller.SetControlIf(evKey.IsPressed(),
                               Controller.ToControlIf(evKey.IsAction("move_down"), Controller.Control.DOWN));
      _controller.SetControlIf(evKey.IsPressed(),
                               Controller.ToControlIf(evKey.IsAction("move_right"), Controller.Control.RIGHT));
    }
    _target.IntegrateControls(_controller);
  }

}
