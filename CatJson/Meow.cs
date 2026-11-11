using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatJson
{
    public class Meow : Cat
    {
        public bool IsEvil
        {
            get => default;
            set
            {
            }
        }

        public void SayMeow()
        {
            throw new System.NotImplementedException();
        }
    }
}