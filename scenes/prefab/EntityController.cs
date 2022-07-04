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
    if (ev is InputEventKey || ev is InputEventJoypadButton)
    {
      _controller.SetControlIf(ev.IsAction("move_up"), Controller.Control.UP, ev.IsPressed());
      _controller.SetControlIf(ev.IsAction("move_left"), Controller.Control.LEFT, ev.IsPressed());
      _controller.SetControlIf(ev.IsAction("move_down"), Controller.Control.DOWN, ev.IsPressed());
      _controller.SetControlIf(ev.IsAction("move_right"), Controller.Control.RIGHT, ev.IsPressed());
    }
    else if (ev is InputEventJoypadMotion)
    {
      var velocity = Input.GetVector("move_left", "move_right", "move_up", "move_down");
      var x = Mathf.Clamp((int) Mathf.Round(velocity.x), -1, 1);
      var y = Mathf.Clamp((int) Mathf.Round(velocity.y), -1, 1);
      _controller.SetControlIf(true, Controller.Control.LEFT, x == -1);
      _controller.SetControlIf(true, Controller.Control.RIGHT, x == 1);
      _controller.SetControlIf(true, Controller.Control.UP, y == -1);
      _controller.SetControlIf(true, Controller.Control.DOWN, y == 1);
    }
    _target.IntegrateControls(_controller);
  }

  private void ClearAllControls() {
    for (int i = 1; i < (int) Controller.Control.MAX; ++i)
    {
      _controller.ClearControl((Controller.Control) i);
    }
  }

  public override void _Notification(int what) {
    switch (what)
    {
    case NotificationUnpaused:
      ClearAllControls();
      break;
    }
  }

}
