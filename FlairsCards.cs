using BepInEx;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using ClassesManagerReborn.Util;
using ClassesManagerReborn;
using FlairsCards.Cards;
using HarmonyLib;
using ModsPlus;
using PSA.Extensions;
using RarityLib.Utils;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using UnityEngine.Profiling;
using WillsWackyManagers.Utils;
using Jotunn.Utils;

namespace FlairsCards
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.classes.manager.reborn", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.willis.rounds.modsplus", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.Poppycars.PSA.Id", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.willuwontu.rounds.managers", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.root.projectile.size.patch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class FlairsCards : BaseUnityPlugin
    {
        private const string ModId = "com.Flair.Mod.FlairsCards";
        private const string ModName = "Flairs Cards";
        public const string Version = "0.2.0";
        public const string ModInitials = "FC";
        public const string CursedModInitials = "FC Curse";
        public static FlairsCards? Instance { get; private set; }

        private static readonly AssetBundle Bundle = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("cardbundle", typeof(FlairsCards).Assembly);

        // Accursed Art
        public static GameObject CardArtAccursed = Bundle.LoadAsset<GameObject>("C_ACCURSED");
        public static GameObject CardArtCursedDraw = Bundle.LoadAsset<GameObject>("C_CURSEDDRAW");
        public static GameObject CardArtFallenAngel = Bundle.LoadAsset<GameObject>("C_FALLENANGEL");
        public static GameObject CardArtCondemnation = Bundle.LoadAsset<GameObject>("C_PLAGUEBEARER");
        public static GameObject CardArtPrayer = Bundle.LoadAsset<GameObject>("C_PRAYER");
        public static GameObject CardArtUnholyCurse = Bundle.LoadAsset<GameObject>("C_UNHOLYCURSE");
        public static GameObject CardArtUnluckySouls = Bundle.LoadAsset<GameObject>("C_UNLUCKYSOULS");

        // Blocker Art
        public static GameObject CardArtBlocker = Bundle.LoadAsset<GameObject>("C_BLOCKER");
        public static GameObject CardArtControlFreak = Bundle.LoadAsset<GameObject>("C_CONTROLFREAK");
        public static GameObject CardArtOvercharged = Bundle.LoadAsset<GameObject>("C_OVERCHARGED");
        public static GameObject CardArtOverloading = Bundle.LoadAsset<GameObject>("C_OVERLOADING");
        public static GameObject CardArtRewind = Bundle.LoadAsset<GameObject>("C_REWIND");
        public static GameObject CardArtSelfSacrifice = Bundle.LoadAsset<GameObject>("C_SELFSACRIFICE");
        public static GameObject CardArtTerminalVelocity = Bundle.LoadAsset<GameObject>("C_TERMINALVELOCITY");

        // Gambler Art
        public static GameObject CardArtRoulette = Bundle.LoadAsset<GameObject>("C_BLACKJACK");
        public static GameObject CardArtCoinflip = Bundle.LoadAsset<GameObject>("C_COINFLIP");
        public static GameObject CardArtCurseAverse = Bundle.LoadAsset<GameObject>("C_CURSEAVERSE");
        public static GameObject CardArtGambler = Bundle.LoadAsset<GameObject>("C_GAMBLER");
        public static GameObject CardArtLuckyBuff = Bundle.LoadAsset<GameObject>("C_LUCKYBUFF");
        public static GameObject CardArtNaturalLuck = Bundle.LoadAsset<GameObject>("C_NATURALLUCK");
        public static GameObject CardArtNeutral = Bundle.LoadAsset<GameObject>("C_NEUTRAL");
        public static GameObject CardArtWildcard = Bundle.LoadAsset<GameObject>("C_WILDCARD");

        // Normal Art
        public static GameObject CardArtCannonball = Bundle.LoadAsset<GameObject>("C_CANNONBALL");

        // Royalty Art
        public static GameObject CardArtArrogance = Bundle.LoadAsset<GameObject>("C_ARROGANCE");
        public static GameObject CardArtKinghood = Bundle.LoadAsset<GameObject>("C_KINGHOOD");
        public static GameObject CardArtPersonalBodyguard = Bundle.LoadAsset<GameObject>("C_PERSONALBODYGUARD");
        public static GameObject CardArtRoyalty = Bundle.LoadAsset<GameObject>("C_ROYALTY");
        public static GameObject CardArtTaxCut = Bundle.LoadAsset<GameObject>("C_TAXCUT");

        // Speedster Art
        public static GameObject CardArtAdrenaline = Bundle.LoadAsset<GameObject>("C_ADRENALINE");
        public static GameObject CardArtCantTouchThis = Bundle.LoadAsset<GameObject>("C_CANTTOUCHTHIS");
        public static GameObject CardArtEnergyConverter = Bundle.LoadAsset<GameObject>("C_ENERGYCONVERTER");
        public static GameObject CardArtEnergyDrink = Bundle.LoadAsset<GameObject>("C_ENERGYDRINK");
        public static GameObject CardArtSpeedster = Bundle.LoadAsset<GameObject>("C_SPEEDSTER");
        public static GameObject CardArtSupersonicCannon = Bundle.LoadAsset<GameObject>("C_SUPERSONICCANNON");
        void Awake()
        {
            RarityUtils.AddRarity("CommonClass", 1.5f, new Color(0.0978f, 0.1088f, 0.1321f), new Color(0.0978f, 0.1088f, 0.1321f));
            RarityUtils.AddRarity("UncommonClass", 0.8f, new Color(0.1745f, 0.6782f, 1f), new Color(0.1934f, 0.3915f, 0.5189f));
            RarityUtils.AddRarity("RareClass", 0.3f, new Color(1f, 0.1765f, 0.7567f), new Color(0.5283f, 0.1969f, 0.4321f));
            new Harmony(ModId).PatchAll();
        }
        void Start()
        {
            Instance = this;
            // Normal Cards
            CustomCard.BuildCard<Cannonball>();

            // Curse Cards
            CustomCard.BuildCard<BattleScarred>(cardInfo => { CurseManager.instance.RegisterCurse(cardInfo); });
            CustomCard.BuildCard<BlindingSpeed>(cardInfo => { CurseManager.instance.RegisterCurse(cardInfo); });
            CustomCard.BuildCard<BucklingPressure>(cardInfo => { CurseManager.instance.RegisterCurse(cardInfo); });
            CustomCard.BuildCard<ClumsyFingers>(cardInfo => { CurseManager.instance.RegisterCurse(cardInfo); });
            CustomCard.BuildCard<Diseased>(cardInfo => { CurseManager.instance.RegisterCurse(cardInfo); });
            CustomCard.BuildCard<RandomDebuff>(cardInfo => { CurseManager.instance.RegisterCurse(cardInfo); });
            CustomCard.BuildCard<WeakenedWill>(cardInfo => { CurseManager.instance.RegisterCurse(cardInfo); }); 

            // Accursed Class
            CustomCard.BuildCard<Accursed>((card) => Accursed.Card = card);
            CustomCard.BuildCard<CursedDraw>((card) => CursedDraw.Card = card);
            CustomCard.BuildCard<FallenAngel>((card) => FallenAngel.Card = card);
            CustomCard.BuildCard<Condemnation>((card) => Condemnation.Card = card);
            CustomCard.BuildCard<Prayer>((card) => Prayer.Card = card);
            CustomCard.BuildCard<UnholyCurse>((card) => UnholyCurse.Card = card);
            CustomCard.BuildCard<UnluckySouls>((card) => UnluckySouls.Card = card);

            // Blocker Class
            CustomCard.BuildCard<Blocker>((card) => Blocker.Card = card);
            CustomCard.BuildCard<ControlFreak>((card) => ControlFreak.Card = card);
            CustomCard.BuildCard<Overcharged>((card) => Overcharged.Card = card);
            CustomCard.BuildCard<Overloading>((card) => Overloading.Card = card);
            CustomCard.BuildCard<Rewind>((card) => Rewind.Card = card);
            CustomCard.BuildCard<SelfSacrifice>((card) => SelfSacrifice.Card = card);
            CustomCard.BuildCard<TerminalVelocity>((card) => TerminalVelocity.Card = card);

            // Gambler Class
            CustomCard.BuildCard<Coinflip>((card) => Coinflip.Card = card);
            CustomCard.BuildCard<CurseAverse>((card) => CurseAverse.Card = card);
            CustomCard.BuildCard<Gambler>((card) => Gambler.Card = card);
            CustomCard.BuildCard<LuckyBuff>((card) => LuckyBuff.Card = card);
            CustomCard.BuildCard<NaturalLuck>((card) => NaturalLuck.Card = card);
            CustomCard.BuildCard<Neutral>((card) => Neutral.Card = card);
            //CustomCard.BuildCard<Roulette>((card) => Roulette.Card = card);
            CustomCard.BuildCard<Wildcard>((card) => Wildcard.Card = card);

            // Royalty Class
            CustomCard.BuildCard<Arrogance>((card) => Arrogance.Card = card);
            CustomCard.BuildCard<PersonalBodyguard>((card) => PersonalBodyguard.Card = card);
            CustomCard.BuildCard<Revenge>((card) => Revenge.Card = card);
            CustomCard.BuildCard<Royalty>((card) => Royalty.Card = card);
            CustomCard.BuildCard<TaxCut>((card) => TaxCut.Card = card);

            // Speedster Class
            CustomCard.BuildCard<Adrenaline>((card) => Adrenaline.Card = card);
            CustomCard.BuildCard<CantTouchThis>((card) => CantTouchThis.Card = card);
            CustomCard.BuildCard<EnergyConverter>((card) => EnergyConverter.Card = card);
            CustomCard.BuildCard<EnergyDrink>((card) => EnergyDrink.Card = card);
            CustomCard.BuildCard<Speedster>((card) => Speedster.Card = card);
            CustomCard.BuildCard<SupersonicCannon>((card) => SupersonicCannon.Card = card);
        }
    }
}