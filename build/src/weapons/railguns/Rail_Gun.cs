﻿namespace DuckGame.EdoMod
{
    [EditorGroup("EdoMod")]
    public class RailGun : Gun
    {
        public StateBinding _aimAngleStateBinding = new StateBinding("_aimAngle");
        public StateBinding _fireAngleStateBinding = new StateBinding("_fireAngle");

        public float _aimAngle;
        public float _fireAngle;

        private bool aiming;

        private SpriteMap sprite;

        public override float angle
        {
            get
            {
                return base.angle + _aimAngle;
            }
            set
            {
                _angle = value;
            }
        }

        public RailGun(float xpos, float ypos)
          : base(xpos, ypos)
        {
            _editorName = "Rail Gun";

            // collision & sprite settings
            sprite = new SpriteMap(Mod.GetPath<EdoMod>("weapons\\rail_gun"), 23, 13);
            graphic = sprite;
            _center = new Vec2(11.5f, 6.5f);
            _collisionSize = new Vec2(23f, 13f);
            _collisionOffset = new Vec2(-11.5f, -6.5f);
            _holdOffset = new Vec2(-4f, -1f);
            _barrelOffsetTL = new Vec2(21f, 5f);
            _laserOffsetTL = new Vec2(22f, 5f);

            // weapon settings
            ammo = 12;
            _ammoType = new ATRail();
            //_fireSound = Mod.GetPath<EdoMod>("SFX\\pewpewpew");
            _fireWait = 3f;
            _kickForce = 2f;
            _weight = 3f;
            laserSight = false;
        }

        public override void OnPressAction()
        {
            if (ammo > 0 && duck != null)
            {
                aiming = true;
                duck.immobilized = true;
                duck.remoteControl = true;
            }
            else
                SFX.Play("click");
        }

        public override void OnReleaseAction()
        {

            Duck d = duck ?? prevOwner as Duck;
            if (d != null)
            {
                d.immobilized = false;
                d.remoteControl = false;
            }
            if (_wait == 0f && aiming)
                Fire();
            aiming = false;
        }

        /*public override void Update()
        {
            if (_wait == 0f && aiming)
            {
                if (duck != null)
                {
                    laserSight = true;

                    if (duck.inputProfile.Down(Triggers.Up) && _fireAngle > -30f)
                        _fireAngle -= 1.5f;
                    if (duck.inputProfile.Down(Triggers.Down) && _fireAngle < 30f)
                        _fireAngle += 1.5f;
                    if (duck.inputProfile.Released(Triggers.Grab))
                    {
                        duck.immobilized = false;
                        duck.remoteControl = false;
                        aiming = false;
                    }
                }
                else
                    aiming = false;
            }
            else
            {
                _fireAngle = MathHelper.Lerp(_fireAngle, 0f, 0.09f);
                laserSight = false;
            }

            _aimAngle = Maths.DegToRad(_fireAngle);
            _aimAngle *= offDir;

            base.Update();
        }*/
    }
}
