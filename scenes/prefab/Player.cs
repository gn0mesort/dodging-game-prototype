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
using System;
using System.Diagnostics;

using Godot;

public class Player : KinematicBody {
  private enum Controls {
    None = -1,
    Left = 0,
    Up,
    Right,
    Down,
    Rotate,
    Max
  }

  [Export]
  public Vector3 BaseSpeed { get; set; } = new Vector3(12f, 12f, 12f);

  /**
   * NodePath indicating the GridMap on which this entity exists.
   */
  [Export]
  public NodePath Map { get; set; }
  private GridMap _map = null;

  private AnimationPlayer _animations = null;

  private Controller _controls = new Controller((int) Controls.Max);

  private void _SetControl(Controls control, bool state) {
    var idx = (int) control;
    _controls.SetControlIf(_controls.IsPressed(idx) != state, idx, state);
  }

  private void _UpdateControls() {
    _SetControl(Controls.Left, Input.IsActionPressed("move_left"));
    _SetControl(Controls.Up, Input.IsActionPressed("move_up"));
    _SetControl(Controls.Right, Input.IsActionPressed("move_right"));
    _SetControl(Controls.Down, Input.IsActionPressed("move_down"));
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _map = GetNode<GridMap>(Map);
    _animations = GetNode<AnimationPlayer>("Animations");
  }

  /**
   * Per frame processing.
   */
  public override void _Process(float delta) {
  }

  private static bool _IsEqualApprox(Vector3 a, Vector3 b, float tolerance) {
    return Mathf.IsEqualApprox(a.x, b.x, tolerance) && Mathf.IsEqualApprox(a.y, b.y, tolerance) &&
           Mathf.IsEqualApprox(a.z, b.z, tolerance);
  }

  /**
   * Per physics frame processing.
   */
  public override void _PhysicsProcess(float delta) {
    _UpdateControls();
    var dirX = 0;
    switch ((Controls) _controls.FirstPressed((int) Controls.Left, (int) Controls.Right))
    {
    case Controls.Left:
      dirX = -1;
      break;
    case Controls.Right:
      dirX = 1;
      break;
    }
    var dirY = 0;
    switch ((Controls) _controls.FirstPressed((int) Controls.Down, (int) Controls.Up))
    {
    case Controls.Down:
      dirY = -1;
      break;
    case Controls.Up:
      dirY = 1;
      break;
    }
    var current = _map.WorldToMap(Translation);
    var next = _map.MapToWorld(dirX, dirY, ((int) current.z) - 1);
    var velocity = (next - Translation).Normalized();
    velocity *= BaseSpeed;
    if (_IsEqualApprox(next, Translation, 0.2f))
    {
      velocity = new Vector3(0f, 0f, velocity.z);
      Translation = next;
    }
    var collision = MoveAndCollide(velocity * delta);
    if (collision != null)
    {
      (collision.Collider as Node).QueueFree();
      if (_animations.IsPlaying())
      {
        _animations.Stop();
      }
      _animations.Play("CollideAndTakeDamage");
    }
  }
}
