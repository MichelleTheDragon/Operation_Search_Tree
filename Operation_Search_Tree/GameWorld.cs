using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Operation_Search_Tree
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<Scene> myScenes = new List<Scene>();
        public int SceneNumber { get; set; } = 0;

        private Texture2D background;

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
            _graphics.ApplyChanges(); //applies the changes

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("Images/TTbg_darkwStars");
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

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
