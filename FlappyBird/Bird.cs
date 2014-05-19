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
    public class Bird : GameComponent
    {
        protected Texture2D _Texture = null;
        protected int _FrameSeqNum = 0;

        protected int _PositionX = 0;
        protected int _PositionY = 0;


        /// <summary>
        /// Get width of the bird in pixel.
        /// </summary>
        static public int Width
        {
            get { return 42; }
        }

        /// <summary>
        /// Get height of the bird in pixel.
        /// </summary>
        static public int Height
        {
            get { return 32; }
        }

        /// <summary>
        /// Get or set x-axis position of the bird on the screen.
        /// </summary>
        public int PositionX
        {
            get { return _PositionX; }
            set { _PositionX = value; }
        }

        /// <summary>
        /// Get or set y-axis position of the bird on the screen.
        /// </summary>
        public int PositionY
        {
            get { return _PositionY; }
            set { _PositionY = value; }
        }


        /// <summary>
        /// Construct default Bird class object.
        /// </summary>
        /// <param name="game">The game that contains this object.</param>
        public Bird(Game game)
            : base(game)
        {
            ;
        }

        /// <summary>
        /// Initialize the parameters and load the texture of the bird.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Load the bird texture
            _Texture = base.Game.Content.Load<Texture2D>("bird");

            // Initialize the frame sequence number
            _FrameSeqNum = 0;
        }

        /// <summary>
        /// Allows the game to run logic and outlook of the bird.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update the frame of the bird
            _FrameSeqNum = (gameTime.TotalGameTime.Milliseconds / 200) % 3;
        }

        /// <summary>
        /// Draw the bird on the screen.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the bird.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the texture of the bird in refer to its body center
            spriteBatch.Begin();
            spriteBatch.Draw(
                _Texture,
                new Rectangle(_PositionX - Width / 2, (((FlappyBird)base.Game).ScreenHeight - _PositionY) - Height / 2, Width, Height),
                new Rectangle(Width * _FrameSeqNum, 0, Width, Height),
                Color.White);
            spriteBatch.End();
        }
    }
}
