[System.Serializable]
public class Instruction {

    public string mode;
    public int x;
    public int y;
    public int r;
    public int g;
    public int b;
    public string description;

    public Instruction(string c, int x, int y, int r, int g, int b)
    {
        this.mode = c;
        this.x = x;
        this.y = y;
        this.r = r;
        this.g = g;
        this.b = b;
    }
}