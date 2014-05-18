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
        protected Random _Random = null;

        protected int _Score = 0;

        protected int _Gravity = 1500;

        protected Bird _Bird = null;
        protected int _BirdVerticalSpeed = 0;

        protected List<Pipe> _Pipes = null;
        protected bool _PipesMaintaining = false;
        protected int _PipesDistance = 180;
        protected int _PipesSpeed = 160;


        /// <summary>
        /// Fill the pipe list with enough pipes and remove outdated pipes.
        /// </summary>
        protected void _MaintainPipes()
        {

            if (!_PipesMaintaining)
            {
                // Set the maintaining state
                _PipesMaintaining = true;

                int countReadyPipes = 0;

                for (int i = 0; i < _Pipes.Count; ++i)
                {
                    // Remove the outdated pipes
                    if (_Pipes[i].PositionX < 0 - Pipe.Width)
                    {
                        _Pipes.Remove(_Pipes[i]);
                        continue;
                    }

                    // Count the ready pipes
                    if (_Pipes[i].PositionX > ScreenWidth + Pipe.Width) ++countReadyPipes;
                }

                // Fill the list with enough ready pipes
                while (countReadyPipes < 3)
                {
                    // Add the first pipe
                    if (_Pipes.Count == 0)
                    {
                        _Pipes.Add(new Pipe(this, 0));
                        _Pipes[0].Initialize();
                        _Pipes[0].PositionX = ScreenWidth + Pipe.Width;
                        _Pipes[0].OpeningAltitude = _Random.Next(_Pipes[0].OpeningMinAltitude, _Pipes[0].OpeningMaxAltitude);
                        ++countReadyPipes;
                    }

                    // Add the other pipes
                    Pipe generatedPipe = new Pipe(this, _Pipes[_Pipes.Count - 1].PipeNumber + 1);
                    generatedPipe.Initialize();
                    generatedPipe.PositionX = _Pipes[_Pipes.Count - 1].PositionX + _PipesDistance + Pipe.Width;
                    generatedPipe.OpeningAltitude = _Random.Next(generatedPipe.OpeningMinAltitude, generatedPipe.OpeningMaxAltitude);
                    _Pipes.Add(generatedPipe);
                    ++countReadyPipes;
                }

                // Reset the maintaining state
                _PipesMaintaining = false;
            }
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
            _Graphics.PreferredBackBufferHeight = 537;
            _Graphics.PreferredBackBufferWidth = 378;

            // Create a new randon generator
            _Random = new Random((int)DateTime.Now.Ticks);

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
            _PipesMaintaining = false;
            _MaintainPipes();
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
            if (_Bird.PositionY < 0)
            {
                _Bird.PositionY = 0;
                _BirdVerticalSpeed = 0;
            }
            if (_Bird.PositionY > ScreenHeight)
            {
                _Bird.PositionY = ScreenHeight;
                _BirdVerticalSpeed = 0;
            }

            // Update the pipe positions and maintain the pipe list
            foreach (Pipe pipe in _Pipes)
            {
                pipe.PositionX -= _PipesSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000;
            }
            _MaintainPipes();
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

            // Draw the bird
            _Bird.Draw(_SpriteBatch);

            // Draw the pipes
            foreach (Pipe pipe in _Pipes)
            {
                pipe.Draw(_SpriteBatch);
            }
        }
    }
}
