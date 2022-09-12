using BepInEx.Configuration;
using Hex3Curses;
using Hex3Curses.Modules;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Hex3Curses.Curses.Tier3
{
    internal class Impatience : CurseBase
    {
        public ConfigEntry<bool> Enabled;
        public ConfigEntry<float> BeginHealthReductionTime;
        public ConfigEntry<float> EndHealthReductionTime;
        public ConfigEntry<float> EndHealthPercent;

        public override string ItemName => "Impatience";
        public override string ItemLangTokenName => ItemName.ToUpper();

        public override CurseTier ItemCurseTier => CurseTier.Tier3;

        public override GameObject ItemModel => Hex3Curses.MainAssets.LoadAsset<GameObject>("Assets/Curses/ImpatiencePrefab.prefab");
        public override Sprite ItemIcon => Hex3Curses.MainAssets.LoadAsset<Sprite>("Assets/Curses/Impatience.png");
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
            Enabled = config.Bind<bool>("Rare Curse: " + ItemName, "Enabled", true, "Enable this curse");
            BeginHealthReductionTime = config.Bind<float>("Rare Curse: " + ItemName, "Minutes Until Curse Activates", 5f, "How many minutes on a stage should it take for this curse to begin its effects?");
            EndHealthReductionTime = config.Bind<float>("Rare Curse: " + ItemName, "Minutes Until Curse Ends", 10f, "How many minutes on a stage should it take for this curse to reach its minimum max health value?");
            EndHealthPercent = config.Bind<float>("Rare Curse: " + ItemName, "Max Health When Curse Ends", 0.25f, "How much of their max health should a player have left when the curse is over?");
            return Enabled.Value;
        }

        public override void CreateLang()
        {
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_NAME", "<style=cEvent>Impatience</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_PICKUP", "<style=cEvent>You've become distinctly aware that difficulty is always increasing with time.</style>");
            LanguageAPI.Add("H3C_" + ItemLangTokenName + "_DESCRIPTION", String.Format("Spending more than <style=cIsDamage>{0}</style> minutes on a stage will begin to <style=cDeath>drain your max health.</style> By <style=cIsDamage>{1}</style> minutes, your max health will be fully reduced to <style=cDeath>{2}% of its original value.</style> Stacking causes health drain to speed up by <style=cIsDamage>10%</style>.", BeginHealthReductionTime.Value, EndHealthReductionTime.Value, EndHealthPercent.Value * 100f));
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
            On.RoR2.CharacterMaster.OnServerStageBegin += GiveAndResetBehavior;
            RecalculateStatsAPI.GetStatCoefficients += ApplyCurseToBody;
        }
        private void ApplyCurseToBody(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body.master && body.master.gameObject && body.master.gameObject.TryGetComponent(out ImpatienceBehavior behaviorOut))
            {
                var component = body.master.gameObject.GetComponent<ImpatienceBehavior>();
                args.healthMultAdd -= component.curseAmount;
            }
        }

        private void GiveAndResetBehavior(On.RoR2.CharacterMaster.orig_OnServerStageBegin orig, CharacterMaster self, Stage stage)
        {
            if (self.inventory && self.inventory.GetItemCount(ItemDef) > 0)
            {
                if (!self.gameObject.TryGetComponent(out ImpatienceBehavior behaviorOut))
                {
                    self.gameObject.AddComponent<ImpatienceBehavior>();
                }
                var component = self.gameObject.GetComponent<ImpatienceBehavior>();
                component.impatienceTimer = 0f;
                component.impatienceMinTime = BeginHealthReductionTime.Value;
                component.impatienceMaxTime = EndHealthReductionTime.Value;
                component.impatienceMinimumHealth = EndHealthPercent.Value;
                component.impatienceItemDef = ItemDef;
                component.ownerMaster = self;
                component.chatMessageDone = false;
            }
            orig(self, stage);
        }
    }

    public class ImpatienceBehavior : MonoBehaviour
    {
        public float impatienceTimer = 0f;
        public float effectTimer = 0f;
        public float impatienceMinTime;
        public float impatienceMaxTime;
        public float impatienceMinimumHealth;
        public ItemDef impatienceItemDef;
        public CharacterMaster ownerMaster;
        public GameObject effectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/PermanentDebuffEffect");
        public float curseAmount;
        public bool chatMessageDone = false;

        void FixedUpdate()
        {
            if (ownerMaster && ownerMaster.inventory && ownerMaster.inventory.GetItemCount(impatienceItemDef) > 0)
            {
                impatienceTimer += Time.fixedDeltaTime;
                if (impatienceTimer > (impatienceMinTime * 60) && ownerMaster.GetBody() && ownerMaster.GetBody().healthComponent)
                {
                    // Audio/visual elements
                    effectTimer += Time.fixedDeltaTime;
                    if (chatMessageDone == false)
                    {
                        Chat.AddMessage("<style=cDeath>The clock is ticking...</style>");
                        chatMessageDone = true;
                    }
                    if (effectTimer >= 4f)
                    {
                        EffectManager.SpawnEffect(effectPrefab, new EffectData
                        {
                            origin = ownerMaster.GetBody().corePosition,
                            scale = 0.5f
                        }, true);
                        effectTimer = 0f;
                    }

                    // Curse calculations
                    float fullCurse = 1f - impatienceMinimumHealth;
                    float realIntervalTime = (impatienceMaxTime - impatienceMinTime) * 60;
                    float realMinTime = impatienceMinTime * 60;
                    float realMaxTime = impatienceMaxTime * 60;
                    if (ownerMaster.inventory.GetItemCount(impatienceItemDef) > 1)
                    {
                        float maxTimeReduction = 0.1f * (ownerMaster.inventory.GetItemCount(impatienceItemDef) - 1);
                        realMaxTime *= (1f - maxTimeReduction);
                    }
                    if (realMinTime + impatienceTimer <= realMaxTime)
                    {
                        float adjustedCurse = fullCurse * ((impatienceTimer - realMinTime) / realIntervalTime);
                        curseAmount = adjustedCurse;
                    }
                    else
                    {
                        curseAmount = fullCurse;
                    }
                }
            }
        }
    }
}
