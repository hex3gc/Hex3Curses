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
using UnityEngine.Diagnostics;

namespace Hex3Curses.Curses.Tier1
{
    internal class Forgetfulness : CurseBase
    {
        public ConfigEntry<bool> Enabled;
        public ConfigEntry<int> ItemRemovePerStage;
        public ConfigEntry<float> CursePrefersCommon;
        public ConfigEntry<float> CursePrefersUncommon;
        public ConfigEntry<float> CursePrefersLegendary;

        public override string ItemName => "Forgetfulness";
        public override string ItemLangTokenName => ItemName.ToUpper();

        public override CurseTier ItemCurseTier => CurseTier.Tier1;

        public override GameObject ItemModel => Hex3Curses.MainAssets.LoadAsset<GameObject>("Assets/Curses/ForgetfulnessPrefab.prefab");
        public override Sprite ItemIcon => Hex3Curses.MainAssets.LoadAsset<Sprite>("Assets/Curses/Forgetfulness.png");
        public static GameObject ItemDisplayPrefab;

        public override void Init(ConfigFile config)
        {
            if (CreateConfig(config))
            {
                CreateLang();
                CreateItem();
                Hooks();
            }
        }

        private bool CreateConfig(ConfigFile config)
        {
            Enabled = config.Bind<bool>("Common Curse: " + ItemName, "Enabled", true, "Enable this curse");
            ItemRemovePerStage = config.Bind<int>("Common Curse: " + ItemName, "Items Lost Per Stage", 2, "How many items will be lost when you enter a stage.");
            CursePrefersCommon = config.Bind<float>("Common Curse: " + ItemName, "Common Chance", 80f, "Percent chance that a common item will be lost. These values should add up to 100.");
            CursePrefersUncommon = config.Bind<float>("Common Curse: " + ItemName, "Uncommon Chance", 20f, "Percent chance that an uncommon item will be lost. These values should add up to 100.");
            CursePrefersLegendary = config.Bind<float>("Common Curse: " + ItemName, "Legendary Chance", 0f, "Percent chance that a legendary item will be lost. These values should add up to 100.");
            return Enabled.Value;
        }

        public override void CreateLang()
        {
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_NAME", "<style=cEvent>Forgetfulness</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_PICKUP", "<style=cEvent>You feel like you've forgotten something important.</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_DESCRIPTION", String.Format("Each time you enter a stage, you will <style=cDeath>lose</style> {0} random items <style=cStack>(+{0} per stack)</style>. Prefers rarities: [{1}%/<style=cIsHealing>{2}%</style>/<style=cDeath>{3}%</style>]", ItemRemovePerStage.Value, CursePrefersCommon.Value, CursePrefersUncommon.Value, CursePrefersLegendary.Value));
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_LORE", "");
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayPrefab = ItemModel;
            var itemDisplay = ItemDisplayPrefab.AddComponent<ItemDisplay>();
            List<CharacterModel.RendererInfo> rendererInfos = new List<CharacterModel.RendererInfo>();
            foreach (Renderer renderer in ItemDisplayPrefab.GetComponentsInChildren<Renderer>())
            {
                CharacterModel.RendererInfo rendererInfo = new CharacterModel.RendererInfo
                {
                    renderer = renderer,
                    defaultMaterial = renderer.material
                };
                rendererInfos.Add(rendererInfo);
            }
            itemDisplay.rendererInfos = rendererInfos.ToArray();

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlHuntress", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlToolbot", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Hip",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlEngi", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlMage", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlMerc", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlTreebot", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "PlatformBase",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlLoader", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlCroco", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Hip",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlCaptain", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlBandit2", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("EngiTurretBody", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Base",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("EngiWalkerTurretBody", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Base",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlScav", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlRailGunner", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            rules.Add("mdlVoidSurvivor", new RoR2.ItemDisplayRule[]{new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = ItemDisplayPrefab,
                        childName = "Pelvis",
                        localPos = new Vector3(0.0F, 0.0F, 0.0F),
                        localAngles = new Vector3(0.0F, 0.0F, 0.0F),
                        localScale = new Vector3(0.0F, 0.0F, 0.0F)
                    }
                }
            );
            return rules;
        }

