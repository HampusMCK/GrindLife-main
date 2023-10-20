using System.Numerics;
using GrindLife;
using Pc;
using Raylib_cs;

Raylib.SetTargetFPS(60);
Raylib.InitWindow(1400, 900, "The Grind Life");
int pcSc = Raylib.GetCurrentMonitor();
float screenRatio = (float)Raylib.GetMonitorWidth(pcSc) / (float)Raylib.GetMonitorHeight(pcSc);
float setWidth = Raylib.GetMonitorWidth(pcSc) - (Raylib.GetMonitorHeight(pcSc) / 8) * screenRatio;
float setHeight = Raylib.GetMonitorHeight(pcSc) - Raylib.GetMonitorHeight(pcSc) / 8;
int width = (int)setWidth;
int height = (int)setHeight;
int wpx = (Raylib.GetMonitorWidth(pcSc) - width) / 2;
int wpy = (Raylib.GetMonitorHeight(pcSc) - height) / 2;
Raylib.SetWindowSize(width, height);
Raylib.SetWindowPosition(wpx, wpy);

int titleFont = height / 20;
int smallFont = height / 45;
int bigFont = height / 18;


string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
string filePath = Path.Combine(path, "worlds.txt");
Worlds level = new();
Store ica = new();
bool jumping = false;
bool showInv = false;
bool isMap = false;
bool mineMenu = false;
bool automanu = false;
bool autobuy = false;
float timer = 0;
float clock = 0;
int ch = 0;
int cm = 0;
Rectangle createButton = new Rectangle(width / 8, height - (height / 8), width / 7, height / 12);
Rectangle loadButton = new();
Rectangle watch = new Rectangle(width - (Raylib.MeasureText("00:00", smallFont) + (smallFont / 2)), 10, Raylib.MeasureText("00:00", smallFont) + (smallFont / 4), smallFont + (smallFont / 4));
Vector2 mousePos = new Vector2();
int playerWidth = width / 28;
string sceene = "start";
Random gen = new();
bool inLevel = false;
bool inComp = false;
string newName = "";
string compName = "";
string sell = "";
int currentLevel = 0;
int currentBuissness = 0;
int matprice;
string time = $"";
string ab = "OFF";
string am = "OFF";
if (File.Exists(filePath))
{
    load();
}
else
{
    save();
}
while (!Raylib.WindowShouldClose())
{
    if (inComp || inLevel)
    {
        clock += Raylib.GetFrameTime();
        if (clock >= 5)
        {
            cm += 10;
            clock = 0;
        }
        if (cm == 60)
        {
            ch++;
            cm = 0;
        }
        if (ch == 24)
        {
            ch = 0;
        }
        time = $"{ch}:{cm}";
    }
    mousePos = Raylib.GetMousePosition();
    Raylib.BeginDrawing();
    if (!inLevel && !inComp)
    {
        if (sceene == "start")
        {
            Raylib.ClearBackground(Color.BLUE);
            Raylib.DrawText("Load level or Create New", (width / 2) - Raylib.MeasureText("Load level or Create New", titleFont) / 2, 30, titleFont, Color.BLACK);
            Raylib.DrawRectangleRec(createButton, Color.GRAY);
            Raylib.DrawRectangleLinesEx(createButton, 2, Color.BLACK);
            Raylib.DrawText("Create new World", (width / 8) + 5, (height - (height / 8)) + 5, (width / 7) / 11, Color.BLACK);
            if (Raylib.CheckCollisionPointRec(mousePos, createButton))
            {
                Raylib.DrawRectangleRec(createButton, Color.SKYBLUE);
                Raylib.DrawRectangleLinesEx(createButton, 3, Color.BLACK);
                Raylib.DrawText("Create new World", (width / 8) + 5, (height - (height / 8)) + 5, (width / 7) / 11, Color.RED);
                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    sceene = "newWorld";
                }
            }
            if (level.worldList.Count > 0)
            {
                int recPosx = (width / 4) + (width / 7);
                int recPosy = height - (height / 8);
                List<Rectangle> loadList = new();
                for (int i = 0; i < level.worldList.Count; i++)
                {
                    if (i < 2 && i > 0)
                    {
                        recPosx += (width / 8) + (width / 7);
                    }
                    else if (i == 2)
                    {
                        recPosy -= (height / 12) + (height / 24);
                    }
                    else if (i > 2)
                    {
                        recPosx -= (width / 8) + (width / 7);
                    }
                    loadButton = new Rectangle(recPosx, recPosy, width / 7, height / 12);
                    loadList.Add(loadButton);
                }
                recPosy = height - (height / 8);
                recPosx = (width / 4) + (width / 7);
                for (int i = 0; i < loadList.Count; i++)
                {
                    if (i < 2 && i > 0)
                    {
                        recPosx += (width / 8) + (width / 7);
                    }
                    else if (i == 2)
                    {
                        recPosy -= (height / 12) + (height / 24);
                    }
                    else if (i > 2)
                    {
                        recPosx -= (width / 8) + (width / 7);
                    }
                    Raylib.DrawRectangleRec(loadList[i], Color.GRAY);
                    Raylib.DrawRectangleLinesEx(loadList[i], 2, Color.BLACK);
                    Raylib.DrawText(level.worldList[i].name, recPosx + 5, recPosy + 5, (width / 7) / 9, Color.BLACK);
                    if (Raylib.CheckCollisionPointRec(mousePos, loadList[i]))
                    {
                        Raylib.DrawRectangleRec(loadList[i], Color.SKYBLUE);
                        Raylib.DrawRectangleLinesEx(loadList[i], 3, Color.BLACK);
                        Raylib.DrawText(level.worldList[i].name, recPosx + 5, recPosy + 5, (width / 7) / 9, Color.RED);
                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                        {
                            sceene = $"{level.worldList[i].name}";
                            level.worldList[i].screen = "start";
                            inLevel = true;
                        }
                    }
                }
            }
        }

        if (sceene == "newWorld")
        {
            Raylib.ClearBackground(Color.GREEN);
            Raylib.DrawText("Name your world", (width / 2) - Raylib.MeasureText("Name your world", titleFont) / 2, 30, titleFont, Color.BLACK);
            Rectangle createNewButton = new Rectangle((width / 2) - width / 7 / 2, height - (height / 8), width / 7, height / 12);
            int key = Raylib.GetKeyPressed();
            if (key != 0)
            {
                if (key == 13)
                {
                    createNewWorld();
                }
                if (key == 259)
                {
                    try
                    {
                        newName = newName.Substring(0, newName.Length - 1);
                    }
                    catch
                    {
                        newName = newName.Substring(0, newName.Length);
                    }
                }
                else if (newName.Length < 12)
                {
                    newName += (char)key;
                }
            }
            Raylib.DrawRectangle((width / 2) - (((bigFont * 8) + 10) / 2) - 10, (height / 2) - ((bigFont / 2) - 25), bigFont * 9, bigFont + 5, Color.GRAY);
            Raylib.DrawText(newName, (width / 2) - Raylib.MeasureText(newName, bigFont) / 2, (height / 2) - ((bigFont / 2) - 25) + (bigFont / 8), bigFont, Color.BLACK);
            Raylib.DrawRectangleRec(createNewButton, Color.GRAY);
            Raylib.DrawRectangleLinesEx(createNewButton, 2, Color.BLACK);
            Raylib.DrawText("Create", (width / 2) - (width / 7 / 2) + 5, height - (height / 8) + width / 7 / 16, width / 7 / 4, Color.BLACK);
            if (Raylib.CheckCollisionPointRec(mousePos, createNewButton))
            {

                Raylib.DrawRectangleRec(createNewButton, Color.SKYBLUE);
                Raylib.DrawRectangleLinesEx(createNewButton, 3, Color.BLACK);
                Raylib.DrawText("Create", ((width / 2) - ((width / 7) / 2)) + 5, (height - (height / 8)) + (width / 7) / 16, (width / 7) / 4, Color.RED);
                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    createNewWorld();
                }
            }
        }
    }

    if (inLevel)
    {
        for (int i = 0; i < level.worldList.Count; i++)
        {
            if (sceene == level.worldList[i].name)
            {
                currentLevel = i;
            }
        }
        Vector2 l = new Vector2(level.worldList[currentLevel].character.location.X, level.worldList[currentLevel].character.location.Y - 120);
        Rectangle Player = new Rectangle((int)level.worldList[currentLevel].character.location.X, (int)level.worldList[currentLevel].character.location.Y, playerWidth, playerWidth);
        Raylib.DrawText($"${level.worldList[currentLevel].character.money}", width - Raylib.MeasureText($"${level.worldList[currentLevel].character.money}", smallFont) - 10, 50, smallFont, Color.BLACK);

        if (level.worldList[currentLevel].screen == "start")
        {
            Raylib.ClearBackground(Color.RED);
            Raylib.DrawText("Press 'M' to Open Map", (width / 2) - (Raylib.MeasureText("Press 'M' to Open Map", titleFont) / 2), 50, titleFont, Color.BLACK);
            Raylib.DrawTextureEx(level.worldList[currentLevel].character.CIMG, l, 0, 0.2f, Color.BLUE);
            if (level.worldList[currentLevel].character.hand != null)
            {
                Raylib.DrawText($"{level.worldList[currentLevel].character.hand.name}", (int)level.worldList[currentLevel].character.location.X, (int)level.worldList[currentLevel].character.location.Y, smallFont, Color.WHITE);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_J))
            {
                level.worldList[currentLevel].character.inventory.Add(new Items { name = "Pickaxe", description = "Tool to mine", hitPower = 15 });
            }
            Vector2 hotPos = new Vector2(20, 50);
            for (int i = 0; i < level.worldList[currentLevel].character.inventory.Count; i++)
            {
                Raylib.DrawText($"{i + 1}. {level.worldList[currentLevel].character.inventory[i].name}", (int)hotPos.X, (int)hotPos.Y, smallFont, Color.BLACK);
                hotPos.Y += smallFont;
            }
            if (level.worldList[currentLevel].character.inventory.Count > 9)
            {
                level.worldList[currentLevel].character.hiddenInventory.Add(level.worldList[currentLevel].character.inventory[8]);
                level.worldList[currentLevel].character.inventory.RemoveAt(8);
            }
            if (!isMap)
            {
                putInHand();
                showInventory();
                move();
            }
            showMap();
        }

        if (level.worldList[currentLevel].screen == "mine")
        {
            Raylib.ClearBackground(Color.DARKGRAY);
            Rectangle pc = new Rectangle(width - (width / 10), height - (width / 14), playerWidth, playerWidth);
            Raylib.DrawRectangleRec(pc, Color.PURPLE);
            Raylib.DrawTextureEx(level.worldList[currentLevel].character.CIMG, l, 0, 0.2f, Color.BLUE);
            if (Raylib.CheckCollisionRecs(Player, pc))
            {
                Raylib.DrawText("Press 'E' to open 'Mine Menu'!", width - Raylib.MeasureText("Press 'E' to open 'Mine Menu'!", smallFont) - 10, (height - (width / 14)) - smallFont * 2, smallFont, Color.GREEN);
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_E))
                {
                    mineMenu = true;
                }
            }
            Raylib.DrawText("Press 'P' to mine!", width / 2 - Raylib.MeasureText("Press 'P' to mine!", titleFont) / 2, 40, titleFont, Color.WHITE);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
            {
                level.worldList[currentLevel].character.money += gen.Next(level.worldList[currentLevel].character.hand.hitPower + 5);
            }
            Vector2 hotPos = new Vector2(20, 50);
            for (int i = 0; i < level.worldList[currentLevel].character.inventory.Count; i++)
            {
                Raylib.DrawText($"{i + 1}. {level.worldList[currentLevel].character.inventory[i].name}", (int)hotPos.X, (int)hotPos.Y, smallFont, Color.BLACK);
                hotPos.Y += smallFont * 2;
            }
            if (!isMap)
            {
                mm();
                putInHand();
                showInventory();
                move();
            }
            showMap();
        }

        if (level.worldList[currentLevel].screen == "store")
        {
            Raylib.ClearBackground(Color.GOLD);
            Raylib.DrawText("What Would You Like To Buy?", (width / 2) - Raylib.MeasureText("What Would You Like To Buy?", titleFont) / 2, 50, titleFont, Color.BLACK);
            Vector2 hotPos = new Vector2(20, 50);
            for (int i = 0; i < level.worldList[currentLevel].character.inventory.Count; i++)
            {
                Raylib.DrawText($"{i + 1}. {level.worldList[currentLevel].character.inventory[i].name}", (int)hotPos.X, (int)hotPos.Y, smallFont, Color.BLACK);
                hotPos.Y += smallFont * 2;
            }
            Store();
            showInventory();
            showMap();
        }

        if (level.worldList[currentLevel].screen == "cb")
        {
            Raylib.ClearBackground(Color.PURPLE);
            Raylib.DrawText("Name Your Company!", (width / 2) - (Raylib.MeasureText("Name Your Company!", titleFont) / 2), 50, titleFont, Color.BLACK);
            createBuisness();
        }

    }

    if (inComp)
    {
        for (int i = 0; i < level.worldList[currentLevel].character.company.Count; i++)
        {
            if (level.worldList[currentLevel].screen == level.worldList[currentLevel].character.company[i].name)
            {
                currentBuissness = i;
            }
        }
        int sel = gen.Next(level.worldList[currentLevel].character.company[currentBuissness].price * (500 - (level.worldList[currentLevel].character.company[currentBuissness].level * 10)));
        if (sel < level.worldList[currentLevel].character.company[currentBuissness].price && level.worldList[currentLevel].character.company[currentBuissness].stock > 0)
        {
            level.worldList[currentLevel].character.company[currentBuissness].money += level.worldList[currentLevel].character.company[currentBuissness].price;
            level.worldList[currentLevel].character.company[currentBuissness].sales++;
            level.worldList[currentLevel].character.company[currentBuissness].stock--;
            level.worldList[currentLevel].character.company[currentBuissness].profit += level.worldList[currentLevel].character.company[currentBuissness].price;
        }
        matprice = 30 + (15 * level.worldList[currentLevel].character.company[currentBuissness].matbuy);
        int prof = level.worldList[currentLevel].character.company[currentBuissness].profit - level.worldList[currentLevel].character.company[currentBuissness].spent;
        Raylib.ClearBackground(Color.LIGHTGRAY);
        if (level.worldList[currentLevel].character.company[currentBuissness].item.Length < 1)
        {
            fc();
        }
        else
        {
            int levelupPrice = 150 + (50 * level.worldList[currentLevel].character.company[currentBuissness].level);
            Raylib.DrawText(level.worldList[currentLevel].character.company[currentBuissness].name, (width / 2) - (Raylib.MeasureText(level.worldList[currentLevel].character.company[currentBuissness].name, titleFont) / 2), 50, titleFont, Color.BLACK);
            Raylib.DrawText($"We Sell: {level.worldList[currentLevel].character.company[currentBuissness].item}", (width / 2) - (Raylib.MeasureText($"We Sell: {level.worldList[currentLevel].character.company[currentBuissness].item}", smallFont) / 2), 50 + (titleFont + 10), smallFont, Color.BLACK);
            Raylib.DrawText($"Current level: {level.worldList[currentLevel].character.company[currentBuissness].level}", (width / 2) - (Raylib.MeasureText($"Current level: {level.worldList[currentLevel].character.company[currentBuissness].level}", smallFont) / 2), 50 + (titleFont + smallFont * 2 + 10), smallFont, Color.BLACK);
            Raylib.DrawText($"{level.worldList[currentLevel].character.company[currentBuissness].name} money: ${level.worldList[currentLevel].character.company[currentBuissness].money}", width - Raylib.MeasureText($"{level.worldList[currentLevel].character.company[currentBuissness].name} money: ${level.worldList[currentLevel].character.company[currentBuissness].money}", smallFont) - 20, 50, smallFont, Color.BLACK);
            Raylib.DrawText($"Your money: ${level.worldList[currentLevel].character.money}", width - Raylib.MeasureText($"Your money: ${level.worldList[currentLevel].character.money}", smallFont) - 20, 50 + smallFont * 2, smallFont, Color.BLACK);
            Raylib.DrawText($"{level.worldList[currentLevel].character.company[currentBuissness].name} has sold {level.worldList[currentLevel].character.company[currentBuissness].sales} Units", width / 2, height / 2, titleFont, Color.BLACK);
            Raylib.DrawText($"{level.worldList[currentLevel].character.company[currentBuissness].name}'s profit Is: ${prof}", width / 2, (height / 2) - (titleFont * 2), titleFont, Color.BLACK);
            Raylib.DrawText($"{level.worldList[currentLevel].character.company[currentBuissness].name}'s stock is: {level.worldList[currentLevel].character.company[currentBuissness].stock}", width / 2, (height / 2) + (titleFont * 2), titleFont, Color.BLACK);

            if (!isMap)
            {
                compButts();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_J))
            {
                level.worldList[currentLevel].character.company[currentBuissness].material += 5;
            }

            showMap();
        }
    }
    if (inLevel || inComp)
    {
        timer += Raylib.GetFrameTime();
        float top = (12 - level.worldList[currentLevel].character.mineLevel) / 2;
        if (timer >= top)
        {
            level.worldList[currentLevel].character.mineMoney += level.worldList[currentLevel].character.workers * level.worldList[currentLevel].character.mineLevel;
            timer = 0;
        }
        if (level.worldList[currentLevel].character.company.Count > 0)
        {
            for (int i = 0; i < level.worldList[currentLevel].character.company.Count; i++)
            {

                matprice = 30 + (15 * level.worldList[currentLevel].character.company[i].matbuy);
                if (level.worldList[currentLevel].character.company[i].workers > 0 && automanu)
                {
                    int make = gen.Next(1500);
                    if (make <= level.worldList[currentLevel].character.company[i].workers * level.worldList[currentLevel].character.company[i].sallery && level.worldList[currentLevel].character.company[i].material > 4)
                    {
                        Manu();
                    }
                }
                if (autobuy && level.worldList[currentLevel].character.company[i].material < 5)
                {
                    for (int l = 0; l < 10; l++)
                    {
                        if (level.worldList[currentLevel].character.company[i].money - matprice > 0)
                        {
                            buyMat();
                        }
                    }
                }
                if (level.worldList[currentLevel].character.company[i].workers > 0 && ch == 23 && cm == 50 && clock == 0)
                {
                    level.worldList[currentLevel].character.company[i].money -= level.worldList[currentLevel].character.company[i].sallery * level.worldList[currentLevel].character.company[i].workers;
                }
            }
        }
        Raylib.DrawRectangleRec(watch, Color.BLACK);
        Raylib.DrawRectangleLinesEx(watch, 3, Color.BLUE);
        Raylib.DrawText(time, (width - (Raylib.MeasureText("00:00", smallFont) + (smallFont / 4))) + 5, 10 + (smallFont / 4), smallFont, Color.WHITE);
    }
    save();
    Raylib.EndDrawing();
}






