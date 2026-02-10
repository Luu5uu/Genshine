using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celeste.Animation{
    /// <summary>
    /// High-level animation states for the player character.
    /// </summary>
    public enum PlayerState{
        Idle,
        Run
    }
    /// <summary>
    /// High-level animation facade for the player character.
    ///
    /// Responsibilities:
    /// - Load and configure all player animations
    /// - Register animations with AnimationController
    /// - Expose semantic methods (Idle, Run, etc.) for gameplay code
    ///
    /// This is the ONLY animation class that gameplay code should use.
    /// <author> Albert Liu </author>
    public sealed class PlayerAnimations{
        private readonly AnimationController<PlayerState> _controller = new();

        private PlayerAnimations() { }

        /// <summary>
        /// Builds and initializes the full player animation set.
        /// Should be called once during LoadContent.
        /// </summary>
        public static PlayerAnimations Build(ContentManager content){
            var anims = new PlayerAnimations();

            var idle = new AutoAnimation();
            idle.Detect(content.Load<Texture2D>("idelA"), 32, 32, 8f, true);

            var run = new AutoAnimation();
            run.Detect(content.Load<Texture2D>("run"), 32, 32, 12f, true);

            anims._controller.Register(PlayerState.Idle, idle, setAsDefault: true);
            anims._controller.Register(PlayerState.Run, run);

            return anims;
        }

        /// <summary>
        /// Switches to the idle animation.
        /// </summary>
        public void Idle(bool restart = false)
            => _controller.SetState(PlayerState.Idle, restart);

        /// <summary>
        /// Switches to the run animation.
        /// </summary>
        public void Run(bool restart = false)
            => _controller.SetState(PlayerState.Run, restart);

        /// <summary>
        /// Updates the currently active animation.
        /// Must be called once per frame.
        /// </summary>
        public void Update(GameTime gameTime)
            => _controller.Update(gameTime);

        /// <summary>
        /// Draws the current animation, automatically handling facing direction.
        /// </summary>
        /// <param name="faceLeft">
        /// If true, the animation is drawn flipped horizontally.
        /// </param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float scale = 1f, bool faceLeft = false){
            var effects = faceLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _controller.Draw(spriteBatch, position, color, scale, effects);
        }

    }
}
