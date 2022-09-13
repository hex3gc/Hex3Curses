using BepInEx.Configuration;
using Hex3Curses;
using Hex3Curses.Modules;
using MonoMod.Cil;
using R2API;
using RewiredConsts;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Hex3Curses.Curses.Tier2
{
    internal class Irresponsibility : CurseBase
    {
        public ConfigEntry<bool> Enabled;
        public ConfigEntry<float> ShrineIncreaseSpawn;
        public ConfigEntry<float> ShrineDecreaseChance;
        public ConfigEntry<float> ShrineDecreaseChanceCap;

        public override string ItemName => "Irresponsibility";
        public override string ItemLangTokenName => ItemName.ToUpper();

        public override CurseTier ItemCurseTier => CurseTier.Tier2;

        public override GameObject ItemModel => Hex3Curses.MainAssets.LoadAsset<GameObject>("Assets/Curses/IrresponsibilityPrefab.prefab");
        public override Sprite ItemIcon => Hex3Curses.MainAssets.LoadAsset<Sprite>("Assets/Curses/Irresponsibility.png");
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
            Enabled = config.Bind<bool>("Uncommon Curse: " + ItemName, "Enabled", true, "Enable this curse");
            ShrineIncreaseSpawn = config.Bind<float>("Uncommon Curse: " + ItemName, "Shrine Of Chance Frequency Increase", 100f, "Added percent chance for a Shrine of Chance to spawn each stage.");
            ShrineDecreaseChance = config.Bind<float>("Uncommon Curse: " + ItemName, "Shrine Of Chance Failure Chance Increase", 10f, "Increase the base chance to fail by this percent.");
            ShrineDecreaseChanceCap = config.Bind<float>("Uncommon Curse: " + ItemName, "Shrine Of Chance Failure Chance Cap", 90f, "The chance for shrines to fail will never be higher than this percent.");
            return Enabled.Value;
        }

        public override void CreateLang()
        {
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_NAME", "<style=cEvent>Irresponsibility</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_PICKUP", "<style=cEvent>The thrill of gambling takes you over.</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_DESCRIPTION", String.Format("<style=cShrine>Shrines of Chance</style> are <style=cDeath>{0}%</style> more common <style=cStack>(+{0}% per stack)</style>, and are <style=cDeath>{1}%</style> more likely to drop <style=cDeath>nothing</style> <style=cStack>(+{1}% per stack, capped at {2}%)</style>.", ShrineIncreaseSpawn.Value, ShrineDecreaseChance.Value, ShrineDecreaseChanceCap.Value));
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
            On.RoR2.ShrineChanceBehavior.AddShrineStack += IncreaseShrineFailureChance;
            On.RoR2.DirectorCardCategorySelection.GenerateDirectorCardWeightedSelection += IncreaseShrineFrequency;
        }

        private void IncreaseShrineFailureChance(On.RoR2.ShrineChanceBehavior.orig_AddShrineStack orig, ShrineChanceBehavior self, Interactor interactor)
        {
            if (interactor.TryGetComponent(out CharacterBody body) && body.inventory && body.inventory.GetItemCount(ItemDef) > 0)
            {
                self.failureChance = 0.4529f + ((ShrineDecreaseChance.Value / 100) * body.inventory.GetItemCount(ItemDef));
                if (self.failureChance > (ShrineDecreaseChanceCap.Value / 100))
                {
                    self.failureChance = (ShrineDecreaseChanceCap.Value / 100);
                }
            }
            orig(self, interactor);
        }

        private WeightedSelection<DirectorCard> IncreaseShrineFrequency(On.RoR2.DirectorCardCategorySelection.orig_GenerateDirectorCardWeightedSelection orig, DirectorCardCategorySelection self)
        {
            int irresponsibilities = 0;
            foreach (PlayerCharacterMasterController playerCharacterMasterController in PlayerCharacterMasterController.instances)
            {
                if (playerCharacterMasterController.master && playerCharacterMasterController.master.inventory)
                {
                    irresponsibilities += playerCharacterMasterController.master.inventory.GetItemCount(ItemDef);
                }
            }
            if (irresponsibilities > 0)
            {
                for (int i = 0; i < self.categories.Length; i++)
                {
                    if (self.categories[i].name == "Shrines")
                    {
                        // Make Shrines 100% more common
                        self.categories[i].selectionWeight += (self.categories[i].selectionWeight * (ShrineIncreaseSpawn.Value / 100f)) * irresponsibilities;
                        for (int j = 0; j < self.categories[i].cards.Length; j++)
                        {
                            // Make specifically Shrines of Chance 100% more common, within the Shrines category
                            if (self.categories[i].cards[j].spawnCard.name == "iscShrineChance" || self.categories[i].cards[j].spawnCard.name == "iscShrineChanceSnowy")
                            {
                                self.categories[i].cards[j].selectionWeight += (int)((self.categories[i].cards[j].selectionWeight * (ShrineIncreaseSpawn.Value / 100)) * irresponsibilities);
                            }
                        }
                    }
                }
            }
            return orig(self);
        }
    }
}
