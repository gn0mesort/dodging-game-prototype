///** Field game entity.
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
//public class Field : Area, IEntity {
//  /**
//   * Game entity movement mode.
//   */
//  [Export]
//  public Level.LevelEntity.EntityMode Mode { get; set; } = Level.LevelEntity.EntityMode.Stationary;
//
//  /**
//   * Game entity maximum scale.
//   *
//   * The minimum scale is (1f, 1f, 1f).
//   */
//  [Export]
//  public Vector3 MaxScale { get; set; } = new Vector3(2f, 2f, 1f);
//
//  private bool _scaleX = false;
//  private bool _scaleY = false;
//
//  private Tween _tween = null;
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
//   * Ignored for fields.
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
//   * @param x Whether or not to scale along the x-axis.
//   * @param y Whether or not to scale along the y-axis.
//   */
//  public void SetScaling(bool x, bool y) {
//    _scaleX = x;
//    _scaleY = y;
//  }
//
//  /**
//   * Update the movement extents of the entity.
//   *
//   * Translating entities are bound to the plane from (-extents.x, -extents.y) to (extents.x, extents.y).
//   *
//   * Ignored for fields.
//   *
//   * @param extents The extent of movement allowed for the entity. extents.z is discarded.
//   */
//  public void UpdateMovementExtents(Vector3 extents) {
//    return;
//  }
//
//  private void _OnBodyEntered(Node body) {
//    var player = body as Player;
//    if (player != null)
//    {
//      player.SpeedMultiplier = 0.5f;
//    }
//  }
//
//  private void _OnBodyExited(Node body) {
//    var player = body as Player;
//    if (player != null)
//    {
//      player.SpeedMultiplier = 1f;
//    }
//  }
//
//  /**
//   * Post-_EnterTree initialization.
//   */
//  public override void _Ready() {
//    _tween = GetNode<Tween>("Tween");
//    Connect("body_entered", this, "_OnBodyEntered");
//    Connect("body_exited", this, "_OnBodyExited");
//    if (Mode == Level.LevelEntity.EntityMode.Translating)
//    {
//      Mode = Level.LevelEntity.EntityMode.Stationary;
//    }
//  }
//
//    private void _ProcessScaling(float delta) {
//    if (_tween.IsActive())
//    {
//      return;
//    }
//    var nextScale = new Vector3(1f, 1f, 1f);
//    if (_scaleX && !Mathf.IsEqualApprox(Scale.x, MaxScale.x))
//    {
//      nextScale.x = MaxScale.y;
//    }
//    if (_scaleY && !Mathf.IsEqualApprox(Scale.y, MaxScale.y))
//    {
//      nextScale.y = MaxScale.y;
//    }
//    _tween.InterpolateProperty(this, "scale", Scale, nextScale, 3f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
//    _tween.Start();
//  }
//
//  /**
//   * Per-frame physics processing.
//   */
//  public override void _PhysicsProcess(float delta) {
//    if (Mode == Level.LevelEntity.EntityMode.Scaling)
//    {
//      _ProcessScaling(delta);
//    }
//  }
//}
