using Pc;
using Raylib_cs;

namespace GrindLife;

public class Store
{
    public List<Items> storeItems = new List<Items>{
    new Items{name = "Pickaxe", description = "Tool to Mine", hitPower = 15, price = 30, img = Raylib.LoadTexture("pc.png")},
    new Items{name = "Drill", description = "Drill to mine more", hitPower = 30, price = 60},
    new Items{name = "Sledge", description = "Break a wall to collect small items", hitPower = 25, price = 50},
    new Items{name = "TNT", description = "Blow a whole mine to gather more", hitPower = 60, price = 100},
    new Items{name = "Steak", description = "Eat to regain health and lower hunger", heal = 25, price = 30},
    new Items{name = "Hamburger", description = "Eat to regain health and lower hunger", heal = 15, price = 16},
    new Items{name = "Protein bar", description = "Eat to regain health and lower hunger", heal = 10, price = 8},
    new Items{name = "Banana", description = "Eat to regain health and lower hunger", heal = 5, price = 3},
};
}
