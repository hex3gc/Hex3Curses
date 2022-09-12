using BepInEx;
using BepInEx.Configuration;
using Hex3Curses.Curses.Tier1;
using Hex3Curses.Curses.Tier2;
using Hex3Curses.Curses.Tier3;
using Hex3Curses.Modules;
using R2API;
using R2API.Networking;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Reflection;
using Unity.Audio;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hex3Curses
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(PrefabAPI), nameof(SoundAPI), nameof(OrbAPI), nameof(NetworkingAPI), nameof(DirectorAPI), nameof(RecalculateStatsAPI), nameof(UnlockableAPI), nameof(EliteAPI), nameof(CommandHelper))]

    public class Hex3Curses : BaseUnityPlugin
    {
        public const string ModGuid = "com.Hex3.Hex3Curses";
        public const string ModAuthor = "Hex3";
        public const string ModName = "Hex3Curses";
        public const string ModVer = "0.1.0";

        internal static BepInEx.Logging.ManualLogSource ModLogger;
        internal static BepInEx.Configuration.ConfigFile MainConfig;

        public static AssetBundle MainAssets;

        public static Dictionary<string, string> ShaderLookup = new Dictionary<string, string>() // Strings of stubbed vs. real shaders
        {
            {"stubbed hopoo games/deferred/standard", "shaders/deferred/hgstandard"}
        };

        public static HashSet<ItemDef> BlacklistedFromPrinter = new HashSet<ItemDef>();
        public static IDictionary<ItemDef, CurseTier> CurseItemDefs = new Dictionary<ItemDef, CurseTier>();

        public static RoR2.ExpansionManagement.ExpansionDef cursesExpansion;

        public void Awake()
        {
            ModLogger = Logger;
            MainConfig = Config;

            ModLogger.LogInfo("- Starting Hex3Curses -");
            ModLogger.LogInfo("Loading assets");

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Hex3Curses.hex3curses"))
            {
                MainAssets = AssetBundle.LoadFromStream(stream);
            }
            var materialAssets = MainAssets.LoadAllAssets<Material>();
            foreach (Material material in materialAssets)
            {
                if (!material.shader.name.StartsWith("Stubbed Hopoo Games"))
                {
                    continue;
                }
                var replacementShader = Resources.Load<Shader>(ShaderLookup[material.shader.name.ToLower()]);
                if (replacementShader)
                {
                    material.shader = replacementShader;
                }
            }

            ModLogger.LogInfo("Initializing Common Curses");
            Forgetfulness forgetfulness = new Forgetfulness(); forgetfulness.Init(Config);
            Exposed exposed = new Exposed(); exposed.Init(Config);
            Tortured tortured = new Tortured(); tortured.Init(Config);

            ModLogger.LogInfo("Initializing Uncommon Curses");
            Fragility fragility = new Fragility(); fragility.Init(Config);

            ModLogger.LogInfo("Initializing Rare Curses");
            Impatience impatience = new Impatience(); impatience.Init(Config);

            ModLogger.LogInfo("Creating Curse Behavior");
            AddCurseStage.Init();

            ModLogger.LogInfo("Creating Expansion");
            var expansionDef = ScriptableObject.CreateInstance<RoR2.ExpansionManagement.ExpansionDef>();
            expansionDef.nameToken = "Hex3Curses";
            expansionDef.descriptionToken = "Inflicts Curses that have powerful negative effects after each stage.";
            expansionDef.iconSprite = MainAssets.LoadAsset<Sprite>("Assets/Curses/expansionCurses.png");
            expansionDef.disabledIconSprite = LegacyResourcesAPI.Load<Sprite>("Textures/MiscIcons/texUnlockIcon");
            expansionDef.requiredEntitlement = null;
            ContentAddition.AddExpansionDef(expansionDef);

            ModLogger.LogInfo("- Hex3Curses done loading -");

            // You can define a command here that gives you curses automatically
        }
    }
}
