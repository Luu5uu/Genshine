using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Celeste.Animation;



namespace Celeste
{
    public class Game1 : Game
    {
        //initial repo test
        //private Texture2D madeLine;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        //！！！！！！！！！Test for PlayerAnimations.cs 
        // ===== Player animation pack =====
        private PlayerAnimations _playerAnims;

        // ===== Item animation pack =====
        private ItemAnimations _stawA;
        private ItemAnimations _stawB;
        // ===== Item initial position =====
        private Vector2 _posNormalStaw = new Vector2(300, 250);
        private Vector2 _posFlyStaw = new Vector2(350, 250);

        // ===== Player movement data =====
        private Vector2 _playerPos = new Vector2(200, 200); //inital position
        private float _moveSpeed = 150f; // pixels per second
        // ===== Facing direction =====
        // true  = face left  (FlipHorizontally)
        // false = face right (normal)
        private bool _faceLeft = false;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //initial repo test
            //madeLine = Content.Load<Texture2D>("idelA"); 

            //！！！！！！！！！！Test for ItemAnimations.cs
            _stawA = ItemAnimations.Build(Content);
            _stawA.NormalStaw();

            _stawB = ItemAnimations.Build(Content);
            _stawB.FlyStaw();

            //！！！！！！！！！！Test for PlayerAnimations.cs
            // Albert alrady regist all the animations in PlayerAnimations (currently only have run and idle but I will update them)
            _playerAnims = PlayerAnimations.Build(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            //Test and template for PlayerAnimation
            var kb = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            bool isMoving = false;

            // ===== A: run to left side =====
            if (kb.IsKeyDown(Keys.A)){
                _playerPos.X -= _moveSpeed * dt;
                _faceLeft = true;          // _faceLeft decide our character face left or right. True face left, False face right
                _playerAnims.Run();        // _playerAnims.Run() means play the anime "Run", I will update others animation
                                           // For example:If you want our character jumpUp then you only need change it to _playerAnims.JumpUp
                isMoving = true;
            }

            // ===== D: run to right side =====
            if (kb.IsKeyDown(Keys.D)){
                _playerPos.X += _moveSpeed * dt;
                _faceLeft = false;        
                _playerAnims.Run();
                isMoving = true;
            }

            // ===== T: Test specific animation =====
            if (kb.IsKeyDown(Keys.T)){
                _faceLeft = false;     
                _playerAnims.ClimbUp();
                isMoving = true;
            }

            // ===== if not move then：Idle =====
            if (!isMoving){
                _playerAnims.Idle();
            }

            // =====!!! update animations, you must call this !!!=====
            _playerAnims.Update(gameTime);
            _stawA.Update(gameTime);
            _stawB.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            // ===== Start drawing =====

            //initial repo test
            //_spriteBatch.Draw(madeLine,Vector2.Zero,Color.White);


            // Test for animationController
            // To teammate: This is the only thing you need to remember is the Draw call
            // ===== Draw: The animation system internally decides which frame to play + whether to reverse =====
            _playerAnims.Draw(_spriteBatch, _playerPos, Color.White, scale: 2f, faceLeft: _faceLeft);

            // ===== Draw: Item =====
            _stawA.Draw(_spriteBatch, _posNormalStaw, Color.White, scale: 2f);
            _stawB.Draw(_spriteBatch, _posFlyStaw, Color.White, scale: 2f);

            // ===== End drawing =====
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
