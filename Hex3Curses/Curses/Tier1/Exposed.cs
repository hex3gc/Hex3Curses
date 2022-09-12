using BepInEx.Configuration;
using Hex3Curses;
using Hex3Curses.Curses.Tier3;
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
    internal class Exposed : CurseBase
    {
        public ConfigEntry<bool> Enabled;
        public ConfigEntry<float> AddCritChance;

        public override string ItemName => "Exposed";
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
            AddCritChance = config.Bind<float>("Common Curse: " + ItemName, "Crit Chance Added to Enemies", 10f, "Crit chance added per curse.");
            return Enabled.Value;
        }

        public override void CreateLang()
        {
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_NAME", "<style=cEvent>Exposed</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_PICKUP", "<style=cEvent>Your foes begin to see your weaknesses.</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_DESCRIPTION", String.Format("Enemies gain <style=cDeath>{0}%</style> <style=cIsDamage>critical strike chance</style> <style=cStack>(+{0}% per stack)</style>.", AddCritChance.Value));
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
            RecalculateStatsAPI.GetStatCoefficients += AddCritChanceStat;
        }

        private void AddCritChanceStat(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body.teamComponent && (body.teamComponent.teamIndex == TeamIndex.Monster || body.teamComponent.teamIndex == TeamIndex.Void || body.teamComponent.teamIndex == TeamIndex.Lunar))
            {
                float totalCritChance = 0f;
                foreach (TeamComponent ally in TeamComponent.GetTeamMembers(TeamIndex.Player))
                {
                    if (ally.body && ally.body.inventory)
                    {
                        totalCritChance += AddCritChance.Value * ally.body.inventory.GetItemCount(ItemDef);
                    }
                }
                if (totalCritChance > 0f)
                {
                    args.critAdd += totalCritChance;
                }
            }
        }
    }
}