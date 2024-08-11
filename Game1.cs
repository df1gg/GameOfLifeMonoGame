using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOfLifeMonoGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D _cellTexture;

    private const int CellSize = 10; // Size of one cell
    private const int GridWidth = 210; // Width
    private const int GridHeight = 110; // Height
    private const int UpdateDelay = 10;

    private int _counter;

    private bool[,] _currentGrid;
    private bool[,] _nextGrid;

    private bool _isSpaceKeyPressed = false;
    private bool _pause = true;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = 1920 - 100;
        _graphics.PreferredBackBufferHeight = 1080 - 100;
        _graphics.ApplyChanges();

        _currentGrid = new bool[GridWidth, GridHeight];
        _nextGrid = new bool[GridWidth, GridHeight];
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        /*Random random = new Random();*/
        /*for (int x = 0; x < GridWidth; x++)*/
        /*{*/
        /*    for (int y = 0; y < GridHeight; y++)*/
        /*    {*/
        /*        _currentGrid[x, y] = random.NextDouble() > 1;*/
        /*    }*/
        /*}*/
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _cellTexture = new Texture2D(GraphicsDevice, 1, 1);
        _cellTexture.SetData(new Color[]{ Color.WhiteSmoke });
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        MouseState mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;

            int gridX = mouseX / CellSize;
            int gridY = mouseY / CellSize;

            if (gridX >= 0 && gridX < GridWidth && gridY >= 0 && gridY < GridHeight)
            {
                _currentGrid[gridX, gridY] = true;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Space) && _isSpaceKeyPressed == false)
        {
            _isSpaceKeyPressed = true;
            _pause = _pause ? false : true;
        }
        if (Keyboard.GetState().IsKeyUp(Keys.Space))
            _isSpaceKeyPressed = false;

        if (Keyboard.GetState().IsKeyDown(Keys.R))
        {
            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    _currentGrid[x, y] = false;
                }
            }
        }

        _counter++;

        if (_counter >= UpdateDelay && _pause == false)
        {
            _counter = 0;

            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    int aliveNeighbors = GetAliveNeighbors(x, y); // Alive neighbors
                    bool isAlive = _currentGrid[x, y];

                    if (aliveNeighbors != 0)
                        Console.WriteLine($"Cell: {x}:{y} | Neighbors: {aliveNeighbors} | Alive: {isAlive}");

                    if (isAlive)
                    {
                        _nextGrid[x, y] = aliveNeighbors == 2 || aliveNeighbors == 3;
                    }
                    else
                    {
                        _nextGrid[x, y] = aliveNeighbors == 3;
                    }
                }
            }

            Console.WriteLine("--- NEW FRAME ---");
        
            var temp = _currentGrid;
            _currentGrid = _nextGrid;
            _nextGrid = temp;
        }



        base.Update(gameTime);
    }

    private int GetAliveNeighbors(int x, int y)
    {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < GridWidth && ny >= 0 && ny < GridHeight)
                {
                    if (_currentGrid[nx, ny])
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);

        _spriteBatch.Begin();

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                if (_currentGrid[x, y])
                {
                    _spriteBatch.Draw(_cellTexture, new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(_cellTexture, new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize), Color.Black);
                }
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
