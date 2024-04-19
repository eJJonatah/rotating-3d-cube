
static partial class Program
{
    const int texture_len = 10; // sizeof(texture)
    const string texture = ".,~+:;%#$@";
    static void Render(Vector3 point)
    {
        Console.CursorLeft = (int)point.X;
        Console.CursorTop = (int)point.Y;
        Console.WriteLine(__getTextureBasedOnZAxis(point.Z));
    }
    static void Render(IEnumerable<Vector3> points) 
    {
        foreach (var point in points) 
        {
            Console.CursorLeft = (int)Math.Round(point.X);
            Console.CursorTop  = (int)Math.Round(point.Y);
            Console.WriteLine(__getTextureBasedOnZAxis(point.Z));
        }
    }
    static IEnumerable<Vector3> SurfaceComputing(Surface surfc)
        => __rasterize(
            a: surfc.AnB.Start,
            b: surfc.AnB.End,
            c: surfc.CnA.Start
        )
    ;
    [Method(Inline)] static char __getTextureBasedOnZAxis(float z)
    {
        int index;
        while (z > texture_len) z /= 10;
        index = (int)Math.Round(z);
        var safeIdx = index - 1;

        return safeIdx < 0? '.' : texture[safeIdx];
    }
    [Method(Inline)] static IEnumerable<Vector3> __lineTrack(Vector3 start, Vector3 end)
    {
        bool is_x_axis_reducing = start.X > end.X;
        bool is_y_axis_reducing = start.Y > end.Y;
        bool is_z_axis_reducing = start.Z > end.Z;

        yield return start;

        bool reachingX, reachingY, reachZ = start.Z == end.Z;
        float x = start.X
            , y = start.Y
            , z = start.Z;

        float factorX, factorY, factorZ; {
            float variation_of_x = MathF.Abs(start.X - end.X)
                , variation_of_y = MathF.Abs(start.Y - end.Y)
                , variation_of_z = MathF.Abs(start.Z - end.Z)
                , iterations = variation_of_x > variation_of_y
                    ? variation_of_x
                    : variation_of_y;

            factorX = MathF.Round(x == end.X ? 0 : variation_of_x / iterations, 2);
            factorY = MathF.Round(y == end.Y ? 0 : variation_of_y / iterations, 2);
            factorZ = MathF.Round(variation_of_z / iterations, 2);
        }

        while (
            ((reachingX = is_x_axis_reducing
                ? MathF.Round(x, 2) > MathF.Round(end.X, 2) 
                : MathF.Round(x, 2) < MathF.Round(end.X, 2))
                && factorX != 0) |
            
            ((reachingY = is_y_axis_reducing
                ? MathF.Round(y, 2) > MathF.Round(end.Y, 2) 
                : MathF.Round(y, 2) < MathF.Round(end.Y, 2))
                && factorY != 0)
        )
        {
            if (reachingX)
                x = is_x_axis_reducing ?
                    x - factorX :
                    x + factorX
            ;
            if (reachingY)
                y = is_y_axis_reducing ?
                    y - factorY :
                    y + factorY
            ;
            if (reachZ is false)
                z = is_z_axis_reducing ? 
                    z - factorZ :
                    z + factorZ
            ;
            yield return new(x, y, z);
        }
    }
    [Method(Inline)] static IEnumerable<Vector3> __rasterize(Vector3 a, Vector3 b, Vector3 c)
    {
        using var AnB = __lineTrack(a, b).GetEnumerator();
        using var AnC = __lineTrack(a, c).GetEnumerator();
        using var CnB = __lineTrack(c, b).GetEnumerator();

        bool AnB_next = true;
        bool AnC_next = true;

        while (
            (AnB_next = AnB_next && AnB.MoveNext()) |
            (AnC_next = AnC_next && AnC.MoveNext())
        )
            if (AnB_next && AnC_next)
                foreach (var point in __lineTrack(AnB.Current, AnC.Current))
                    yield return point;
            
            else if(CnB.MoveNext())
                foreach (var point in __lineTrack(CnB.Current, AnB_next? AnB.Current: AnC.Current))
                    yield return point;

            else break;
    }
}