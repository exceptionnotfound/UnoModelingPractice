using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoModellingPractice.GameObjects
{
    public class GameManager
    {
        public List<Player> Players { get; set; }
        public CardDeck DrawPile { get; set; }
        public List<Card> DiscardPile { get; set; }

        public GameManager(int numPlayers)
        {
            Players = new List<Player>();
            DrawPile = new CardDeck();

            for(int i = 1; i <= numPlayers; i++)
            {
                Players.Add(new Player()
                {
                    Position = i
                });
            }

            DrawPile.Shuffle();

            int maxCards = 7 * Players.Count;
            int dealtCards = 0;

            while(dealtCards < maxCards)
            {
                for(int i = 0; i < numPlayers; i ++)
                {
                    Players[i].Hand.Add(DrawPile.Cards.First());
                    DrawPile.Cards.RemoveAt(0);
                    dealtCards++;
                }
            }

            DiscardPile = new List<Card>();
            DiscardPile.Add(DrawPile.Cards.First());
            DrawPile.Cards.RemoveAt(0);

            while(DiscardPile.First().Value == CardValue.Wild || DiscardPile.First().Value == CardValue.DrawFour)
            {
                DiscardPile.Insert(0, DrawPile.Cards.First());
                DrawPile.Cards.RemoveAt(0);
            }
        }

        public void PlayGame()
        {
            int i = 0;
            bool isAscending = true;

            //First, let's show what each player starts with
            foreach (var player in Players)
            {
                player.ShowHand();
            }

            Console.ReadLine();

            PlayerTurn currentTurn = new PlayerTurn()
            {
                Result = TurnResult.GameStart,
                Card = DiscardPile.First(),
                DeclaredColor = DiscardPile.First().Color,
                IsAttacking = DiscardPile.First().IsAttackingCard()
            };

            CardColor currentColor = CardColor.Red;

            Console.WriteLine("First card is a " + currentTurn.Card.DisplayValue + ".");

            while(!Players.Any(x => !x.Hand.Any()))
            {
                if(currentTurn.Result == TurnResult.WildCard)
                {
                    currentColor = currentTurn.DeclaredColor;
                }
                else
                {
                    currentColor = currentTurn.Card.Color;
                }

                if(DrawPile.Cards.Count < 4) //Cheating a bit here
                {
                    var currentCard = DiscardPile.First();
                    DrawPile.Cards = DiscardPile.Skip(1).ToList();
                    DrawPile.Shuffle();
                    Console.WriteLine("Shuffling cards!");
                }

                var currentPlayer = Players[i];

                currentTurn = Players[i].PlayTurn(currentTurn, DrawPile);

                if (currentTurn.Result == TurnResult.PlayedCard 
                    || currentTurn.Result == TurnResult.DrawTwo
                    || currentTurn.Result == TurnResult.Skip
                    || currentTurn.Result == TurnResult.WildCard
                    || currentTurn.Result == TurnResult.WildDrawFour
                    || currentTurn.Result == TurnResult.Reversed)
                {
                    DiscardPile.Insert(0, currentTurn.Card);
                }

                if (currentTurn.Result == TurnResult.Reversed)
                {
                    isAscending = !isAscending;
                }
                if (isAscending)
                {
                    i++;
                    if (i >= Players.Count) //Reset player counter
                    {
                        i = 0;
                    }
                }
                else
                {
                    i--;
                    if (i < 0)
                    {
                        i = Players.Count - 1;
                    }
                }        
            }

            var winningPlayer = Players.Where(x => !x.Hand.Any()).First();
            Console.WriteLine("Player " + winningPlayer.Position.ToString() + " wins!!");
        }
    }
}
