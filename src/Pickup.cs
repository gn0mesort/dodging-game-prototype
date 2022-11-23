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
using Godot;

/**
 * @brief Abstract base class for in game pickup items (e.g., health).
 */
public abstract class Pickup : StaticBody {
  /**
   * @brief The bonus that will be applied by the Pickup.
   */
  [Export]
  public uint Bonus { get; set; } = 0;

  /**
   * @brief Applies the Pickup's effect to the Player.
   *
   * @param player The Player to apply the effect to.
   */
  public abstract void ApplyEffect(Player player);
}