//-------------------------------------------- Voids --------------------------------------------------------------------------------










void compButts()
{
    int levelupPrice = 150 + (50 * level.worldList[currentLevel].character.company[currentBuissness].level);
    List<string> buttsT = new()
            {
                $"Hire!\n\nEmployed: {level.worldList[currentLevel].character.company[currentBuissness].workers}",
                "Withdraw",
                "Deposit",
                $"Price +$50\n\nCurrent: ${level.worldList[currentLevel].character.company[currentBuissness].price}",
                $"Price -$50\n\nCurrent: ${level.worldList[currentLevel].character.company[currentBuissness].price}",
                $"Salary +$5\n\nCurrent: ${level.worldList[currentLevel].character.company[currentBuissness].sallery}",
                $"Salary -$5\n\nCurrent: ${level.worldList[currentLevel].character.company[currentBuissness].sallery}",
                $"Upgrade! ${levelupPrice}",
                $"Buy Material ${matprice}",
                $"Manufacture, {level.worldList[currentLevel].character.company[currentBuissness].material}",
                $"Auto-Buy: {ab}",
                $"Auto-Manufacture:\n{am}"
            };
    List<Action> butt = new()
    {
        hire,
        withdraw,
        deposit,
        upPrice,
        downPrice,
        upSal,
        downSal,
        upgrade,
        buyMat,
        Manu,
        autoBuy,
        autoManu
    };
    int posx = width / 21;
    int posy = height / 24;
    for (int i = 0; i < buttsT.Count; i++)
    {
        Rectangle y = new Rectangle(posx, posy, width / 7, height / 12);
        Raylib.DrawRectangleRec(y, Color.GRAY);
        Raylib.DrawRectangleLinesEx(y, 2, Color.BLACK);
        Raylib.DrawText($"{buttsT[i]}", (posx + ((width / 7) / 2)) - (Raylib.MeasureText($"{buttsT[i]}", smallFont) / 2), (posy + (height / 24)) - (smallFont / 2), smallFont, Color.BLACK);
        if (Raylib.CheckCollisionPointRec(mousePos, y))
        {
            Raylib.DrawRectangleRec(y, Color.SKYBLUE);
            Raylib.DrawRectangleLinesEx(y, 3, Color.BLACK);
            Raylib.DrawText($"{buttsT[i]}", (posx + ((width / 7) / 2)) - (Raylib.MeasureText($"{buttsT[i]}", smallFont) / 2), (posy + (height / 24)) - (smallFont / 2), smallFont, Color.RED);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                butt[i]();
            }
        }
        posy += height / 6;
        if (i == 5)
        {
            posx += (width / 7) + (width / 21);
            posy = height / 24;
        }
    }
}

