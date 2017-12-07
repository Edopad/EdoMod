using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod.src.core
{
    class EdoDuck : Duck
    {

        public EdoDuck(float xval, float yval, Profile pro) : base(xval, yval, pro)
        {

        }

        private void UpdateQuack()
        {
            if (!this.dead)
            {
                if (this.inputProfile.Pressed("QUACK", false))
                {
                    if (Network.isActive)
                    {
                        this._netQuack.Play(1f, this.inputProfile.leftTrigger - this.inputProfile.rightTrigger);
                    }
                    else
                    {
                        Hat h = this.GetEquipment(typeof(Hat)) as Hat;
                        if (h != null)
                        {
                            h.Quack(1f, this.inputProfile.leftTrigger - this.inputProfile.rightTrigger);
                        }
                        else
                        {
                            this._netQuack.Play(1f, this.inputProfile.leftTrigger - this.inputProfile.rightTrigger);
                        }
                    }
                    if (base.isServerForObject)
                    {
                        //Global.data.quacks.valueInt++;
                    }
                    this.profile.stats.quacks++;
                    this.quack = 20;
                }
                if (!this.inputProfile.Down("QUACK"))
                {
                    this.quack = Maths.CountDown(this.quack, 1, 0);
                }
                if (this.inputProfile.Released("QUACK"))
                {
                    this.quack = 0;
                }
            }
        }

        public void UpdateMove()
        {
            if (base.isServerForObject && base.y > Level.activeLevel.lowestPoint + 100f && !this.dead)
            {
                this.Kill(new DTFall());
                SFX.Play(Mod.GetPath<EdoMod>("SFX\\wilhelm"));
                this.profile.stats.fallDeaths++;
            }
            _UpdateMove();
        }
        public void _UpdateMove()
        {
            Console.WriteLine("This is here as a placeholder.");
        }

        private void OnKill(DestroyType type = null)
        {
            if (type is DTFall) SFX.Play(Mod.GetPath<EdoMod>("SFX\\wilhelm"));
            //if (type is DTShot) SFX.Play();
            _OnKill(type);
        }
        private void _OnKill(DestroyType type = null)
        {
            Console.WriteLine("This is here as a placeholder.");
        }
    }
}
