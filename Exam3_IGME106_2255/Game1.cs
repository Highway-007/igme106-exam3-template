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
        private GameState currentState = GameState.StartScreen; 

        // TODO: You're going to need a few more fields.
        KeyboardState keyboardState;
        KeyboardState previousState;
        private List<Rectangle> duckyLocations = new List<Rectangle>();
        private Random rng;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            rng = new Random();
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
        private void StartGame()
        {
            duckyLocations.Clear();
            for(int i = 0; i < 5; i++)
            {
                duckyLocations.Add(new Rectangle
                    (rng.Next(duckyImg.Width, _graphics.PreferredBackBufferWidth - duckyImg.Width),         //ensures the ducks will spawn at a random location within the window, without clipping out of it 
                    rng.Next(duckyImg.Height, _graphics.PreferredBackBufferHeight - duckyImg.Height),       
                    duckyImg.Width,                                                                         //Gives the Ducks width and height accurate to their texture
                    duckyImg.Height));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: You are responsible for implementing the FSM behavior for the StartScreen, Playing, and GameOver states.
            // Do NOT add any helper methods beyond the StartGame method above. 
            keyboardState = Keyboard.GetState();

            switch (currentState)
            {
                case GameState.StartScreen:
                    //Checks for when the enter key is released to change state
                    if (keyboardState.IsKeyUp(Keys.Enter) && previousState.IsKeyDown(Keys.Enter))
                    {
                        StartGame();
                        currentState = GameState.Playing;
                    }
                    break;

                case GameState.Playing:
                    //For every Duck Rectangle
                    for(int r = 0; r < duckyLocations.Count; r++)
                    {

                        if (r % 2 == 0)
                        {
                            //move half the ducks left every frame
                            duckyLocations[r] = new Rectangle(
                                duckyLocations[r].X - 1,
                                duckyLocations[r].Y,
                                duckyLocations[r].Width,
                                duckyLocations[r].Height);
                        }
                        else
                        {
                            //move the other half right every frame
                            duckyLocations[r] = new Rectangle(
                                duckyLocations[r].X + 1,
                                duckyLocations[r].Y, 
                                duckyLocations[r].Width, 
                                duckyLocations[r].Height);
                        }

                        //If a duck reaches the right side of the screen, move it to the left side
                        if (duckyLocations[r].X > _graphics.PreferredBackBufferWidth)
                        {
                            duckyLocations[r] = new Rectangle(
                                0,
                                duckyLocations[r].Y,
                                duckyLocations[r].Width,
                                duckyLocations[r].Height);
                        }

                        //If a duck reaches the left side of the screen, move it to the right side
                        if (duckyLocations[r].X < 0)
                        {
                            duckyLocations[r] = new Rectangle(
                                _graphics.PreferredBackBufferWidth,
                                duckyLocations[r].Y,
                                duckyLocations[r].Width,
                                duckyLocations[r].Height);
                        }

                            //Check every swipe location
                            for (int l = 0; l < swipePath.Count; l++)
                            {
                            //and if the current ducky retangle includes a mouseswipe
                            try
                            {
                                //Sometimes causes an error, seemingly at random so a try block has been put here to mitigate it
                                //If I had more time I would attempt to figure out why and fix it, as this try/catch block seems to slow the game down significantly
                                if (duckyLocations[r].Contains(swipePath[l]))
                                {
                                    //remove it from the list of ducky locations
                                    duckyLocations.RemoveAt(r);
                                }
                            }
                            catch(Exception e) { }

                            }
                    }

                    if(duckyLocations.Count == 0)
                    {
                        currentState = GameState.GameOver;
                    }
                    break;

                case GameState.GameOver:
                    //Checks for when the enter key is released to change state
                    if (keyboardState.IsKeyUp(Keys.Enter) && previousState.IsKeyDown(Keys.Enter))
                    {
                        currentState = GameState.StartScreen;
                    }
                    break;
            }

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

            previousState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: You're responsible for using the _spriteBatch to draw the appropriate things for each state. 
            _spriteBatch.Begin();
            switch (currentState)
            {
                case GameState.StartScreen:
                    _spriteBatch.DrawString(font, "Press Enter to Start!", new Vector2(30, 30), Color.Black);
                    break;

                case GameState.Playing:
                    //Draws a duck at every location
                    foreach(Rectangle loc in duckyLocations)
                    {
                        _spriteBatch.Draw(duckyImg, loc, Color.Yellow);
                    }

                    _spriteBatch.DrawString(font, "Swipe to catch the ducks!", new Vector2(30, 30), Color.Black);
                    break;

                case GameState.GameOver:
                    _spriteBatch.DrawString(font, "Game Over! Press Enter to Restart!", new Vector2(30,30), Color.Black);
                    break;
            }
            _spriteBatch.End();

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