void hire()
{
    level.worldList[currentLevel].character.company[currentBuissness].workers++;
}
void withdraw()
{
    level.worldList[currentLevel].character.money += level.worldList[currentLevel].character.company[currentBuissness].money;
    level.worldList[currentLevel].character.company[currentBuissness].money = 0;
}
void deposit()
{
    level.worldList[currentLevel].character.company[currentBuissness].money += level.worldList[currentLevel].character.money;
    level.worldList[currentLevel].character.money = 0;
}
void buyMat()
{
    level.worldList[currentLevel].character.company[currentBuissness].money -= matprice;
    if (level.worldList[currentLevel].character.company[currentBuissness].money < 0)
    {
        level.worldList[currentLevel].character.company[currentBuissness].money += matprice;
    }
    else
    {
        level.worldList[currentLevel].character.company[currentBuissness].material++;
        level.worldList[currentLevel].character.company[currentBuissness].matbuy++;
        level.worldList[currentLevel].character.company[currentBuissness].spent += matprice;
    }
}
void Manu()
{
    if (level.worldList[currentLevel].character.company[currentBuissness].material > 4)
    {
        level.worldList[currentLevel].character.company[currentBuissness].stock++;
        level.worldList[currentLevel].character.company[currentBuissness].material -= 5;
        level.worldList[currentLevel].character.company[currentBuissness].made++;
    }
}
void upPrice()
{
    level.worldList[currentLevel].character.company[currentBuissness].price += 50;
}
void downPrice()
{
    level.worldList[currentLevel].character.company[currentBuissness].price -= 50;
}
void upSal()
{
    level.worldList[currentLevel].character.company[currentBuissness].sallery += 5;
}
void downSal()
{
    level.worldList[currentLevel].character.company[currentBuissness].sallery -= 5;
}
void autoBuy()
{
    if (!autobuy)
    {
        autobuy = true;
        ab = "ON";
    }
    else if (autobuy)
    {
        autobuy = false;
        ab = "OFF";
    }
}
void autoManu()
{
    if (!automanu)
    {
        automanu = true;
        am = "ON";
    }
    else if (automanu)
    {
        automanu = false;
        am = "OFF";
    }
}
void upgrade()
{
    int levelupPrice = 150 + (50 * level.worldList[currentLevel].character.company[currentBuissness].level);
    if (level.worldList[currentLevel].character.company[currentBuissness].level < 46 && level.worldList[currentLevel].character.company[currentBuissness].money - levelupPrice > -1)
    {
        level.worldList[currentLevel].character.company[currentBuissness].money -= levelupPrice;
        level.worldList[currentLevel].character.company[currentBuissness].level++;
    }
}