        public override void Hooks()
        {
            On.RoR2.CharacterMaster.OnServerStageBegin += RemoveItems;
        }

        private void RemoveItems(On.RoR2.CharacterMaster.orig_OnServerStageBegin orig, CharacterMaster self, Stage stage)
        {
            if (self.inventory && self.inventory.GetItemCount(ItemDef) > 0)
            {
                Xoroshiro128Plus rng = new Xoroshiro128Plus(Run.instance.stageRng.nextUlong);
                bool activated = true;
                int itemCount = self.inventory.GetItemCount(ItemDef);
                for (int i = 0; i < (itemCount * ItemRemovePerStage.Value); i++)
                {
                    int totalItems = 0;
                    if (CursePrefersCommon.Value > 0) { totalItems += self.inventory.GetTotalItemCountOfTier(ItemTier.Tier1); }
                    if (CursePrefersUncommon.Value > 0) { totalItems += self.inventory.GetTotalItemCountOfTier(ItemTier.Tier2); }
                    if (CursePrefersLegendary.Value > 0) { totalItems += self.inventory.GetTotalItemCountOfTier(ItemTier.Tier3); }
                    if (totalItems > 0)
                    {
                        if (activated)
                        {
                            int messageChoice = rng.RangeInt(1, 5);
                            switch (messageChoice)
                            {
                                case 1: Chat.AddMessage("<style=cEvent>You must've dropped something along the way...</style>"); break;
                                case 2: Chat.AddMessage("<style=cEvent>Your pockets feel inexpliciably light...</style>"); break;
                                case 3: Chat.AddMessage("<style=cEvent>You search your inventory for a certain item, but find nothing...</style>"); break;
                                case 4: Chat.AddMessage("<style=cEvent>Did I forget my keys? Wait, no, it was something else...</style>"); break;
                            }
                            activated = false;
                        }

                        float percentageSum = CursePrefersCommon.Value + CursePrefersUncommon.Value + CursePrefersLegendary.Value;
                        float x = rng.RangeFloat(0f, percentageSum);
                        List<ItemIndex> inventoryList = new List<ItemIndex>(self.inventory.itemAcquisitionOrder);
                        Util.ShuffleList(inventoryList, rng);

                        if (self.inventory.GetTotalItemCountOfTier(ItemTier.Tier1) > 0 && (CursePrefersCommon.Value > 0) && ((x -= CursePrefersCommon.Value) < 0))
                        {
                            foreach (ItemIndex itemIndex in inventoryList)
                            {
                                if (ItemCatalog.GetItemDef(itemIndex).tier == ItemTier.Tier1)
                                {
                                    self.inventory.RemoveItem(itemIndex);
                                    break;
                                }
                            }
                        }
                        else if (self.inventory.GetTotalItemCountOfTier(ItemTier.Tier2) > 0 && (CursePrefersUncommon.Value > 0) && ((x -= CursePrefersUncommon.Value) < 0))
                        {
                            foreach (ItemIndex itemIndex in inventoryList)
                            {
                                if (ItemCatalog.GetItemDef(itemIndex).tier == ItemTier.Tier2)
                                {
                                    self.inventory.RemoveItem(itemIndex);
                                    break;
                                }
                            }
                        }
                        else if (self.inventory.GetTotalItemCountOfTier(ItemTier.Tier3) > 0 && (CursePrefersLegendary.Value > 0) && ((x -= CursePrefersLegendary.Value) < 0))
                        {
                            foreach (ItemIndex itemIndex in inventoryList)
                            {
                                if (ItemCatalog.GetItemDef(itemIndex).tier == ItemTier.Tier3)
                                {
                                    self.inventory.RemoveItem(itemIndex);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            i--;
                        }
                    }
                    else
                    {
                        Chat.AddMessage("<style=cEvent>And then there was nothing left for you to forget.</style>");
                        break;
                    }
                }
            }
            orig(self, stage);
        }
    }
}
