///** Letterbox game entity.
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
//using System;
//
//public class Letterbox : KinematicBody, IEntity {
//  private AnimationPlayer _animations = null;
//  private bool _isClosed = false;
//
//  /**
//   * Game entity movement mode.
//   */
//  [Export]
//  public Level.LevelEntity.EntityMode Mode { get; set; } = Level.LevelEntity.EntityMode.Stationary;
//
//  /**
//   * Set the entity mode.
//   *
//   * @param mode The mode to set. Valid modes depend on the implementing entity.
//   */
//  public void SetMode(Level.LevelEntity.EntityMode mode) {
//    Mode = mode;
//  }
//
//  /**
//   * Set the translation direction of the entity.
//   *
//   * Ignored for letterboxes.
//   *
//   * @param x The x-axis direction of translation.
//   * @param y The y-axis direction of translation.
//   */
//  public void SetDirection(Level.LevelEntity.Direction x, Level.LevelEntity.Direction y) {
//    return;
//  }
//
//  /**
//   * Set the scaling axes of the entity.
//   *
//   * Ignored for letterboxes. Scaling causes an open/close effect instead.
//   *
//   * @param x Whether or not to scale along the x-axis.
//   * @param y Whether or not to scale along the y-axis.
//   */
//  public void SetScaling(bool x, bool y) {
//    return;
//  }
//
//  /**
//   * Update the movement extents of the entity.
//   *
//   * Translating entities are bound to the plane from (-extents.x, -extents.y) to (extents.x, extents.y).
//   *
//   * Ignored for letterboxes.
//   *
//   * @param extents The extent of movement allowed for the entity. extents.z is discarded.
//   */
//  public void UpdateMovementExtents(Vector3 extents) {
//    return;
//  }
//
//  private void _OnAnimationFinished(string animation) {
//    _isClosed = !_isClosed;
//    if (_isClosed)
//    {
//      _animations.PlayBackwards(animation);
//    }
//    else
//    {
//      _animations.Play(animation);
//    }
//  }
//
//  /**
//   * Post-_EnterTree initialization.
//   */
//  public override void _Ready() {
//    _animations = GetNode<AnimationPlayer>("Animations");
//    if (Mode == Level.LevelEntity.EntityMode.Translating)
//    {
//      Mode = Level.LevelEntity.EntityMode.Stationary;
//    }
//    _animations.Connect("animation_finished", this, "_OnAnimationFinished");
//    _animations.Play("Close");
//  }
//
//}