void fc()
{
    Rectangle next = new Rectangle((width / 2) - (width / 14), height - (height / 10), width / 7, height / 12);
    Raylib.DrawText("What are you selling?", (width / 2) - (Raylib.MeasureText("What are you selling?", titleFont) / 2), 50, titleFont, Color.BLACK);
    Raylib.DrawRectangle((width / 2) - (width / 6), (height / 2) - ((bigFont / 2) + 10), width / 3, bigFont + 20, Color.SKYBLUE);
    Raylib.DrawText(sell, (width / 2) - (Raylib.MeasureText(sell, bigFont) / 2), (height / 2) - (bigFont / 2), bigFont, Color.BLACK);
    Raylib.DrawRectangleRec(next, Color.GRAY);
    Raylib.DrawRectangleLinesEx(next, 2, Color.BLACK);
    Raylib.DrawText("NEXT", ((width / 2) - (width / 14)) + 5, (height - (height / 10)) + 5, smallFont, Color.BLACK);
    if (Raylib.CheckCollisionPointRec(mousePos, next))
    {
        Raylib.DrawRectangleRec(next, Color.SKYBLUE);
        Raylib.DrawRectangleLinesEx(next, 3, Color.BLACK);
        Raylib.DrawText("NEXT", ((width / 2) - (width / 14)) + 5, (height - (height / 10)) + 5, smallFont, Color.RED);
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            level.worldList[currentLevel].character.company[currentBuissness].item = sell;
            sell = "";
        }
    }
    int key = Raylib.GetKeyPressed();
    if (key != 0)
    {
        if (key == 13)
        {

        }
        if (key == 259)
        {
            try
            {
                sell = sell.Substring(0, sell.Length - 1);
            }
            catch
            {
                sell = sell.Substring(0, sell.Length);
            }
        }
        else if (sell.Length < 17)
        {
            sell += (char)key;
        }
    }
}

