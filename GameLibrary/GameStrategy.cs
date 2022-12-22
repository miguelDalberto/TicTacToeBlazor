namespace TicTacToeBlazor.GameLibrary;

using static GameBoard;

public static class GameStrategy
{
    internal static int[] DiagonalA = { 0, 4, 8};
    internal static int[] DiagonalB = { 2, 4, 6};
    internal static int[] Corners = { 0, 2, 6, 8};
    internal static int[] MiddleSides = { 1, 3, 5, 7};
    internal static Dictionary<int, int> OppositeCorners = new Dictionary<int, int>() 
    {
        [0] = 8,
        [2] = 6,
        [6] = 2,
        [8] = 0,
    };

    // strategy order: win, block, fork, block fork, center, opposite corner, empty corner, empty side
    public static string OptimalPlay(string board, char player)
    {
        char[] newBoard = board.ToCharArray();
        if (TryWin(board, player, out int i))
            newBoard[i] = player;
        else if (TryBlock(board, player, out i))
        // if (TryBlock(board, player, out int i))
            newBoard[i] = player;
        else if (TryFork(board, player, out i))
            newBoard[i] = player;
        else if (TryBlockFork(board, player, out i))
            newBoard[i] = player;
        else if (TryCenter(board, out i))
            newBoard[i] = player;
        else if (TryOppositeCorner(board, player, out i))
            newBoard[i] = player;
        else if (TryEmptyCorner(board, out i))
            newBoard[i] = player;
        else if (TryEmptySide(board, out i))
            newBoard[i] = player;

        return new string(newBoard);
    }

    public static string PlayNoForking (string board, char player)
    {
        char[] newBoard = board.ToCharArray();
        if (TryWin(board, player, out int i))
            newBoard[i] = player;
        else if (TryBlock(board, player, out i))
            newBoard[i] = player;
        else if (TryCenter(board, out i))
            newBoard[i] = player;
        else if (TryOppositeCorner(board, player, out i))
            newBoard[i] = player;
        else if (TryEmptyCorner(board, out i))
            newBoard[i] = player;
        else if (TryEmptySide(board, out i))
            newBoard[i] = player;

        return new string(newBoard);
    }

    public static string PlayWinOrRandom (string board, char player)
    {
        char[] newBoard = board.ToCharArray();

        if (TryWin(board, player, out int i))
        {
            newBoard[i] = player;
        }
        else
        {
            var validMoves = board.Select((c, i) => c == ' ' ? i : -1).Where(e => e != -1);
            i = validMoves.ElementAt(new Random().Next(0, validMoves.Count()));
            newBoard[i] = player;
        }

        return new string(newBoard);
    }

    /// <summary>
    /// attempts to make a winning move
    /// </summary>
    /// <param name="board">the game board</param>
    /// <param name="player">the character which the player is using, 'o' or 'x'</param>
    /// <param name="i">index of board character array to play</param>
    /// <returns></returns>
    public static bool TryWin(string board, char player, out int i)
    {
        for (i = 0; i < 9; ++i) 
        {
            if (board[i] != ' ') continue; // only check not filled squares

            if (DiagonalA.Contains(i))
                if (board[Down(Right(i))] == player && board[Up(Left(i))] == player) 
                    return true;

            if (DiagonalB.Contains(i))
                if (board[Down(Left(i))] == player && board[Up(Right(i))] == player) 
                    return true;
            
            if (board[Left(i)] == player && board[Right(i)] == player)
                return true;
            if (board[Up(i)] == player && board[Down(i)] == player)
                return true;
        }
        i = -1;
        return false;
    }

    /// <summary>
    /// attempts to block an opponent's winning move
    /// </summary>
    /// <param name="board">the game board</param>
    /// <param name="player">the character the player is using, 'o' or 'x'</param>
    /// <param name="i">index of board character array to play</param>
    /// <returns></returns>
    public static bool TryBlock(string board, char player, out int i)
    {
        return player == 'o' ?
            TryWin(board, 'x', out i)
            :
            TryWin(board, 'o', out i);
    }

    /// <summary>
    /// attempts to fork (making two winning possibilities with one move)
    /// </summary>
    /// <param name="board">the board string</param>
    /// <param name="player">the character the player is using, 'o' or 'x'</param>
    /// <param name="i">index of board character array to play</param>
    /// <returns></returns>
    public static bool TryFork(string board, char player, out int i)
    {
        for (i = 0; i < 9; ++i)
        {
            if (board[i] != ' ') continue;

            int aligned = 0;

            if (board[Up(i)] == player)
                ++aligned;
            if (board[Down(i)] == player)
                ++aligned;
            if (board[Left(i)] == player)
                ++aligned;
            if (board[Right(i)] == player)
                ++aligned;

            if (DiagonalA.Contains(i))
            {
                if (board[Down(Right(i))] == player)
                    ++aligned;
                if (board[Up(Left(i))] == player)
                    ++aligned;
            }

            if (DiagonalB.Contains(i))
            {
                if (board[Down(Left(i))] == player)
                    ++aligned;
                if (board[Up(Right(i))] == player)
                    ++aligned;
            }

            if (aligned > 1)
                return true;
        }

        i = -1;
        return false;
    }

    public static bool TryBlockFork(string board, char player, out int i)
    {
        return player == 'o' ?
            TryFork(board, 'x', out i)
            :
            TryFork(board, 'o', out i);
    }

    public static bool TryCenter(string board, out int i)
    {
        if (board.Count(e => e == ' ') == 9)
        {
            i =  Corners[new Random().Next(0, 4)];
            return true;
        } 
        if (board[4] == ' ') 
        {
            i = 4;
            return true;
        }
        
        i = -1;
        return false;
    }

    public static bool TryOppositeCorner(string board, char player, out int i)
    {
        char opponent = player == 'o' ? 'x' : 'o';
        for (i = 0; i < 9; ++i)
        {
            if (Corners.Contains(i) &&  board[i] == opponent && board[OppositeCorners[i]] == ' ')
            {
                i = OppositeCorners[i];
                return true;
            }
        }

        i = -1;
        return false;
    }

    public static bool TryEmptyCorner(string board, out int i)
    {
        foreach(var corner in Corners.OrderBy(_ => new Random().Next()))
        {
            if (board[corner] == ' ')
            {
                i = corner;
                return true;
            }
        }

        i = -1;
        return false;
    }

    public static bool TryEmptySide(string board, out int i)
    {
        for (i = 0; i < 9; ++i)
        {
            if (MiddleSides.Contains(i) && board[i] == ' ')
                return true;
        }
        i = -1;
        return false;
    }
}
