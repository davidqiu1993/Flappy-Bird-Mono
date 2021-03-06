﻿using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using BmFont;

namespace FlappyBird
{
    /// <summary>
    /// The state of the game.
    /// </summary>
    public enum GameState
    {
        Ready = 0,
        Start = 1,
        GameOver = 2
    }


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FlappyBird : Game
    {
        protected GraphicsDeviceManager _Graphics = null;
        protected SpriteBatch _SpriteBatch = null;
        protected Texture2D _Background = null;
        protected Texture2D _ReadyCover = null;
        protected Texture2D _GameoverCover = null;
        protected FontFile _TipsFontFile = null;
        protected Texture2D _TipsFontTexture = null;
        protected FontRenderer _TipsFontRenderer = null;
        protected Random _Random = null;

        protected int _Score = 0;
        protected FontFile _ScoreFontFile = null;
        protected Texture2D _ScoreFontTexture = null;
        protected FontRenderer _ScoreFontRenderer = null;
        protected int _ScoreFontWidth = 0;

        protected GameState _GameState = GameState.Ready;
        protected int _Gravity = 1500;
        protected int _SceneSpeed = 160;

        protected Bird _Bird = null;
        protected int _BirdVerticalSpeed = 0;

        protected List<Pipe> _Pipes = null;
        protected bool _PipesMaintaining = false;
        protected int _PipesDistance = 135;

        protected List<Floor> _Floors = null;
        protected bool _FloorsMaintaining = false;


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
                        _Pipes.Add(new Pipe(this, 0, Floor.Height));
                        _Pipes[0].Initialize();
                        _Pipes[0].PositionX = ScreenWidth + Pipe.Width;
                        _Pipes[0].OpeningAltitude = _Random.Next(_Pipes[0].OpeningMinAltitude, _Pipes[0].OpeningMaxAltitude);
                        ++countReadyPipes;
                    }

                    // Add the other pipes
                    Pipe generatedPipe = new Pipe(this, _Pipes[_Pipes.Count - 1].PipeNumber + 1, Floor.Height);
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
        /// Fill the floor list with enough floors and remove outdated floors.
        /// </summary>
        protected void _MaintainFloors()
        {
            if (!_FloorsMaintaining)
            {
                // Set the maintaining state
                _FloorsMaintaining = true;

                int countReadyFloors = 0;

                for (int i = 0; i < _Floors.Count; ++i)
                {
                    // Remove the outdated floors
                    if (_Floors[i].PositionX < 0 - Floor.Width)
                    {
                        _Floors.Remove(_Floors[i]);
                        continue;
                    }

                    // Count the ready floors
                    if (_Floors[i].PositionX > ScreenWidth + Floor.Width) ++countReadyFloors;
                }

                // Fill the list with enough ready floors
                while (countReadyFloors < 3)
                {
                    // Add the first floor
                    if (_Floors.Count == 0)
                    {
                        _Floors.Add(new Floor(this));
                        _Floors[0].Initialize();
                        _Floors[0].PositionX = 0;
                        _Floors[0].PositionY = ScreenHeight - Floor.Height;
                        ++countReadyFloors;
                    }

                    // Add the other floors
                    Floor generatedFloor = new Floor(this);
                    generatedFloor.Initialize();
                    generatedFloor.PositionX = _Floors[_Floors.Count - 1].PositionX + Floor.Width;
                    generatedFloor.PositionY = ScreenHeight - Floor.Height;
                    _Floors.Add(generatedFloor);
                    ++countReadyFloors;
                }

                // Reset the maintaining state
                _FloorsMaintaining = false;
            }
        }

