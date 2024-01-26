using System;
using System.Threading;

class Program
{
    const int Height = 30;
    const int Width = 100;
    const int PlayerHeight = 4;
    const int PlayerWidth = 7;
    const int BulletSpeed = 1;
    const int EnemySpeed = 1;
    const int MaxBullets = 10;

    static void Main()
    {
        int playerX = Width / 2 - PlayerWidth / 2;
        int playerY = Height - 5;
        int enemyX = Width / 2 - PlayerWidth / 2;
        int enemyY = 2;
        int enemyTwoX = Width / 2 - PlayerWidth / 2;
        int enemyTwoY = 6;
        bool enemyOneMovingRight = true;
        bool enemyTwoMovingRight = false;
        char[,] canvas = new char[Height, Width];
        int[] bulletsX = new int[MaxBullets];
        int[] bulletsY = new int[MaxBullets];
        bool[] bulletActive = new bool[MaxBullets];
        int[] enemyBulletsX = new int[MaxBullets];
        int[] enemyBulletsY = new int[MaxBullets];
        bool[] enemyBulletActive = new bool[MaxBullets];
        int[] enemyTwoBulletsX = new int[MaxBullets];
        int[] enemyTwoBulletsY = new int[MaxBullets];
        bool[] enemyTwoBulletActive = new bool[MaxBullets];

        int shootCounter = 0;
        int enemyShootCounter = 0;
        int enemyTwoShootCounter = 0;
        int health = 10;
        int enemyOneHealth = 5;
        int enemyTwoHealth = 5;
        int score = 0;

        DrawCanvas(canvas);
        StartScreen();

        while (true)
        {
            RedrawCanvas(canvas, health, score);
            RemoveCharacter(canvas, playerX, playerY);

            if (enemyOneHealth > 0)
            {
                RemoveCharacter(canvas, enemyX, enemyY);
            }

            if (enemyTwoHealth > 0)
            {
                RemoveCharacter(canvas, enemyTwoX, enemyTwoY);
            }

            Thread.Sleep(100);

            if (CheckBulletCollision(playerX, playerY, enemyBulletsX, enemyBulletsY, enemyBulletActive) ||
                CheckBulletCollision(playerX, playerY, enemyTwoBulletsX, enemyTwoBulletsY, enemyTwoBulletActive))
            {
                --health;
            }

            if (health <= 0)
            {
                GameOverScreen();
                break;
            }

            if (CheckBulletCollision(enemyX, enemyY, bulletsX, bulletsY, bulletActive))
            {
                score += 10;
                --enemyOneHealth;
                RemoveCharacter(canvas, enemyX, enemyY);
            }

            if (CheckBulletCollision(enemyTwoX, enemyTwoY, bulletsX, bulletsY, bulletActive))
            {
                score += 10;
                --enemyTwoHealth;
                RemoveCharacter(canvas, enemyTwoX, enemyTwoY);
            }

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Spacebar)
                {
                    ShootBullet(bulletsX, bulletsY, bulletActive, playerX + PlayerWidth / 2, playerY, ref shootCounter, 5);
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    MovePlayerRight(ref playerX);
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    MovePlayerLeft(ref playerX);
                }
            }

            DrawPlayer(canvas, playerX, playerY);

            if (enemyOneHealth > 0)
            {
                PatrolEnemy(ref enemyX, ref enemyOneMovingRight);
                DrawEnemy(canvas, enemyX, enemyY);
                ShootBullet(enemyBulletsX, enemyBulletsY, enemyBulletActive, enemyX + PlayerWidth / 2, enemyY + 5, ref enemyShootCounter, 10);
            }

            if (enemyTwoHealth > 0)
            {
                PatrolEnemy(ref enemyTwoX, ref enemyTwoMovingRight);
                DrawEnemy(canvas, enemyTwoX, enemyTwoY);
                ShootBullet(enemyTwoBulletsX, enemyTwoBulletsY, enemyTwoBulletActive, enemyTwoX + PlayerWidth / 2, enemyTwoY + 5, ref enemyTwoShootCounter, 8);
            }

            MoveBullets(canvas, enemyTwoBulletsX, enemyTwoBulletsY, enemyTwoBulletActive, BulletSpeed, 1);
            MoveBullets(canvas, enemyBulletsX, enemyBulletsY, enemyBulletActive, BulletSpeed, Height - 5);

            if (health <= 0)
            {
                GameOverScreen();
                break;
            }

