namespace wubzy.Shared;

public class Line
{
    public IList<(double X, double Y)> Points { get; set; }
    public string Color { get; set; }
    public double Thickness { get; set; }

    public Line(string color, double thickness)
    {
        Points = new List<(double X, double Y)>();
        Color = color;
        Thickness = thickness;
    }

    public void AddPoint(double x, double y)
    {
        Points.Add((x, y));
    }
}