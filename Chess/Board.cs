using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Board
    {
        public string fen { get; private set; }
        Figure[,] figures;
        public Color moveColor { get; private set; } //чей ход
        public int moveNumber { get; private set; } //номер хода

        public Board(string fen)
        {
            this.fen = fen;
            figures = new Figure[8, 8];
            Init();
        
        }


        void Init()
        {
            //"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            // 0                                           1 2    3 4 5
            string[] parts = fen.Split();
            if (parts.Length != 6) { return; }
            InitFigures(parts[0]);
            moveColor = parts[1] == "b" ? Color.black : Color.white;
            moveNumber = int.Parse(parts[5]);
            //SetFigureAt(new Square("a1"), Figure.whiteKing);
            //SetFigureAt(new Square("h8"), Figure.blackKing);
            moveColor = Color.white;
        }
        void InitFigures(string data)
        {
            for(int j = 8; j >= 2; j--)
            {
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
            }
            data = data.Replace("1", ".");
            string[] lines = data.Split('/');

            for(int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    figures[x, y] = lines[7 - y][x] == '.' ? Figure.none : (Figure)lines[7 - y][x];
                }
            }
        }

        public IEnumerable<FigureOnSquare> YieldFigures() //функция перебирающая все фигуры
        {
            foreach (Square square in Square.YieldSquares()) //перебор всех пустых клеток
            {
                if (GetFigureAt(square).GetColor()==moveColor) //если на квадрате находится фигура цвета, чем сейчас ход
                {
                    yield return new FigureOnSquare(GetFigureAt(square), square);
                }
            }
        }

        void GenerateFEN()
        {
            fen=  FenFigures() + " " + (moveColor==Color.white ? "w" : "b") + " - - 0 " + moveNumber.ToString();
        }
        public Figure GetFigureAt(Square square)
        {
            if (square.OnBoard()) //если квадрат находится на доске
            {
                return figures[square.x, square.y];
            }

            return Figure.none;
        }

        string FenFigures()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    sb.Append(figures[x, y] == Figure.none ? '1' : (char)figures[x, y]);
                }
                if (y > 0) { sb.Append('/'); }
            }
            string eight = "11111111";
            for(int j = 8; j >= 2; j--)
            {
                sb.Replace(eight.Substring(0, j), j.ToString());
            }
            return sb.ToString();
        }

        void SetFigureAt(Square square, Figure figure) //установка фигуры
        {
            if (square.OnBoard())
            {
                figures[square.x, square.y] = figure;
            }
        }

        public Board Move(FigureMoving fm)
        {

            Board next = new Board(fen); //ход
            next.SetFigureAt(fm.from, Figure.none); //устанавливаю фигуры откуда пошла и туда, где пусто
            next.SetFigureAt(fm.to, fm.promotion == Figure.none ? fm.figure : fm.promotion); //указываю куда поставить фигуру и обозначаю  смену пешки на другую фигуру
            if (moveColor == Color.black) //если сейчас ходят чёрные
            {
                next.moveNumber++;
            }

            next.moveColor = moveColor.FlipColor(); //сменить цвет
            next.GenerateFEN();
            return next;

        }

        bool CanEatKing()
        {
            Square badKing = FindBadKing(); //координата вражеского короля
            Moves moves = new Moves(this); //все возможные ходы
            foreach (FigureOnSquare fs in YieldFigures()) //перебор всех фигур на доске
            {
                FigureMoving fm = new FigureMoving(fs, badKing); //координаты фигуры, которая может идти в сторону короля
                if (moves.CanMove(fm)) //если можно пойти на клетку чужого короля
                {
                    return true;
                }
            }
            return false;
        }

        private Square FindBadKing() //выбор короля
        {
            Figure badKing = moveColor == Color.black ? Figure.whiteKing : Figure.blackKing;
            foreach (Square square in Square.YieldSquares()) //перебор всех квадратов, где может быть король
            {
                if (GetFigureAt(square) == badKing)  //если на клетке находится вражеский король
                {
                    return square;
                }
              
            }
            return Square.none;
        }

        public bool IsCheck() //объявлен ли шах
        {
            Board after = new Board(fen); //после хода
            after.moveColor = moveColor.FlipColor();
            return after.CanEatKing();
        }

        public bool IsCheckAfterMove(FigureMoving fm) //шах после хода
        {
            Board after = Move(fm); //совершение хода
            return after.CanEatKing(); //можно ли съесть короля после хода

        }
                    
    }
}
