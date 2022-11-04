using Godot;

public class Field : Area {
  private void _OnBodyEntered(Node body) {
    if (body is IVelocityModifiable)
    {
      var target = body as IVelocityModifiable;
      target.ModifyVelocity(0.5f, 0.5f, 0.5f, 0.5f);
    }
  }

  private void _OnBodyExited(Node body) {
    if (body is IVelocityModifiable)
    {
      var target = body as IVelocityModifiable;
      target.ModifyVelocity(1f, 1f, 1f, 1f);
    }
  }

  public override void _EnterTree() {
    Connect("body_entered", this, "_OnBodyEntered");
    Connect("body_exited", this, "_OnBodyExited");
  }
}
