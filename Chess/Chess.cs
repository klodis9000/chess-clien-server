using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Chess
    {
        public string fen { get; private set; } //позиция в шахматах
        Board board; //доска
        Moves moves;
        List<FigureMoving> allMoves; //список всех возможных ходов
        public Chess(string fen= "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") //изначальная позиция в шахматах
        {
            this.fen = fen;
            board = new Board(fen); //созданная доска с базовой расстановкой фигур
            moves = new Moves(board);
            //FindAllMoves();
        }

        Chess (Board board)
        {
            this.board = board;
            this.fen = board.fen;
            moves = new Moves(board);
        }

        public Chess Move(string move) //Pe2e4 Pe7e8Q
        {
            FigureMoving fm = new FigureMoving(move); //создание хода
            if (!moves.CanMove(fm)) { return this; } //если не может идти, то оставить текущую позицию
            if (board.IsCheckAfterMove(fm)) { return this; } //если шах
            Board nextBoard = board.Move(fm); //создание новой доски после выполнения хода
            Chess nextChess = new Chess(nextBoard); //новая шахматная позиция
            return nextChess;
        }

        public char GetFigureAt(int x,int y) //место положение той или иной фигуры
        {
            Square square = new Square(x, y);
            Figure f = board.GetFigureAt(square);
            return f == Figure.none ? '.' : (char)f;
        }

        void FindAllMoves()
        {
            allMoves = new List<FigureMoving>();
            foreach (FigureOnSquare fs in board.YieldFigures()) //перебор всех фигур на доске того цвета, который сейчас ходит
            {
                foreach (Square to in Square.YieldSquares()) //перебор всех клеток на доске на которые можно пойти
                {
                    FigureMoving fm = new FigureMoving(fs, to);
                    if (moves.CanMove(fm)) //если может быть выполнен этот ход
                    {
                        if (!board.IsCheckAfterMove(fm)) //нет ли шаха 
                        {
                            allMoves.Add(fm);
                        }
                    }
                }
            }
        }

        public List<string> GetAllMoves()
        {
            FindAllMoves();
            List<string> list = new List<string>();
            foreach (FigureMoving fm in allMoves) //для каждой фигуры из списка
            {
                list.Add(fm.ToString());
            }
            return list;
        }

        public bool IsCheck()
        {
            return board.IsCheck();
        }


    }
}
