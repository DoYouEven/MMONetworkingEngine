using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTData
{
    public class Pokemon
    {

        public int number = 0;
        public string name = "Pikachu";
        public int level = 5;
        public float xp = 0;
        public float hp = 1;
        private float pp;
        public float PP
        {
            get { return pp; }
            set { pp = value; currentPP = value; }
        }
        public string iconName;
        //public List<Move> moves = new List<Move> ();
       // public List<MoveData> moves = new List<MoveData>();
        public bool isPlayer = false;
        public float currentHealth = 10;
        public float currentXP = 0;
        public float currentPP = 30;
        public float health = 10;
        public float attack = 10;
        public float defence = 10;
        public float damage = 0;
        public float speed = 10;
    }
}
