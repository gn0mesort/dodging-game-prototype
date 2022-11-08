///** Interface for non-player game entities.
// *
// * Copyright (C) 2022 Alexander Rothman <gnomesort@megate.ch>
// *
// * This program is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Affero General Public License as published
// * by the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * This program is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Affero General Public License for more details.
// *
// * You should have received a copy of the GNU Affero General Public License
// * along with this program.  If not, see <https://www.gnu.org/licenses/>.
// */
//using Godot;
//
//public interface IEntity {
//  /**
//   * Set the entity mode.
//   *
//   * @param mode The mode to set. Valid modes depend on the implementing entity.
//   */
//  void SetMode(Level.LevelEntity.EntityMode mode);
//
//  /**
//   * Set the translation direction of the entity.
//   *
//   * @param x The x-axis direction of translation.
//   * @param y The y-axis direction of translation.
//   */
//  void SetDirection(Level.LevelEntity.Direction x, Level.LevelEntity.Direction y);
//
//  /**
//   * Set the scaling axes of the entity.
//   *
//   * @param x Whether or not to scale along the x-axis.
//   * @param y Whether or not to scale along the y-axis.
//   */
//  void SetScaling(bool x, bool y);
//
//  /**
//   * Update the movement extents of the entity.
//   *
//   * Translating entities are bound to the plane from (-extents.x, -extents.y) to (extents.x, extents.y).
//   *
//   * @param extents The extent of movement allowed for the entity. extents.z is discarded.
//   */
//  void UpdateMovementExtents(Vector3 extents);
//}
