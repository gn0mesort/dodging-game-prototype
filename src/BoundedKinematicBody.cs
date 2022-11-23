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
 * @brief A KinematicBody that can only move within certain specific X and Y axis bounds.
 *
 * This is used by the Player and Cube entities.
 */
public abstract class BoundedKinematicBody : KinematicBody {
  protected Vector2 _bounds = new Vector2();
  protected Vector2 _maxPosition = new Vector2();
  protected Vector2 _minPosition = new Vector2();

  /**
   * @brief A Vector3 representing the body's speed when moving along each axis.
   */
  [Export]
  public Vector3 Speed { get; set; } = new Vector3(1f, 1f, 1f);

  /**
   * @brief A Vector2 representing the bounds within which the body can move along the X and Y axes.
   */
  [Export]
  public Vector2 MovementBounds {
    get { return _bounds; }
    set { _UpdateMovementBounds(_GetOrigin(), value); }
  }

  abstract protected void _SetOrigin(Vector3 origin);
  abstract protected Vector3 _GetOrigin();

  protected void _UpdateMovementBounds(Vector3 origin, Vector2 bounds) {
    var origin2d = new Vector2(origin.x, origin.y);
    _maxPosition = origin2d + bounds;
    _minPosition = origin2d - bounds;
    _bounds = bounds;
  }

  protected Vector3 _BoundTranslation(Vector3 translation) {
    translation.x = Mathf.Clamp(translation.x, _minPosition.x, _maxPosition.x);
    translation.y = Mathf.Clamp(translation.y, _minPosition.y, _maxPosition.y);
    return translation;
  }
}