void mm()
{
    int price = 150 + (level.worldList[currentLevel].character.workers * 50);
    int minePrice = 200 + (level.worldList[currentLevel].character.mineLevel * 100);
    if (mineMenu)
    {
        Rectangle close = new Rectangle(width - ((width / 7) + (width / 14)), height - height / 10, width / 7, height / 12);
        Rectangle buy = new Rectangle(width / 7, height / 2 - height / 24, width / 7, height / 12);
        Rectangle collect = new Rectangle((width / 7) * 3, height / 2 - height / 24, width / 7, height / 12);
        Rectangle upgrade = new Rectangle((width / 7) * 5, height / 2 - height / 24, width / 7, height / 12);
        Raylib.DrawRectangle(0, 0, width, height, Color.PURPLE);
        Raylib.DrawText($"${level.worldList[currentLevel].character.money}", width - Raylib.MeasureText($"${level.worldList[currentLevel].character.money}", smallFont) - 10, 50, smallFont, Color.BLACK);
        Raylib.DrawText($"Money to Collect: ${level.worldList[currentLevel].character.mineMoney}", width / 2 - Raylib.MeasureText($"Money to Collect: ${level.worldList[currentLevel].character.mineMoney}", titleFont) / 2, height / 4, titleFont, Color.BLACK);
        Raylib.DrawText($"Current Level: {level.worldList[currentLevel].character.mineLevel}", width / 2 - Raylib.MeasureText($"Current Level: {level.worldList[currentLevel].character.mineLevel}", titleFont) / 2, (height / 4) - (titleFont * 2), titleFont, Color.BLACK);
        Raylib.DrawRectangleRec(close, Color.GRAY);
        Raylib.DrawRectangleLinesEx(close, 2, Color.BLACK);
        Raylib.DrawText("CLOSE", (width - ((width / 7) + (width / 14))) + 5, (height - height / 10) + 5, smallFont, Color.BLACK);
        if (Raylib.CheckCollisionPointRec(mousePos, close))
        {
            Raylib.DrawRectangleRec(close, Color.SKYBLUE);
            Raylib.DrawRectangleLinesEx(close, 3, Color.BLACK);
            Raylib.DrawText("CLOSE", (width - ((width / 7) + (width / 14))) + 5, (height - height / 10) + 5, smallFont, Color.RED);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                mineMenu = false;
            }
        }
        Raylib.DrawRectangleRec(buy, Color.GRAY);
        Raylib.DrawRectangleLinesEx(buy, 2, Color.BLACK);
        Raylib.DrawText($"BUY WORKER, ${price}", (width / 7) + 5, (height / 2 - height / 24) + 5, smallFont, Color.BLACK);
        if (Raylib.CheckCollisionPointRec(mousePos, buy))
        {
            Raylib.DrawRectangleRec(buy, Color.SKYBLUE);
            Raylib.DrawRectangleLinesEx(buy, 3, Color.BLACK);
            Raylib.DrawText($"BUY WORKER, ${price}", (width / 7) + 5, (height / 2 - height / 24) + 5, smallFont, Color.RED);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                level.worldList[currentLevel].character.money -= price;
                if (level.worldList[currentLevel].character.money < 0)
                {
                    level.worldList[currentLevel].character.money += price;
                }
                else
                {
                    level.worldList[currentLevel].character.workers++;
                }
            }
        }
        Raylib.DrawRectangleRec(collect, Color.GRAY);
        Raylib.DrawRectangleLinesEx(collect, 2, Color.BLACK);
        Raylib.DrawText("COLLECT MONEY", ((width / 7) * 3) + 5, (height / 2 - height / 24) + 5, smallFont, Color.BLACK);
        if (Raylib.CheckCollisionPointRec(mousePos, collect))
        {
            Raylib.DrawRectangleRec(collect, Color.SKYBLUE);
            Raylib.DrawRectangleLinesEx(collect, 3, Color.BLACK);
            Raylib.DrawText("COLLECT MONEY", ((width / 7) * 3) + 5, (height / 2 - height / 24) + 5, smallFont, Color.RED);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                level.worldList[currentLevel].character.money += level.worldList[currentLevel].character.mineMoney;
                level.worldList[currentLevel].character.mineMoney = 0;
            }
        }
        if (level.worldList[currentLevel].character.mineLevel < 10)
        {
            Raylib.DrawRectangleRec(upgrade, Color.GRAY);
            Raylib.DrawRectangleLinesEx(upgrade, 2, Color.BLACK);
            Raylib.DrawText($"UPGRADE MINE, ${minePrice}", ((width / 7) * 5) + 5, (height / 2 - height / 24) + 5, smallFont, Color.BLACK);
            if (Raylib.CheckCollisionPointRec(mousePos, upgrade))
            {
                Raylib.DrawRectangleRec(upgrade, Color.SKYBLUE);
                Raylib.DrawRectangleLinesEx(upgrade, 3, Color.BLACK);
                Raylib.DrawText($"UPGRADE MINE, ${minePrice}", ((width / 7) * 5) + 5, (height / 2 - height / 24) + 5, smallFont, Color.RED);
                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    level.worldList[currentLevel].character.money -= minePrice;
                    if (level.worldList[currentLevel].character.money < 0)
                    {
                        level.worldList[currentLevel].character.money += minePrice;
                    }
                    else
                    {
                        level.worldList[currentLevel].character.mineLevel++;
                    }
                }
            }
        }
        else
        {
            Raylib.DrawRectangleRec(upgrade, Color.DARKGRAY);
            Raylib.DrawRectangleLinesEx(upgrade, 2, Color.BLACK);
            Raylib.DrawText($"MAXED!", ((width / 7) * 5) + 5, (height / 2 - height / 24) + 5, smallFont, Color.BLACK);
        }
    }
}

