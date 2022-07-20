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

public class Obstacle : StaticBody {

  [Export]
  public NodePath Target { get; set; } = "";
  private Player _target = null;

  private AnimationPlayer _animations = null;

  public override void _Ready() {
    if (Target == "")
    {
      return;
    }
    _target = GetNode<Player>(Target);
    _animations = GetNode<AnimationPlayer>("Animations");
    _animations.Play("Idle");
  }

  /**
   * Per physics frame processing.
   */
  public override void _PhysicsProcess(float delta) {
    var distance = (Translation - _target.Translation).z;
    if (distance > Mathf.Abs(5 * _target.BaseSpeed.z))
    {
      QueueFree();
    }
  }
}
