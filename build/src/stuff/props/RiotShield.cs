namespace DuckGame.EdoMod
{
    [EditorGroup("EdoMod")]
    public class RiotShield : Holdable, IPlatform
    {
        /*public StateBinding netSFX_tingStateBinding = new NetSoundBinding("netSFX_ting");

        public NetSoundEffect netSFX_ting = new NetSoundEffect(new string[1]
        {
            Mod.GetPath<EdoMod>("SFX\\anvilTing")
        });*/

        private SpriteMap sprite;

        public RiotShield(float xpos, float ypos)
            : base(xpos, ypos)
        {
            // editor settings
            _editorName = "Riot Shield";

            // general settings
            sprite = new SpriteMap(Mod.GetPath<EdoMod>("stuff\\props\\metalShield"), 11, 24);
            graphic = sprite;
            center = new Vec2(5f, 11.5f);
            collisionOffset = new Vec2(-5f, -11.5f);
            collisionSize = new Vec2(11f, 24f);
            _holdOffset = new Vec2(0f, 0f);
            depth = -0.5f;
            thickness = 2f;
            weight = 4f;
            flammable = 0f;
            friction = 0.25f;
            physicsMaterial = PhysicsMaterial.Plastic;
            _maxHealth = 15f;
            _hitPoints = 15f;
            buoyancy = 0f;
        }

        protected override bool OnDestroy(DestroyType type = null)
        {
            return false;
        }

        public override bool Hit(Bullet bullet, Vec2 hitPos)
        {
            Level.Add(MetalRebound.New(hitPos.x, hitPos.y, (double)bullet.travelDirNormalized.x > 0.0 ? 1 : -1));
            //SFX.Play(Mod.GetPath<EdoMod>("SFX\\anvilTing"));
            hitPos -= bullet.travelDirNormalized;
            for (int index = 0; index < 3; ++index)
                Level.Add(Spark.New(hitPos.x, hitPos.y, bullet.travelDirNormalized, 0.02f));
            return thickness > bullet.ammo.penetration;
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            //if (with is IPlatform && impactPowerV >= 1f)
            //netSFX_ting.Play(1f, Rando.Float(0.15f));
        }

        public override void ExitHit(Bullet bullet, Vec2 exitPos)
        {
            /* do nothing */
        }
    }
}