void Store()
{
    int bposx = width / 9;
    int bposy = height - (height / 4);
    for (int i = 0; i < 8; i++)
    {
        Rectangle bb = new Rectangle(bposx, bposy, width / 7, height / 12);
        Raylib.DrawRectangleRec(bb, Color.GRAY);
        Raylib.DrawRectangleLinesEx(bb, 2, Color.BLACK);
        Raylib.DrawText(ica.storeItems[i].name, (bposx + (width / 14)) - Raylib.MeasureText(ica.storeItems[i].name, smallFont) / 2, (bposy + (height / 24)) - smallFont / 2, smallFont, Color.BLACK);
        if (Raylib.CheckCollisionPointRec(mousePos, bb))
        {
            Raylib.DrawRectangleRec(bb, Color.SKYBLUE);
            Raylib.DrawRectangleLinesEx(bb, 3, Color.BLACK);
            Raylib.DrawText(ica.storeItems[i].name, (bposx + (width / 14)) - Raylib.MeasureText(ica.storeItems[i].name, smallFont) / 2, (bposy + (height / 24)) - smallFont / 2, smallFont, Color.RED);
            if (i < 4)
            {
                Raylib.DrawText($"{ica.storeItems[i].description}\nPower: {ica.storeItems[i].hitPower}, Price: ${ica.storeItems[i].price}", bposx, bposy + (height / 12), smallFont, Color.BLACK);
            }
            if (i > 3)
            {
                Raylib.DrawText($"{ica.storeItems[i].description}\nHealth Regain: {ica.storeItems[i].heal}, Price: ${ica.storeItems[i].price}", bposx, bposy + (height / 12), smallFont, Color.BLACK);
            }
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                level.worldList[currentLevel].character.money -= ica.storeItems[i].price;
                if (level.worldList[currentLevel].character.money < 0)
                {
                    level.worldList[currentLevel].character.money += ica.storeItems[i].price;
                }
                else
                {
                    level.worldList[currentLevel].character.inventory.Add(ica.storeItems[i]);
                }
            }
        }
        if (i < 3)
        {
            bposx += (width / 7) + (width / 7 / 2);
        }
        if (i > 2)
        {
            bposy = (height - (height / 4)) - (height / 6);
        }
        if (i > 3)
        {
            bposx -= (width / 7) + ((width / 7) / 2);
        }
    }
}

