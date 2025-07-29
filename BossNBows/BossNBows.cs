using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace BossNBows
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class BossNBows : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.BossNBows";
        public const string PluginName = "BossNBows";
        public const string PluginVersion = "1.0.1";

        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        public static AssetBundle BossNBowBundle;

        public static CustomStatusEffect PiercingShotStatusEffect;
        public static CustomStatusEffect RootShotStatusEffect;
        public static CustomStatusEffect PoisonShotStatusEffect;
        public static CustomStatusEffect FireShotStatusEffect;
        public static CustomStatusEffect RegenShotStatusEffect;
        public static CustomStatusEffect LightningShotStatusEffect;

        public static ConfigEntry<bool> EnableDash;
        public static ConfigEntry<KeyboardShortcut> BowModeKey;



        private void Awake()
        {
            BossNBowsConfig.Bind(Config);
            CreateConfig();
            LoadAssets();
            AddStatusEffects();
            AddBows();
            AddLocalizations();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGUID);
        }



        private void CreateConfig()
        {
            EnableDash = Config.Bind("Dash Mode", "EnableDash", true,new ConfigDescription("Enable dash ability (local client setting)"));

            BowModeKey = Config.Bind("Bow Mode Shortcut", "Bow Mode Shortcut", new KeyboardShortcut(KeyCode.G), new ConfigDescription("Shortcut for bow mode", null, new ConfigurationManagerAttributes { IsAdminOnly = false }));
        }

        public static class BossNBowsConfig
        {
            public static ConfigEntry<string> EikthyrbowCraftingStation;
            public static ConfigEntry<string> EikthyrbowRepairStation;
            public static ConfigEntry<string> EikthyrbowRequirements;

            public static ConfigEntry<string> ElderbowCraftingStation;
            public static ConfigEntry<string> ElderbowRepairStation;
            public static ConfigEntry<string> ElderbowRequirements;

            public static ConfigEntry<string> BonemassbowCraftingStation;
            public static ConfigEntry<string> BonemassbowRepairStation;
            public static ConfigEntry<string> BonemassbowRequirements;

            public static ConfigEntry<string> ModerbowCraftingStation;
            public static ConfigEntry<string> ModerbowRepairStation;
            public static ConfigEntry<string> ModerbowRequirements;

            public static ConfigEntry<string> YagluthbowCraftingStation;
            public static ConfigEntry<string> YagluthbowRepairStation;
            public static ConfigEntry<string> YagluthbowRequirements;

            public static ConfigEntry<string> QueenbowCraftingStation;
            public static ConfigEntry<string> QueenbowRepairStation;
            public static ConfigEntry<string> QueenbowRequirements;

            public static ConfigEntry<string> FaderbowCraftingStation;
            public static ConfigEntry<string> FaderbowRepairStation;
            public static ConfigEntry<string> FaderbowRequirements;


            public static void Bind(ConfigFile config)
            {
                var adminOnly = new ConfigurationManagerAttributes { IsAdminOnly = true };

                EikthyrbowCraftingStation = config.Bind("Eikthyrbow", "CraftingStation", "piece_workbench", new ConfigDescription("Crafting station for Eikthyr Bow", null, adminOnly));
                EikthyrbowRepairStation = config.Bind("Eikthyrbow", "RepairStation", "piece_workbench", new ConfigDescription("Repair Station for Eikthyr Bow", null, adminOnly));
                EikthyrbowRequirements = config.Bind("Eikthyrbow", "Requirements", "TrophyEikthyr:1:1,HardAntler:10:15,Bronze:10:20,SurtlingCore:5:10", new ConfigDescription("Craft requirement", null, adminOnly));

                ElderbowCraftingStation = config.Bind("Elderbow", "CraftingStation", "piece_workbench", new ConfigDescription("Crafting station for Elder Bow", null, adminOnly));
                ElderbowRepairStation = config.Bind("Elderbow", "RepairStation", "piece_workbench", new ConfigDescription("Repair Station for Elder Bow", null, adminOnly));
                ElderbowRequirements = config.Bind("Elderbow", "Requirements", "TrophyTheElder:1:1,Iron:30:30,ElderBark:20:30,Chain:2:3", new ConfigDescription("Craft requirement", null, adminOnly));

                BonemassbowCraftingStation = config.Bind("Bonemassbow", "CraftingStation", "piece_workbench", new ConfigDescription("Crafting station for Bonemass Bow", null, adminOnly));
                BonemassbowRepairStation = config.Bind("Bonemassbow", "RepairStation", "piece_workbench", new ConfigDescription("Repair Station for Bonemass Bow", null, adminOnly));
                BonemassbowRequirements = config.Bind("Bonemassbow", "Requirements", "TrophyBonemass:1:1,Silver:30:30,Obsidian:20:25,WolfHairBundle:25:30", new ConfigDescription("Craft requirement", null, adminOnly));

                ModerbowCraftingStation = config.Bind("Moderbow", "CraftingStation", "piece_workbench", new ConfigDescription("Crafting station for Moder Bow", null, adminOnly));
                ModerbowRepairStation = config.Bind("Moderbow", "RepairStation", "piece_workbench", new ConfigDescription("Repair Station for Moder Bow", null, adminOnly));
                ModerbowRequirements = config.Bind("Moderbow", "Requirements", "TrophyDragonQueen:1:1,BlackMetal:30:60,DragonEgg:3:2,DragonTear:10:20", new ConfigDescription("Craft requirement", null, adminOnly));

                YagluthbowCraftingStation = config.Bind("Yagluthbow", "CraftingStation", "blackforge", new ConfigDescription("Crafting station for Yagluth Bow", null, adminOnly));
                YagluthbowRepairStation = config.Bind("Yagluthbow", "RepairStation", "blackforge", new ConfigDescription("Repair Station for Yagluth Bow", null, adminOnly));
                YagluthbowRequirements = config.Bind("Yagluthbow", "Requirements", "TrophyGoblinKing:1:1,Eitr:30:60,BlackCore:10:10,YggdrasilWood:20:40", new ConfigDescription("Craft requirement", null, adminOnly));

                QueenbowCraftingStation = config.Bind("Queenbow", "CraftingStation", "blackforge", new ConfigDescription("Crafting station for Queen Bow", null, adminOnly));
                QueenbowRepairStation = config.Bind("Queenbow", "RepairStation", "blackforge", new ConfigDescription("Repair Station for Queen Bow", null, adminOnly));
                QueenbowRequirements = config.Bind("Queenbow", "Requirements", "TrophySeekerQueen:1:1,FlametalNew:60:120,Blackwood:30:100,Carapace:20:50", new ConfigDescription("Craft requirement", null, adminOnly));

                FaderbowCraftingStation = config.Bind("Faderbow", "CraftingStation", "blackforge", new ConfigDescription("Crafting station for Fader Bow", null, adminOnly));
                FaderbowRepairStation = config.Bind("Faderbow", "RepairStation", "blackforge", new ConfigDescription("Repair Station for Fader Bow", null, adminOnly));
                FaderbowRequirements = config.Bind("Faderbow", "Requirements", "TrophyFader:1:1,GemstoneBlue:10:20,GemstoneRed:10:20,GemstoneGreen:10:20", new ConfigDescription("Craft requirement", null, adminOnly));

            }
        }

        public static class RecipeHelper
        {
            public static List<RequirementConfig> ParseRequirements(string input)
            {
                var result = new List<RequirementConfig>();
                foreach (var entry in input.Split(','))
                {
                    var parts = entry.Split(':');
                    if (parts.Length >= 3 && int.TryParse(parts[1], out int amount) && int.TryParse(parts[2], out int maxAmount))
                    {
                        result.Add(new RequirementConfig(parts[0], amount, maxAmount));
                    }
                }
                return result;
            }
        }

        private void LoadAssets()
        {
            BossNBowBundle = AssetUtils.LoadAssetBundleFromResources("bossnbows");
        }
        private void AddBows()
        {

            GameObject eikthyrbowPrefab = BossNBowBundle.LoadAsset<GameObject>("assets/bossnbows/eikthyrbow.prefab");
            GameObject elderbowPrefab = BossNBowBundle.LoadAsset<GameObject>("assets/bossnbows/elderbow.prefab");
            GameObject bonemassbowPrefab = BossNBowBundle.LoadAsset<GameObject>("assets/bossnbows/bonemassbow.prefab");
            GameObject moderbowPrefab = BossNBowBundle.LoadAsset<GameObject>("assets/bossnbows/moderbow.prefab");
            GameObject yagluthbowPrefab = BossNBowBundle.LoadAsset<GameObject>("assets/bossnbows/yagluthbow.prefab");
            GameObject queenbowPrefab = BossNBowBundle.LoadAsset<GameObject>("assets/bossnbows/queenbow.prefab");
            GameObject faderbowPrefab = BossNBowBundle.LoadAsset<GameObject>("assets/bossnbows/faderbow.prefab");

            ItemConfig bow1config = new ItemConfig();
            bow1config.Requirements = RecipeHelper.ParseRequirements(BossNBowsConfig.EikthyrbowRequirements.Value).ToArray();
            bow1config.CraftingStation = BossNBowsConfig.EikthyrbowCraftingStation.Value;
            bow1config.RepairStation = BossNBowsConfig.EikthyrbowRepairStation.Value;
            var bow1 = new CustomItem(eikthyrbowPrefab, fixReference: true, bow1config);
            BossNBows.AttackSpeedSystem.RegisterMainAttacks(eikthyrbowPrefab, 2.2f);
            DashOnShootSystem.RegisterDashWeapon(eikthyrbowPrefab, 7f);
            ItemManager.Instance.AddItem(bow1);
            bow1.ItemDrop.m_itemData.m_shared.m_equipStatusEffect = PiercingShotStatusEffect.StatusEffect;



            ItemConfig bow2config = new ItemConfig();
            bow2config.Requirements = RecipeHelper.ParseRequirements(BossNBowsConfig.ElderbowRequirements.Value).ToArray();
            bow2config.CraftingStation = BossNBowsConfig.ElderbowCraftingStation.Value;
            bow2config.RepairStation = BossNBowsConfig.ElderbowRepairStation.Value;
            var bow2 = new CustomItem(elderbowPrefab, fixReference: true, bow2config);
            BossNBows.AttackSpeedSystem.RegisterMainAttacks(elderbowPrefab, 2.2f);
            DashOnShootSystem.RegisterDashWeapon(elderbowPrefab, 7f);
            ItemManager.Instance.AddItem(bow2);
            bow2.ItemDrop.m_itemData.m_shared.m_attackStatusEffect = RootShotStatusEffect.StatusEffect;
            bow2.ItemDrop.m_itemData.m_shared.m_attackStatusEffectChance = 0.15f;



            ItemConfig bow3config = new ItemConfig();
            bow3config.Requirements = RecipeHelper.ParseRequirements(BossNBowsConfig.BonemassbowRequirements.Value).ToArray();
            bow3config.CraftingStation = BossNBowsConfig.BonemassbowCraftingStation.Value;
            bow3config.RepairStation = BossNBowsConfig.BonemassbowRepairStation.Value;
            var bow3 = new CustomItem(bonemassbowPrefab, fixReference: true, bow3config);
            BossNBows.AttackSpeedSystem.RegisterMainAttacks(bonemassbowPrefab, 2.2f);
            DashOnShootSystem.RegisterDashWeapon(bonemassbowPrefab, 7f);
            ItemManager.Instance.AddItem(bow3);
            bow3.ItemDrop.m_itemData.m_shared.m_equipStatusEffect = PoisonShotStatusEffect.StatusEffect;


            ItemConfig bow4config = new ItemConfig();
            bow4config.Requirements = RecipeHelper.ParseRequirements(BossNBowsConfig.ModerbowRequirements.Value).ToArray();
            bow4config.CraftingStation = BossNBowsConfig.ModerbowCraftingStation.Value;
            bow4config.RepairStation = BossNBowsConfig.ModerbowRepairStation.Value;
            var bow4 = new CustomItem(moderbowPrefab, fixReference: true, bow4config);
            BossNBows.AttackSpeedSystem.RegisterMainAttacks(moderbowPrefab, 2.2f);
            DashOnShootSystem.RegisterDashWeapon(moderbowPrefab, 7f);
            ItemManager.Instance.AddItem(bow4);
            bow4.ItemDrop.m_itemData.m_shared.m_equipStatusEffect = PiercingShotStatusEffect.StatusEffect;


            ItemConfig bow5config = new ItemConfig();
            bow5config.Requirements = RecipeHelper.ParseRequirements(BossNBowsConfig.YagluthbowRequirements.Value).ToArray();
            bow5config.CraftingStation = BossNBowsConfig.YagluthbowCraftingStation.Value;
            bow5config.RepairStation = BossNBowsConfig.YagluthbowRepairStation.Value;
            var bow5 = new CustomItem(yagluthbowPrefab, fixReference: true, bow5config);
            BossNBows.AttackSpeedSystem.RegisterMainAttacks(yagluthbowPrefab, 2.2f);
            DashOnShootSystem.RegisterDashWeapon(yagluthbowPrefab, 7f);
            ItemManager.Instance.AddItem(bow5);
            bow5.ItemDrop.m_itemData.m_shared.m_equipStatusEffect = FireShotStatusEffect.StatusEffect;


            ItemConfig bow6config = new ItemConfig();
            bow6config.Requirements = RecipeHelper.ParseRequirements(BossNBowsConfig.QueenbowRequirements.Value).ToArray();
            bow6config.CraftingStation = BossNBowsConfig.QueenbowCraftingStation.Value;
            bow6config.RepairStation = BossNBowsConfig.QueenbowRepairStation.Value;
            var bow6 = new CustomItem(queenbowPrefab, fixReference: true, bow6config);
            BossNBows.AttackSpeedSystem.RegisterMainAttacks(queenbowPrefab, 2.2f);
            DashOnShootSystem.RegisterDashWeapon(queenbowPrefab, 7f);
            ItemManager.Instance.AddItem(bow6);
            bow6.ItemDrop.m_itemData.m_shared.m_equipStatusEffect = RegenShotStatusEffect.StatusEffect;


            ItemConfig bow7config = new ItemConfig();
            bow7config.Requirements = RecipeHelper.ParseRequirements(BossNBowsConfig.FaderbowRequirements.Value).ToArray();
            bow7config.CraftingStation = BossNBowsConfig.FaderbowCraftingStation.Value;
            bow7config.RepairStation = BossNBowsConfig.FaderbowRepairStation.Value;
            var bow7 = new CustomItem(faderbowPrefab, fixReference: true, bow7config);
            BossNBows.AttackSpeedSystem.RegisterMainAttacks(faderbowPrefab, 2.2f);
            DashOnShootSystem.RegisterDashWeapon(faderbowPrefab, 7f);
            ItemManager.Instance.AddItem(bow7);
            bow7.ItemDrop.m_itemData.m_shared.m_equipStatusEffect = LightningShotStatusEffect.StatusEffect;
        }


        private void AddStatusEffects()
        {
            StatusEffect effect1 = ScriptableObject.CreateInstance<SE_PiercingShot>();
            effect1.name = "PiercingShot";
            effect1.m_name = "$se_piercingshot";
            effect1.m_tooltip = "$se_piercingshot_tooltip";

            PiercingShotStatusEffect = new CustomStatusEffect(effect1, fixReference: false);  
            ItemManager.Instance.AddStatusEffect(PiercingShotStatusEffect);

            StatusEffect effect2 = ScriptableObject.CreateInstance<SE_RootShot>();
            effect2.name = "RootShot";
            effect2.m_name = "$se_rootshot";
            effect2.m_tooltip = "$se_rootshot_tooltip";
            effect2.m_ttl = 2f;

            RootShotStatusEffect = new CustomStatusEffect(effect2, fixReference: false);  
            ItemManager.Instance.AddStatusEffect(RootShotStatusEffect);

            StatusEffect effect3 = ScriptableObject.CreateInstance<SE_PoisonShot>();
            effect3.name = "PoisonShot";
            effect3.m_name = "$se_poisonshot";
            effect3.m_tooltip = "$se_poisonshot_tooltip";

            PoisonShotStatusEffect = new CustomStatusEffect(effect3, fixReference: false);  
            ItemManager.Instance.AddStatusEffect(PoisonShotStatusEffect);

            StatusEffect effect4 = ScriptableObject.CreateInstance<SE_FireShot>();
            effect4.name = "FireShot";
            effect4.m_name = "$se_fireshot";
            effect4.m_tooltip = "$se_fireshot_tooltip";

            FireShotStatusEffect = new CustomStatusEffect(effect4, fixReference: false);
            ItemManager.Instance.AddStatusEffect(FireShotStatusEffect);

            StatusEffect effect5 = ScriptableObject.CreateInstance<SE_RegenShot>();
            effect5.name = "RegenShot";
            effect5.m_name = "$se_regenshot";
            effect5.m_tooltip = "$se_regenshot_tooltip";

            RegenShotStatusEffect = new CustomStatusEffect(effect5, fixReference: false);
            ItemManager.Instance.AddStatusEffect(RegenShotStatusEffect);

            StatusEffect effect7 = ScriptableObject.CreateInstance<SE_LightningShot>();
            effect7.name = "LightningShot";
            effect7.m_name = "$se_lightningshot";
            effect7.m_tooltip = "$se_lightningshot_tooltip";

            LightningShotStatusEffect = new CustomStatusEffect(effect7, fixReference: false);  
            ItemManager.Instance.AddStatusEffect(LightningShotStatusEffect);
        }

        public class SE_PiercingShot : StatusEffect
        {
            private int shotCount = 0;

            public override void ModifyAttack(Skills.SkillType skill, ref HitData hitData)
            {
                if (skill != Skills.SkillType.Bows)
                    return;

                shotCount++;

                if (shotCount >= 5)
                {
                    shotCount = 0;
                    hitData.m_damage.Modify(2.0f);
                }
            }

            public override void ResetTime()
            {
                base.ResetTime();
                shotCount = 0;
            }
        }

        public class SE_RootShot : SE_Stats
        {
            public override void ModifySpeed(float baseSpeed, ref float speed, Character character, Vector3 dir)
            {
                speed = 0f;
            }

            public override void ModifyJump(Vector3 baseJump, ref Vector3 jump)
            {
                jump = Vector3.zero;
            }
        }

        public class SE_PoisonShot : SE_Stats
        {
            public override void ModifySkillLevel(Skills.SkillType skill, ref float level)
            {
                if (skill == Skills.SkillType.Bows)
                {
                    level += 3f;
                }
            }

            public override void ModifyAttack(Skills.SkillType skill, ref HitData hitData)
            {
                hitData.m_damage.m_pierce *= 1.03f;
            }
        }

        public class SE_FireShot : SE_Stats
        {
            public override void ModifySkillLevel(Skills.SkillType skill, ref float level)
            {
                if (skill == Skills.SkillType.Bows)
                {
                    level += 6f;
                }
            }

            public override void ModifyAttack(Skills.SkillType skill, ref HitData hitData)
            {
                hitData.m_damage.m_slash *= 1.06f;
            }
        }

        public class SE_LightningShot : SE_Stats
        {
            public override void ModifySkillLevel(Skills.SkillType skill, ref float level)
            {
                if (skill == Skills.SkillType.Bows)
                {
                    level += 10f;
                }
            }

            public override void ModifyAttack(Skills.SkillType skill, ref HitData hitData)
            {
                hitData.m_damage.m_pierce *= 1.1f;
            }
        }

        public class SE_RegenShot : StatusEffect
        {
            private int shotCounter = 0;

            public override void ModifyAttack(Skills.SkillType skill, ref HitData hitData)
            {

                if (skill == Skills.SkillType.Bows)
                {
                    shotCounter++;

                    if (shotCounter >= 4)
                    {
                        shotCounter = 0;
                        if (m_character && m_character.IsPlayer())
                        {
                            Player.m_localPlayer.AddStamina(10);                            
                        }
                    }
                }
            }
        }



        [HarmonyPatch(typeof(CharacterAnimEvent), "CustomFixedUpdate")]
        static class DashOnShootSystem
        {
            private static readonly Dictionary<string, float> _data_DashForce = new Dictionary<string, float>();

            public static void RegisterDashWeapon(GameObject weaponPrefab, float dashForce)
            {
                if (weaponPrefab != null && !_data_DashForce.ContainsKey(weaponPrefab.name))
                {
                    _data_DashForce.Add(weaponPrefab.name, dashForce);
                }
            }

            static void Prefix(CharacterAnimEvent __instance)
            {
                Player player = Player.m_localPlayer;
                if (player == null || player != __instance.m_character)
                    return;

                if (!player.InAttack()) 
                    return;

                if (!player.IsOnGround()) 
                    return;

                ItemDrop.ItemData currentWeapon = player.GetCurrentWeapon();
                if (currentWeapon == null)
                    return;

                GameObject weaponPrefab = currentWeapon.m_dropPrefab;
                if (weaponPrefab == null)
                    return;

                if (player.m_seman.HaveStatusEffect("SE_HeavyShot".GetStableHashCode()))
                    return;
                if (EnableDash.Value)
                {
                    if (_data_DashForce.TryGetValue(weaponPrefab.name, out float dashForce))
                    {
                        Rigidbody rb = player.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            Vector3 moveDir = rb.velocity;
                            moveDir.y = 0f;

                            if (moveDir.sqrMagnitude > 0.1f)
                            {
                                moveDir.Normalize();
                                rb.AddForce(moveDir * dashForce, ForceMode.VelocityChange);
                            }

                        }
                    }
                }
            }
        }


        // This one belong to Therzie, thanks! :3
        [HarmonyPatch(typeof(CharacterAnimEvent), "CustomFixedUpdate")]
        private static class AttackSpeedSystem
        {
            public static void RegisterMainAttacks(GameObject item, float speed)
            {
                BossNBows.AttackSpeedSystem._data_Main[item.name] = speed;
            }

            public static void RegisterSecondaryAttacks(GameObject item, float speed)
            {
                BossNBows.AttackSpeedSystem._data_Secondary[item.name] = speed;
            }

            private static void Prefix(CharacterAnimEvent __instance)
            {
                Player localPlayer = Player.m_localPlayer;
                bool flag = localPlayer != __instance.m_character;

                if (!flag)
                {
                    bool flag2 = !localPlayer.InAttack();
                    if (!flag2)
                    {
                        ItemDrop.ItemData currentWeapon = localPlayer.GetCurrentWeapon();
                        GameObject gameObject = ((currentWeapon != null) ? currentWeapon.m_dropPrefab : null);
                        float num = 0f;
                        bool flag3 = gameObject && BossNBows.AttackSpeedSystem._data_Main.TryGetValue(gameObject.name, out num) && !localPlayer.m_currentAttackIsSecondary;
                        if (flag3)
                        {
                            __instance.m_animator.speed = num;
                        }
                        else
                        {
                            bool flag4 = gameObject && BossNBows.AttackSpeedSystem._data_Secondary.TryGetValue(gameObject.name, out num) && localPlayer.m_currentAttackIsSecondary;
                            if (flag4)
                            {
                                __instance.m_animator.speed = num;
                            }
                        }
                    }
                }
            }

            private static readonly Dictionary<string, float> _data_Main = new Dictionary<string, float>();

            private static readonly Dictionary<string, float> _data_Secondary = new Dictionary<string, float>();
        }
        private void AddLocalizations()
        {
            var localization = LocalizationManager.Instance.GetLocalization();

            string basePath = System.IO.Path.Combine(BepInEx.Paths.ConfigPath, "BossNBowsTranslation");

            if (!System.IO.Directory.Exists(basePath))
            {
                Jotunn.Logger.LogWarning($"Localization directory not found: {basePath}");
                return;
            }

            foreach (var file in System.IO.Directory.GetFiles(basePath, "*.json"))
            {
                string language = System.IO.Path.GetFileNameWithoutExtension(file);
                try
                {
                    string jsonContent = System.IO.File.ReadAllText(file);
                    var translations = SimpleJson.SimpleJson.DeserializeObject<Dictionary<string, string>>(jsonContent);

                    foreach (var entry in translations)
                    {
                        localization.AddTranslation(language, entry.Key, entry.Value);
                    }

                    Jotunn.Logger.LogInfo($"Loaded localization for {language}");
                }
                catch (Exception ex)
                {
                    Jotunn.Logger.LogError($"Failed to load localization from {file}: {ex.Message}");
                }
            }
        }

        public enum BowFireMode { Normal, Fast, Heavy, Turret }

        public static class BowFireModeManager
        {
            public static BowFireMode CurrentMode = BowFireMode.Normal;

            public static void CycleMode(Player player)
            {
                if (PlayerUtils.HasBowEquipped(player))
                {
                    CurrentMode = (BowFireMode)(((int)CurrentMode + 1) % 4);
                }
            }

            public static void ResetMode(Player player)
            {
                CurrentMode = BowFireMode.Normal;

                if (player?.m_seman != null)
                {
                    player.m_seman.RemoveStatusEffect(StatusEffectManager.FastShot.name.GetStableHashCode());
                    player.m_seman.RemoveStatusEffect(StatusEffectManager.TurretShot.name.GetStableHashCode());
                    player.m_seman.RemoveStatusEffect(StatusEffectManager.HeavyShot.name.GetStableHashCode());
                }
            }
        }

        public static class PlayerUtils
        {
            public static bool HasBowEquipped(Player player)
            {
                if (player == null) return false;

                var equipped = player.GetInventory().GetEquippedItems();

                foreach (var item in equipped)
                {
                    if (item != null && item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Bow)
                    {
                        return true;
                    }
                }

                return false;
            }

            
        }

        private static bool IsKeyDown(KeyboardShortcut shortcut)
        {
            if (shortcut.Equals(default)) return false;
            if (!Input.GetKeyDown(shortcut.MainKey)) return false;

            foreach (var mod in shortcut.Modifiers)
            {
                if (!Input.GetKey(mod)) return false;
            }

            return true;
        }


        [HarmonyPatch(typeof(Player), nameof(Player.Update))]
        public static class BowModeInputPatch
        {
            private static FieldInfo m_inputFieldInfo = typeof(Chat).GetField("m_input", BindingFlags.NonPublic | BindingFlags.Instance);

            private static bool IsChatFocused()
            {
                var chat = Chat.instance;
                if (chat == null || m_inputFieldInfo == null) return false;

                var inputField = m_inputFieldInfo.GetValue(chat);
                if (inputField is InputField ui) return ui.isFocused;
                if (inputField is TMP_InputField tmp) return tmp.isFocused;

                return false;
            }

            public static void Postfix(Player __instance)
            {
                if (__instance != Player.m_localPlayer) return;
                if (IsChatFocused()) return;

                if (!PlayerUtils.HasBowEquipped(__instance))
                {
                    BowFireModeManager.ResetMode(__instance);
                    return;
                }

                if (IsKeyDown(BossNBows.BowModeKey.Value))
                {
                    BowFireModeManager.CycleMode(__instance);

                    string modeName = BowFireModeManager.CurrentMode.ToString();
                    __instance.Message(
                        MessageHud.MessageType.Center,
                        $"<color=orange>Bow Mode :</color> {modeName}"
                    );
                }

                ApplyBowFireModeEffect(__instance);
            }

            private static void ApplyBowFireModeEffect(Player player)
            {
                var seman = player.m_seman;
                seman.RemoveStatusEffect(StatusEffectManager.FastShot.name.GetStableHashCode());
                seman.RemoveStatusEffect(StatusEffectManager.TurretShot.name.GetStableHashCode());
                seman.RemoveStatusEffect(StatusEffectManager.HeavyShot.name.GetStableHashCode());

                switch (BowFireModeManager.CurrentMode)
                {
                    case BowFireMode.Fast:
                        seman.AddStatusEffect(StatusEffectManager.FastShot);
                        break;
                    case BowFireMode.Heavy:
                        seman.AddStatusEffect(StatusEffectManager.HeavyShot);
                        break;
                    case BowFireMode.Turret:
                        seman.AddStatusEffect(StatusEffectManager.TurretShot);
                        break;
                }
            }
        }

        public class SE_FastShot : StatusEffect
        {
            public SE_FastShot()
            {
                m_name = "SE_FastShot";
            }

            public override void ModifySpeed(float baseSpeed, ref float speed, Character character, Vector3 dir)
            {
                if (character is Player)
                {
                    speed = baseSpeed * 1.1f;
                }
            }

            public override void ModifyAttack(Skills.SkillType skill, ref HitData hitData)
            {
                if (skill == Skills.SkillType.Bows)
                {
                    hitData.m_damage.Modify(0.8f);
                }
            }
        }

        public class SE_TurretShot : StatusEffect
        {
            public SE_TurretShot()
            {
                m_name = "SE_TurretShot";
                m_ttl = 0;
            }

            public override void ModifySpeed(float baseSpeed, ref float speed, Character character, Vector3 dir)
            {
                speed = 0f;
            }

            public override void ModifyJump(Vector3 baseJump, ref Vector3 jump)
            {
                jump = Vector3.zero;
            }

            public override void UpdateStatusEffect(float dt)
            {
                base.UpdateStatusEffect(dt);

                if (m_character is Player player && player == Player.m_localPlayer)
                {
                    player.m_run = false;
                    player.m_autoRun = false;
                }
            }

            public override void ModifyAttack(Skills.SkillType skill, ref HitData hitData)
            {
                if (skill == Skills.SkillType.Bows)
                {
                    hitData.m_damage.Modify(1.5f);
                }
            }
        }

        public class SE_HeavyShot : StatusEffect
        {
            public SE_HeavyShot()
            {
                m_name = "SE_HeavyShot";
            }

            public override void ModifySpeed(float baseSpeed, ref float speed, Character character, Vector3 dir)
            {
                speed = baseSpeed * 0.7f;
            }

            public override void ModifyAttack(Skills.SkillType skill, ref HitData hitData)
            {
                if (skill == Skills.SkillType.Bows)
                {
                    hitData.m_damage.Modify(1.25f);
                }
            }
        }

        public static class StatusEffectManager
        {
            public static SE_FastShot FastShot = ScriptableObject.CreateInstance<SE_FastShot>();
            public static SE_HeavyShot HeavyShot = ScriptableObject.CreateInstance<SE_HeavyShot>();
            public static SE_TurretShot TurretShot = ScriptableObject.CreateInstance<SE_TurretShot>();

            static StatusEffectManager()
            {
                FastShot.name = "SE_FastShot";
                HeavyShot.name = "SE_HeavyShot";
                TurretShot.name = "SE_TurretShot";
            }
        }
    }
}

