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
    internal class Cursed : CurseBase
    {
        public ConfigEntry<bool> Enabled;
        public ConfigEntry<int> AddedCurses;

        public override string ItemName => "Cursed";
        public override string ItemLangTokenName => ItemName.ToUpper();

        public override CurseTier ItemCurseTier => CurseTier.Tier2;

        public override GameObject ItemModel => Hex3Curses.MainAssets.LoadAsset<GameObject>("Assets/Curses/CursedPrefab.prefab");
        public override Sprite ItemIcon => Hex3Curses.MainAssets.LoadAsset<Sprite>("Assets/Curses/Cursed.png");
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
            AddedCurses = config.Bind<int>("Uncommon Curse: " + ItemName, "Additional Curses", 1, "How many more curses to add per stage.");
            return Enabled.Value;
        }

        public override void CreateLang()
        {
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_NAME", "<style=cEvent>Cursed</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_PICKUP", "<style=cEvent>This planet has decided that you do not belong on it.</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_DESCRIPTION", String.Format("For every stack, add <style=cDeath>{0}</style> more curse(s) to your inventory every stage.", AddedCurses.Value));
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

        }
    }
}
