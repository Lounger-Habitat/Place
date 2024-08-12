[System.Serializable]
public class Instruction {

    public string mode;
    public int x;
    public int y;
    public int dx;
    public int dy;
    public int ex;
    public int ey;
    public int r;
    public int g;
    public int b;
    public string description;

    public int needInkCount;

    public Instruction(string c, int x=0, int y=0, int dx=0, int dy=0, int ex=0, int ey=0,int r=0, int g=0, int b=0)
    {
        this.mode = c;
        this.x = x;
        this.y = y;
        this.dx = dx;
        this.dy = dy;
        this.ex = ex;
        this.ey = ey;
        this.r = r;
        this.g = g;
        this.b = b;
        this.needInkCount = 0;
    }
}