void putInHand()
{
    int key = Raylib.GetKeyPressed();
    if (key > 48 && key < 58 && key - 49 < level.worldList[currentLevel].character.inventory.Count)
    {
        level.worldList[currentLevel].character.hand = level.worldList[currentLevel].character.inventory[key - 49];
    }
}

void createBuisness()
{
    Rectangle create = new Rectangle((width / 2) - (width / 7), height - (height / 10), width / 7, height / 12);
    Raylib.DrawRectangle(width / 4, (height / 2) - (bigFont / 2) - 10, width / 2, bigFont * 2 + 20, Color.GRAY);
    Raylib.DrawText(compName, (width / 2) - (Raylib.MeasureText(compName, bigFont) / 2), (height / 2) - (bigFont / 2), bigFont, Color.BLACK);
    Raylib.DrawRectangleRec(create, Color.GRAY);
    Raylib.DrawRectangleLinesEx(create, 2, Color.BLACK);
    Raylib.DrawText("Create", ((width / 2) - (width / 7)) + 5, (height - (height / 10)) + 5, smallFont, Color.BLACK);
    if (Raylib.CheckCollisionPointRec(mousePos, create))
    {
        Raylib.DrawRectangleRec(create, Color.SKYBLUE);
        Raylib.DrawRectangleLinesEx(create, 3, Color.BLACK);
        Raylib.DrawText("Create", ((width / 2) - (width / 7)) + 5, (height - (height / 10)) + 5, smallFont, Color.RED);
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            level.worldList[currentLevel].character.company.Add(new Buisness { name = compName });
            level.worldList[currentLevel].screen = compName;
            inLevel = false;
            inComp = true;
            compName = "";
        }
    }
    int key = Raylib.GetKeyPressed();
    if (key != 0)
    {
        if (key == 13)
        {

        }
        if (key == 259)
        {
            try
            {
                compName = compName.Substring(0, compName.Length - 1);
            }
            catch
            {
                compName = compName.Substring(0, compName.Length);
            }
        }
        else if (compName.Length < 12)
        {
            compName += (char)key;
        }
    }
}

