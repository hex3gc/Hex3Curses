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

namespace Hex3Curses.Curses
{
    // Based off of the TILER2 ItemBase class: https://thunderstore.io/package/ThinkInvis/TILER2/
    // Some code taken from KomradeSpectre's Aetherium: https://thunderstore.io/package/KomradeSpectre/Aetherium/
    // If you want to add a global functionality to all curses, use this
    public abstract class CurseBase
    {
        public abstract string ItemName { get; }
        public abstract string ItemLangTokenName { get; }

        public abstract CurseTier ItemCurseTier { get; }

        public abstract GameObject ItemModel { get; }
        public abstract Sprite ItemIcon { get; }

        public ItemDef ItemDef;

        public abstract void Init(ConfigFile config);

        public abstract void CreateLang();

        public abstract ItemDisplayRuleDict CreateItemDisplayRules();

        protected void CreateItem()
        {
            ItemDef = ScriptableObject.CreateInstance<ItemDef>();
            ItemDef.name = "H3C_" + ItemLangTokenName;
            ItemDef.nameToken = "H3C_" + ItemLangTokenName + "_NAME";
            ItemDef.pickupToken = "H3C_" + ItemLangTokenName + "_PICKUP";
            ItemDef.descriptionToken = "H3C_" + ItemLangTokenName + "_DESCRIPTION";
            ItemDef.loreToken = "H3C_" + ItemLangTokenName + "_LORE";
            ItemDef.pickupModelPrefab = ItemModel;
            ItemDef.pickupIconSprite = ItemIcon;
            ItemDef.hidden = false;
            ItemDef.canRemove = false;
            ItemDef.deprecatedTier = ItemTier.NoTier;
            ItemDef.tags = new ItemTag[] { ItemTag.AIBlacklist, ItemTag.BrotherBlacklist, ItemTag.CannotCopy, ItemTag.CannotSteal, ItemTag.CannotDuplicate };
            ItemDef.requiredExpansion = Hex3Curses.cursesExpansion;

            Hex3Curses.BlacklistedFromPrinter.Add(ItemDef);
            Hex3Curses.CurseItemDefs.Add(ItemDef, ItemCurseTier);

            ItemAPI.Add(new CustomItem(ItemDef, CreateItemDisplayRules()));
        }

        public abstract void Hooks();

        public static void BlacklistFromPrinter(ILContext il)
        {
            var c = new ILCursor(il);

            int listIndex = -1;
            int thisIndex = -1;
            c.GotoNext(x => x.MatchSwitch(out _));
            var gotThisIndex = c.TryGotoNext(x => x.MatchLdarg(out thisIndex));
            var gotListIndex = c.TryGotoNext(x => x.MatchLdloc(out listIndex));
            c.GotoNext(MoveType.Before, x => x.MatchCall(out _));
            if (gotThisIndex && gotListIndex)
            {
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg, thisIndex);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldloc, listIndex);
                c.EmitDelegate<Action<ShopTerminalBehavior, List<PickupIndex>>>((shopTerminalBehavior, list) =>
                {
                    if (shopTerminalBehavior && shopTerminalBehavior.gameObject.name.Contains("Duplicator"))
                    {
                        list.RemoveAll(x => Hex3Curses.BlacklistedFromPrinter.Contains(ItemCatalog.GetItemDef(PickupCatalog.GetPickupDef(x).itemIndex)));
                    }
                });
            }
        }
    }
}
