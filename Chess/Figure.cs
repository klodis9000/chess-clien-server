using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    enum Figure
    {
        none,

        whiteKing='K',
        whiteQueen='Q',
        whiteRook='R', //ладья
        whiteBishop='B', //слон
        whiteKnight='N', //конь
        whitePawn='P', //пешка

        blackKing = 'k',
        blackQueen = 'q',
        blackRook = 'r', //ладья
        blackBishop = 'b', //слон
        blackKnight = 'n', //конь
        blackPawn = 'p' //пешка

    }

    static class FigureMethods
    {
        public static Color GetColor(this Figure figure) //получение цвета белой или чёрной фигуры
        {
            if (figure == Figure.none) { return Color.none; }
            return (figure == Figure.whiteKing || 
                    figure == Figure.whiteQueen || 
                    figure == Figure.whiteRook || 
                    figure == Figure.whiteBishop ||
                    figure == Figure.whiteKnight || 
                    figure == Figure.whitePawn) ? Color.white : Color.black;
        }
    }




}
