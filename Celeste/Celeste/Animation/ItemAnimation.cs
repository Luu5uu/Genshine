using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celeste.Animation
{
    /// <summary>
    /// High-level animation states for items.
    /// </summary>
    public enum ItemState
    {
        NormalStaw,
        FlyStaw
    }

    /// <summary>
    /// High-level animation facade for items.
    /// Gameplay code should only depend on this facade.
    /// </summary>
    public sealed class ItemAnimations
    {
        private readonly AnimationController<ItemState> _ctrl = new();

        private ItemAnimations() { }

        /// <summary>
        /// Build the item animation set. Call once in LoadContent.
        /// </summary>
        public static ItemAnimations Build(ContentManager content)
        {
            var anims = new ItemAnimations();

            // Normal Strawberry
            var normalStaw = new AutoAnimation();
            var flyStaw = new AutoAnimation();

            // Normal Strawberry
            normalStaw.Detect(content.Load<Texture2D>("normalStaw"), frameWidth: 32, frameHeight: 32, fps: 12f, loop: true);
            flyStaw.Detect(content.Load<Texture2D>("flyStaw"), frameWidth: 40, frameHeight: 40, fps: 12f, loop: true);

            anims._ctrl.Register(ItemState.NormalStaw, normalStaw, setAsDefault: true);
            anims._ctrl.Register(ItemState.FlyStaw,flyStaw);

            return anims;
        }

        // Semantic API
        public void NormalStaw(bool restart = false) => _ctrl.SetState(ItemState.NormalStaw, restart);
        public void FlyStaw(bool restart = false) => _ctrl.SetState(ItemState.FlyStaw, restart);

        public void Update(GameTime gameTime) => _ctrl.Update(gameTime);

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float scale = 1f, bool faceLeft = false)
        {
            var effects = faceLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _ctrl.Draw(spriteBatch, position, color, scale, effects);
        }
    }
}