﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOfLifeMonoGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _cellTexture;
    private Texture2D _lineTexture;

    private GridManager _gridManager;
    private int _counter;
    private bool _isSpaceKeyPressed;
    private bool _isPaused = true;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = GameSettings.WindowWidth;
        _graphics.PreferredBackBufferHeight = GameSettings.WindowHeight;
        _graphics.ApplyChanges();

        _gridManager = new GridManager(GameSettings.GridWidth, GameSettings.GridHeight);
    }

    protected override void Initialize()
    {
        base.Initialize();
        _gridManager.InitializeGrid();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _cellTexture = new Texture2D(GraphicsDevice, 1, 1);
        _cellTexture.SetData(new Color[]{ Color.White });

        _lineTexture = new Texture2D(GraphicsDevice, 1, 1);
        _lineTexture.SetData(new Color[]{ Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        HandleInput();

        if (!_isPaused)
        {
            _counter++;
            if (_counter > GameSettings.UpdateDelay)
            {
                _counter = 0;
                _gridManager.UpdateGrid();
            }
        }

        base.Update(gameTime);
    }

    private void HandleInput()
    {
        var mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            int gridX = mouseState.X / GameSettings.CellSize;
            int gridY = mouseState.Y / GameSettings.CellSize;
            _gridManager.ToggleCell(gridX, gridY, true);
        }
        if (mouseState.RightButton == ButtonState.Pressed)
        {
            int gridX = mouseState.X / GameSettings.CellSize;
            int gridY = mouseState.Y / GameSettings.CellSize;
            _gridManager.ToggleCell(gridX, gridY, false);
        }

        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Space) && !_isSpaceKeyPressed)
        {
            _isSpaceKeyPressed = true;
            _isPaused = !_isPaused;
        }
        if (keyboardState.IsKeyUp(Keys.Space))
            _isSpaceKeyPressed = false;

        if (keyboardState.IsKeyDown(Keys.R))
        {
            _gridManager.InitializeGrid();
        }
    }

    private void DrawGridLines()
    {
        // Draw vertical lines
        for (int x = 0; x <= GameSettings.GridWidth; x++)
        {
            _spriteBatch.Draw(_lineTexture, new Rectangle(x * GameSettings.CellSize, 0, 1, GameSettings.GridHeight * GameSettings.CellSize), new Color(50, 50, 50, 5));
        }

        // Draw horizontal lines
        for (int y = 0; y <= GameSettings.GridHeight; y++)
        {
            _spriteBatch.Draw(_lineTexture, new Rectangle(0, y * GameSettings.CellSize, GameSettings.GridWidth * GameSettings.CellSize, 1), new Color(50, 50, 50, 5));
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);

        _spriteBatch.Begin();

        for (int x = 0; x < GameSettings.GridWidth; x++)
        {
            for (int y = 0; y < GameSettings.GridHeight; y++)
            {
                var color = _gridManager.CurrentGrid[x, y] ? Color.CornflowerBlue : new Color(40, 40, 40);
                _spriteBatch.Draw(_cellTexture, new Rectangle(x * GameSettings.CellSize, y * GameSettings.CellSize, GameSettings.CellSize, GameSettings.CellSize), color);
            }
        }

        // Draw lines
        DrawGridLines();

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
