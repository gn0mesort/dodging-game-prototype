/** Player entity.
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

public class Player : KinematicBody {
  /**
   * NodePath indicating the GridMap on which this entity exists.
   */
  [Export]
  public NodePath Map { get; set; }

  private GridMap _map = null;

  private Vector3 _direction = new Vector3();

  /**
   * Read a Controller and update entity state.
   *
   * @param controller A Controller structure containing the current input state.
   */
  public void IntegrateControls(Controller controller) {
    if (controller.IsSet(Controller.Control.LEFT))
    {
      _direction.x = -1;
    }
    else if (controller.IsSet(Controller.Control.RIGHT))
    {
      _direction.x = 1;
    }
    else
    {
      _direction.x = 0;
    }
    if (controller.IsSet(Controller.Control.UP))
    {
      _direction.y = 1;
    }
    else if (controller.IsSet(Controller.Control.DOWN))
    {
      _direction.y = -1;
    }
    else
    {
      _direction.y = 0;
    }
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _map = GetNode<GridMap>(Map);
  }

  /**
   * Per physics frame processing.
   */
  public override void _PhysicsProcess(float delta) {
    Translation = _map.MapToWorld((int) _direction.x, (int) _direction.y, 0);
  }
}
