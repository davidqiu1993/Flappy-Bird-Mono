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
    public class Floor : GameComponent
    {
        protected Texture2D _Texture = null;
        protected int _PositionX = 0;
        protected int _PositionY = 0;


        /// <summary>
        /// Get the width of the floor in pixel.
        /// </summary>
        static public int Width
        {
            get { return 530; }
        }

        /// <summary>
        /// Get the height of the floor in pixel.
        /// </summary>
        static public int Height
        {
            get { return 115; }
        }

        /// <summary>
        /// Get or set x-axis position of the pipe in refer to the left-top point.
        /// </summary>
        public int PositionX
        {
            get { return _PositionX; }
            set { _PositionX = value; }
        }

        /// <summary>
        /// Get or set y-axis position of the pipe in refer to the left-top point.
        /// </summary>
        public int PositionY
        {
            get { return _PositionY; }
            set { _PositionY = value; }
        }


        /// <summary>
        /// Construct default floor class object.
        /// </summary>
        /// <param name="game">The game that contains this object.</param>
        public Floor(Game game)
            : base(game)
        {
            ;
        }

        /// <summary>
        /// Initialize the parameters and load the textures of the floor.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Initialize the default parameters
            _PositionX = 0;
            _PositionY = 0;

            // Load the pipe textures
            _Texture = base.Game.Content.Load<Texture2D>("floor");
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
        /// Draw the floor on the screen.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the floor.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the floor
            spriteBatch.Begin();
            spriteBatch.Draw(_Texture, new Rectangle(_PositionX, _PositionY, Width, Height), Color.White);
            spriteBatch.End();
        }
    }
}
