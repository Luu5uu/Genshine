using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celeste.Animation{
    /// <summary>
    /// Routes animation updates and rendering based on a high-level state key.
    ///
    /// Responsibilities:
    /// - Maintain a mapping from state (e.g., Idle, Run) to AutoAnimation
    /// - Ensure only the active animation is updated and drawn
    /// - Provide safe state switching with optional restart semantics
    ///
    /// This class does NOT:
    /// - Load content
    /// - Interpret input
    /// - Decide which state should be active
    /// </summary>
    /// <author> Albert Liu </author>
    internal sealed class AnimationController<TState> where TState : notnull{
        private readonly Dictionary<TState, AutoAnimation> _animations = new();

        /// <summary>Current active state. Null until a default is registered/selected.</summary>
        public TState CurrentState { get; private set; }
        //private bool _hasState;

        /// <summary>Returns true if this controller has an animation registered for <paramref name="state"/>.</summary>
        public bool HasState(TState state) => _animations.ContainsKey(state);

        /// <summary>
        /// Registers an animation for a given state.
        /// </summary>
        /// <param name="state">State key associated with the animation.</param>
        /// <param name="animation">Initialized AutoAnimation instance.</param>
        /// <param name="setAsDefault">Whether this state should become the initial active state.</param>
        public void Register(TState state, AutoAnimation animation, bool setAsDefault = false){
            if (animation == null) throw new ArgumentNullException(nameof(animation));

            _animations[state] = animation;

            if (setAsDefault || CurrentState == null){
                CurrentState = state;
                // Ensure it is playing when first selected
                animation.Play();
            }
        }

        /// <summary>
        /// Switches the current animation state.
        /// </summary>
        /// <param name="state">The target state.</param>
        /// <param name="restart">
        /// If true, the animation restarts from the first frame;
        /// otherwise, it continues playing.
        /// </param>
        public void SetState(TState state, bool restart = false){
            if (!_animations.TryGetValue(state, out AutoAnimation next))
                throw new KeyNotFoundException($"No animation registered for state '{state}'.");

            // No-op if unchanged (unless caller wants a restart).
            if (CurrentState != null && EqualityComparer<TState>.Default.Equals(CurrentState, state) && !restart)
                return;

            CurrentState = state;

            if (restart){
                next.Stop(); // resets frame/time
                next.Play();
            }
            else{
                next.Play();
            }
        }

        /// <summary>Update only the active animation.</summary>
        public void Update(GameTime gameTime){
            if (CurrentState == null) return;
            _animations[CurrentState].Update(gameTime);
        }

        /// <summary>
        /// Draw the active animation at a position. Caller owns Begin/End.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float scale, SpriteEffects effects){
            if (CurrentState == null) return;
            _animations[CurrentState].Draw(spriteBatch, position, color, scale, effects);
        }

        /// <summary>
        /// Access a registered animation directly. May not use, or if you want to edit it you can.
        /// </summary>
        public AutoAnimation Get(TState state) => _animations[state];
    }
}
