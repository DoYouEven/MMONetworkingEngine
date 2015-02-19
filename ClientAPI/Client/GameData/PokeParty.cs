using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTData
{
    public class PokeParty
    {
        public const int PARTY_MAX = 6;

        Trainer trainer; //Enables it to be usable by any trainer in multiplayer (independant)
        List<PokeSlot> slots;
        private int selectedIndex;

        public void SetPokeSlotsInActive()
        {
            selectedIndex = -1;

        }
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { if (SlotCount() >= selectedIndex) selectedIndex = value; }
        }
        public PokeParty(Trainer trainer)
        {

            this.trainer = trainer;
            slots = new List<PokeSlot>();
            GetPokeSlot(-1); //Assume the trainer has no pokemon
        }

        public int SlotCount()
        {
            return slots.Count;
        }

        public bool HasPokemon()
        {
            return SlotCount() > 0;
        }

        public List<PokeSlot> GetSlots()
        {
            return slots;
        }

        public PokeSlot GetSlot(int index)
        {
            return GetSlots()[index];
        }

        public PokeSlot GetActive()
        {
            if (selectedIndex == -1)
                return null;

            var slot = GetSlot(selectedIndex);
            return (slot != null) ? slot : null;
        }

        public Pokemon GetActivePokemon()
        {
            var slot = GetActive();
            return (slot != null) ? slot.pokemon : null;
        }

        public bool IsActive(Pokemon pokemon)
        {
            return GetActivePokemon() == pokemon;
        }

        public bool CanAddPokemon()
        {
            return SlotCount() + 1 < PARTY_MAX;
        }

        public bool AddPokemon(Pokemon pokemon)
        {
            if (!CanAddPokemon())
                return false;

            var slot = new PokeSlot(this, pokemon);
            GetSlots().Add(slot);

            if (selectedIndex == -1)
                GetPokeSlot(slot.index); //Select by default if no pokemon is selectedIndex

            return true;
        }

        public void RemovePokemon(int index)
        {
            var slot = GetSlot(index);
            slots.RemoveAt(index);

            if (selectedIndex == index) //If the current Pokemon was removed, select the previous. (If there are none left, it will set it correctly to -1)
                GetPokeSlot(SlotCount() - 1);
        }
        public int GetSelectedIndex()
        {
            return selectedIndex;
        }
        public int GetActiveSlotIndex()
        {
            return selectedIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PokeSlot GetPokeSlot(int index)
        {
            if (index < 0 || index >= SlotCount())
            { //Check if it's an invalid slot
                selectedIndex = -1;
                return null;
            }

            var slot = GetSlots()[index];
            selectedIndex = index;

            return slot;
        }
        public Pokemon GetPokeBySlot(int index)
        {
            if (index < 0 || index >= SlotCount())
            { //Check if it's an invalid slot
                selectedIndex = -1;
                return null;
            }

            var slot = GetSlots()[index];

            return slot.pokemon;
        }

        public PokeSlot SelectNext()
        {
            var index = (selectedIndex - 1) % SlotCount();  //Loop slot index when beyond bounds
            return GetPokeSlot(index);
        }

        public PokeSlot SelectPrev()
        {
            var index = ((selectedIndex - 1) + SlotCount() - 1) % SlotCount();  //Loop slot index when below bounds
            return GetPokeSlot(index);
        }

        public void Swap(int index1, int index2)
        {
            var slots = GetSlots();

            if (System.Diagnostics.Debugger.IsAttached)
            {
                if ((index1 < 0 || index1 >= SlotCount()) || (index2 < 0 || index2 >= SlotCount()))
                    throw new Exception("Error: A PokeParty swap index is invalid.");
            }

            PokeSlot slot = slots[index1];
            slots[index1] = slots[index2];
            slots[index2] = slot;
        }

        public Pokemon GetPokemon(int id)
        {
            foreach (var slot in slots)
            {
                if (slot.pokemon.number == id)
                    return slot.pokemon;
            }

            return null;
        }

        public class PokeSlot
        {
            public int index
            {
                get { return pokeParty.GetSlots().FindIndex(v => (v != null) ? v == this : false); }
                set { }
            } //Directly refer to List<> for the index
            public Pokemon pokemon;

            private PokeParty pokeParty;

            public PokeSlot(PokeParty pokeParty, Pokemon pokemon)
            {
                this.pokeParty = pokeParty;
                this.index = -1;
                this.pokemon = pokemon;
            }
        }

        public string GetPokeSlotName(int index)
        {
            return GetPokeSlot(index).pokemon.name;
        }

        public string GetPokeSlotIcon(int index)
        {
            return GetPokeSlot(index).pokemon.iconName;
        }

        public int GetPokeSlotLevel(int index)
        {
            return GetPokeSlot(index).pokemon.level;
        }
    }
}
