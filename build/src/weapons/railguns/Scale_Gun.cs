using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod
{
    //[EditorGroup("EdoMod")]
    /*[BaggedProperty("isSuperWeapon", true)]*/
    public class ScaleGun : Gun
    {
        private SpriteMap sprite;

        public PhysicsObject _target;

        public ScaleGun(float xpos, float ypos)
            : base(xpos, ypos)
        {
            // editor name
            _editorName = "Scale Gun";

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
            ammo = 1;
            _weight = 3f;
        }

        public override void Update()
        {
            if (duck != null)
            {
                float ang = angle + (offDir < 0 ? (float)Math.PI : 0f);

                foreach (PhysicsObject physicsObject in Level.CheckCircleAll<PhysicsObject>(Offset(barrelOffset) + 90f * new Vec2((float)Math.Cos(ang), (float)Math.Sin(ang)), 90f))
                {
                    if (physicsObject != owner
                        && (physicsObject is Duck || physicsObject is RagdollPart || physicsObject is TrappedDuck)
                        && Level.CheckLine<Block>(Offset(barrelOffset), physicsObject.position, physicsObject) == null
                        && (_target == null || (Offset(barrelOffset) - physicsObject.position).length < (Offset(barrelOffset) - _target.position).length))
                        _target = physicsObject;
                }

                if (_target != null)
                {
                    if (!Level.CheckCircleAll<PhysicsObject>(Offset(barrelOffset) + 90f * new Vec2((float)Math.Cos(ang), (float)Math.Sin(ang)), 90f).Contains(_target) || Level.CheckLine<Block>(Offset(barrelOffset), _target.position, _target) != null)
                    {
                        _target = null;
                    }
                }
                if(_target != null)
                {
                    _target.scale /= 1.1f;
                    if (_target.scale.x < 0.1f || _target.scale.y < 0.1f)
                    {
                        if(_target is Duck)
                        ((Duck)_target).Kill(new DTImpact(this));
                    }
                }
            }

            base.Update();
        }

        public override void OnPressAction()
        {
            if (owner != null)
            {
                if (_target != null)
                {
                    //_target.addToImaginaryListoffthingstoSizeDown();
                }
            }
        }

        public override void Fire()
        {
        }
    }
}
