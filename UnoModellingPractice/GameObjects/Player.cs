using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoModellingPractice.GameObjects
{
    public class Player
    {
        public List<Card> Hand { get; set; }
        public int Position { get; set; }

        public Player()
        {
            Hand = new List<Card>();
        }

        public PlayerTurn PlayTurn(PlayerTurn previousTurn, CardDeck drawPile)
        {
            PlayerTurn turn = new PlayerTurn();
            if (previousTurn.Result == TurnResult.Skip
                || previousTurn.Result == TurnResult.DrawTwo
                || previousTurn.Result == TurnResult.WildDrawFour)
            {
                return ProcessAttack(previousTurn.Card, drawPile);
            }
            else if ((previousTurn.Result == TurnResult.WildCard || previousTurn.Result == TurnResult.Attacked || previousTurn.Result == TurnResult.ForceDraw) 
                        && HasMatch(previousTurn.DeclaredColor))
            {
                turn = PlayMatchingColor(previousTurn.DeclaredColor);
            }
            else if (HasMatch(previousTurn.Card))
            {
                turn = PlayMatchingCard(previousTurn.Card);
            }
            else
            {
                var drawnCard = drawPile.Draw(1);
                Hand.AddRange(drawnCard);

                if (HasMatch(previousTurn.Card))
                {
                    turn = PlayMatchingCard(previousTurn.Card);
                    turn.Result = TurnResult.ForceDrawPlay;
                }
                else
                {
                    turn.Result = TurnResult.ForceDraw;
                    turn.Card = previousTurn.Card;
                }
            }

            DisplayTurn(turn);
            return turn;
        }

        private void DisplayTurn(PlayerTurn currentTurn)
        {
            if (currentTurn.Result == TurnResult.ForceDraw)
            {
                Console.WriteLine("Player " + Position.ToString() + " is forced to draw.");
            }
            if(currentTurn.Result == TurnResult.ForceDrawPlay)
            {
                Console.WriteLine("Player " + Position.ToString() + " is forced to draw AND can play the drawn card!");
            }

            if (currentTurn.Result == TurnResult.PlayedCard
                || currentTurn.Result == TurnResult.Skip
                || currentTurn.Result == TurnResult.DrawTwo 
                || currentTurn.Result == TurnResult.WildCard
                || currentTurn.Result == TurnResult.WildDrawFour
                || currentTurn.Result == TurnResult.Reversed
                || currentTurn.Result == TurnResult.ForceDrawPlay)
            {
                Console.WriteLine("Player " + Position.ToString() + " plays a " + currentTurn.Card.DisplayValue + " card.");
                if(currentTurn.Card.Color == CardColor.Wild)
                {
                    Console.WriteLine("Player " + Position.ToString() + " declares " + currentTurn.DeclaredColor.ToString() + " as the new color.");
                }
                if(currentTurn.Result == TurnResult.Reversed)
                {
                    Console.WriteLine("Turn order reversed!");
                }
            }

            if (Hand.Count == 1)
            {
                Console.WriteLine("Player " + Position.ToString() + " shouts Uno!");
            }
        }

        private PlayerTurn ProcessAttack(Card currentDiscard, CardDeck drawPile)
        {
            PlayerTurn turn = new PlayerTurn();
            turn.Result = TurnResult.Attacked;
            turn.Card = currentDiscard;
            turn.DeclaredColor = currentDiscard.Color;
            if(currentDiscard.Value == CardValue.Skip)
            {
                Console.WriteLine("Player " + Position.ToString() + " was skipped!");
                return turn;
            }
            else if(currentDiscard.Value == CardValue.DrawTwo)
            {
                Console.WriteLine("Player " + Position.ToString() + " must draw two cards!");
                Hand.AddRange(drawPile.Draw(2));
            }
            else if(currentDiscard.Value == CardValue.DrawFour)
            {
                Console.WriteLine("Player " + Position.ToString() + " must draw four cards!");
                Hand.AddRange(drawPile.Draw(4));
            }

            return turn;
        }

        private bool HasMatch(Card card)
        {
            return Hand.Any(x => x.Color == card.Color || x.Value == card.Value || x.Color == CardColor.Wild);
        }

        private bool HasMatch(CardColor color)
        {
            return Hand.Any(x => x.Color == color || x.Color == CardColor.Wild);
        }

        private PlayerTurn PlayMatchingColor(CardColor color)
        {
            var turn = new PlayerTurn();
            turn.Result = TurnResult.PlayedCard;
            var matching = Hand.Where(x => x.Color == color || x.Color == CardColor.Wild).ToList();

            //We cannot play wild draw four unless there are no other matches.
            if (matching.All(x => x.Value == CardValue.DrawFour))
            {
                Hand.Remove(matching.First());
                turn.Card = matching.First();
                turn.DeclaredColor = SelectDominantColor();
                turn.Result = TurnResult.WildCard;
                return turn;
            }

            //Otherwise, we play the card that would cause the most damage to the next player.
            if (matching.Any(x => x.Value == CardValue.DrawTwo))
            {
                turn.Card = matching.First(x => x.Value == CardValue.DrawTwo);
                turn.Result = TurnResult.DrawTwo;
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(turn.Card);
                return turn;
            }

            if (matching.Any(x => x.Value == CardValue.Skip))
            {
                turn.Card = matching.First(x => x.Value == CardValue.Skip);
                turn.Result = TurnResult.Skip;
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(turn.Card);
                return turn;
            }

            if (matching.Any(x => x.Value == CardValue.Reverse))
            {
                turn.Card = matching.First(x => x.Value == CardValue.Reverse);
                turn.Result = TurnResult.Reversed;
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(turn.Card);
                return turn;
            }

            if (matching.Any(x => x.Value == CardValue.Wild))
            {
                turn.Card = matching.First(x => x.Value == CardValue.Wild);
                Hand.Remove(turn.Card);
                turn.DeclaredColor = SelectDominantColor();
                turn.Result = TurnResult.WildCard;
                return turn;
            }

            var matchOnColor = matching.Where(x => x.Color == color);
            if (matchOnColor.Any())
            {
                Hand.Remove(matchOnColor.First());
                turn.Card = matchOnColor.First();
                turn.DeclaredColor = turn.Card.Color;
                return turn;
            }

            //This should never happen
            turn.Result = TurnResult.ForceDraw;
            return turn;
        }

        private PlayerTurn PlayMatchingCard(Card currentDiscard)
        {
            var turn = new PlayerTurn();
            turn.Result = TurnResult.PlayedCard;
            var matching = Hand.Where(x => x.Color == currentDiscard.Color || x.Value == currentDiscard.Value || x.Color == CardColor.Wild).ToList();

            //We cannot play wild draw four unless there are no other matches.
            if(matching.All(x => x.Value == CardValue.DrawFour))
            {
                Hand.Remove(matching.First());
                turn.Card = matching.First();
                turn.DeclaredColor = SelectDominantColor();
                turn.Result = TurnResult.WildCard;

                return turn;
            }

            //Otherwise, we play the card that would cause the most damage to the next player.
            if(matching.Any(x=> x.Value == CardValue.DrawTwo))
            {
                turn.Card = matching.First(x => x.Value == CardValue.DrawTwo);
                turn.Result = TurnResult.DrawTwo;
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(turn.Card);

                return turn;
            }

            if(matching.Any(x => x.Value == CardValue.Skip))
            {
                turn.Card = matching.First(x => x.Value == CardValue.Skip);
                turn.Result = TurnResult.Skip;
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(turn.Card);
                return turn;
            }

            if (matching.Any(x => x.Value == CardValue.Reverse))
            {
                turn.Card = matching.First(x => x.Value == CardValue.Reverse);
                turn.Result = TurnResult.Reversed;
                turn.DeclaredColor = turn.Card.Color;
                Hand.Remove(turn.Card);
                return turn;
            }

            if(matching.Any(x=>x.Value == CardValue.Wild))
            {
                turn.Card = matching.First(x => x.Value == CardValue.Wild);
                Hand.Remove(turn.Card);
                turn.DeclaredColor = SelectDominantColor();
                turn.Result = TurnResult.WildCard;
                return turn;
            }

            //At this point the player has a choice of sorts
            //Assuming he has a match on color AND a match on value, he can choose which to play
            //For this demo, we'll assume that playing the match with MORE possible plays from his hand is the better option.

            var matchOnColor = matching.Where(x => x.Color == currentDiscard.Color);
            var matchOnValue = matching.Where(x => x.Value == currentDiscard.Value);
            if(matchOnColor.Any() && matchOnValue.Any())
            {
                var correspondingColor = Hand.Where(x => x.Color == matchOnColor.First().Color);
                var correspondingValue = Hand.Where(x => x.Value == matchOnValue.First().Value);
                if(correspondingColor.Count() >= correspondingValue.Count())
                {
                    Hand.Remove(matchOnColor.First());
                    turn.Card = matchOnColor.First();
                    turn.DeclaredColor = turn.Card.Color;
                    return turn;
                }
                else //Match on value
                {
                    Hand.Remove(matchOnValue.First());
                    turn.Card = matchOnValue.First();
                    turn.DeclaredColor = turn.Card.Color;
                    return turn;
                }
                //Figure out which of these is better
            }
            else if(matchOnColor.Any())
            {
                Hand.Remove(matchOnColor.First());
                turn.Card = matchOnColor.First();
                turn.DeclaredColor = turn.Card.Color;
                return turn;
            }
            else if(matchOnValue.Any())
            {
                Hand.Remove(matchOnValue.First());
                turn.Card = matchOnValue.First();
                turn.DeclaredColor = turn.Card.Color;
                return turn;
            }

            //This should never happen
            turn.Result = TurnResult.ForceDraw;
            return turn;
        }

        private CardColor SelectDominantColor()
        {
            if (!Hand.Any())
            {
                return CardColor.Wild;
            }
            var colors = Hand.GroupBy(x => x.Color).OrderByDescending(x => x.Count());
            return colors.First().First().Color;
        }

        private void SortHand()
        {
            this.Hand = this.Hand.OrderBy(x => x.Color).ThenBy(x => x.Value).ToList();
        }

        public void ShowHand()
        {
            SortHand();
            Console.WriteLine("Player " + Position + "'s Hand: ");
            foreach (var card in Hand)
            {
                Console.Write(Enum.GetName(typeof(CardColor), card.Color) + " " + Enum.GetName(typeof(CardValue), card.Value) + "  ");
            }
            Console.WriteLine("");
        }
    }
}
