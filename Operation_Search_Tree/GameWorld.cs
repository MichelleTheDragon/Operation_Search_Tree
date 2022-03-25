using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Operation_Search_Tree
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //public static Camera MyCamera { get; private set; }

        private static List<Scene> myScenes = new List<Scene>(); 
        public static int SceneNumber { get; set; } = 1; //Start at scene 1, skipping Main Menu

        private Texture2D background;
        private static Texture2D aPixelSprite;
        private Texture2D baseButton;
        private SpriteFont baseFont;
        private UI myUI;

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height - 150; //sets the height of the window
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - 150; //sets the width of the window
            //_graphics.ToggleFullScreen();
            _graphics.ApplyChanges(); //applies the changes

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("Images/TTbg_darkwStars"); //background image
            aPixelSprite = Content.Load<Texture2D>("Sprites/1px"); //1px sprite used for lines
            baseFont = Content.Load<SpriteFont>("Fonts/Base"); //Font
            baseButton = Content.Load<Texture2D>("Sprites/1px"); //buttons temporarily using the 1px sprite too

            //Scenes
            myScenes.Add(new MainMenu());
            NodeTree newNodeTree = new NodeTree(Content, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2));
            myScenes.Add(newNodeTree);

            myUI = new UI(baseFont, baseButton, newNodeTree, GraphicsDevice.Viewport);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (myScenes.Count > 0) //Update current scene if any exists
            {
                myScenes[SceneNumber].Update(gameTime);
            }
            myUI.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(); //background layer
            _spriteBatch.Draw(background, new Rectangle(0,0, background.Width, background.Height), Color.White);
            _spriteBatch.End();

            if (myScenes.Count > 0) //scene layer
            {
                myScenes[SceneNumber].Draw(_spriteBatch);
            }

            _spriteBatch.Begin(); //UI layer
            myUI.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1) //Function that creates and draws a line between two points
        {
            Rectangle newRect = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width); //creates a rectangle starting at node 1 position
            Vector2 newVector = Vector2.Normalize(begin - end); //vector between node 2 and 1
            float angle = (float)Math.Acos(Vector2.Dot(newVector, -Vector2.UnitX)); //angle from node 1 to 2
            if (begin.Y > end.Y) {  
                angle = MathHelper.TwoPi - angle; 
            }
            spriteBatch.Draw(aPixelSprite, newRect, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
