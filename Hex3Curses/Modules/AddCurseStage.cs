using BepInEx.Configuration;
using Hex3Curses;
using Hex3Curses.Modules;
using MonoMod.Cil;
using R2API;
using R2API.ContentManagement;
using R2API.ScriptableObjects;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Hex3Curses.Modules
{
    public enum Difficulty
    {
        drizzle,
        rainstorm,
        monsoon,
        eclipse3plus,
        // More Difficulties
        sunny,
        thunderstorm,
        calypso,
        tempest,
        armageddon,
        // Untitled Difficulty Mod
        deluge,
        charybdis,
        // Starstorm 2
        typhoon
    }

    internal static class AddCurseStage
    {
        private static void AddCurseBehaviorToPlayers(On.RoR2.PlayerCharacterMasterController.orig_Awake orig, PlayerCharacterMasterController self)
        {
            orig(self);
            if (Run.instance && self.master && !self.master.gameObject.TryGetComponent(out CurseBehavior componentOut))
            {
                Difficulty difficulty = new Difficulty();
                switch (Run.instance.selectedDifficulty) // Vanilla difficulties
                {
                    case DifficultyIndex.Easy: difficulty = Difficulty.drizzle;  break;
                    case DifficultyIndex.Normal: difficulty = Difficulty.rainstorm; break;
                    case DifficultyIndex.Hard: difficulty = Difficulty.monsoon; break;
                    case DifficultyIndex.Eclipse3: difficulty = Difficulty.eclipse3plus; break;
                    case DifficultyIndex.Eclipse4: difficulty = Difficulty.eclipse3plus; break;
                    case DifficultyIndex.Eclipse5: difficulty = Difficulty.eclipse3plus; break;
                    case DifficultyIndex.Eclipse6: difficulty = Difficulty.eclipse3plus; break;
                    case DifficultyIndex.Eclipse7: difficulty = Difficulty.eclipse3plus; break;
                    case DifficultyIndex.Eclipse8: difficulty = Difficulty.eclipse3plus; break;
                    case DifficultyIndex.Invalid: difficulty = Difficulty.drizzle; break;
                }
                self.master.gameObject.AddComponent<CurseBehavior>();
                var behavior = self.master.gameObject.GetComponent<CurseBehavior>();
                behavior.selectedDifficulty = difficulty;
                behavior.master = self.master;
            }
        }

        private static void AddCurseAfterTeleporter(On.RoR2.TeleporterInteraction.orig_AttemptToSpawnAllEligiblePortals orig, TeleporterInteraction self)
        {
            Xoroshiro128Plus rng = new Xoroshiro128Plus(Run.instance.stageRng.nextUlong);
            var listOfCurseBehaviors = GameObject.FindObjectsOfType<CurseBehavior>();
            foreach (CurseBehavior behavior in listOfCurseBehaviors)
            {
                if (behavior.master.inventory)
                {
                    int cursesToAdd = 0;
                    switch (behavior.selectedDifficulty)
                    {
                        // Vanilla
                        case Difficulty.drizzle: cursesToAdd = 0; break;
                        case Difficulty.rainstorm: cursesToAdd = 1; break;
                        case Difficulty.monsoon: cursesToAdd = 2; break;
                        case Difficulty.eclipse3plus: cursesToAdd = 3; break;
                        // More Difficulties
                        case Difficulty.sunny: cursesToAdd = 0; break;
                        case Difficulty.thunderstorm: cursesToAdd = 1; break;
                        case Difficulty.calypso: cursesToAdd = 3; break;
                        case Difficulty.tempest: cursesToAdd = 4; break;
                        case Difficulty.armageddon: cursesToAdd = 5; break;
                        // Untitled Difficulty Mod
                        case Difficulty.deluge: cursesToAdd = 3; break;
                        case Difficulty.charybdis: cursesToAdd = 4; break;
                        // Starstorm 2
                        case Difficulty.typhoon: cursesToAdd = 3; break;
                        default: cursesToAdd = 0; break;
                    }
                    for (int i = 0; i < cursesToAdd; i++)
                    {
                        int commonWeight = 70;
                        int uncommonWeight = 25;
                        int rareWeight = 5;
                        int rollResult = rng.RangeInt(1, 101);

                        if (rollResult <= commonWeight) // Common roll
                        {
                            List<ItemDef> commonCurses = new List<ItemDef>();
                            foreach (var keyPair in Hex3Curses.CurseItemDefs)
                            {
                                if (keyPair.Value == CurseTier.Tier1)
                                {
                                    commonCurses.Add(keyPair.Key);
                                }
                            }
                            if (commonCurses.Count > 0)
                            {
                                Util.ShuffleList(commonCurses);
                                var curse = commonCurses.First();
                                CharacterMasterNotificationQueue.PushItemNotification(behavior.master, curse.itemIndex);
                                behavior.master.inventory.GiveItem(curse);
                            }
                        }
                        else if ((rollResult <= (uncommonWeight + commonWeight)) && rollResult > commonWeight)  // Uncommon roll
                        {
                            List<ItemDef> uncommonCurses = new List<ItemDef>();
                            foreach (var keyPair in Hex3Curses.CurseItemDefs)
                            {
                                if (keyPair.Value == CurseTier.Tier2)
                                {
                                    uncommonCurses.Add(keyPair.Key);
                                }
                            }
                            if (uncommonCurses.Count > 0)
                            {
                                Util.ShuffleList(uncommonCurses);
                                var curse = uncommonCurses.First();
                                CharacterMasterNotificationQueue.PushItemNotification(behavior.master, curse.itemIndex);
                                behavior.master.inventory.GiveItem(curse);
                            }
                        }
                        else // Rare roll
                        {
                            List<ItemDef> rareCurses = new List<ItemDef>();
                            foreach (var keyPair in Hex3Curses.CurseItemDefs)
                            {
                                if (keyPair.Value == CurseTier.Tier3)
                                {
                                    rareCurses.Add(keyPair.Key);
                                }
                            }
                            if (rareCurses.Count > 0)
                            {
                                Util.ShuffleList(rareCurses);
                                var curse = rareCurses.First();
                                CharacterMasterNotificationQueue.PushItemNotification(behavior.master, curse.itemIndex);
                                behavior.master.inventory.GiveItem(curse);
                            }
                        }
                    }
                }
            }
        }

        internal static void Init()
        {
            // Add code for changing / appending to difficulty descriptions

            On.RoR2.TeleporterInteraction.AttemptToSpawnAllEligiblePortals += AddCurseAfterTeleporter;
            On.RoR2.PlayerCharacterMasterController.Awake += AddCurseBehaviorToPlayers;
        }
    }

    public class CurseBehavior : MonoBehaviour
    {
        public Difficulty selectedDifficulty;
        public CharacterMaster master;
    }
}
