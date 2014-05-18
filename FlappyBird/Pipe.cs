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
    public class Pipe : GameComponent
    {
        protected Texture2D _Texture_Body = null;
        protected Texture2D _Texture_UpwardEnd = null;
        protected Texture2D _Texture_DownwardEnd = null;
        protected int _Texture_Body_Height = 1;
        protected int _Texture_End_Height = 35;

        protected int _PositionX = 0;

        protected int _OpeningAltitude = 0;

        protected int _PipeNumber = 0;


        /// <summary>
        /// Get or set the number of the pipe.
        /// </summary>
        public int PipeNumber
        {
            get { return _PipeNumber; }
            set { _PipeNumber = value; }
        }

        /// <summary>
        /// Get the width of the pipe in pixel.
        /// </summary>
        public int Width
        {
            get { return 75; }
        }

        /// <summary>
        /// Get or set x-axis position of the pipe.
        /// </summary>
        public int PositionX
        {
            get { return _PositionX; }
            set { _PositionX = value; }
        }

        /// <summary>
        /// Get the opening size of the pipe.
        /// </summary>
        public int OpeningSize
        {
            get { return 80; }
        }

        /// <summary>
        /// Get the minimun altitude of the pipe opening.
        /// </summary>
        public int OpeningMinAltitude
        {
            get { return 40 + OpeningSize / 2; }
        }

        /// <summary>
        /// Get the maximum altitude of the pipe opening.
        /// </summary>
        public int OpeningMaxAltitude
        {
            get { return ((FlappyBird)base.Game).ScreenHeight - OpeningMinAltitude; }
        }

        /// <summary>
        /// Get or set the opening altitude of the pipe. If the set value 
        /// is smaller than the minimum altitude, the opening altitude will 
        /// be set to the minimum altitude. If the set value is larger than 
        /// the maximum altitude, the opening altitude will be set to the 
        /// maximum altitude.
        /// </summary>
        public int OpeningAltitude
        {
            get { return _OpeningAltitude; }
            set
            {
                if (value < OpeningMinAltitude) _OpeningAltitude = OpeningMinAltitude;
                else if (value > OpeningMaxAltitude) _OpeningAltitude = OpeningMaxAltitude;
                else _OpeningAltitude = value;
            }
        }


        /// <summary>
        /// Construct default Pipe class object.
        /// </summary>
        /// <param name="game">The game that contains this object.</param>
        public Pipe(Game game)
            : base(game)
        {
            ;
        }

        /// <summary>
        /// Construct a pipe class object with its number.
        /// </summary>
        /// <param name="game">The game that contains this object.</param>
        /// <param name="pipeNumber">The number of the pipe.</param>
        public Pipe(Game game, int pipeNumber)
            : base(game)
        {
            // Set the pipe number
            _PipeNumber = pipeNumber;
        }

        /// <summary>
        /// Initialize the parameters and load the textures of the pipe.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Load the pipe textures
            _Texture_Body = base.Game.Content.Load<Texture2D>("pipe_body");
            _Texture_DownwardEnd = base.Game.Content.Load<Texture2D>("pipe_end_downward");
            _Texture_UpwardEnd = base.Game.Content.Load<Texture2D>("pipe_end_upward");
        }

        /// <summary>
        /// Update the game logic of the pipe.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the pipe on the screen.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the pipe.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw the upper body
            spriteBatch.Draw(
                _Texture_Body,
                new Rectangle(_PositionX - Width / 2, 0, Width, ((FlappyBird)base.Game).ScreenHeight - _OpeningAltitude - OpeningSize / 2 - _Texture_End_Height),
                new Rectangle(0, 0, Width, _Texture_Body_Height),
                Color.White);

            // Draw the upper end which is downward
            spriteBatch.Draw(
                _Texture_DownwardEnd,
                new Rectangle(_PositionX - Width / 2, ((FlappyBird)base.Game).ScreenHeight - _OpeningAltitude - OpeningSize / 2 - _Texture_End_Height, Width, _Texture_End_Height),
                new Rectangle(0, 0, Width, _Texture_End_Height),
                Color.White);

            // Draw the lower body
            spriteBatch.Draw(
                _Texture_Body,
                new Rectangle(_PositionX - Width / 2, ((FlappyBird)base.Game).ScreenHeight - _OpeningAltitude + OpeningSize / 2 + _Texture_End_Height, Width, _OpeningAltitude - OpeningSize / 2 - _Texture_End_Height),
                new Rectangle(0, 0, Width, _Texture_Body_Height),
                Color.White);

            // Draw the upper end which is downward
            spriteBatch.Draw(
                _Texture_DownwardEnd,
                new Rectangle(_PositionX - Width / 2, ((FlappyBird)base.Game).ScreenHeight - _OpeningAltitude + OpeningSize / 2, Width, _Texture_End_Height),
                new Rectangle(0, 0, Width, _Texture_End_Height),
                Color.White);

            spriteBatch.End();
        }
    }
}
