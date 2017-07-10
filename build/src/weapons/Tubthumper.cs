using System;
using System.Collections.Generic;

namespace DuckGame.EdoMod
{
    [EditorGroup("EdoMod|misc")]
    public class Tubthumper : Grenade
    {
        protected SpriteMap sprite;

        public Tubthumper(float xpos, float ypos) :
            base(xpos, ypos)
        {
            sprite = new SpriteMap(Mod.GetPath<EdoMod>("weapons\\tubthumper"), 8, 10); // where are we storing these?
            graphic = sprite;

            _editorName = "Tubthumper";
        }

        protected override void Explosion()
        {
            //gotta time this with music
            foreach (PhysicsObject physicsObject in Level.CheckCircleAll<PhysicsObject>(position, 96f))
                if (Level.CheckLine<Block>(position, physicsObject.position, physicsObject) == null)
                {
                    if (physicsObject.owner == null)
                        Fondle(physicsObject);
                    Vec2 propulsion = (physicsObject.position - position).normalized * (15f - ((physicsObject.position - position).length / 8f));
                    physicsObject.hSpeed = propulsion.x;
                    physicsObject.vSpeed = propulsion.y;

                    // ragdoll hit ducks
                    if (physicsObject is Duck)
                    {
                        Duck theDuck = physicsObject as Duck;
                        if (theDuck.isServerForObject)
                        {
                            Holdable heldItem = theDuck.holdObject;
                            if (heldItem != null)
                            {
                                theDuck.ThrowItem(false);
                                physicsObject.vSpeed -= 4f;
                                physicsObject.hSpeed = theDuck.hSpeed * 0.8f;
                                physicsObject.clip.Add(theDuck);
                                theDuck.clip.Add(heldItem);
                            }
                            theDuck.GoRagdoll();
                            if (heldItem != null)
                            {
                                theDuck.ragdoll.part1.clip.Add(heldItem);
                                theDuck.ragdoll.part2.clip.Add(heldItem);
                                theDuck.ragdoll.part3.clip.Add(heldItem);
                                heldItem.clip.Add(theDuck.ragdoll.part1);
                                heldItem.clip.Add(theDuck.ragdoll.part2);
                                heldItem.clip.Add(theDuck.ragdoll.part3);
                            }
                        }
                    }
                }
        }

        public override void OnNetworkBulletsFired(Vec2 pos)
        {
            // do nothing
        }
    }
}
