using BepInEx.Configuration;
using Hex3Curses;
using Hex3Curses.Modules;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Hex3Curses.Modules
{
    // CurseTier determines the rarity of each curse
    public enum CurseTier
    {
        Tier1,
        Tier2,
        Tier3
    }

    internal class CurseTiers
    {
        // Retrieves all curses from an inventory
        List<ItemDef> GetCurses(Inventory inventory)
        {
            List<ItemDef> curses = new List<ItemDef>();
            foreach (ItemIndex item in inventory.itemAcquisitionOrder)
            {
                ItemDef itemDef = ItemCatalog.GetItemDef(item);
                if (Hex3Curses.CurseItemDefs.ContainsKey(itemDef))
                {
                    curses.Add(itemDef);
                }
            }
            return curses;
        }

        // Retrieves all curses of a certain tier from an inventory
        List<ItemDef> GetCursesOfTier(Inventory inventory, CurseTier curseTier)
        {
            List<ItemDef> cursesOfTier = new List<ItemDef>();
            foreach (ItemIndex item in inventory.itemAcquisitionOrder)
            {
                ItemDef itemDef = ItemCatalog.GetItemDef(item);
                if (Hex3Curses.CurseItemDefs.ContainsKey(itemDef))
                {
                    Hex3Curses.CurseItemDefs.TryGetValue(itemDef, out CurseTier tier);
                    if (tier == curseTier)
                    {
                        cursesOfTier.Add(itemDef);
                    }
                }
            }
            return cursesOfTier;
        }
    }
}
