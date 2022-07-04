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

  private int _directionX= 0;
  private int _directionY = 0;

  /**
   * Read a Controller and update entity state.
   *
   * @param controller A Controller structure containing the current input state.
   */
  public void IntegrateControls(Controller controller) {
    switch (controller.FirstPressed(Controller.Control.LEFT, Controller.Control.RIGHT))
    {
    case Controller.Control.LEFT:
      _directionX = -1;
      break;
    case Controller.Control.RIGHT:
       _directionX = 1;
      break;
    case Controller.Control.NONE:
      _directionX = 0;
      break;
    }
    switch (controller.FirstPressed(Controller.Control.UP, Controller.Control.DOWN))
    {
    case Controller.Control.UP:
      _directionY = 1;
      break;
    case Controller.Control.DOWN:
      _directionY = -1;
      break;
    case Controller.Control.NONE:
      _directionY = 0;
      break;
    }
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _map = GetNode<GridMap>(Map);
  }

  private float interpT = 0.0f;
  private Vector3 next = new Vector3();

  /**
   * Per physics frame processing.
   */
  public override void _PhysicsProcess(float delta) {
    if (interpT < Mathf.Epsilon)
    {
      next = _map.MapToWorld(_directionX, _directionY, 0);
    }
    interpT += 6f * delta;
    Translation = Translation.LinearInterpolate(next, interpT);
    if (interpT - 1.0f < Mathf.Epsilon)
    {
      interpT = 0.0f;
    }
  }
}
