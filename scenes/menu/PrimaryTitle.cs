using Godot;

using MenuScenes = MenuRoot.MenuScenes;

public class PrimaryTitle : VBoxContainer {
  [Signal]
  public delegate void Transition(MenuScenes to);

  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  private void _OnAnyPress() {
    EmitSignal("Transition", MenuScenes.Secondary);
  }

  public override void _Ready() {
    GetNode<Label>("PressAnyKey").Connect("Pressed", this, "_OnAnyPress");
  }
}
