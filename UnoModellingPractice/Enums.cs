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

    public enum TurnResult
    {
        //Start of game.
        GameStart,

        //Player played a normal number card.
        PlayedCard,
        
        //Player played a skip card.
        Skip,

        //Player played a draw two card.
        DrawTwo,

        //Player was forced to draw by other player's card.
        Attacked,

        //Player was forced to draw because s/he couldn't match the current discard.
        ForceDraw,

        //Player was forced to draw because s/he couldn't match the current discard, but the drawn card was played.
        ForceDrawPlay,

        //Player played a regular wild card.
        WildCard,

        //Player played a draw-four wild card.
        WildDrawFour,

        //Player played a reverse card.
        Reversed
    }
}