            if (enemyOneHealth <= 0 && enemyTwoHealth <= 0)
            {
                YouWinScreen();
                break;
            }
        }
    }

    static void PatrolEnemy(ref int enemyX, ref bool movingRight)
    {
        if (movingRight)
        {
            MoveEnemyRight(ref enemyX, ref movingRight);
        }
        else
        {
            MoveEnemyLeft(ref enemyX, ref movingRight);
        }
    }

    static void RedrawCanvas(char[,] canvas, int health, int score)
    {
        Console.Clear();
        string canvasString = "";
        for (int i = 0; i < Height; ++i)
        {
            for (int j = 0; j < Width; ++j)
            {
                canvasString += canvas[i, j];
            }
            canvasString += '\n';
        }

        Console.Write(canvasString);
        Console.WriteLine($"Health: {health} | Score: {score}");
    }

    static void DrawCanvas(char[,] canvas)
    {
        for (int i = 0; i < Height; ++i)
        {
            for (int j = 0; j < Width; ++j)
            {
                canvas[i, j] = ' ';
            }
        }

        for (int i = 0; i < Height; ++i)
        {
            canvas[i, 0] = '#';
            canvas[i, Width - 1] = '#';
        }

        for (int j = 0; j < Width; ++j)
        {
            canvas[0, j] = '#';
            canvas[Height - 1, j] = '#';
        }
    }

    static void DrawPlayer(char[,] canvas, int x, int y)
    {
        char[,] playerCharacters = {
            { ' ', ' ', ' ', '.', ' ', ' ', ' ' },
            { ' ', ' ', '.', ' ', '.', ' ', ' ' },
            { '/', '|', ' ', 'o', ' ', '|', '\\' },
            { ' ', ' ', '\'', ' ', '\'', ' ', ' ' }
        };

        for (int i = 0; i < PlayerHeight; ++i)
        {
            for (int j = 0; j < PlayerWidth; ++j)
            {
                if (playerCharacters[i, j] != ' ')
                {
                    canvas[y + i, x + j] = playerCharacters[i, j];
                }
            }
        }
    }

    static void RemoveCharacter(char[,] canvas, int x, int y)
    {
        for (int i = 0; i < PlayerHeight; ++i)
        {
            for (int j = 0; j < PlayerWidth; ++j)
            {
                if (canvas[y + i, x + j] != '#')
                {
                    canvas[y + i, x + j] = ' ';
                }
            }
        }
    }

    static void MovePlayerLeft(ref int playerX)
    {
        if (playerX > 1)
        {
            --playerX;
        }
    }

    static void MovePlayerRight(ref int playerX)
    {
        if (playerX < Width - PlayerWidth - 1)
        {
            ++playerX;
        }
    }

    static bool CheckBulletCollision(int playerX, int playerY, int[] bulletsX, int[] bulletsY, bool[] bulletActive)
    {
        for (int i = 0; i < MaxBullets; ++i)
        {
            if (bulletActive[i] && bulletsX[i] >= playerX && bulletsX[i] < playerX + PlayerWidth &&
                bulletsY[i] >= playerY && bulletsY[i] < playerY + PlayerHeight)
            {
                return true;
            }
        }
        return false;
    }

    static void MoveBullets(char[,] canvas, int[] bulletsX, int[] bulletsY, bool[] bulletActive, int dir, int limit)
    {
        for (int i = 0; i < MaxBullets; ++i)
        {
            if (bulletActive[i])
            {
                RemoveCharacter(canvas, bulletsX[i], bulletsY[i]);
                if (dir == 0)
                {
                    if (bulletsY[i] > limit)
                    {
                        --bulletsY[i];
                        canvas[bulletsY[i], bulletsX[i]] = '.';
                    }
                    else
                    {
                        bulletActive[i] = false;
                    }
                }
                else if (dir == 1)
                {
                    if (bulletsY[i] < limit)
                    {
                        ++bulletsY[i];
                        canvas[bulletsY[i], bulletsX[i]] = '.';
                    }
                    else
                    {
                        bulletActive[i] = false;
                    }
                }
            }
        }
    }

    static void ShootBullet(int[] bulletsX, int[] bulletsY, bool[] bulletActive, int x, int y, ref int shootCounter, int interval)
    {
        ++shootCounter;

        if (shootCounter >= interval)
        {
            for (int i = 0; i < MaxBullets; ++i)
            {
                if (!bulletActive[i])
                {
                    bulletsX[i] = x;
                    bulletsY[i] = y;
                    bulletActive[i] = true;
                    shootCounter = 0;
                    break;
                }
            }
        }
    }
    // drawing enemy 
    static void DrawEnemy(char[,] canvas, int x, int y)
    {
        char[,] enemyCharacter = {
            { ' ', ' ', '\'', ' ', '\'', ' ', ' ' },
            { '\\', '?', ' ', 'o', ' ', '?', '/' },
            { ' ', ' ', '.', ' ', '.', ' ', ' ' },
            { ' ', ' ', ' ', '/', ' ', ' ', ' ' }
        };

        for (int i = 0; i < PlayerHeight; ++i)
        {
            for (int j = 0; j < PlayerWidth; ++j)
            {
                if (enemyCharacter[i, j] != ' ')
                {
                    canvas[y + i, x + j] = enemyCharacter[i, j];
                }
            }
        }
    }
    //  moving left 
    static void MoveEnemyLeft(ref int enemyX, ref bool dir)
    {
        if (enemyX > 1)
        {
            --enemyX;
        }
        else
        {
            dir = !dir;
        }
    }
    // moving right
    static void MoveEnemyRight(ref int enemyX, ref bool dir)
    {
        if (enemyX < Width - PlayerWidth - 1)
        {
            ++enemyX;
        }
        else
        {
            dir = !dir;
        }
    }

    static void YouWinScreen()
    {
        Console.Clear();
        Console.WriteLine("Congratulations! You won!");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void StartScreen()
    {
        Console.Clear();
        Console.WriteLine("===============================");
        Console.WriteLine("      Welcome to Your Game      ");
        Console.WriteLine("===============================");
        Console.WriteLine("Press any key to start...");
        Console.ReadKey();
    }

    static void GameOverScreen()
    {
        Console.Clear();
        Console.WriteLine("===============================");
        Console.WriteLine("         Game Over!            ");
        Console.WriteLine("===============================");
    }
}