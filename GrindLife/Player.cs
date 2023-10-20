using System.Numerics;
using GrindLife;
using Raylib_cs;

namespace Pc;

public class Player
{
    public Texture2D CIMG = Raylib.LoadTexture("pc.png");
    public string name;
    public int health = 100;
    public int strength = 5;
    public int money = 200;
    public int hunger = 0;
    public int energy = 500;
    public Vector2 location = new();
    public List<Items> inventory = new();
    public List<Items> hiddenInventory = new();
    public List<Food> ownedFood = new();
    public List<Buisness> company = new();
    public Items hand = new();
    public int workers = 0;
    public int mineMoney = 0;
    public int mineLevel = 1;
}