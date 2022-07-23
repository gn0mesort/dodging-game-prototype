/** Target following camera.
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

public class FollowCamera : Camera {
  private Spatial _target = null;

  /**
   * The NodePath pointing to the followed entity.
   * Must be a Spatial.
   */
  [Export]
  public NodePath Target { get; set; } = "";

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    SetAsToplevel(true);
    _target = Target != "" ? GetNode<Spatial>(Target) : GetParent<Spatial>();
  }

  /**
   * Per physics frame processing.
   */
  public override void _PhysicsProcess(float delta) {
    var target = _target.Translation * new Vector3(0f, 0f, 1f);
    LookAtFromPosition(target - new Vector3(0f, 0f, -12f), target, new Vector3(0f, 1f, 0f));
  }
}
