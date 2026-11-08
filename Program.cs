
class Program
{
    static void Main(string[] args)
    {   
        
        string[,] board = new string[8, 8];

        // Заполняем всю доску пустыми клетками
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                board[row, col] = ".";
            }
        }

        // Расставляем белые фигуры
        board[7, 4] = "K";
        board[7, 3] = "Q";
        board[7, 0] = "R";
        board[7, 7] = "R";
        board[7, 1] = "N";
        board[7, 2] = "B";
        board[7, 5] = "B";
        board[7, 6] = "N";

        // Расставляем чёрные фигуры
        board[0, 4] = "k";
        board[0, 3] = "q";
        board[0, 0] = "r";
        board[0, 7] = "r";
        board[0, 1] = "n";
        board[0, 2] = "b";
        board[0, 5] = "b";
        board[0, 6] = "n";

        // Белые пешки
        for (int col = 0; col < 8; col++)
        {
            board[6, col] = "P";
        }

        // Чёрные пешки
        for (int col = 0; col < 8; col++)
        {
            board[1, col] = "p";
        }

        Console.WriteLine("Начальная расстановка фигур:");
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Console.Write(board[row, col] + " ");
            }
            Console.WriteLine();
        }

        bool whiteTurn = true;
        

        while (true)
        {
            Console.WriteLine("Введите ход или 'exit'");
            string? move = Console.ReadLine();

            if (move?.ToLower() == "exit")
            {
              break;
            }

            if (move?.Length != 4)
            {
                Console.WriteLine("Неверно введен ход. Формат хода: e2e4");
                continue;
            }

            bool moveSuccessful = MovePiece(board, move, whiteTurn);
            if (moveSuccessful)
            {
                whiteTurn = !whiteTurn;
            }

            for (int row = 0; row < 8; row++)
            {
             for (int col = 0; col < 8; col++)
                {
                    Console.Write(board[row, col] + " ");
                }

                Console.WriteLine();
            }
          
        }


    }

    static int GetRow(char rank)
    {
            return 8 - (rank - '0');
    }

    static int GetCol(char file)
    {
            return file - 'a';
    }

    static bool MovePiece(string[,] board, string move, bool whiteTurn)
    {
        // Пример хода: "e2e4"
        char fromFile = move[0];
        char fromRank = move[1];
        char toFile = move[2];
        char toRank = move[3];

        if (fromFile < 'a' || fromFile > 'h' || toFile < 'a' || toFile > 'h' ||
            fromRank < '1' || fromRank > '8' || toRank < '1' || toRank > '8')
        {
            Console.WriteLine("Неверно введен ход. Формат хода: e2e4");
            return false;
        }

        int fromRow = GetRow(fromRank);
        int fromCol = GetCol(fromFile);
        int toRow = GetRow(toRank);
        int toCol = GetCol(toFile);

        if (board[fromRow, fromCol] == ".")
        {
            Console.WriteLine("Нет фигуры на исходной позиции.");
            return false;
        }
        if (fromRow == toRow && fromCol == toCol)
        {
            Console.WriteLine("Нельзя перемещать фигуру на ту же клетку.");
            return false;
        }

        char movingPiece = board[fromRow, fromCol][0];
        if (whiteTurn && char.IsLower(movingPiece))
        {
            Console.WriteLine("Сейчас ход белых. Вы не можете ходить черными фигурами.");
            return false;
        }
        if (!whiteTurn && char.IsUpper(movingPiece))
        {
            Console.WriteLine("Сейчас ход черных. Вы не можете ходить белыми фигурами.");
            return false;
        }

        if (board[toRow, toCol] != ".")
        {
            if ((char.IsUpper(board[toRow, toCol][0]) && char.IsUpper(movingPiece)) || (char.IsLower(board[toRow, toCol][0]) && char.IsLower(movingPiece)))
            {
                Console.WriteLine("Нельзя съесть свою фигуру.");
                return false;
            }
            else if (char.IsUpper(board[toRow, toCol][0]) && char.IsLower(movingPiece))
            {
                Console.WriteLine("Вы съели белую фигуру: " + board[toRow, toCol]);
            }
            else if (char.IsLower(board[toRow, toCol][0]) && char.IsUpper(movingPiece))
            {
                Console.WriteLine("Вы съели черную фигуру: " + board[toRow, toCol]);
            }
            
        }
        // Проверка на легальность хода
        // Пешка
        if (char.ToUpper(movingPiece) == 'P')
        {
            if (!MoveValidator.IsPawnMoveValid(fromRow, fromCol, toRow, toCol, whiteTurn, board))
            {
                Console.WriteLine("Неверный ход пешки.");
                return false;
            }
        }
        // Конь
        else if (char.ToUpper(movingPiece) == 'N')
        {
            if(!MoveValidator.IsKnightMoveValid(fromRow, fromCol, toRow, toCol))
            {
                Console.WriteLine("Неверный ход коня.");
                return false;
            }
        }
        // Слон
        else if (char.ToUpper(movingPiece) == 'B')
        {
            if(!MoveValidator.IsBishopMoveValid(fromRow, fromCol, toRow, toCol, board))
            {
                Console.WriteLine("Неверный ход слона.");
                return false;
            }
        }
        // Ладья
        else if (char.ToUpper(movingPiece) == 'R')
        {
            if(!MoveValidator.IsRookMoveValid(fromRow, fromCol, toRow, toCol, board))
            {
                Console.WriteLine("Неверный ход ладьи.");
                return false;
            }
        }
        // Ферзь
        else if (char.ToUpper(movingPiece) == 'Q')
        {
            if(!MoveValidator.IsQueenMoveValid(fromRow, fromCol, toRow, toCol, board))
            {
                Console.WriteLine("Неверный ход ферзем.");
                return false;
            }
        }
        // Король
        else if (char.ToUpper(movingPiece) == 'K')
        {
            if(!MoveValidator.IsKingMoveValid(fromRow, fromCol, toRow, toCol, board))
            {
                Console.WriteLine("Неверный ход королем.");
                return false;
            }
        }

        // Проверка безопасноти собственного короля
        string capturedPiece = board[toRow, toCol];
        // Временно перемещаем фигуру
        board[toRow, toCol] = board[fromRow, fromCol];
        board[fromRow, fromCol] = ".";

        bool inCheck = KingSafety.IsKingInCheck(board, whiteTurn);

        // Восстанавливаем доску
        board[fromRow, fromCol] = board[toRow, toCol];
        board[toRow, toCol] = capturedPiece;

        if (inCheck)
        {
            Console.WriteLine("Нельзя оставлять своего короля под шахом.");
            return false;
        }

        
        // Перемещаем фигуру
        board[toRow, toCol] = board[fromRow, fromCol];
        board[fromRow, fromCol] = ".";
        return true;
    }
}

