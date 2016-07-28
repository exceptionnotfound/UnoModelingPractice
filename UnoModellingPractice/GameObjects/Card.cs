using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoModellingPractice.GameObjects
{
    public class Card
    {
        public CardColor Color { get; set; }
        public CardValue Value { get; set; }

        public bool IsAttackingCard()
        {
            return Value == CardValue.Skip
                || Value == CardValue.DrawTwo
                || Value == CardValue.DrawFour;
        }

        public string DisplayValue
        {
            get
            {
                if(Value == CardValue.Wild)
                {
                    return Value.ToString();
                }
                return Color.ToString() + " " + Value.ToString(); 
            }
        }
    }
}
