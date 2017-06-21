using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Linq;

[assembly: AssemblyTitle("Michael's Hat Pack")]
[assembly: AssemblyCompany("Edopad")]
[assembly: AssemblyDescription("There's a hat for that!")]
//"Duck Game but with memes instead of hats!"
[assembly: AssemblyVersion("1.3.7.1")]



namespace DuckGame.EdoMod
{
    public class EdoMod : Mod
    {
        internal static EdoMain Main { get; private set; }
        internal static string AssemblyName { get; private set; }

        // This function is run before all mods are finished loading.
        protected override void OnPreInitialize()
        {
            /* uncomment for blacklist
            if (Array.Exists(Blacklist, e => e == Steam.user.id))
                System.Environment.Exit(0);
            */
            ModSettings.init();
            base.OnPreInitialize();
        }

        // This function is run after all mods are loaded.
        protected override void OnPostInitialize()
        {

            //TODO:
            //add Thomas the Train hat
            //CUSTOMIZE groot! hat
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
            Teams.core.teams.Add(new Team("Groot", GetPath<EdoMod>("hats\\groot")));
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


            //Add dynamic hats!

            //Heavy Rain Glitch ("Press X to Shaun!")
            ShaunHat.addHat();
            ShaunMPHat.addHat();
            //Woody the Woodpecker laugh
            HahaHat.addHat();
            //noot!
            NootHat.addHat();
            //airhorn. pretty self-explanitory.
            AirHat.addHat();
            //all star but everything is a shed
            ShedHat.addHat();
            //Hitmarker
            HitmarkHat.addHat();
            //"Smoke Weed Everyday"
            WeedHat.addHat();
            
            //[[Censored]]
            CensoredHat.addHat();
            //Just a firetruck.
            FiretruckHat.addHat();
            //Finding Nemo: "Mine!" Seagulls
            GullHat.addHat();
            //Marvelous Breadfish
            BreadfishHat.addHat();
            //Shia's inspirational speech
            DoItHat.addHat();
            //Jar Jar Binks, "Oh no!" -Star Wars
            JarJarHat.addHat();

            //Special Hats! These do things!
            //NO!
            NoHat.addHat();
            //Bomb!
            BombHat.addHat();
            //CHKN
            ChickenHat.addHat();
            //Ditto from Pokemon
            DittoHat.addHat();
            //asdf movie: Pie Flavor!
            PiesHat.addHat();
            //Fidget Spinners
            FinnerHat.addHat();

            //Developer Hats
            if ((ModSettings.isDeveloper || ModSettings.isFriend) && ModSettings.enableDevHats)
            {
                //DENIED Hat
                DenHat.addHat();
                //Milk Hat
                MilkHat.addHat();

                UpsideHat.addHat();
            }

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
