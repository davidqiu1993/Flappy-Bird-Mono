using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace FlappyBird
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FlappyBird : Game
    {
        protected GraphicsDeviceManager _Graphics = null;
        protected SpriteBatch _SpriteBatch = null;

        protected int _Score = 0;

        protected int _Gravity = 1000;

        protected Bird _Bird = null;
        protected int _BirdVerticalSpeed = 0;

        protected List<Pipe> _Pipes = null;
        

        /// <summary>
        /// Fill the pipe list with enough pipes.
        /// </summary>
        protected void _FillPipes()
        {
            ;
        }

        /// <summary>
        /// Check if the bird cross any new pipe and update the score.
        /// </summary>
        protected void _UpdateScore()
        {
            ;
        }


        /// <summary>
        /// Height of the current screen.
        /// </summary>
        public int ScreenHeight
        {
            get { return _Graphics.PreferredBackBufferHeight; }
        }

        /// <summary>
        /// Width of the current screen.
        /// </summary>
        public int ScreenWidth
        {
            get { return _Graphics.PreferredBackBufferWidth; }
        }


        /// <summary>
        /// Construct default FlappyBird class object.
        /// </summary>
        public FlappyBird()
            : base()
        {
            // Create a new graphics device manager and set the screen size
            _Graphics = new GraphicsDeviceManager(this);
            _Graphics.IsFullScreen = false;
            _Graphics.PreferredBackBufferHeight = 600;
            _Graphics.PreferredBackBufferWidth = 384;

            // Set the content root directory
            Content.RootDirectory = "Content";

            // Create game components
            _Bird = new Bird(this);
            _Pipes = new List<Pipe>(8);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Initialize the bird
            _Bird.Initialize();
            _Bird.PositionX = _Graphics.PreferredBackBufferWidth / 3;
            _Bird.PositionY = _Graphics.PreferredBackBufferHeight / 2;

            // Initialize the pipes
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            ;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Check exit action
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update the base class
            base.Update(gameTime);

            // Update game components
            _Bird.Update(gameTime);

            // Update the motion of the bird
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) _BirdVerticalSpeed = 400;
            else _BirdVerticalSpeed += -_Gravity * gameTime.ElapsedGameTime.Milliseconds / 1000;
            _Bird.PositionY += _BirdVerticalSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000;
            if (_Bird.PositionY < 0) { _Bird.PositionY = 0; _BirdVerticalSpeed = 0; }
            if (_Bird.PositionY > ScreenHeight) _Bird.PositionY = ScreenHeight;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the graphics
            GraphicsDevice.Clear(Color.White);

            // Draw the base class
            base.Draw(gameTime);

            // Draw the components
            _Bird.Draw(_SpriteBatch);
        }
    }
}
