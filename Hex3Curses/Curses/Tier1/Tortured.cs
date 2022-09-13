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
    internal class Tortured : CurseBase
    {
        public ConfigEntry<bool> Enabled;
        public ConfigEntry<float> PercentHpCost;
        public ConfigEntry<float> PercentHpCostStack;

        public override string ItemName => "Tortured";
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
            PercentHpCost = config.Bind<float>("Common Curse: " + ItemName, "HP Cost Per Interactable", 10f, "Every interactable will cost this percent of your HP.");
            PercentHpCostStack = config.Bind<float>("Common Curse: " + ItemName, "HP Cost Per Interactable Per Stack", 5f, "Every interactable will cost this percent of your HP for additional stacks.");
            return Enabled.Value;
        }

        public override void CreateLang()
        {
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_NAME", "<style=cEvent>Tortured</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_PICKUP", "<style=cEvent>There isn't a painless moment ahead of you.</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_DESCRIPTION", String.Format("All interactables have a <style=cDeath>LETHAL blood cost</style> of <style=cDeath>{0}% hp</style> <style=cStack>(+{1}% per stack, capped at 90%)</style>.", PercentHpCost.Value, PercentHpCostStack.Value));
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
            On.RoR2.Interactor.PerformInteraction += HpCostOnInteract;
        }

        private void HpCostOnInteract(On.RoR2.Interactor.orig_PerformInteraction orig, Interactor self, GameObject interactableObject)
        {
            GameObject effectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/PermanentDebuffEffect");

            if (self.TryGetComponent(out CharacterBody body) && body.inventory && body.inventory.GetItemCount(ItemDef) > 0 && body.healthComponent)
            {
                bool anyInteractionSucceeded = false;
                foreach (IInteractable interactable in interactableObject.GetComponents<IInteractable>())
                {
                    Interactability interactability = interactable.GetInteractability(self);
                    if (interactability == Interactability.Available)
                    {
                        anyInteractionSucceeded = true;
                    }
                }
                if (anyInteractionSucceeded)
                {
                    if (interactableObject.name != "GenericPickup" && interactableObject.name != "GenericPickup(Clone)")
                    {
                        DamageInfo damageInfo = new DamageInfo();
                        damageInfo.damage = (body.maxHealth + body.maxShield) * ((PercentHpCost.Value / 100f) + ((PercentHpCostStack.Value * (body.inventory.GetItemCount(ItemDef) - 1)) / 100f));
                        damageInfo.damageType = DamageType.BypassArmor & DamageType.BypassBlock & DamageType.BypassOneShotProtection;
                        damageInfo.crit = false;
                        damageInfo.inflictor = interactableObject;
                        damageInfo.position = body.corePosition;
                        damageInfo.damageColorIndex = DamageColorIndex.SuperBleed;
                        damageInfo.procCoefficient = 0f;
                        damageInfo.procChainMask = new ProcChainMask();
                        damageInfo.attacker = body.gameObject;
                        // Cap damage at 90%
                        if (damageInfo.damage > ((body.maxHealth + body.maxShield) * 0.9f))
                        {
                            damageInfo.damage = (body.maxHealth + body.maxShield) * 0.9f;
                        }
                        EffectManager.SpawnEffect(effectPrefab, new EffectData
                        {
                            origin = body.corePosition,
                            scale = 0.5f
                        }, true);
                        body.healthComponent.TakeDamage(damageInfo);
                    }
                }
            }
            orig(self, interactableObject);
        }
    }
}