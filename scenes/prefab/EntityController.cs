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
    else if (ev is InputEventJoypadButton evButton)
    {
      _controller.SetControlIf(evButton.IsPressed(),
                               Controller.ToControlIf(evButton.IsAction("move_up"), Controller.Control.UP));
      _controller.SetControlIf(evButton.IsPressed(),
                               Controller.ToControlIf(evButton.IsAction("move_left"), Controller.Control.LEFT));
      _controller.SetControlIf(evButton.IsPressed(),
                               Controller.ToControlIf(evButton.IsAction("move_down"), Controller.Control.DOWN));
      _controller.SetControlIf(evButton.IsPressed(),
                               Controller.ToControlIf(evButton.IsAction("move_right"), Controller.Control.RIGHT));
    }
    else if (ev is InputEventJoypadMotion evMotion)
    {
      // Maybe optimize this with a specific overload for _target.IntegrateControls.
      var velocity = Input.GetVector("move_left", "move_right", "move_up", "move_down");
      var x = Mathf.Clamp((int) Mathf.Round(velocity.x), -1, 1);
      var y = Mathf.Clamp((int) Mathf.Round(velocity.y), -1, 1);
      _controller.SetControlIf(x == -1, Controller.Control.LEFT);
      _controller.SetControlIf(x == 1, Controller.Control.RIGHT);
      _controller.SetControlIf(y == -1, Controller.Control.UP);
      _controller.SetControlIf(y == 1, Controller.Control.DOWN);
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
