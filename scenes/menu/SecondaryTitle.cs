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

  private void _OnRebirthPressed() {
    EmitSignal("Transition", MenuScenes.Rebirth);
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
    }
    else
    {
      playButton.Text = "Continue";
    }
    if (_main.Player.IsInitialized())
    {
      rebirthButton.Disabled = true;
    }
    else
    {
      rebirthButton.Connect("pressed", this, "_OnRebirthPressed");
    }
    playButton.Connect("pressed", this, "_OnPlayPressed");
    GetNode<Button>("Exit").Connect("pressed", this, "_OnExitPressed");
  }

}
