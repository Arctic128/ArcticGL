using NWT_Process;

namespace ArcticGL
{

    public class Math_Basis
    {
        public class Point3D
        {
            public double x;
            public double y;
            public double z;
            public Point3D(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            //重载运算符
            public static Vector operator -(Point3D a, Point3D b)
            {
                return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
            }
            //方法
            public static Point3D Move_Unit(Point3D point0, Vector vector, double argv)
            {
                return new Point3D(point0.x + Vector.UnitVector(vector).x * argv, point0.y + Vector.UnitVector(vector).y * argv, point0.z + Vector.UnitVector(vector).z * argv);
            }
            public static Point3D Move_Real(Point3D point0, Vector vector)
            {
                return new Point3D(point0.x + vector.x, point0.y + vector.y, point0.z + vector.z);
            }
            public static Point3D Move_Distance(Point3D point0, Vector vector, double distance)
            {
                return Move_Real(point0, Vector.Zoom(vector, distance / Vector.Length(vector)));
            }
            public static Point3D VectorToPoint3D(Vector vector)
            {
                return new Point3D(vector.x, vector.y, vector.z);
            }
        }
        public class Line
        {

        }
        public class Plane
        {
            public Vector NormalVector;
            public Point3D Point0;
            public Point3D Point1;//alternative

            public readonly double A;
            public readonly double B;
            public readonly double C;
            public readonly double D;
            public Plane(Point3D point0, Vector normal)
            {
                this.NormalVector = normal;
                this.Point0 = point0;
                this.Point1 = Point3D.Move_Unit(this.Point0, NormalVector, 1);

                this.A = this.NormalVector.x;
                this.B = this.NormalVector.y;
                this.C = this.NormalVector.z;
                this.D = -(this.A * this.Point0.x + this.B * this.Point0.y + this.C * Point0.z);
            }
            public Plane(Point3D point0, Point3D point1)
            {
                this.Point0 = point0;
                this.Point1 = point1;
                NormalVector = point1 - point0;

                this.A = this.NormalVector.x;
                this.B = this.NormalVector.y;
                this.C = this.NormalVector.z;
                this.D = -(this.A * this.Point0.x + this.B * this.Point0.y + this.C * Point0.z);
            }
        }
        public class Vector
        {
            public double x;
            public double y;
            public double z;

            public static readonly Vector X = new Vector(1, 0, 0);
            public static readonly Vector Y = new Vector(0, 1, 0);
            public static readonly Vector Z = new Vector(0, 0, 1);
            public Vector(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            public Vector(Point3D point0, Point3D point1)
            {
                this.x = point1.x - point0.x;
                this.y = point1.y - point0.y;
                this.z = point1.z - point0.z;
            }
            //重载运算符
            public static Vector operator +(Vector a, Vector b)
            {
                return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
            }
            public static Vector operator -(Vector a, Vector b)
            {
                return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
            }
            public static double operator *(Vector a, Vector b)
            {
                double Value;
                Value = a.x * b.x + a.y * b.y + a.z * b.z;
                return Value;
            }
            public static Vector operator %(Vector a, Vector b)
            {
                return new Vector(a.y * b.z - a.z * b.y, a.x * b.z - a.z * b.x, a.x * b.y - a.y * b.x);
            }
            public static Vector operator !(Vector vector)
            {
                return new Vector(-vector.x, -vector.y, -vector.z);
            }
            //方法
            public static double Length(Vector vector)
            {
                return System.Math.Sqrt(System.Math.Pow(vector.x, 2) + System.Math.Pow(vector.y, 2) + System.Math.Pow(vector.z, 2));
            }
            public static double Cosine(Vector vector1, Vector vector2)
            {
                return (vector1 * vector2) / (Length(vector1) * Length(vector2));
            }
            public static Vector UnitVector(Vector vector)
            {
                return new Vector(vector.x / Length(vector), vector.y / Length(vector), vector.z / Length(vector));
            }
            public static Vector Zoom(Vector vector, double argv)
            {
                return new Vector(argv * vector.x, argv * vector.y, argv * vector.z);
            }
            public static Vector Point3DToVector(Point3D point)
            {
                return new Vector(point.x, point.y, point.z);
            }
        }
    }
    public class Graphics3D
    {
        public class Camera
        {
            //Renderer_Main
            Math_Basis.Point3D Eyes;
            Math_Basis.Plane VisionPlane;
            Math_Basis.Vector VisionVector;
            double VisionDistance;
            //int Points_Render_Connect_Length;
            //Renderer_Deuterogenic
            Math_Basis.Point3D Origin;
            Math_Basis.Vector Vision2D_X;
            Math_Basis.Vector Vision2D_Y;
            //Render Points

