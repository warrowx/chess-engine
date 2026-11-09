using System.Net;

class MoveValidator
{
    public static bool IsPawnMoveValid(
        int fromRow, int fromCol, int toRow, int toCol, bool isWhiteTurn, string[,] board, (int row, int col)? enPassantTarget
    )
    {
        int whiteStartRow = 6;
        int blackStartRow = 1;
       
        // Проверка на легальность хода пешки вперед
        if (fromCol == toCol)
        {
            if (isWhiteTurn && toRow == fromRow - 1 && board[toRow, toCol] == ".")
            {
                return true;
            }
            if (!isWhiteTurn && toRow == fromRow + 1 && board[toRow, toCol] == ".")
            {
                return true;
            }
        }
        // Проверка на возможность двинуть пешку на два хода с начальной позиции
        if (isWhiteTurn && fromCol == toCol && fromRow == whiteStartRow && toRow == whiteStartRow - 2 && board[fromRow - 1, fromCol] == "." && board[toRow, toCol] == ".")
        {
            return true;
        }
        if (!isWhiteTurn && fromCol == toCol && fromRow == blackStartRow && toRow == blackStartRow + 2 && board[fromRow + 1, fromCol] == "." && board[toRow, toCol] == ".")
        {
            return true;
        }
        // Проверка на взятие фигуры по диагонали
        if (isWhiteTurn && toRow == fromRow - 1 && (toCol == fromCol - 1 || toCol == fromCol + 1) && char.IsLower(board[toRow, toCol][0]))
        {
            return true;
        }
        if (!isWhiteTurn && toRow == fromRow + 1 && (toCol == fromCol - 1 || toCol == fromCol + 1) && char.IsUpper(board[toRow, toCol][0]))
        {
            return true;
        }

        // Проверка на взятие на проходе
        if (enPassantTarget.HasValue)
        {
            if (toRow == enPassantTarget.Value.row && 
            toCol == enPassantTarget.Value.col && 
            board[toRow, toCol] == "." && 
            Math.Abs(toCol - fromCol) == 1 && 
            (isWhiteTurn ? toRow == fromRow - 1 : toRow == fromRow + 1) && 
            (isWhiteTurn ? board[fromRow, toCol] == "p" : board[fromRow, toCol] == "P"))
            {
                return true;
            }
        }
        return false;
    }

    // Проверка на легальность хода коня
    public static bool IsKnightMoveValid(int fromRow, int fromCol, int toRow, int toCol)
    {
        int rowDiff = Math.Abs(toRow - fromRow);
        int colDiff = Math.Abs(toCol - fromCol);
        return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
    }

    // Проверка на легальность хода слона
    public static bool IsBishopMoveValid(int fromRow, int fromCol, int toRow, int toCol, string[,] board)
    {
        int rowDiff = Math.Abs(toRow - fromRow);
        int colDiff = Math.Abs(toCol - fromCol);
        if (rowDiff != colDiff || rowDiff == 0)
        {
            return false;
        }
        
        int currentRow = fromRow;
        int currentCol = fromCol;

        int rowStep = Math.Sign(toRow - fromRow);
        int colStep = Math.Sign(toCol - fromCol);
        while (currentRow != toRow || currentCol != toCol)
        {
            currentRow += rowStep;
            currentCol += colStep;

            if (currentRow == toRow && currentCol == toCol) break;

            if (board[currentRow, currentCol] != ".")
            {
                return false;
            }
        }
        return true;
    }

    // Проверка на легальность хода ладьи
    public static bool IsRookMoveValid(int fromRow, int fromCol, int toRow, int toCol, string [,] board)
    {
        int currentRow = fromRow;
        int currentCol = fromCol;
        if (toRow == fromRow && toCol == fromCol)
        {
            return false;
        }
        if (toRow != fromRow && toCol != fromCol)
        {
            return false;
        }
        
        int rowStep = Math.Sign(toRow - fromRow);
        int colStep = Math.Sign(toCol - fromCol);
        while (currentRow != toRow || currentCol != toCol)
        {
            currentRow += rowStep;
            currentCol += colStep;

            if (currentRow == toRow && currentCol == toCol) break;

            if (board[currentRow, currentCol] != ".")
            {
                return false;
            }
        }
        return true;

    }

    // Проверка на легальность хода ферзя
    public static bool IsQueenMoveValid(int fromRow, int fromCol, int toRow, int toCol, string[,] board)
    {
        return IsBishopMoveValid(fromRow, fromCol, toRow, toCol, board) || IsRookMoveValid(fromRow, fromCol, toRow, toCol, board);
    }

    // Проверка на легальность хода короля
    public static bool IsKingMoveValid(int fromRow, int fromCol, int toRow, int toCol, string[,] board)
    {
        int rowDiff = Math.Abs(toRow - fromRow);
        int colDiff = Math.Abs(toCol - fromCol);
        if (rowDiff > 1 || colDiff > 1 || rowDiff == 0 && colDiff == 0)
        {
            return false;
        }
        return true;
    }
}