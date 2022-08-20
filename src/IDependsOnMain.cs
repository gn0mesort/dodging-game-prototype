using Godot;
using System;

public interface IDependsOnMain {
  void SetMainNode(Main main);
  Main GetMainNode();
}
