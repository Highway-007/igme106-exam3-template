using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DO NOT MODIFY *anything* in this file EXCEPT where marked with TODO comments
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
namespace Exam3_IGME106_2255
{
    enum GameState
    {
        StartScreen,
        Playing,
        GameOver
    }

    public class Game1 : Game
    {
        // The starter code needs these fields. Leave them alone!
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Vector2> swipePath = new List<Vector2>();
        private Texture2D duckyImg;
        private SpriteFont font;

        // TODO: Change this to be whatever game state you are testing & eventually StartScreen when you are done.
        private GameState currentState = GameState.Playing; 

        // TODO: You're going to need a few more fields.

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
            duckyImg = Content.Load<Texture2D>("ducky");
            font = Content.Load<SpriteFont>("font");
        }

        // TODO: Add a method called StartGame that initializes the mouseLocs list with NumLocations
        // random locations within the screen.
        // Note that _graphics.PreferredBackBufferWidth & _graphics.PreferredBackBufferHeight
        // will give you the dimensions of the screen. 

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: You are responsible for implementing the FSM behavior for the StartScreen, Playing, and GameOver states.
            // Do NOT add any helper methods beyond the StartGame method above. 


            // Draws the swipe line if in the playing state. Don't add any code to this method below this point.
            // You do NOT have to worry about merging this with your other FSM behavior.
            // Just add your FSM code above this point and it should work fine.
            if (GameState.Playing == currentState && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                swipePath.Add(Mouse.GetState().Position.ToVector2());
            }
            else
            {
                swipePath.Clear();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: You're responsible for using the _spriteBatch to draw the appropriate things for each state. 


            // Draws the swipe line if in the playing state. Do NOT add any code to this method below this point.
            if (GameState.Playing == currentState && swipePath.Count > 1)
            {
                ShapeBatch.Begin(GraphicsDevice);
                for (int i = 1; i < swipePath.Count; i++)
                {
                    ShapeBatch.Line(swipePath[i - 1], swipePath[i], 2, Color.Green);
                }
                ShapeBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
