﻿using DuckGame.EdoMod.src.core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;

[assembly: AssemblyTitle("Michael's Hat Pack")]
[assembly: AssemblyCompany("Edopad")]
[assembly: AssemblyDescription("I put spongebob music over Duck Game!")]
//[1.5.0.0] "I put spongebob music over Duck Game!"
//[1.0.0.0] "There's a hat for that!"
//[0.0.0.0] "Duck Game but with memes instead of hats!"
[assembly: AssemblyVersion("1.6.3.0")]

namespace DuckGame.EdoMod
{
    public class EdoMod : Mod
    {
        internal static EdoMain Main { get; private set; }
        internal static string AssemblyName { get; private set; }
        public EventHandler Load { get; private set; }

        private static void DoDangerousInjections()
        {
            {
                MethodInfo orig = typeof(EdoDuck).GetMethod("_UpdateMove", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                MethodInfo newer = typeof(Duck).GetMethod("UpdateMove", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                DynamicMojo.SwapMethodBodies(newer, orig);
            }
            {
                MethodInfo orig = typeof(Duck).GetMethod("UpdateMove", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                MethodInfo newer = typeof(EdoDuck).GetMethod("UpdateMove", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                DynamicMojo.SwapMethodBodies(newer, orig);
            }
        }

        // This function is run before all mods are finished loading.
        protected override void OnPreInitialize()
        {
            /* uncomment for blacklist
            if (Array.Exists(Blacklist, e => e == Steam.user.id))
                System.Environment.Exit(0);
            */
            ModSettings.init();
            FriendManager.init();

            //UNSAFE! HA! - allows quacking in base TeamHat class to support right trigger for low pitch
            if(ModSettings.enableDangerousInjections)
            {
                MethodInfo orig = typeof(Duck).GetMethod("UpdateQuack", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                MethodInfo newer = typeof(EdoDuck).GetMethod("UpdateQuack", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                DynamicMojo.SwapMethodBodies(newer, orig);
            }
            if (ModSettings.enableDangerousInjections) DoDangerousInjections();
            
            //var sfxdict = typeof(SFX).GetField("_sounds", BindingFlags.Static | BindingFlags.NonPublic);
            /*FieldInfo sounds = typeof(SFX).GetField("_sounds", BindingFlags.Static | BindingFlags.NonPublic);
            Dictionary<string, SoundEffect> _sounds = ( Dictionary < string, SoundEffect> ) sounds.GetValue(null);
            _sounds["shotgunFire2"] = _sounds[GetPath<EdoMod>("SFX\\humpday")];
            SFX.Play("shotgunFire2");*/

            base.OnPreInitialize();
        }

        void HandleCustomEvent(object sender)
        {
            // Do something useful here.  
        }

        // This function is run after all mods are loaded.
        protected override void OnPostInitialize()
        {
            /*FieldInfo sounds = typeof(SFX).GetField("_sounds", BindingFlags.Static | BindingFlags.NonPublic);
            Dictionary<string, SoundEffect> _sounds = (Dictionary<string, SoundEffect>)sounds.GetValue(null);
            _sounds["shotgunFire2"] = _sounds[GetPath<EdoMod>("SFX\\humpday")];
            SFX.Play("shotgunFire2");*/

            //TODO:
            //add Thomas the Train hat
            //CUSTOMIZE groot! hat CHECK
            //add nayan hat (full)
            //add traffic cone hat, "VLC"
            //[?] super long subway hat

            //Add static hats!
            //Trolololol
            Teams.core.teams.Add(new Team("Trolls", GetPath<EdoMod>("hats\\trollface")));
            //Minecraft
            Teams.core.teams.Add(new Team("Apples", GetPath<EdoMod>("hats\\apples")));
            //Peanut Butter Jelly Time!
            Teams.core.teams.Add(new Team("PBJ Time", GetPath<EdoMod>("hats\\pbjbanana")));
            //asdf movie
            Teams.core.teams.Add(new Team("Cows", GetPath<EdoMod>("hats\\burger")));
            Teams.core.teams.Add(new Team("Pineapples", GetPath<EdoMod>("hats\\pineapple")));
            //Teams.core.teams.Add(new Team("Pies", GetPath<EdoMod>("hats\\pies")));
            //Movie: "Rubber"
            Teams.core.teams.Add(new Team("Rubber", GetPath<EdoMod>("hats\\rubber")));
            //Guardians of the Galaxy
            //Teams.core.teams.Add(new Team("Groot", GetPath<EdoMod>("hats\\groot")));
            //2016/2017 U.S. Presidential Election
            Teams.core.teams.Add(new Team("Trillary", GetPath<EdoMod>("hats\\trillary")));
            //[[Redacted]]
            Teams.core.teams.Add(new Team("Reduckted", GetPath<EdoMod>("hats\\censored_2")));
            //NTSC/PAL Test Pattern
            Teams.core.teams.Add(new Team("Technical Difficulties", GetPath<EdoMod>("hats\\tvtest")));
            //Grumpy Cat
            Teams.core.teams.Add(new Team("Grumpy", GetPath<EdoMod>("hats\\grumpycat")));
            //Schrödinger's Cat
            Teams.core.teams.Add(new Team("Schrodinger's Hat", GetPath<EdoMod>("hats\\schrodinger")));
            //MissingNo from Pokemon. Duck Game style.
            Teams.core.teams.Add(new Team("MissingNo", GetPath<EdoMod>("hats\\missingno")));
            //Nationwide's logo looks like they forgot to make one.
            Teams.core.teams.Add(new Team("Nationwide", GetPath<EdoMod>("hats\\nationwide")));

            //NEW to 1.3.8.4
            //Sarah. "GET OFF THE ROAD BIKERS!"
            //Teams.core.teams.Add(new Team("Sarah", GetPath<EdoMod>("hats\\sarah")));
            //Hammerhead shark. Self explanatory.
            Teams.core.teams.Add(new Team("Hammerhead", GetPath<EdoMod>("hats\\hammerhead")));
            //Flipping amazing.
            Teams.core.teams.Add(new Team("Finger", GetPath<EdoMod>("hats\\middlefinger")));
            //Macro- and Micro-wave AP Testing this spring!
            Teams.core.teams.Add(new Team("Microwave", GetPath<EdoMod>("hats\\microwave")));
            //Duck Game's sprite. Is this copyright infringement???
            Teams.core.teams.Add(new Team("XBOX", GetPath<EdoMod>("hats\\x360")));
            //Aku, from Samurai Jack -- Poorly made atm (1.3.8.4)
            Teams.core.teams.Add(new Team("Aku", GetPath<EdoMod>("hats\\aku")));
            
            Teams.core.teams.Add(new Team("Spaze Soot", GetPath<EdoMod>("hats\\spacebubble")));
            Teams.core.teams.Add(new Team("Gumballs", GetPath<EdoMod>("hats\\gumballduck")));
            Teams.core.teams.Add(new Team("Mutants", GetPath<EdoMod>("hats\\duomutant")));
            Teams.core.teams.Add(new Team("Sheep", GetPath<EdoMod>("hats\\sheep")));
            //Teams.core.teams.Add(new Team(, GetPath<EdoMod>()));
            new TurbanData("Rainbows", "hats\\rainbows", GetPath<EdoMod>("SFX\\hats\\Rainbows_long_AMP"));
            Teams.core.teams.Add(new Team("Google", GetPath<EdoMod>("hats\\google")));

            new TurbanData("Bigmouth", "hats\\unu1", GetPath<EdoMod>("SFX\\waa"));
            //TODO Implement another way to NetQuack (extended version) where it is given a percentage random chance for different SFX

            //Hump Day! GEICO commercial
            new TurbanData("Humps", "hats\\humpday", GetPath<EdoMod>("SFX\\humpday"));
            //PewDiePie Hat!
            new TurbanData("PewDiePie", "hats\\pewdiepie", GetPath<EdoMod>("SFX\\PewDiePie\\intro_1"));
            //Staples Button: "That was easy."
            new TurbanData("Staples", "hats\\easy", GetPath<EdoMod>("SFX\\thatwaseasy"));
            //The Turban Test Duck Hat
            if(ModSettings.enableDevHats)
                new TurbanData("Test Turban", "hats\\testduck", GetPath<EdoMod>("SFX\\airhorn_long"), FriendLevel.Dev);
            //You've got Mail! AOL sound effect
            new TurbanData("Mail", "hats\\mailbox", GetPath<EdoMod>("SFX\\youve_got_mail"));

            //Migratory Hats

            //Woody the Woodpecker laugh
            new TurbanData("Woody", "hats\\woody", GetPath<EdoMod>("SFX\\hahaha"));
            //noot!
            new TurbanData("Noot", "hats\\noot", GetPath<EdoMod>("SFX\\noot"));
            //airhorn. pretty self-explanitory.
            new TurbanData("Noisy", "hats\\airhorn", GetPath<EdoMod>("SFX\\airhorn_long"));
            //all star but everything is a shed
            new TurbanData("Smash Mouth", "hats\\shed", GetPath<EdoMod>("SFX\\shed"));
            //Hitmarker
            new TurbanData("Noscope", "hats\\hitmark", GetPath<EdoMod>("SFX\\hitmarker"));
            //"Smoke Weed Everyday"
            new TurbanData("Weed", "hats\\weed", GetPath<EdoMod>("SFX\\smokeweedeveryday"));
            //Just a firetruck.
            new TurbanData("Fire", "hats\\firetruck", GetPath<EdoMod>("SFX\\firehorn_short"));
            //Finding Nemo: "Mine!" Seagulls
            new TurbanData("Gulls", "hats\\gull", GetPath<EdoMod>("SFX\\hats\\Mine_2"));
            //Jar Jar Binks, "Oh no!" -Star Wars
            new TurbanData("Binks", "hats\\jarjar", GetPath<EdoMod>("SFX\\ohno"));
            //Guardians of the Galaxy : Groot
            new TurbanData("Groot", "hats\\groot", new[] {
                GetPath<EdoMod>("SFX\\hats\\Groot\\IAmGroot1"),
                GetPath<EdoMod>("SFX\\hats\\Groot\\IAmGroot2")
            });
            //MGS Alert
            new TurbanData("MGS", "hats\\mgs", GetPath<EdoMod>("SFX\\mgs"));
            //Show Me What You Got! - Rick and Morty
            new TurbanData("Rocks", "hats\\ShowMeWhatYouGot", GetPath<EdoMod>("SFX\\hats\\ShowMeWhatYouGot"));

            //Heavy Rain Glitch ("Press X to Shaun!")
            /*string[] quacks = {
                Mod.GetPath<EdoMod>("SFX\\shaun1"),
                Mod.GetPath<EdoMod>("SFX\\shaun2"),
                Mod.GetPath<EdoMod>("SFX\\shaun3")
            };*/
            new TurbanData("Shaun", "hats\\shaun", new[] {
                GetPath<EdoMod>("SFX\\shaun1"),
                GetPath<EdoMod>("SFX\\shaun2"),
                GetPath<EdoMod>("SFX\\shaun3")
            });

            //Shia LaBeouf's inspirational speech
            new TurbanData("Shia", "hats\\LaBeouf",
            new[] {
                GetPath<EdoMod>("SFX\\hats\\Shia\\YesYouCan"),
                GetPath<EdoMod>("SFX\\hats\\Shia\\DontLetYourDreamsBeDreams"),
                GetPath<EdoMod>("SFX\\hats\\Shia\\JustDoIt"),
                GetPath<EdoMod>("SFX\\hats\\Shia\\JustDoIt_2"),
                GetPath<EdoMod>("SFX\\hats\\Shia\\No"),
                GetPath<EdoMod>("SFX\\hats\\Shia\\NothingIsImpossible"),
                GetPath<EdoMod>("SFX\\hats\\Shia\\StopGivingUp"),
                GetPath<EdoMod>("SFX\\hats\\Shia\\DoIt")
            });

            new TurbanData("JG Wentworth", "hats\\money", new[] {
                GetPath<EdoMod>("SFX\\hats\\JGWentworth\\IMMAININ_1"),
                GetPath<EdoMod>("SFX\\hats\\JGWentworth\\IMMAININ_2"),
                GetPath<EdoMod>("SFX\\hats\\JGWentworth\\IMMAININ_3"),
                GetPath<EdoMod>("SFX\\hats\\JGWentworth\\IMMAININ_4"),
                GetPath<EdoMod>("SFX\\hats\\JGWentworth\\IMMAININ_5")
            });

            new TurbanData("Trump", "hats\\trump", new[] {
                GetPath<EdoMod>("SFX\\hats\\Trump\\ImReallyRich"),
                GetPath<EdoMod>("SFX\\hats\\Trump\\WeNeedToBuildAWall"),
                GetPath<EdoMod>("SFX\\hats\\Trump\\YoureFired")
            });

            //Special Hats! These do things!

            //[[Censored]]
            CensoredHat.addHat();
            //Have you ever seen the marvelous breadfish?
            BreadfishHat.addHat();
            //Annoying Google Ad.
            AdsHat.addHat();
            //Annnddd IIIIIIIIIIIIIIIIIIIIIIIIIIIIII
            AndIHat.addHat();

            //NO! (mutes other hats via brute force)
            NoHat.addHat();
            //Bomb!
            BombHat.addHat();
            //CHKN
            ChickenHat.addHat();
            //Ditto from Pokemon
            DittoHat.addHat();
            //asdf movie: Pie Flavor!
            PiesHat.addHat();
            //DJ Hat - changes music paylist!
            DJHat.addHat();

            //Developer Hats
            if (FriendManager.canuse(FriendLevel.Tester) && ModSettings.enableDevHats)
            {
                //Fidget Spinners
                FinnerHat.addHat();
                //DENIED Hat
                DenHat.addHat();
                //Milk Hat
                MilkHat.addHat();
                //Supposed to invert (vertically) the appearance of the duck.
                UpsideHat.addHat();
            }
            //Jar Jar Binks, "Oh no!" -Star Wars
            //new TurbanData("Lasers", "weapons\\firinMyLaser", GetPath<EdoMod>("SFX\\hats\\ImFirinMyLaser"));
            //FirinMyLaser.addHat();

            base.OnPostInitialize();

            Thread thread = new Thread(ExecuteOnceLoaded);
            thread.Start();
        }
        
        private void ExecuteOnceLoaded()
        {
            if(ModSettings.enableCustomIntro)
            {
                while (!(Level.current is EdoLogo) && !(Level.current is TeamSelect2))
                {
                    if (Level.current is CorptronLogo) Level.current = new EdoLogo();
                    Thread.Sleep(20);
                }
            }
            
            while (Level.current == null
                    || !(Level.current.ToString() == "DuckGame.TitleScreen"
                    || Level.current.ToString() == "DuckGame.TeamSelect2"))
                Thread.Sleep(200);

            Main = new EdoMain();
        }
    }
}
