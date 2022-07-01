using Godot;
using System;

public class Player : KinematicBody {
  [Export]
  public NodePath Map { get; set; }

  private GridMap _map = null;

  private Vector3 _direction = new Vector3();

  public void IntegrateControls(Controller controller) {
    if (controller.IsSet(Controller.Control.LEFT))
    {
      _direction.x = -1;
    }
    else if (controller.IsSet(Controller.Control.RIGHT))
    {
      _direction.x = 1;
    }
    else
    {
      _direction.x = 0;
    }
    if (controller.IsSet(Controller.Control.UP))
    {
      _direction.y = 1;
    }
    else if (controller.IsSet(Controller.Control.DOWN))
    {
      _direction.y = -1;
    }
    else
    {
      _direction.y = 0;
    }
  }

  public override void _Ready() {
    _map = GetNode<GridMap>(Map);
  }

  public override void _PhysicsProcess(float delta) {
    Translation = _map.MapToWorld((int) _direction.x, (int) _direction.y, 0);
  }
}
