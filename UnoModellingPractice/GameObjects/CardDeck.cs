using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoModellingPractice.GameObjects
{
    public class CardDeck
    {
        public List<Card> Cards { get; set; }

        public CardDeck()
        {
            Cards = new List<Card>();

            #region Create the cards

            foreach (CardColor color in Enum.GetValues(typeof(CardColor)))
            {
                if (color != CardColor.Wild)
                {
                    foreach (CardValue val in Enum.GetValues(typeof(CardValue)))
                    {
                        switch (val)
                        {
                            case CardValue.One:
                            case CardValue.Two:
                            case CardValue.Three:
                            case CardValue.Four:
                            case CardValue.Five:
                            case CardValue.Six:
                            case CardValue.Seven:
                            case CardValue.Eight:
                            case CardValue.Nine:
                            case CardValue.Skip:
                            case CardValue.Reverse:
                            case CardValue.DrawTwo:
                                Cards.Add(new Card()
                                {
                                    Color = color,
                                    Value = val
                                });
                                Cards.Add(new Card()
                                {
                                    Color = color,
                                    Value = val
                                });
                                break;

                            case CardValue.Zero:
                                Cards.Add(new Card()
                                {
                                    Color = color,
                                    Value = val
                                });
                                break;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        Cards.Add(new Card()
                        {
                            Color = color,
                            Value = CardValue.Wild
                        });
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        Cards.Add(new Card()
                        {
                            Color = color,
                            Value = CardValue.DrawFour
                        });
                    }
                }
            }
            #endregion
        }

        public CardDeck(List<Card> cards)
        {
            Cards = cards;
        }

        public void Shuffle()
        {
            Random r = new Random();

            List<Card> cards = Cards;

            for (int n = cards.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Card temp = cards[n];
                cards[n] = cards[k];
                cards[k] = temp;
            }
        }

        public List<Card> Draw(int count)
        {
            var drawnCards = Cards.Take(count).ToList();
            Cards.RemoveAll(x => drawnCards.Contains(x));
            return drawnCards;
        }
    }
}
