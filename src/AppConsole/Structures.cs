
using System.Drawing;

readonly struct Line
{
    public readonly Vector3 Start;
    public readonly Vector3 End;
    public Line(Vector3 start, Vector3 end)
    {
        Start = start;
        End = end;
    }
}
readonly struct Surface
{
    public readonly Line AnB;
    public readonly Line BnC;
    public readonly Line CnA;
    public Surface(in Line AnB, in Line BnC, in Line CnA)
    {
        this.AnB = AnB;
        this.BnC = BnC;
        this.CnA = CnA;

        if (AnB.End != BnC.Start ||
            BnC.End != CnA.Start ||
            CnA.End != AnB.Start
        )
        {
            throw new FormatException();
        }
    }
    public readonly SurfaceLinesIterator GetEnumerator()
        => new() {
            AnB = AnB,
            BnC = BnC,
            CnA = CnA,
        }
    ;
    public ref struct SurfaceLinesIterator
    {
        bool? state;
        public required Line AnB { get; init; }
        public required Line BnC { get; init; } 
        public required Line CnA { get; init; }
        public readonly Line Current => state switch
        {
            null  => AnB,
            false => BnC,
            true  => CnA,
        };
        public bool MoveNext()
        {
            switch (state)
            {
                case null:  state = false; return true;
                case false: state = true; return true;
                case true:  return false;
            }
        }
    }
}
readonly struct Triangle
{
    readonly float
        x0, y0, z0,
        x1, y1, z1,
        x2, y2, z2
    ;
    public Triangle(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        (x0, y0, z0) = (pointA.X, pointA.Y, pointA.Z);
        (x1, y1, z1) = (pointB.X, pointB.Y, pointB.Z);
        (x2, y2, z2) = (pointC.X, pointC.Y, pointC.Z);
    }
    public Triangle(
        float x0, float y0, float z0,
        float x1, float y1, float z1,
        float x2, float y2, float z2
    )
    {
        this.x0 = x0; this.y0 = y0; this.z0 = z0;
        this.x1 = x1; this.y1 = y1; this.z1 = z1;
        this.x2 = x2; this.y2 = y2; this.z2 = z2;
    }

    public (Line AnB, Line BnC, Line CnA) GetOutlines()
    {
        return (
            AnB: new(new(x0, y0, z0), new(x2, y2, z2)),
            BnC: new(new(x2, y2, z2), new(x1, y1, z1)),
            CnA: new(new(x1, y1, z1), new(x0, y0, z0))
        );
    }
    public Surface GetSurface()
    {
        var (a_b, b_c, c_a) = GetOutlines();
        return new(a_b, b_c, c_a);
    }
}