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
 * @brief Behavior script for Cube entities.
 */
public class Cube : BoundedKinematicBody {
  /**
   * @brief An enumeration representing the axes on which Cubes can move.
   */
  public enum Axis {
    None = 0,
    X,
    Y
  }

  /**
   * @brief The current axis on which the Cube will move or None if the Cube is stationary.
   */
  [Export]
  public Axis MovementAxis { get; set; } = Axis.None;

  private Vector3 _origin = new Vector3();
  private float _direction = 1f;

  protected override void _SetOrigin(Vector3 origin) {
    _origin = origin;
  }

  protected override Vector3 _GetOrigin() {
    return _origin;
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _SetOrigin(Translation);
    _UpdateMovementBounds(_GetOrigin(), MovementBounds);
  }


  /**
   * @brief Per-frame physics processing.
   *
   * @param delta The amount of time that has passed since the previous physics frame.
   */
  public override void _PhysicsProcess(float delta) {
    var velocity = new Vector3();
    switch (MovementAxis)
    {
    case Axis.X:
      if (Mathf.IsEqualApprox(Translation.x, _minPosition.x) || Mathf.IsEqualApprox(Translation.x, _maxPosition.x))
      {
        _direction *= -1f;
      }
      velocity = _direction * new Vector3(1f, 0f, 0f) * Speed;
      break;
    case Axis.Y:
      if (Mathf.IsEqualApprox(Translation.y, _minPosition.y) || Mathf.IsEqualApprox(Translation.y, _maxPosition.y))
      {
        _direction *= -1f;
      }
      velocity = _direction * new Vector3(0f, 1f, 0f) * Speed;
      break;
    }
    MoveAndCollide(velocity * delta);
    Translation = _BoundTranslation(Translation);
  }
}
