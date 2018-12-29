using DuckGame.EdoMod.src.weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;

namespace DuckGame.EdoMod
{

    internal class EdoMain : IAutoUpdate
    {
        private IDictionary<TeamHat, Cape> teamSpawnsDone;

        private static Dictionary<string, SoundEffect> _sounds;

        //replacement sound effects 
        private static Dictionary<string, Tuple<float, string>[]> _sfxr = new Dictionary<string, Tuple<float, string>[]>();

        //a backup of replacement sound effects original SoundEffect(s).
        private static Dictionary<string, SoundEffect> _sounds_bak = new Dictionary<string, SoundEffect>();
        

        public static EdoMain instance;

        public EdoMain()
        {
            //static initialization for SFX replacement definitions
            //these still need to be runtime due to mod paths.
            //if these get big enough they will cause lag on game entry,
            //so they will need to be moved to EdoMod.cs
            //
            //values are based on 10-game, 4-player sessions. (3 deaths * 10 rounds = 30 deaths/session avg.)
            _sfxr["death"] = new[]{
                Tuple.Create(2f / 30f, Mod.GetPath<EdoMod>("SFX\\RobloxDeath")), //2 per session
                Tuple.Create(1f / 120f, Mod.GetPath<EdoMod>("SFX\\RobloxDeathSlow")) //1 per 4 sessions
            };

            

            //some stuff left over from UFFMod a long time age; should probably be cleaned up but I'm too lazy
            teamSpawnsDone = new Dictionary<TeamHat, Cape>();
            //here so this gets added to the list of things to update each frame
            AutoUpdatables.Add(this);
            //because many things are effectively static, but AutoUpdatables requires this to be an object for frame updates
            instance = this;

            if (ModSettings.enableCustomSFX)
            {
                //create a pointer to the '_sounds' field in the SFX class.
                FieldInfo sounds = typeof(SFX).GetField("_sounds", BindingFlags.Static | BindingFlags.NonPublic);
                _sounds = (Dictionary<string, SoundEffect>)sounds.GetValue(null);

                //store a backup of the intended sfx's
                foreach (string key in _sfxr.Keys)
                {
                    _sounds_bak[key] = _sounds[key];
                }
                //_sounds["shotgunFire2"] = _sounds[Mod.GetPath<EdoMod>("SFX\\MansNotQuack")];

            }

            DuckGame.DevConsole.Log(ModLoader.currentModLoadString, Color.White);

        }

        public void Update()
        {
            //some basic error checking
            if (Level.current == null
                || Steam.user == null)
                return;

            //fancy SFX replacement. yeah yeah, it's per frame, whatever. deal with it.
            if (ModSettings.enableDangerousInjections)
            {
                foreach(KeyValuePair<string, Tuple<float, string>[]> entry in _sfxr)
                {
                    if (entry.Value.Length > 0) {
                        bool changed = false;
                        foreach (Tuple<float, string> val in entry.Value)
                        {
                            //no breaking is done intentionally. later entries have priority.
                            if (Rando.Float(0.0f, 1.0f) < val.Item1)
                            {
                                _sounds[entry.Key] = _sounds[val.Item2];
                                changed = true;
                            }
                        }
                        //if not changed, reset to default
                        if (!changed)
                            _sounds[entry.Key] = _sounds_bak[entry.Key];
                    }
                }

            }
            

            //hat replacement
            if (!(Level.current is Editor))
            {
                IEnumerable<Thing> teamHatList = Level.current.things[typeof(TeamHat)];
                //int nhats = 0;
                foreach (TeamHat teamHat in teamHatList)
                {
                    //nhats++;
                    if (teamHat.team != null && teamHat.team.customData == null && teamHat.team.hasHat)
                    {
                        if (!(teamHat is EdoHat))
                        {
                            //'Normal' Special Hats
                            if (CensoredHat.isHat(teamHat))
                                ReplaceHat(teamHat, new CensoredHat(teamHat.x, teamHat.y, teamHat.team));
                            if (BreadfishHat.isHat(teamHat))
                                ReplaceHat(teamHat, new BreadfishHat(teamHat.x, teamHat.y, teamHat.team));
                            if (AdsHat.isHat(teamHat))
                                ReplaceHat(teamHat, new AdsHat(teamHat.x, teamHat.y, teamHat.team));
                            if (AndIHat.isHat(teamHat))
                                ReplaceHat(teamHat, new AndIHat(teamHat.x, teamHat.y, teamHat.team));
                            //Special Ability Hats
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
                            if (DJHat.isHat(teamHat))
                                ReplaceHat(teamHat, new DJHat(teamHat.x, teamHat.y, teamHat.team));
                            //Developer Hats
                            if (DenHat.isHat(teamHat))
                                ReplaceHat(teamHat, new DenHat(teamHat.x, teamHat.y, teamHat.team));
                            if (MilkHat.isHat(teamHat))
                                ReplaceHat(teamHat, new MilkHat(teamHat.x, teamHat.y, teamHat.team));
                            if (UpsideHat.isHat(teamHat))
                                ReplaceHat(teamHat, new UpsideHat(teamHat.x, teamHat.y, teamHat.team));
                            //turbans
                            if (TurbanData.find(teamHat.sprite.texture.textureIndex) != null)
                            {
                                Turban turban = new Turban(teamHat.x, teamHat.y, teamHat.team);
                                ReplaceHat(teamHat, turban);
                                SpawnCape(turban, new Sprite(Mod.GetPath<EdoMod>("capes\\king")).texture);
                            }
                                
                        }
                        /*if(!(teamHat is Turban))
                        {
                            Turban turban = TurbanData.findHat(teamHat);
                            if (turban != null)
                            {
                                ReplaceHat(teamHat, turban);
                            }
                            if (teamHat is Turban)
                            {
                                SpawnCape(turban, new Sprite(Mod.GetPath<EdoMod>("capes\\king")).texture);
                            }
                        }*/
                        
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

                //DIVEKICK!
                //Duck.grounded

                IEnumerable<Thing> ducks = Level.current.things[typeof(Duck)];
                foreach(Duck duck in ducks)
                {
                    if(!duck.grounded && duck.sliding)
                    {
                        //Duck target = Level.current.NearestThing<Duck>(duck.position);
                        foreach(Duck target in ducks)
                        {
                            //can't target null or self
                            if (target == null || target == duck) continue;
                            //can't kill a dead target
                            if (target.dead) continue;
                            //near to duck && falling down
                            if ((target.position - duck.position).length < 10f && duck.velocity.y > 0)
                            {
                                //Performed DIIIVE KICK!
                                SFX.Play(EdoMod.GetPath<EdoMod>("SFX\\DiveKick"));
                                target.Kill(new DTImpact(duck));
                            }
                        }
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