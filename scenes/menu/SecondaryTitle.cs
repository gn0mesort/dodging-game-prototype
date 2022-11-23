using Godot;

using MenuScenes = MenuRoot.MenuScenes;

public class SecondaryTitle : VBoxContainer {

  /**
   * @brief A Signal emitted to request a transition between menu scenes.
   *
   * @param to The MenuScene to transition to.
   */
  [Signal]
  public delegate void Transition(MenuScenes to);

  /**
   * @brief A Signal emitted to request a transition between root scenes.
   *
   * @param to The RootScene to transition to.
   */
  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  private Main _main = null;

  private void _OnExitPressed() {
    EmitSignal("TransitionRoot", RootScenes.Exit);
  }

  private void _OnPlayPressed() {
    EmitSignal("TransitionRoot", RootScenes.Play);
  }

  private void _OnSettingsPressed() {
    EmitSignal("Transition", MenuScenes.Settings);
  }

  private void _OnRebirthPressed() {
    EmitSignal("Transition", MenuScenes.Rebirth);
  }

  /**
   * @brief Initialization method.
   */
  public override void _EnterTree() {
    _main = GetNode<Main>("/root/Main");
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
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
    GetNode<Button>("Settings").Connect("pressed", this, "_OnSettingsPressed");
    GetNode<Button>("Exit").Connect("pressed", this, "_OnExitPressed");
    playButton.GrabFocus();
  }

}
