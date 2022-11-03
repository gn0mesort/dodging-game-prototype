using Godot;

using MenuScenes = MenuRoot.MenuScenes;

public class SecondaryTitle : VBoxContainer {
  [Signal]
  public delegate void Transition(MenuScenes to);

  [Signal]
  public delegate void TransitionRoot(RootScenes to);


  private void _OnExitPressed() {
    EmitSignal("TransitionRoot", RootScenes.Exit);
  }

  private void _OnStartGamePressed() {
    EmitSignal("TransitionRoot", RootScenes.Play);
  }

  public override void _Ready() {
    GetNode("StartGame").Connect("pressed", this, "_OnStartGamePressed");
    GetNode("Exit").Connect("pressed", this, "_OnExitPressed");
  }

}
