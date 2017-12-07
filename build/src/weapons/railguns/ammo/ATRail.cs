namespace DuckGame.EdoMod
{
    internal class ATRail : AmmoType
    {
        public ATRail()
        {
            accuracy = 1f;
            range = 1000f;
            penetration = 0.2f;
            bulletSpeed = 6f;
            bulletThickness = 1.2f;
            affectedByGravity = true;
            sprite = new SpriteMap(Mod.GetPath<EdoMod>("weapons\\ATRail"), 18, 16);
            //sprite = new Sprite(Mod.GetPath<EdoMod>("weapons\\ATRail"), 23f, 13f);
            sprite.center = new Vec2(8.5f,7.5f);
            //sprite.CenterOrigin();
        }

        public override void OnHit(bool destroyed, Bullet b)
        {
            if (penetration >= 2f)
                foreach (Door door in Level.CheckCircleAll<Door>(b.position, 2f))
                {
                    if (b.isLocal)
                        Thing.Fondle(door, DuckNetwork.localConnection);
                    if (Level.CheckLine<Block>(b.position, door.position, door) == null)
                        door.Destroy(new DTImpact(b));
                }
            base.OnHit(destroyed, b);
        }
    }
}
