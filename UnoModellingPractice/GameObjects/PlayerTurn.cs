using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoModellingPractice.GameObjects
{
    public class PlayerTurn
    {
        public Card Card { get; set; }
        public CardColor DeclaredColor { get; set; }
        public TurnResult Result { get; set; }
        public bool IsAttacking { get; set; }
        public bool OrderReversed { get; set; }
    }
}
