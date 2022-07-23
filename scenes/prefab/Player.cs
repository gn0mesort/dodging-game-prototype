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
    Max
  }

  [Signal]
  public delegate void StatusChanged();

  [Export]
  public Vector3 BaseSpeed { get; set; } = new Vector3(36f, 36f, -36f);

  [Export]
  public Vector3 MovementExtents { get; set; } = new Vector3(12f, 6f, 0f);
  private Vector3 _LEFT = new Vector3(-1f, 0f, 0f);
  private Vector3 _UP = new Vector3(0f, 1f, 0f);
  private Vector3 _RIGHT = new Vector3(1f, 0f, 0f);
  private Vector3 _DOWN = new Vector3(0f, -1f, 0f);

  [Export]
  public uint Score {
    get {
      return _score;
    }
    set {
      EmitSignal("StatusChanged");
      _score = (uint) Mathf.Clamp((long) value, 0, UInt32.MaxValue);
    }
  }
  private uint _score = 0;

  [Export]
  public uint Health {
    get {
      return _health;
    }
    set {
      EmitSignal("StatusChanged");
      _health = (uint) Mathf.Clamp((long) value, 0, UInt32.MaxValue);
    }
  }
  private uint _health = 3;

  private AnimationPlayer _animations = null;
  private Tween _tweens = null;
  private CSGBox _mesh = null;

  private Controller _controls = new Controller((int) Controls.Max);

  private void _SetControl(Controls control, bool state) {
    var idx = (int) control;
    _controls.SetControlIf(_controls.IsPressed(idx) != state, idx, state);
  }

  private static readonly Vector3 _VerticalOrientation = new Vector3(0f, 0f, 90f);
  private static readonly Vector3 _HorizontalOrientation = new Vector3(0, 0, 0);
  private Vector3 _orientation = _HorizontalOrientation;
  private bool _rotating = false;

  private void _UpdateControls() {
    _SetControl(Controls.Left, Input.IsActionPressed("move_left"));
    _SetControl(Controls.Up, Input.IsActionPressed("move_up"));
    _SetControl(Controls.Right, Input.IsActionPressed("move_right"));
    _SetControl(Controls.Down, Input.IsActionPressed("move_down"));
  }

  private Vector3 _ChooseDirection() {
    var res = new Vector3(0f, 0f, 0f);
    switch ((Controls) _controls.FirstPressed((int) Controls.Left, (int) Controls.Right))
    {
    case Controls.Left:
      res += _LEFT;
      break;
    case Controls.Right:
      res += _RIGHT;
      break;
    }
    switch ((Controls) _controls.FirstPressed((int) Controls.Down, (int) Controls.Up))
    {
    case Controls.Down:
      res += _DOWN;
      break;
    case Controls.Up:
      res += _UP;
      break;
    }
    return res;
  }

  private void _OnTweenCompleted(object obj, NodePath key) {
    if (key == ":rotation_degrees")
    {
      RotationDegrees = _orientation;
      _rotating = false;
    }
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
     _LEFT *= MovementExtents;
       _UP *= MovementExtents;
    _RIGHT *= MovementExtents;
     _DOWN *= MovementExtents;
    _animations = GetNode<AnimationPlayer>("Animations");
    _tweens = GetNode<Tween>("Tweens");
    _mesh = GetNode<CSGBox>("CSGBox");

    _tweens.Connect("tween_completed", this, "_OnTweenCompleted");
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
    var next = _ChooseDirection();
    if (!_rotating && Input.IsActionJustPressed("move_rotate"))
    {
      _orientation = _IsEqualApprox(RotationDegrees, _HorizontalOrientation, 0.2f) ? _VerticalOrientation : _HorizontalOrientation;
      _tweens.InterpolateProperty(this, "rotation_degrees", RotationDegrees, _orientation, 0.25f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
      _tweens.Start();
      _rotating = true;
    }
    var transNoZ = Translation * new Vector3(1f, 1f, 0f);
    var velocity = (next - transNoZ).Normalized();
    velocity.z = 1f;
    velocity *= BaseSpeed;
    if (_IsEqualApprox(next, transNoZ, 0.3f))
    {
      velocity = new Vector3(0f, 0f, velocity.z);
      Translation = next + (Translation * new Vector3(0f, 0f, 1f));
    }
    var collision = MoveAndCollide(velocity * delta);
    if (collision != null)
    {
      if (--Health == 0)
      {
        // TODO game over
      }
      (collision.Collider as Node).QueueFree();
      if (_animations.IsPlaying())
      {
        _animations.Stop();
      }
      _animations.Play("CollideAndTakeDamage");
    }
  }
}
