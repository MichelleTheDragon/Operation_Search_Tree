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
        public static int SceneNumber { get; set; } = 1;

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
            //MyCamera = new Camera(GraphicsDevice.Viewport);

            background = Content.Load<Texture2D>("Images/TTbg_darkwStars");
            aPixelSprite = Content.Load<Texture2D>("Sprites/1px");
            baseFont = Content.Load<SpriteFont>("Fonts/Base");
            baseButton = Content.Load<Texture2D>("Sprites/1px");

            //Scenes
            myScenes.Add(new MainMenu());
            NodeTree newNodeTree = new NodeTree(Content, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2));
            myScenes.Add(newNodeTree);// - 200)));

            myUI = new UI(baseFont, baseButton, newNodeTree, GraphicsDevice.Viewport);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (myScenes.Count > 0)
            {
                myScenes[SceneNumber].Update(gameTime);
            }
            myUI.Update(gameTime);

            //MyCamera.UpdateCamera(GraphicsDevice.Viewport);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0,0, background.Width, background.Height), Color.White);
            _spriteBatch.End();

            if (myScenes.Count > 0)
            {
                myScenes[SceneNumber].Draw(_spriteBatch);
            }

            _spriteBatch.Begin();
            myUI.Draw(_spriteBatch);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) { 
                angle = MathHelper.TwoPi - angle; 
            }
            spriteBatch.Draw(aPixelSprite, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        //public static void UpdateAllRects()
        //{
        //    foreach (GameObject myGameObject in myScenes[SceneNumber].MyGameObjects)
        //    {
        //        myGameObject.UpdateRect();
        //    }
        //}
    }
}
