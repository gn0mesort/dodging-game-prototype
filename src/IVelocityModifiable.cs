/**
 * @brief An interface representing objects that can have their velocity modified by in game effects.
 *
 * This should be applied to objects that are affected by the Field entity.
 */
public interface IVelocityModifiable {
  /**
   * @brief Modify the velocity of the target object by the given factor.
   *
   * Each factor is applied as-if by multiplication. That is to say, ModifyVelocity(0.5, 0.5, 0.5, 0.5) will halve all
   * the velocity of the target object along all three axes and halve the speed at which it can rotate. Similarly,
   * ModifyVelocity(1, 1, 1, 1) will restore the default behavior.
   *
   * @param x The X-axis velocity modifier.
   * @param y The Y-axis velocity modifier.
   * @param z The Z-axis velocity modifier.
   * @param rotation The rotational velocity modifier.
   */
  void ModifyVelocity(float x, float y, float z, float rotation);
}
