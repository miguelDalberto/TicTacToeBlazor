namespace TicTacToeBlazor.GameLibrary;

/// <summary>
/// Tic-tac-toe board validation and position logic utils
/// </summary>
public static class GameBoard
{
    /// <summary>
    /// verifies that board has length 9, only contains x's o's or spaces, and difference between x's and o's is not larger than 1
    /// </summary>
    /// <param name="board">the board string</param>
    /// <returns>true if board is valid, false if otherwise</returns>
    public static bool IsBoardValid (string board)
    {
        if (board.Length != 9)
            return false;

        if (board.Any((i) => i != 'x' && i != 'o' && i != ' '))
            return false;

        int xCount = board.Count(i => i == 'x');
        int oCount = board.Count(i => i == 'o');
        if (xCount - oCount > 1 || oCount - xCount > 1)
            return false;

        return true;
    }

    /// <summary>
    /// verifies that board is not filled and there are no three in a row x's or o's
    /// </summary>
    /// <param name="board">the board string</param>
    /// <returns>true if game is over, false if otherwise</returns>
    public static bool IsGameOver(string board)
    {
        if (!board.Any(i => i == ' ')) 
            return true;
        for (int i = 0; i < 9; i += 4)
        {
            if (board[i] == ' ') continue;
            if (
                (board[i] == board[Up(i)] && board[i] == board[Up(Up(i))]) ||
                (board[i] == board[Left(i)] && board[i] == board[Left(Left(i))]) ||
                (board[i] == board[Right(Down(i))] && board[i] == board[Right(Down(Right(Down(i))))]) ||
                (i == 4 && (board[i] == board[Left(Down(i))] && board[i] == board[Left(Down(Left(Down(i))))]))
            ) 
                return true;
        }
        return false;
    }

    /// <summary>
    /// <param name="board"></param>
    /// <param name="player">the character the player is using, 'o' or 'x' </param>
    /// <returns>true if it's possibly the player's turn, false if otherwise</returns>
    public static bool PossiblyTurnOf (string board, char player)
    {
        return player == 'x' ?
            board.Count(i => i == 'x') <= board.Count(i => i == 'o')
            :
            board.Count(i => i == 'o') <= board.Count(i => i == 'x');
    }

    public static int Up(int p) => p - 3 >= 0 ? p - 3 : p + 6;
    public static int Down(int p) => p + 3 < 9 ? p + 3 : p - 6;
    public static int Left(int p) => p % 3 != 0 ? p - 1 : p + 2;
    public static int Right(int p) => (p - 2) % 3 != 0 ? p + 1 : p - 2;
}
