using Godot;

using MenuScenes = MenuRoot.MenuScenes;

/**
 * @brief Behavior script for the Rebirth menu.
 */
public class Rebirth : CenterContainer {

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

  private void _OnConfirmPressed() {
    var main = GetNode<Main>("/root/Main");
    main.InitializePlayerData();
    main.StorePlayerData();
    _OnBackPressed();
  }

  private void _OnBackPressed() {
    EmitSignal("Transition", MenuScenes.Secondary);
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    var confirm = GetNode<Button>("VBoxContainer/VBoxContainer2/Confirm");
    confirm.Connect("pressed", this, "_OnConfirmPressed");
    GetNode<Button>("VBoxContainer/VBoxContainer2/Back").Connect("pressed", this, "_OnBackPressed");
    confirm.GrabFocus();
  }
}
