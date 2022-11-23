using Godot;

using MenuScenes = MenuRoot.MenuScenes;

public class Settings : GridContainer {
  /**
   * @brief A Signal emitted to request a transition between root scenes.
   *
   * @param to The RootScene to transition to.
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

  /**
   * @brief A Signal emitted to request a return to the previous menu.
   */
  [Signal]
  public delegate void RequestBack();

  private void _OnBackPressed() {
    EmitSignal("Transition", MenuScenes.Secondary);
    EmitSignal("RequestBack");
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    var back = GetNode<Button>("Back");
    back.Connect("pressed", this, "_OnBackPressed");
    back.GrabFocus();
  }
}
