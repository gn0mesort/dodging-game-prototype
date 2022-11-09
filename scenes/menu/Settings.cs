using Godot;

using MenuScenes = MenuRoot.MenuScenes;

public class Settings : GridContainer {
  [Signal]
  public delegate void Transition(MenuScenes to);

  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  private void _OnBackPressed() {
    EmitSignal("Transition", MenuScenes.Secondary);
  }

  public override void _Ready() {
    GetNode<Button>("Back").Connect("pressed", this, "_OnBackPressed");
  }
}