        /// <summary>
        /// Update the score and check if game over.
        /// </summary>
        protected void _UpdateScore()
        {
            // Check score update
            for (int i = 0; i < _Pipes.Count; ++i)
            {
                if (_Pipes[i].PositionX < _Bird.PositionX && _Pipes[i].PipeNumber + 1 > _Score)
                {
                    _Score = _Pipes[i].PipeNumber + 1;
                }
            }

            // Check collision with the floor
            if (_Bird.PositionY < Floor.Height + Bird.Height / 2)
            {
                _GameState = GameState.GameOver;
            }

            // Check collision with the pipes
            for (int i = 0; i < _Pipes.Count; ++i)
            {
                // Check x-axis collision
                if ((_Bird.PositionX + Bird.Width / 2 > _Pipes[i].PositionX - Pipe.Width / 2 &&
                     _Bird.PositionX + Bird.Width / 2 < _Pipes[i].PositionX + Pipe.Width / 2) ||
                    (_Bird.PositionX - Bird.Width / 2 < _Pipes[i].PositionX + Pipe.Width / 2 &&
                     _Bird.PositionX - Bird.Width / 2 > _Pipes[i].PositionX - Pipe.Width / 2))
                {
                    // Check y-axis collision
                    if (_Bird.PositionY + Bird.Height / 2 > _Pipes[i].OpeningAltitude + Pipe.OpeningSize / 2 ||
                        _Bird.PositionY - Bird.Height / 2 < _Pipes[i].OpeningAltitude - Pipe.OpeningSize / 2)
                    {
                        _GameState = GameState.GameOver;
                    }
                }
            }
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
            _Floors = new List<Floor>(8);
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

            // Initialize the pipe maintaining state
            _PipesMaintaining = false;

            // Initialize the floors
            _FloorsMaintaining = false;
            _MaintainFloors();

            // Initialize the game state
            _GameState = GameState.Ready;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the textures of the main scene
            _Background = this.Content.Load<Texture2D>("background");
            _ReadyCover = this.Content.Load<Texture2D>("ready");
            _GameoverCover = this.Content.Load<Texture2D>("gameover");

            // Load the score font
            _ScoreFontFile = FontLoader.Load(Path.Combine(this.Content.RootDirectory, "score_font.fnt"));
            _ScoreFontTexture = this.Content.Load<Texture2D>("score_font_0");
            _ScoreFontRenderer = new FontRenderer(_ScoreFontFile, _ScoreFontTexture);

            // Load the tips font
            _TipsFontFile = FontLoader.Load(Path.Combine(this.Content.RootDirectory, "tips_font.fnt"));
            _TipsFontTexture = this.Content.Load<Texture2D>("tips_font_0");
            _TipsFontRenderer = new FontRenderer(_TipsFontFile, _TipsFontTexture);
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


            // Check the game state
            switch (_GameState)
            {
                case GameState.Ready:
                    {
                        // Update the floor positions and maintain the floor list
                        foreach (Floor floor in _Floors)
                        {
                            floor.PositionX -= _SceneSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000;
                        }
                        _MaintainFloors();

                        // Check if game starts
                        if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        {
                            // Reset the score
                            _Score = 0;

                            // Set the vertical speed of the bird
                            _BirdVerticalSpeed = 400;

                            // Switch the game state
                            _GameState = GameState.Start;
                        }
                        else
                        {
                            _BirdVerticalSpeed = 0;
                            _Bird.PositionY = ScreenHeight / 2;
                        }
                    }
                    break;


                case GameState.Start:
                    {
                        // Update the floor positions and maintain the floor list
                        foreach (Floor floor in _Floors)
                        {
                            floor.PositionX -= _SceneSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000;
                        }
                        _MaintainFloors();

                        // Update the motion of the bird
                        if (Keyboard.GetState().IsKeyDown(Keys.Space)) _BirdVerticalSpeed = 400;
                        else _BirdVerticalSpeed += -_Gravity * gameTime.ElapsedGameTime.Milliseconds / 1000;
                        _Bird.PositionY += _BirdVerticalSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000;
                        if (_Bird.PositionY < Floor.Height)
                        {
                            _Bird.PositionY = Floor.Height;
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
                            pipe.PositionX -= _SceneSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000;
                        }
                        _MaintainPipes();

                        // Update the score and check if game over
                        _UpdateScore();
                    }
                    break;


                case GameState.GameOver:
                    {
                        // Check if game gets ready
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            // Reset the pipes
                            _Pipes.Clear();
                            _Pipes.Add(new Pipe(this, 0, Floor.Height));
                            _Pipes[0].Initialize();
                            _Pipes[0].PositionX = ScreenWidth + Pipe.Width + 200;
                            _Pipes[0].OpeningAltitude = _Random.Next(_Pipes[0].OpeningMinAltitude, _Pipes[0].OpeningMaxAltitude);

                            // Switch the game state to ready
                            _GameState = GameState.Ready;
                        }

                        // Update the motion of the bird
                        _BirdVerticalSpeed += -_Gravity * gameTime.ElapsedGameTime.Milliseconds / 1000;
                        _Bird.PositionY += _BirdVerticalSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000;
                        if (_Bird.PositionY < Floor.Height)
                        {
                            _Bird.PositionY = Floor.Height;
                            _BirdVerticalSpeed = 0;
                        }
                        if (_Bird.PositionY > ScreenHeight)
                        {
                            _Bird.PositionY = ScreenHeight;
                            _BirdVerticalSpeed = 0;
                        }
                    }
                    break;
            }
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

            // Draw the background
            _SpriteBatch.Begin();
            _SpriteBatch.Draw(_Background, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
            _SpriteBatch.End();

            // Draw the bird
            _Bird.Draw(_SpriteBatch);

            // Draw the pipes
            foreach (Pipe pipe in _Pipes)
            {
                pipe.Draw(_SpriteBatch);
            }

            // Draw the floors
            foreach (Floor floor in _Floors)
            {
                floor.Draw(_SpriteBatch);
            }

            
            // Check the game state
            switch (_GameState)
            {
                case GameState.Ready:
                    {
                        // Draw the ready cover
                        _SpriteBatch.Begin();
                        _SpriteBatch.Draw(_ReadyCover, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
                        _SpriteBatch.End();

                        // Draw the tips
                        _TipsFontRenderer.DrawText(_SpriteBatch, 10, ScreenHeight - 50, "Press [Space] to start...", Color.Black);
                    }
                    break;


                case GameState.Start:
                    {
                        // Draw the score
                        _ScoreFontWidth = _ScoreFontRenderer.DrawText(_SpriteBatch, ScreenWidth / 2 - _ScoreFontWidth / 2, 80, _Score.ToString(),Color.White);
                    }
                    break;


                case GameState.GameOver:
                    {
                        // Draw the game over cover
                        _SpriteBatch.Begin();
                        _SpriteBatch.Draw(_GameoverCover, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
                        _SpriteBatch.End();
                        _ScoreFontWidth = _ScoreFontRenderer.DrawText(_SpriteBatch, 300 - _ScoreFontWidth, 250, _Score.ToString(), Color.White);

                        // Draw the tips
                        _TipsFontRenderer.DrawText(_SpriteBatch, 10, ScreenHeight - 50, "Press [Enter] to get ready...", Color.Black);
                    }
                    break;
            }
        }
    }
}
