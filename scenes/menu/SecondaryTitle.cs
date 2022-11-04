using Godot;

using MenuScenes = MenuRoot.MenuScenes;

public class SecondaryTitle : VBoxContainer {
  [Signal]
  public delegate void Transition(MenuScenes to);

  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  private GameMain _main = null;

  private void _OnExitPressed() {
    EmitSignal("TransitionRoot", RootScenes.Exit);
  }

  private void _OnPlayPressed() {
    EmitSignal("TransitionRoot", RootScenes.Play);
  }

  public override void _EnterTree() {
    _main = GetNode<GameMain>("/root/Main");
  }

  public override void _Ready() {
    var playButton = GetNode<Button>("Play");
    var rebirthButton = GetNode<Button>("Rebirth");
    if (_main.Player.Progress == 0)
    {
      playButton.Text = "New Game";
      rebirthButton.Disabled = true;
    }
    else
    {
      playButton.Text = "Continue";
    }
    playButton.Connect("pressed", this, "_OnPlayPressed");
    GetNode("Exit").Connect("pressed", this, "_OnExitPressed");
  }

}
