using Godot;
using System;

public class InitialTitle : VBoxContainer {
  [Signal]
  public delegate void Transition(GameState to);

  private void _OnAnyPress() {
    EmitSignal("Transition", GameState.SecondaryTitle);
  }

  public override void _Ready() {
    GetNode<Label>("PressAnyKey").Connect("Pressed", this, "_OnAnyPress");
  }
}
