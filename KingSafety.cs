
using System.Data;

class KingSafety
{
    // Позиция короля
    public static (int row, int col) GetKingPosition(string[,] board, bool isWhite)
    {
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == (isWhite ? "K" : "k"))
                {
                    return (row, col);
                }
            }
        }
        return (-1, -1);
        
    }
    public static bool ValidateBoard(string[,] board)
    {
        int whiteKings = 0;
        int blackKings = 0;

        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == "K") whiteKings++;
                else if (board[row, col] == "k") blackKings++;
            }
        }
        if (whiteKings != 1 || blackKings != 1) throw new InvalidOperationException("На доске должно быть ровно по одному королю каждого цвета");
        return true;
    } 
    // Под шахом?
    public static bool IsKingInCheck(string[,] board, bool isWhite)
    {
        var kingPosition = GetKingPosition(board, isWhite);
        int kingRow = kingPosition.row;
        int kingCol = kingPosition.col;
        bool attackingWhite = !isWhite;

        if (kingRow == -1 || kingCol == -1) throw new InvalidOperationException("Король не найден на доске!");

        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] != ".")
                {
                    char piece = board[row, col][0];
                    if ((attackingWhite && char.IsUpper(piece)) || (!attackingWhite && char.IsLower(piece)))
                    {
                        switch (char.ToLower(piece))
                        {
                            case 'p':
                                if (attackingWhite)
                                {
                                    if (row - 1 == kingRow && Math.Abs(col - kingCol) == 1)
                                    {
                                        return true;
                                    }
                                }
                                else
                                {
                                    if (row + 1 == kingRow && Math.Abs(col - kingCol) == 1)
                                    {
                                        return true;
                                    }
                                }
                                
                                break;
                            case 'n':
                                if (MoveValidator.IsKnightMoveValid(row, col, kingRow, kingCol))
                                {
                                    return true;
                                }
                                break;
                            case 'b':
                                if (MoveValidator.IsBishopMoveValid(row, col, kingRow, kingCol, board))
                                {
                                    return true;
                                }
                                break;
                            case 'r':
                                if (MoveValidator.IsRookMoveValid(row, col, kingRow, kingCol, board))
                                {
                                    return true;
                                }
                                break;
                            case 'q':
                                if (MoveValidator.IsQueenMoveValid(row, col, kingRow, kingCol, board))
                                {
                                    return true;
                                }
                                break;
                            case 'k':
                                if (MoveValidator.IsKingMoveValid(row, col, kingRow, kingCol, board))
                                {
                                    return true;
                                }
                                break;
                        }


                    }
                }
            }
        }
        return false;
    }
}


