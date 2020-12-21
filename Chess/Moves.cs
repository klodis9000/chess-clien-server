using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Moves
    {
        FigureMoving fm;
        Board board;

        public Moves (Board board)
        {
            this.board = board;
        }

        public bool CanMove(FigureMoving fm)
        {
            this.fm = fm;
            return CanMoveFrom() && CanMoveTo() && CanFigureMove();
        }


        bool CanMoveFrom() //можно ли пойти с клетки с которой идёт
        {
            return fm.from.OnBoard() && fm.figure.GetColor() == board.moveColor;
        }
        bool CanMoveTo() //можно ли пойти на ту клетку куда планирую пойти
        {
            return fm.to.OnBoard() && fm.from != fm.to && board.GetFigureAt(fm.to).GetColor() != board.moveColor;
            
        }
        bool CanFigureMove() //может ли фигура сделать данных ход
        {
            switch (fm.figure)
            {
                case Figure.whiteKing:
                case Figure.blackKing:
                    return CanKingMove();

                case Figure.whiteQueen:
                case Figure.blackQueen:
                    return CanStraightMove();

                case Figure.whiteRook:
                case Figure.blackRook:
                    return (fm.SignX == 0 || fm.SignY == 0) && CanStraightMove();

                case Figure.whiteBishop:
                case Figure.blackBishop:
                    return (fm.SignX != 0 && fm.SignY != 0) && CanStraightMove();

                case Figure.whiteKnight:
                case Figure.blackKnight:
                    return CanKnightMove();

                case Figure.whitePawn:
                case Figure.blackPawn:
                    return CanPawnMove();

                default: return false;
            }
        }

        private bool CanPawnMove()
        {
            if(fm.from.y<1 || fm.from.y > 6)
            {
                return false;
            }
            int stepY = fm.figure.GetColor() == Color.white ? 1 : -1; //если цвет пешки белый то идёт вверх по Y, иначе вниз 
            return CanPawnGo(stepY) || CanPawnJump(stepY) || CanPawnEat(stepY);
        }

        private bool CanPawnGo(int stepY) //может ли пешка идти
        {
            if (board.GetFigureAt(fm.to) == Figure.none) //если пустая клетка
            {
                if (fm.DeltaX == 0) //если идёт прямо
                {
                    if (fm.DeltaY == stepY) //если идёт в нужном направлении
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CanPawnJump(int stepY)
        {
            if (board.GetFigureAt(fm.to) == Figure.none) //если пустая клетка
            {
                if (fm.DeltaX == 0) //если идёт прямо
                {
                    if (fm.DeltaY == 2 * stepY) //если смещение пешки 2 квадрата
                    {
                        if(fm.from.y==1 || fm.from.y == 6) //если совпадает координата направления для белых или черных
                        {
                            if (board.GetFigureAt(new Square(fm.from.x, fm.from.y + stepY)) == Figure.none)  //если не перепрыгивает фигуру и клетка на которую идёт пустая
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool CanPawnEat(int stepY)
        {
            if (board.GetFigureAt(fm.to) != Figure.none) //если клетка не пустая
            {
                if (fm.AbsDeltaX == 1) //она по X смещается на 1
                {
                    if (fm.DeltaY == stepY) //если пешка идёт в нужном направлении
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CanStraightMove()
        {
            Square at = fm.from; //квадрат откуда идёт
            do
            {
                at = new Square(at.x + fm.SignX, at.y + fm.SignY); //движение в выбранном направлении
                if (at == fm.to) { return true; } //если пришёл на ту клетку, куда можно встать

            } while (at.OnBoard() && board.GetFigureAt(at) == Figure.none);  //пока фигура на доске и встаёт на пустое место
            return false;
        }

        private bool CanKingMove()
        {
            if (fm.AbsDeltaX <= 1 && fm.AbsDeltaY <= 1) { return true; } //если фигура идёт не больше чем на 1 клетку
            return false;
        }

        private bool CanKnightMove()
        {
            if (fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2) { return true; }
            if (fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1) { return true; }
            return false;
        }
    }
}
