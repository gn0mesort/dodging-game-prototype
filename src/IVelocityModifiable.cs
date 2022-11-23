/**
 * Copyright (C) 2022 Alexander Rothman <gnomesort@megate.ch>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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
