using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoModellingPractice
{
    public enum CardColor
    {
        Red,
        Blue,
        Yellow,
        Green,
        Wild
    }

    public enum CardValue
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Reverse,
        Skip,
        DrawTwo,
        DrawFour,
        Wild
    }

    public enum PlayDirection
    {
        Ascending,
        Descending
    }

    public enum TurnResult
    {
        GameStart,
        PlayedCard,
        Skip,
        DrawTwo,
        Attacked,
        ForceDraw,
        ForceDrawPlay,
        WildCard,
        WildDrawFour,
        Reversed
    }
}
