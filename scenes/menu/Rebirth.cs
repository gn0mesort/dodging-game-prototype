using Godot;

using MenuScenes = MenuRoot.MenuScenes;

public class Rebirth : CenterContainer {
  [Signal]
  public delegate void Transition(MenuScenes to);

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

  public override void _Ready() {
    var confirm = GetNode<Button>("VBoxContainer/VBoxContainer2/Confirm");
    confirm.Connect("pressed", this, "_OnConfirmPressed");
    GetNode<Button>("VBoxContainer/VBoxContainer2/Back").Connect("pressed", this, "_OnBackPressed");
    confirm.GrabFocus();
  }
}
