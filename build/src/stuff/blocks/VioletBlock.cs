using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame.EdoMod
{
    [EditorGroup("EdoMod")]
    public class VioletBlock : ItemBox
    {
        private Sprite _scanner;

        private Sprite _projector;

        private Sprite _none;

        private Sprite _projectorGlitch;

        private Sprite _currentProjection;

        private SinWave _wave = 1f;

        private SinWave _projectionWave = 0.04f;

        private SinWave _projectionWave2 = 0.05f;

        private SinWave _projectionFlashWave = 0.8f;

        private bool _useWave;

        private bool _alternate;

        private float _double;

        private float _glitch;

        private static MTEffect _grayscaleEffect = Content.Load<MTEffect>("Shaders/greyscale");

        private static System.Collections.Generic.List<StoredItem> _storedItems = new System.Collections.Generic.List<StoredItem>();

        //private System.Collections.Generic.List<Profile> _served = new System.Collections.Generic.List<Profile>();

        private System.Collections.Generic.List<Profile> _close = new System.Collections.Generic.List<Profile>();

        private float _closeWait;

        private int _closeIndex;

        private float _projectorAlpha;

        private bool _closeGlitch;

        private Holdable _hoverItem;

        private Thing _contextThing;

        private float hitWait;

        public static void Reset()
        {
            _storedItems.Clear();
        }

        public VioletBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            base.graphic = new Sprite(Mod.GetPath<EdoMod>("blocks\\violetBlock"), 0f, 0f);
            base.graphic.center = new Vec2(8f, 8f);
            this._scanner = new Sprite("purpleScanner", 0f, 0f);
            this._scanner.center = new Vec2(4f, 1f);
            this._scanner.alpha = 0.7f;
            this._scanner.depth = 0.9f;
            this._projector = new Sprite("purpleProjector", 0f, 0f);
            this._projector.center = new Vec2(8f, 16f);
            this._projector.alpha = 0.7f;
            this._projector.depth = 0.9f;
            this._none = new Sprite("none", 0f, 0f);
            this._none.center = new Vec2(8f, 8f);
            this._none.alpha = 0.7f;
            this._projectorGlitch = new Sprite("projectorGlitch", 0f, 0f);
            this._projectorGlitch.center = new Vec2(8f, 8f);
            this._projectorGlitch.alpha = 0.7f;
            this._projectorGlitch.depth = 0.91f;
            this._currentProjection = this._none;
            base.impactThreshold = 0.2f;
        }

        public override void Initialize()
        {
        }

        public static StoredItem GetStoredItem(Profile p)
        {
            StoredItem item = _storedItems.FirstOrDefault((StoredItem i) => i.profile == p);
            if (item == null)
            {
                item = new StoredItem
                {
                    profile = p
                };
                _storedItems.Add(item);
            }
            return item;
        }

        public static void StoreItem(Profile p, Thing t)
        {
            if ((Network.isActive && t is RagdollPart) || t is TrappedDuck)
            {
                return;
            }
            if (t is WeightBall)
            {
                t = (t as WeightBall).collar;
            }
            StoredItem item = GetStoredItem(p);
            System.Type type = t.GetType();
            if (item.type != type || item.type == typeof(RagdollPart) || (t is TeamHat && item.thing is TeamHat && (t as TeamHat).team != (item.thing as TeamHat).team))
            {
                Thing newThing;
                if (t is RagdollPart)
                {
                    if (item.thing != null && item.thing is SpriteThing && (item.thing as SpriteThing).persona == (t as RagdollPart)._persona)
                    {
                        return;
                    }
                    newThing = new SpriteThing(0f, 0f, (t as RagdollPart)._persona.defaultHead);
                    (newThing as SpriteThing).persona = (t as RagdollPart)._persona;
                }
                else
                {
                    newThing = (System.Activator.CreateInstance(type, Editor.GetConstructorParameters(type)) as Thing);
                }
                if (newThing is TeamHat)
                {
                    TeamHat hat = newThing as TeamHat;
                    TeamHat otherHat = t as TeamHat;
                    hat.sprite = otherHat.sprite.CloneMap();
                    hat.graphic = hat.sprite;
                    hat.pickupSprite = otherHat.pickupSprite.Clone();
                    hat.team = otherHat.team;
                }
                else if (newThing.graphic == null)
                {
                    newThing.graphic = t.graphic.Clone();
                }
                item.sprite = newThing.GetEditorImage(0, 0, true, _grayscaleEffect, null);
                item.sprite.CenterOrigin();
                if (t is RagdollPart)
                {
                    item.sprite.centerx += 2f;
                    item.sprite.centery += 4f;
                }
                item.type = type;
                item.thing = newThing;
                SFX.Play("scanBeep", 1f, 0f, 0f, false);
            }
        }

        private void BreakHoverBond()
        {
            this._hoverItem.gravMultiplier = 1f;
            this._hoverItem = null;
        }

        public override void Update()
        {
            if (this.hitWait > 0f)
            {
                this.hitWait -= 0.1f;
            }
            else
            {
                this.hitWait = 0f;
            }
            this._alternate = !this._alternate;
            this._scanner.alpha = 0.4f + this._wave.normalized * 0.6f;
            this._projector.alpha = (0.4f + this._wave.normalized * 0.6f) * this._projectorAlpha;
            this._currentProjection.alpha = 0.4f + this._projectionFlashWave.normalized * 0.6f;
            this._currentProjection.alpha -= this._glitch * 3f;
            this._currentProjection.alpha *= this._projectorAlpha;
            this._double = Maths.CountDown(this._double, 0.15f, 0f);
            this._glitch = Maths.CountDown(this._glitch, 0.1f, 0f);
            if (Rando.Float(1f) < 0.01f)
            {
                this._glitch = 0.3f;
                this._projectorGlitch.xscale = 0.8f + Rando.Float(0.7f);
                this._projectorGlitch.yscale = 0.6f + Rando.Float(0.5f);
                this._projectorGlitch.flipH = (Rando.Float(1f) > 0.5f);
            }
            if (Rando.Float(1f) < 0.005f)
            {
                this._glitch = 0.3f;
                this._projectorGlitch.xscale = 0.8f + Rando.Float(0.7f);
                this._projectorGlitch.yscale = 0.6f + Rando.Float(0.5f);
                this._projectorGlitch.flipH = (Rando.Float(1f) > 0.5f);
                this._useWave = !this._useWave;
            }
            if (Rando.Float(1f) < 0.008f)
            {
                this._glitch = 0.3f;
                this._projectorGlitch.xscale = 0.8f + Rando.Float(0.7f);
                this._projectorGlitch.yscale = 0.6f + Rando.Float(0.5f);
                this._projectorGlitch.flipH = (Rando.Float(1f) > 0.5f);
                this._useWave = !this._useWave;
                this._double = 0.6f + Rando.Float(0.6f);
            }
            this._close.Clear();
            if (this._hoverItem != null && this._hoverItem.owner != null)
            {
                this.BreakHoverBond();
            }
            if (this._hoverItem == null)
            {
                Holdable g = Level.Nearest<Holdable>(base.x, base.y, null, null);
                if (g != null && g.owner == null && g != null && g.canPickUp && g.bottom <= this.top && System.Math.Abs(g.hSpeed) + System.Math.Abs(g.vSpeed) < 2f)
                {
                    float dist = 999f;
                    if (g != null)
                    {
                        dist = (this.position - g.position).length;
                    }
                    if (dist < 24f)
                    {
                        this._hoverItem = g;
                    }
                }
            }
            else if (System.Math.Abs(this._hoverItem.hSpeed) + System.Math.Abs(this._hoverItem.vSpeed) > 2f || (this._hoverItem.position - this.position).length > 25f)
            {
                this.BreakHoverBond();
            }
            else
            {
                this._hoverItem.position = Lerp.Vec2Smooth(this._hoverItem.position, this.position + new Vec2(0f, -12f - this._hoverItem.collisionSize.y / 2f + this._projectionWave * 2f), 0.2f);
                this._hoverItem.vSpeed = 0f;
                this._hoverItem.gravMultiplier = 0f;
            }
            using (System.Collections.Generic.IEnumerator<Thing> enumerator = this._level.things[typeof(Duck)].GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Duck d = (Duck)enumerator.Current;
                    if (!d.dead && (d.position - this.position).length < 64f)
                    {
                        this._close.Add(d.profile);
                        this._closeGlitch = false;
                    }
                }
            }
            this._closeWait = Maths.CountDown(this._closeWait, 0.05f, 0f);
            for (int i = 0; i < this._close.Count; i++)
            {
                if (this._close.Count == 1)
                {
                    this._closeIndex = 0;
                }
                else if (this._close.Count > 1 && i == this._closeIndex && this._closeWait <= 0f)
                {
                    this._closeIndex = (this._closeIndex + 1) % this._close.Count;
                    this._closeWait = 1f;
                    this._glitch = 0.3f;
                    this._projectorGlitch.xscale = 0.8f + Rando.Float(0.7f);
                    this._projectorGlitch.yscale = 0.6f + Rando.Float(0.5f);
                    this._projectorGlitch.flipH = (Rando.Float(1f) > 0.5f);
                    this._useWave = !this._useWave;
                    this._double = 0.6f + Rando.Float(0.6f);
                    break;
                }
            }
            if (this._closeIndex >= this._close.Count)
            {
                this._closeIndex = 0;
            }
            if (this._close.Count == 0)
            {
                if (!this._closeGlitch)
                {
                    this._closeWait = 1f;
                    this._glitch = 0.3f;
                    this._projectorGlitch.xscale = 0.8f + Rando.Float(0.7f);
                    this._projectorGlitch.yscale = 0.6f + Rando.Float(0.5f);
                    this._projectorGlitch.flipH = (Rando.Float(1f) > 0.5f);
                    this._useWave = !this._useWave;
                    this._double = 0.6f + Rando.Float(0.6f);
                    this._closeGlitch = true;
                }
                this._projectorAlpha = Maths.CountDown(this._projectorAlpha, 0.1f, 0f);
                this._currentProjection = this._none;
            }
            else
            {
                StoredItem item = GetStoredItem(this._close[this._closeIndex]);
                if (item.sprite == null)
                {
                    this._currentProjection = this._none;
                }
                else
                {
                    this._currentProjection = item.sprite;
                }
                this._projectorAlpha = Maths.CountUp(this._projectorAlpha, 0.1f, 1f);
            }
            this._projectorGlitch.alpha = this._glitch * this._projectorAlpha;
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
            if (this._alternate)
            {
                Graphics.Draw(this._scanner, base.x, base.y + 9f);
            }
            if (!this._alternate)
            {
                Graphics.Draw(this._projector, base.x, base.y - 8f);
            }
            float wave = this._useWave ? this._projectionWave : this._projectionWave2;
            if (this._double > 0f)
            {
                this._currentProjection.alpha = (0.2f + this._projectionFlashWave.normalized * 0.2f + this._glitch * 1f) * this._projectorAlpha;
                Graphics.Draw(this._currentProjection, base.x - this._double * 2f, base.y - 16f - wave);
                Graphics.Draw(this._currentProjection, base.x + this._double * 2f, base.y - 16f - wave);
            }
            else
            {
                this._currentProjection.alpha = (0.4f + this._projectionFlashWave.normalized * 0.6f + this._glitch * 1f) * this._projectorAlpha;
                Graphics.Draw(this._currentProjection, base.x, base.y - 16f - wave);
            }
            if (this._glitch > 0f)
            {
                Graphics.Draw(this._projectorGlitch, base.x, base.y - 16f);
            }
        }

        public override PhysicsObject GetSpawnItem()
        {
            if (!(base.contains == typeof(RagdollPart)))
            {
                return base.GetSpawnItem();
            }
            SpriteThing thing = this._contextThing as SpriteThing;
            if (thing != null)
            {
                Ragdoll rag = new Ragdoll(base.x, base.y, null, false, 0f, 0, Vec2.Zero, thing.persona);
                Level.Add(rag);
                rag.RunInit();
                foreach (PhysicsObject obj in this._aboveList)
                {
                    rag.part1.clip.Add(obj);
                    rag.part2.clip.Add(obj);
                    rag.part3.clip.Add(obj);
                }
                rag.part1.vSpeed = -3.5f;
                rag.part2.vSpeed = -3.5f;
                rag.part3.vSpeed = -3.5f;
                rag.part1.clip.Add(this);
                rag.part2.clip.Add(this);
                rag.part3.clip.Add(this);
                Block leftWall = Level.CheckPoint<Block>(this.position + new Vec2(-16f, 0f), null, null);
                if (leftWall != null)
                {
                    rag.part1.clip.Add(leftWall);
                    rag.part2.clip.Add(leftWall);
                    rag.part3.clip.Add(leftWall);
                }
                Block rightWall = Level.CheckPoint<Block>(this.position + new Vec2(16f, 0f), null, null);
                if (rightWall != null)
                {
                    rag.part1.clip.Add(rightWall);
                    rag.part2.clip.Add(rightWall);
                    rag.part3.clip.Add(rightWall);
                }
                SFX.Play("hitBox", 1f, 0f, 0f, false);
                base.containedObject = null;
                return null;
            }
            return null;
        }

        public override void SpawnItem()
        {
            base.SpawnItem();
            if (this.lastSpawnItem != null)
            {
                TeamHat hat = this.lastSpawnItem as TeamHat;
                TeamHat contextHat = this._contextThing as TeamHat;
                if (hat != null && contextHat != null)
                {
                    hat.sprite = contextHat.sprite.CloneMap();
                    hat.graphic = hat.sprite;
                    hat.team = contextHat.team;
                    hat.pickupSprite = contextHat.pickupSprite.Clone();
                }
            }
            this.lastSpawnItem = null;
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (from == ImpactedFrom.Bottom && this.hitWait == 0f && with.isServerForObject)
            {
                with.Fondle(this);
            }
            if (base.isServerForObject && from == ImpactedFrom.Bottom && this.hitWait == 0f)
            {
                this.hitWait = 1f;
                Holdable h = with as Holdable;
                if (h != null && (h.lastThrownBy != null || (h is RagdollPart && !Network.isActive)))
                {
                    Duck d = h.lastThrownBy as Duck;
                    if (h is RagdollPart)
                    {
                        return;
                    }
                    if (d != null)
                    {
                        StoreItem(d.profile, with);
                    }
                    base.Bounce();
                    return;
                }
                else
                {
                    Duck duck = with as Duck;
                    if (duck != null)
                    {
                        StoredItem item = GetStoredItem(duck.profile);
                        if (item.type != null)
                        {
                            base.contains = item.type;
                            this._contextThing = item.thing;
                            base.Pop();
                            this._hit = false;
                        }
                        else
                        {
                                SFX.Play("scanFail", 1f, 0f, 0f, false);
                            base.Bounce();
                        }
                        if (duck.holdObject != null)
                        {
                            Holdable hold = duck.holdObject;
                            if (hold != null)
                            {
                                StoreItem(duck.profile, hold);
                            }
                        }
                    }
                }
            }
        }
    }
}