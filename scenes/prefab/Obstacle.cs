/** Obstacle entity.
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

public class Obstacle : KinematicBody {
  [Export]
  public float AccelerationFactor { get; set; } = 500.0f;

  /**
   * NodePath indicating the GridMap on which this entity exists.
   */
  [Export]
  public NodePath Map { get; set; } = "";
  private GridMap _map = null;

  private float _step = 1.0f;
  private Vector3 _next = new Vector3();
  private bool _killed = false;

  /**
   * Set the exported GridMap value.
   *
   * This is useful when updating Obstacles programmatically.
   */
  public void SetMap(GridMap map) {
    _map = map;
  }

  /**
   * Start "kill" processing for the Obstacle.
   */
  public void Kill() {
    var pos = _map.WorldToMap(Translation) * 5;
    _next = _map.MapToWorld((int) pos.x, (int) pos.y, (int) pos.z);
    _step = 0.0f;
    _killed = true;
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    if (Map != "")
    {
      SetMap(GetNode<GridMap>(Map));
    }
  }

  /**
   * Per frame processing.
   */
  public override void _Process(float delta) {
    if (_killed && Mathf.Abs(_step - 1.0f) < Mathf.Epsilon)
    {
      QueueFree();
    }
  }

  /**
   * Per physics frame processing.
   */
  public override void _PhysicsProcess(float delta) {
    if (Mathf.Abs(_step - 1.0f) > Mathf.Epsilon)
    {
      _step += delta;
      Translation = Translation.LinearInterpolate(_next, _step);
    }
  }
}
