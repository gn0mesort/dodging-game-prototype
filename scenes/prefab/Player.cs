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
using Godot;

public class Player : KinematicBody {
  /**
   * NodePath indicating the GridMap on which this entity exists.
   */
  [Export]
  public NodePath Map { get; set; }
  private GridMap _map = null;

  private CSGBox _mesh = null;

  private int _directionX= 0;
  private int _directionY = 0;
  private float _step = 0.0f;
  private float _albedoStep = 0.0f;
  private bool _isHit = false;
  private Vector3 _next = new Vector3();

  /**
   * Read a Controller and update entity state.
   *
   * @param controller A Controller structure containing the current input state.
   */
  public void IntegrateControls(Controller controller) {
    switch (controller.FirstPressed(Controller.Control.LEFT, Controller.Control.RIGHT))
    {
    case Controller.Control.LEFT:
      _directionX = -1;
      break;
    case Controller.Control.RIGHT:
       _directionX = 1;
      break;
    case Controller.Control.NONE:
      _directionX = 0;
      break;
    }
    switch (controller.FirstPressed(Controller.Control.UP, Controller.Control.DOWN))
    {
    case Controller.Control.UP:
      _directionY = 1;
      break;
    case Controller.Control.DOWN:
      _directionY = -1;
      break;
    case Controller.Control.NONE:
      _directionY = 0;
      break;
    }
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _map = GetNode<GridMap>(Map);
    _mesh = GetNode<CSGBox>("CSGBox");
  }


  private void Hit() {
    _albedoStep = 0.0f;
    _isHit = true;
  }

  /**
   * Per frame processing.
   */
  public override void _Process(float delta) {
    var material = _mesh.Material as SpatialMaterial;
    if (_isHit && Mathf.Abs(_albedoStep - 1.0f) > Mathf.Epsilon)
    {
      _albedoStep += 3f * delta;
      material.AlbedoColor = material.AlbedoColor.LinearInterpolate(new Color(1, 0, 0), _albedoStep);
    }
    else if (_isHit)
    {
      _isHit = false;
      return;
    }
    if (!_isHit && Mathf.Abs(_albedoStep) > Mathf.Epsilon)
    {
      _albedoStep -= 3f * delta;
      material.AlbedoColor = material.AlbedoColor.LinearInterpolate(new Color(1, 1, 1), 1.0f - _albedoStep);
    }
  }

  /**
   * Per physics frame processing.
   */
  public override void _PhysicsProcess(float delta) {
    if (_step < Mathf.Epsilon)
    {
      _next = _map.MapToWorld(_directionX, _directionY, 0);
    }
    var collision = MoveAndCollide(Translation, true, true, true);
    if (collision != null)
    {
      var obstacle = collision.Collider as Obstacle;
      Hit();
      obstacle.Kill();
    }
    _step += 4f * delta;
    Translation = Translation.LinearInterpolate(_next, _step);
    if (Mathf.Abs(_step - 1.0f) < Mathf.Epsilon)
    {
      _step = 0.0f;
    }
  }
}
