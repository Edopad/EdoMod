using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame.EdoMod
{

    internal class EdoMain : IAutoUpdate
    {
        private IDictionary<TeamHat, Cape> teamSpawnsDone;

        public static EdoMain instance;

        public EdoMain()
        {
            teamSpawnsDone = new Dictionary<TeamHat, Cape>();
            AutoUpdatables.Add(this);
            instance = this;
        }

        public void Update()
        {
            if (Level.current == null
                || Steam.user == null)
                return;

            if (!(Level.current is Editor))
            {
                IEnumerable<Thing> teamHatList = Level.current.things[typeof(TeamHat)];
                //int nhats = 0;
                foreach (TeamHat teamHat in teamHatList)
                {
                    //nhats++;
                    if (teamHat.team != null && teamHat.team.customData == null && teamHat.team.hasHat)
                    {
                        //Normal Hats
                        if (ShaunHat.isHat(teamHat))
                            ReplaceHat(teamHat, new ShaunHat(teamHat.x, teamHat.y, teamHat.team));
                        if (CensoredHat.isHat(teamHat))
                            ReplaceHat(teamHat, new CensoredHat(teamHat.x, teamHat.y, teamHat.team));
                        if (BreadfishHat.isHat(teamHat))
                            ReplaceHat(teamHat, new BreadfishHat(teamHat.x, teamHat.y, teamHat.team));
                        if (DoItHat.isHat(teamHat))
                            ReplaceHat(teamHat, new DoItHat(teamHat.x, teamHat.y, teamHat.team));
                        //Special Hats
                        if (NoHat.isHat(teamHat))
                            ReplaceHat(teamHat, new NoHat(teamHat.x, teamHat.y, teamHat.team));
                        if (DittoHat.isHat(teamHat))
                            ReplaceHat(teamHat, new DittoHat(teamHat.x, teamHat.y, teamHat.team));
                        if (BombHat.isHat(teamHat))
                            ReplaceHat(teamHat, new BombHat(teamHat.x, teamHat.y, teamHat.team));
                        if (ChickenHat.isHat(teamHat))
                            ReplaceHat(teamHat, new ChickenHat(teamHat.x, teamHat.y, teamHat.team));
                        if (PiesHat.isHat(teamHat))
                            ReplaceHat(teamHat, new PiesHat(teamHat.x, teamHat.y, teamHat.team));
                        if (FinnerHat.isHat(teamHat))
                            ReplaceHat(teamHat, new FinnerHat(teamHat.x, teamHat.y, teamHat.team));
                        if (EasyHat.isHat(teamHat))
                            ReplaceHat(teamHat, new EasyHat(teamHat.x, teamHat.y, teamHat.team));
                        //Developer Hats
                        if (DenHat.isHat(teamHat))
                            ReplaceHat(teamHat, new DenHat(teamHat.x, teamHat.y, teamHat.team));
                        if (MilkHat.isHat(teamHat))
                            ReplaceHat(teamHat, new MilkHat(teamHat.x, teamHat.y, teamHat.team));
                        if (UpsideHat.isHat(teamHat))
                            ReplaceHat(teamHat, new UpsideHat(teamHat.x, teamHat.y, teamHat.team));


                        Turban turban = TurbanData.findHat(teamHat);
                        if (turban != null)
                        {
                            ReplaceHat(teamHat, turban);
                        }
                        if(teamHat is Turban)
                        {
                            SpawnCape(turban, new Sprite(Mod.GetPath<EdoMod>("capes\\king")).texture);
                        }
                    }
                }
                List<Thing> removedHats = teamSpawnsDone.Keys.Except(Level.current.things[typeof(TeamHat)]).ToList();
                foreach (Thing thing in removedHats)
                {
                    TeamHat teamHat = thing as TeamHat;
                    if (teamHat != null && teamSpawnsDone.ContainsKey(teamHat))
                    {
                        Cape cape;
                        if (teamSpawnsDone.TryGetValue(teamHat, out cape))
                            Level.Remove(cape);
                        teamSpawnsDone.Remove(teamHat);
                    }
                }
            }
        }

        public void ReplaceHat(TeamHat teamHat, TeamHat newHat)
        {
            if (teamHat == null
                || teamSpawnsDone.ContainsKey(teamHat)
                || Level.current == null
                )

                //|| !(Level.current is GameLevel
                //|| Level.current is Editor
               // || Level.current is TeamSelect2))
                //throw new Exception("DUCKZ!" + Level.current.ToString());
                return;

            //throw new Exception("Attempted to replace a hat! " + teamHat.team.hat.texture.textureName);

            if (teamHat.isServerForObject)
            {
                Level.Add(newHat);
                Duck d = teamHat.equippedDuck;
                if (d != null)
                {
                    d.Equip(newHat, false);
                    d.Fondle(newHat);

                    TeamSelect2 lobby = Level.current as TeamSelect2;
                    if (lobby != null)
                    {
                        ProfileBox2 box = lobby.GetBox(d.PlayerIndex());
                        box._hatSelector.hat = newHat;
                    }
                }
            }
            Level.Remove(teamHat);
            teamSpawnsDone.Add(teamHat, null);
        }

        private void SpawnCape(TeamHat teamHat, Tex2D capeTexture, bool glitch = false)
        {
            if (teamHat == null
                || teamSpawnsDone.ContainsKey(teamHat)
                || Level.current == null)
               // || !(Level.current is GameLevel
               // || Level.current is Editor
               // || Level.current is TeamSelect2))
                return;

            Cape cape = new Cape(teamHat.x, teamHat.y, teamHat);
            cape._capeTexture = capeTexture;
            Level.Add(cape);
            teamSpawnsDone.Add(teamHat, cape);
        }
    }
}