using Godot;

using MenuScenes = MenuRoot.MenuScenes;

/**
 * @brief Behavior script for the primary title menu.
 */
public class PrimaryTitle : VBoxContainer {

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

  private void _OnAnyPress() {
    EmitSignal("Transition", MenuScenes.Secondary);
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    GetNode<Label>("PressAnyKey").Connect("Pressed", this, "_OnAnyPress");
  }
}
