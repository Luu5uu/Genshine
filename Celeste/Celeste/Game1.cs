using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Celeste.Animation;
using Celeste.Sprites;

namespace Celeste
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // ===== Composite player sprite (body + procedural hair) =====
        private MaddySprite _maddy;

        // ===== Legacy player animation pack (kept for comparison, press H to toggle) =====
        private PlayerAnimations _playerAnims;
        private bool _useComposite = true;

        // ===== Player movement data =====
        private Vector2 _playerPos = new Vector2(200, 200);
        private float _moveSpeed = 150f;
        private bool _faceLeft = false;

        // ===== Toggle debounce =====
        private bool _hWasDown = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Build the new composite sprite (body + procedural hair).
            _maddy = MaddySprite.Build(Content, GraphicsDevice);

            // Keep the legacy animation pack so the team can compare.
            _playerAnims = PlayerAnimations.Build(Content);

            // TODO: Re-add items once ItemAnimations is rebuilt on the new clip system.
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kb = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // ===== H: toggle between composite and legacy rendering =====
            bool hDown = kb.IsKeyDown(Keys.H);
            if (hDown && !_hWasDown)
                _useComposite = !_useComposite;
            _hWasDown = hDown;

            bool isMoving = false;

            // ===== A: run left =====
            if (kb.IsKeyDown(Keys.A))
            {
                _playerPos.X -= _moveSpeed * dt;
                _faceLeft = true;
                _maddy.Run();
                _playerAnims.Run();
                isMoving = true;
            }

            // ===== D: run right =====
            if (kb.IsKeyDown(Keys.D))
            {
                _playerPos.X += _moveSpeed * dt;
                _faceLeft = false;
                _maddy.Run();
                _playerAnims.Run();
                isMoving = true;
            }

            // ===== T: test climb animation =====
            if (kb.IsKeyDown(Keys.T))
            {
                _faceLeft = false;
                _maddy.ClimbUp();
                _playerAnims.ClimbUp();
                isMoving = true;
            }

            // ===== Idle when no movement =====
            if (!isMoving)
            {
                _maddy.Idle();
                _playerAnims.Idle();
            }

            // ===== Update both systems =====
            _maddy.SetPosition(_playerPos, scale: 2f, faceLeft: _faceLeft);
            _maddy.Update(gameTime);

            _playerAnims.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // ===== Draw player (press H to toggle composite vs legacy) =====
            if (_useComposite)
            {
                _maddy.Draw(_spriteBatch, _playerPos, Color.White, scale: 2f, faceLeft: _faceLeft);
            }
            else
            {
                _playerAnims.Draw(_spriteBatch, _playerPos, Color.White, scale: 2f, faceLeft: _faceLeft);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
