//#define STROKE

Span<Triangle> triangles = [
    new (
        75, 20, 10,
        75, 27, 10,
        90, 20, 10
    ),
    new (
        90, 20, 10,
        90, 27, 5,
        75, 27, 7
    ),
    new (
        67, 17, 5,
        75, 20, 10,
        75, 27, 10
    ),
    new(
        67, 23, 8,
        67, 17, 4,
        75, 27, 10
    ),
    new(
        81, 17, 1,
        67, 17, 5,
        75, 20, 10
    ),
    new(
        75, 20, 10,
        90, 20, 10,
        81, 17, 1
    )
];

foreach (var triangle in triangles)
{
    var outline = triangle.GetOutlines();
    Render([outline.AnB.Start, outline.BnC.Start, outline.CnA.Start]);
}
Console.ReadKey(true);

while (true)
{
    Console.Clear();
    foreach (var triangle in triangles)
        Render(SurfaceComputing(triangle.GetSurface()));

    float radsX = (float)(Math.PI / 180 * .00 )
        , radsY = (float)(Math.PI / 180 * .00 )
        , radsZ = (float)(Math.PI / 180 * 004 )
    ;
    Vector3 center = new(77, 24, 1);
    
    foreach (ref var triangle in triangles)
        rotate(ref triangle, (radsX, radsY, radsZ), center);

    //Thread.Sleep(Renderlag_ms);
}

static void rotate(ref Triangle tri, (float X, float Y, float Z) rads, Vector3 center)
{
    var matrixX = Matrix4x4.CreateRotationX(rads.X, center);
    var matrixY = Matrix4x4.CreateRotationY(rads.Y, center);
    var matrixZ = Matrix4x4.CreateRotationZ(rads.Z, center);

    foreach (var matrix in (Span<Matrix4x4>)[matrixX, matrixY, matrixZ])
    {
        var (a_b, b_c, c_a) = tri.GetOutlines();
        tri = new(
            Vector3.Transform(a_b.Start, matrix),
            Vector3.Transform(b_c.Start, matrix),
            Vector3.Transform(c_a.Start, matrix)
        );
    }
}