            /*
            Pen RenderPen = new Pen(Color.White, 1);
            Font RenderFont = new Font("Arial", 16, FontStyle.Bold);
            */
            //Other
            //private Math_Basis.Point3D Point_000 = new Math_Basis.Point3D(0, 0, 0);//3d
            //private Point Point_00 = new Point(0, 0);//2d
            public int ResolutionRatio_X = 1920;
            public int ResolutionRatio_Y = 1080;
            public double Zoom = 2500.65326543;
            //Connect

            //public bool[,] Connect = new bool[9, 9];
            //bool IfConnect;

            //Fault
            bool FailToRender = false;
            //Error
            public double Errorzoom = 1000.159354;
            //构造函数
            public Camera(Math_Basis.Point3D eyes, Math_Basis.Vector visionvector, double visiondistance, double errorzoom = 1000.159354)
            {
                this.Errorzoom = errorzoom;
                visionvector = Math_Basis.Vector.Zoom(visionvector, Errorzoom);
                Math_Basis.Plane visionplane = new Math_Basis.Plane(eyes, visionvector);
                //Start
                this.Eyes = eyes;
                this.VisionPlane = visionplane;
                this.VisionVector = visionvector;
                this.VisionDistance = visiondistance;
                //Perform Actions
                ////Create a two-dimensional coordinate system
                Math_Basis.Vector ProsomalVector2D_X = new Math_Basis.Vector(VisionPlane.B, -VisionPlane.A, 0);

                Origin = Math_Basis.Point3D.Move_Distance(Eyes, VisionVector, VisionDistance);
                this.VisionVector = Origin - Eyes;
                this.Vision2D_X = Math_Basis.Vector.UnitVector(Math_Basis.Vector.Zoom(ProsomalVector2D_X, (ProsomalVector2D_X % VisionVector) * Math_Basis.Vector.Z));
                this.Vision2D_Y = Math_Basis.Vector.UnitVector(Vision2D_X % VisionVector);
                //Subsequent initialization
                /*
                for (int i = 0; i <= 2; ++i)
                {
                    Points_Real[i] = Point_000;
                }
                /*
                for (int i = 0, j = 0; i <= 8 && j <= 8; ++j)
                {
                    Connect[i, j] = false;
                    if (j == 8)
                    {
                        j = 0;
                        ++i;
                    }
                }
                */
                /*
                for (int i = 0; i <= 2; ++i)
                {
                    Points_Render[i] = Point_00;
                }
                /*
                for (int i = 0; i <= 27; ++i)
                {
                    Points_Render_Connect[i] = Point_00;
                }
                */
            }
            public Point[] Render(DiscribledGraph graph)
            {
                Math_Basis.Point3D[] Points_Real = new Math_Basis.Point3D[graph.Points.Length];//3d////////////////////////////////////////////////////////////////////////////////
                Point[] Points_Render = new Point[graph.Points.Length];//2d
                                                                       //public Point[] Points_Render_Connect = new Point[120];//120
                                                                       //Pen
                                                                       //Get Vision Point's two-dimensional coordinate
                if (graph.Points.Length != Points_Real.Length)
                {
                    FailToRender = true;
                    return Points_Render;
                }
                /*
                if (connect.Length != Connect.Length)
                {
                    FailToRender = true;
                    return Points_Render_Connect;
                }
                */
                Points_Real = graph.Points;
                //this.Connect = connect;
                Math_Basis.Point3D Point_Real_Move;
                for (int i = 0; i <= (graph.Points.Length - 1); ++i)
                {
                    /*
                    if (Math_Basis.Vector.Cosine(Points_Real[i] - Eyes, VisionVector) <= 0)
                    {
                        this.Points_Real[i] = new Math_Basis.Point3D(0, 0, 0);
                    }
                    */
                    if ((Math_Basis.Vector.Cosine(Points_Real[i] - Eyes, VisionVector) * Math_Basis.Vector.Length(Points_Real[i] - Eyes)) < Math_Basis.Vector.Length(VisionVector))
                    {
                        Points_Real[i] = new Math_Basis.Point3D(0, 0, 0);
                    }
                    Point_Real_Move = Math_Basis.Point3D.Move_Distance(Eyes, Points_Real[i] - Eyes, VisionDistance / Math_Basis.Vector.Cosine(VisionVector, Points_Real[i] - Eyes));
                    Points_Render[i] = new Point((int)(((Point_Real_Move - Origin) * Vision2D_X) * Zoom) + ResolutionRatio_X / 2, -(int)(((Point_Real_Move - Origin) * Vision2D_Y) * Zoom) + ResolutionRatio_Y / 2);
                }
                /*
                for (int i = 1, j = 1, k = 0; i <= 8 && j <= 8; ++j)
                {
                    if (Connect[i, j] == true && i != j)
                    {
                        Points_Render_Connect[k] = Points_Render[i - 1];//-1
                        ++k;
                        Points_Render_Connect[k] = Points_Render[j - 1];//-1
                        ++k;
                    }
                    this.Points_Render_Connect_Length = k + 1;
                    if (j == 8)
                    {
                        j = 0;
                        ++i;
                    }
                }
                Point[] Points_Render_Connect_Return = new Point[Points_Render_Connect_Length];
                for (int i = 0; i <= (Points_Render_Connect_Length - 1); ++i)
                {
                    Points_Render_Connect_Return[i] = Points_Render_Connect[i];
                }
                */
                return Points_Render;
            }
        }
        public class DiscribledGraph
        {
            public Math_Basis.Point3D[] Points;
            public Pen StuffPen;
            public bool IfStuff = false;
            public bool IsaFinitePlane = false;
            public Math_Basis.Vector NormalVector;//alternative
            //构造函数
            public DiscribledGraph(Math_Basis.Point3D[] points, Pen stuffpen, bool ifstuff = false)
            {
                this.Points = points;
                this.StuffPen = stuffpen;
                this.IfStuff = ifstuff;
                if (this.Points.Length == 3)
                {
                    this.NormalVector = (this.Points[1] - this.Points[0]) % (this.Points[2] - this.Points[0]);
                    this.IsaFinitePlane = true;
                }
            }
            //方法
            
            public static object IfOneSide(DiscribledGraph discribledGraph0, DiscribledGraph discribledGraph1)
            {
                if ((discribledGraph0.IsaFinitePlane = false) || (discribledGraph1.IsaFinitePlane = false))
                {
                    return 0;
                }
                //Math_Basis.Vector NormalVector0 = (discribledGraph0.Points[1] - discribledGraph0.Points[0]) % (discribledGraph0.Points[2] - discribledGraph0.Points[0]);
                for (int i = 0, j = 0; ; ++j)
                {
                    if (i <= (discribledGraph0.Points.Length - 1))
                    {
                        if (NWT_Process.Math.IsSameSign(Math_Basis.Vector.Cosine(discribledGraph1.Points[i] - discribledGraph0.Points[j], discribledGraph0.NormalVector), ))
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                    if (j >= (discribledGraph0.Points.Length - 1))
                    {
                        j = 0;
                        ++i;
                    }
                }
                
            }
            
        }
    }
}



//the things have not been done:
//透视算法
//误差调整
//质量设定（user）
//connect
//line