void showMap()
{
    if (Raylib.IsKeyPressed(KeyboardKey.KEY_M))
    {
        if (isMap)
        {
            isMap = false;
        }
        else
        {
            isMap = true;
        }
    }
    if (isMap)
    {
        Rectangle cb = new Rectangle(width - (width / 4), (height - (height / 4)) - (height / 9), width / 7, height / 12);
        Rectangle mineButton = new Rectangle(width / 8, height - (height / 4), width / 7, height / 12);
        Rectangle startButton = new Rectangle(width - (width / 4), height - (height / 4), width / 7, height / 12);
        Rectangle shopButton = new Rectangle(width / 8, (height - (height / 4)) - (height / 9), width / 7, height / 12);
        Raylib.DrawRectangle(50, 50, width - 100, height - 100, Color.WHITE);
        Raylib.DrawRectangleRec(mineButton, Color.GRAY);
        Raylib.DrawRectangleLinesEx(mineButton, 2, Color.BLACK);
        Raylib.DrawText("Mine", (width / 8) + 5, (height - (height / 4)) + 5, smallFont, Color.BLACK);
        Raylib.DrawRectangleRec(startButton, Color.GRAY);
        Raylib.DrawRectangleLinesEx(startButton, 2, Color.BLACK);
        Raylib.DrawText("Start", width - (width / 4) + 5, (height - (height / 4)) + 5, smallFont, Color.BLACK);
        Raylib.DrawRectangleRec(shopButton, Color.GRAY);
        Raylib.DrawRectangleLinesEx(shopButton, 2, Color.BLACK);
        Raylib.DrawText("Store", (width / 8) + 5, ((height - (height / 4)) - (height / 9)) + 5, smallFont, Color.BLACK);
        Raylib.DrawRectangleRec(cb, Color.GRAY);
        Raylib.DrawRectangleLinesEx(cb, 2, Color.BLACK);
        Raylib.DrawText("New Buissness, $750", (width - (width / 4)) + 5, ((height - (height / 4)) - (height / 9)) + 5, smallFont, Color.BLACK);
        if (Raylib.CheckCollisionPointRec(mousePos, cb))
        {
            Raylib.DrawRectangleRec(cb, Color.SKYBLUE);
            Raylib.DrawRectangleLinesEx(cb, 3, Color.BLACK);
            Raylib.DrawText("New Buissness, $750", (width - (width / 4)) + 5, ((height - (height / 4)) - (height / 9)) + 5, smallFont, Color.RED);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && level.worldList[currentLevel].character.money - 750 > -1)
            {
                level.worldList[currentLevel].character.money -= 750;
                level.worldList[currentLevel].screen = "cb";
                isMap = false;
                inLevel = true;
                inComp = false;
            }
        }
        if (Raylib.CheckCollisionPointRec(mousePos, mineButton))
        {
            Raylib.DrawRectangleRec(mineButton, Color.SKYBLUE);
            Raylib.DrawRectangleLinesEx(mineButton, 3, Color.BLACK);
            Raylib.DrawText("Mine", (width / 8) + 5, (height - (height / 4)) + 5, smallFont, Color.RED);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                level.worldList[currentLevel].screen = "mine";
                isMap = false;
                inLevel = true;
                inComp = false;
            }
        }
        if (Raylib.CheckCollisionPointRec(mousePos, startButton))
        {
            Raylib.DrawRectangleRec(startButton, Color.SKYBLUE);
            Raylib.DrawRectangleLinesEx(startButton, 3, Color.BLACK);
            Raylib.DrawText("Start", width - (width / 4) + 5, (height - (height / 4)) + 5, smallFont, Color.RED);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                level.worldList[currentLevel].screen = "start";
                isMap = false;
                inLevel = true;
                inComp = false;
            }
        }
        if (Raylib.CheckCollisionPointRec(mousePos, shopButton))
        {
            Raylib.DrawRectangleRec(shopButton, Color.SKYBLUE);
            Raylib.DrawRectangleLinesEx(shopButton, 3, Color.BLACK);
            Raylib.DrawText("Store", (width / 8) + 5, ((height - (height / 4)) - (height / 9)) + 5, smallFont, Color.RED);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                level.worldList[currentLevel].screen = "store";
                isMap = false;
                inLevel = true;
                inComp = false;
            }
        }
        int x = width / 18;
        int y = height / 18;
        for (int i = 0; i < level.worldList[currentLevel].character.company.Count; i++)
        {
            Rectangle r = new Rectangle(x, y, width / 7, height / 12);
            Raylib.DrawRectangleRec(r, Color.GRAY);
            Raylib.DrawRectangleLinesEx(r, 2, Color.BLACK);
            Raylib.DrawText(level.worldList[currentLevel].character.company[i].name, x + 5, y + 5, smallFont, Color.BLACK);
            if (Raylib.CheckCollisionPointRec(mousePos, r))
            {
                Raylib.DrawRectangleRec(r, Color.SKYBLUE);
                Raylib.DrawRectangleLinesEx(r, 3, Color.BLACK);
                Raylib.DrawText(level.worldList[currentLevel].character.company[i].name, x + 5, y + 5, smallFont, Color.RED);
                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    level.worldList[currentLevel].screen = level.worldList[currentLevel].character.company[i].name;
                    inLevel = false;
                    inComp = true;
                    isMap = false;
                }
            }
            x += (width / 7) + (width / 14);
        }
    }
}

void showInventory()
{
    if (Raylib.IsKeyPressed(KeyboardKey.KEY_I))
    {
        if (!showInv)
        {
            showInv = true;
        }
        else
        {
            showInv = false;
        }
    }
    if (showInv)
    {
        Vector2 textPos = new Vector2(width - width / 7, 100);
        for (int i = 0; i < level.worldList[currentLevel].character.hiddenInventory.Count; i++)
        {
            Raylib.DrawText(level.worldList[currentLevel].character.hiddenInventory[i].name, (int)textPos.X, (int)textPos.Y, smallFont, Color.BLACK);
            textPos.Y += smallFont * 2;
        }
    }
}



void createNewWorld()
{
    Vector2 startPos = new Vector2(20, height - (width / 14));
    level.worldList.Add(new worldInfo { name = newName, seed = gen.Next(1000), character = new Player { name = $"Character{level.worldList.Count}", location = startPos } });
    inLevel = true;
    sceene = $"{newName}";
    save();
}


void move()
{
    float fallingSpeed = height / 450;
    int speed = width / 700;
    if (level.worldList[currentLevel].character.location.Y < height - (width / 14))
    {
        fallingSpeed = (level.worldList[currentLevel].character.location.Y - (height - (height / 3))) / 75;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
    {
        level.worldList[currentLevel].character.location.X += speed;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
    {
        level.worldList[currentLevel].character.location.X -= speed;
    }
    if (level.worldList[currentLevel].character.location.Y < height - (width / 14) && !jumping)
    {
        level.worldList[currentLevel].character.location.Y += fallingSpeed;
    }
    if (level.worldList[currentLevel].character.location.Y > height - (width / 14))
    {
        level.worldList[currentLevel].character.location.Y = height - (width / 14);
    }
    if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) && level.worldList[currentLevel].character.location.Y == height - (width / 14))
    {
        jumping = true;
    }
    if (jumping)
    {
        level.worldList[currentLevel].character.location.Y -= fallingSpeed;
        if (level.worldList[currentLevel].character.location.Y <= (height - (width / 14)) - (width / 14))
        {
            jumping = false;
        }
    }
}



void save()
{
    var jsonSetting = new Newtonsoft.Json.JsonSerializerSettings
    {
        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
    };
    var serializedObject = Newtonsoft.Json.JsonConvert.SerializeObject(level, Newtonsoft.Json.Formatting.Indented, jsonSetting);
    using (StreamWriter sw = new StreamWriter(filePath))
    {
        sw.Write(serializedObject);
    }
}

void load()
{
    var jsonSetting = new Newtonsoft.Json.JsonSerializerSettings
    {
        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
    };
    string content = null;
    using (StreamReader sr = new StreamReader(filePath))
    {
        content = sr.ReadToEnd();
    }
    var loadedInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Worlds>(content, jsonSetting);
    level = loadedInfo;
}