using System;
using System.Drawing;

namespace Project_v1
{
    [Serializable]
    public abstract class Shape
    {

        protected int x, y;//pos
        protected static int R;//radius
        protected static Color clr;//color
        [NonSerialized]
        protected bool flag;//move_flag
        protected int diffx, diffy;//
        protected int status; //1-крайние
        protected int move_figure;//idk
        protected bool hide;

        public bool Hide
        {
            get { return hide; }
            set { hide = value; }
        }

        public static int Rad
        {
            get { return R; }
            set { R = Math.Abs(value); }
        }
        public static Color Clr
        {
            get { return clr; }
            set { clr = value; }
        }
        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        public int DiffX
        {
            get { return diffx; }
            set { diffx = value; }
        }
        public int DiffY
        {
            get { return diffy; }
            set { diffy = value; }
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        static Shape()
        {
            R = 60;//база по конспекту 


        }
        protected Shape()
        {
            flag = false;
            diffx = 0; diffy = 0;
            status = 0;
            move_figure = 0;
        }
        //
        public abstract void Draw(Graphics g);
        //
        protected Shape(int x, int y)
        {
            this.x = x; this.y = y;
        }

        public abstract bool IsInside(int x, int y);
    }
    //Выше абстрактный класс Shape, ниже классы Circle, Square, Triangle, которые от него наследуются
    [Serializable]
    public class Circle : Shape
    {
        public Circle(int x, int y) : base(x, y)
        {
            //Просто напоминалка, не раскомментировать
            //this.x = x;this.y = y;

        }
        //
        public override void Draw(Graphics g)
        {
            int x0 = x - R, y0 = y - R;
            g.DrawEllipse(new Pen(clr), x0, y0, 2 * R, 2 * R);
        }
        public override bool IsInside(int x, int y)
        {
            int distx = Math.Abs(this.x - x);
            int disty = Math.Abs(this.y - y);
            double distFromCircleCenter = (Math.Sqrt(distx * distx + disty * disty));
            if (distFromCircleCenter <= R) return true;
            else return false;
        }

    }
    [Serializable]
    public class Sqare : Shape
    {
        public Sqare(int x, int y) : base(x, y)
        {
            //Просто напоминалка, не раскомментировать
            //this.x = x;this.y = y;
        }
        //
        public override void Draw(Graphics g)
        {
            double side = Math.Sqrt(2) * R * 0.5;
            int x1 = (int)(x - side), y1 = (int)(y + side), x2 = (int)(x + side), y2 = (int)(y + side), x3 = (int)(x + side), y3 = (int)(y - side), x4 = (int)(x - side), y4 = (int)(y - side);
            Pen p = new Pen(clr);
            g.DrawLine(p, x1, y1, x2, y2);
            g.DrawLine(p, x2, y2, x3, y3);
            g.DrawLine(p, x3, y3, x4, y4);
            g.DrawLine(p, x4, y4, x1, y1);
        }
        public override bool IsInside(int x, int y)
        {
            double temp = (0.5 * Math.Sqrt(2 * R * R));
            if (Math.Abs(this.x - x) <= temp && Math.Abs(this.y - y) <= temp) return true;
            else return false;
        }
    }
    [Serializable]
    public class Triangle : Shape
    {
        public Triangle(int x, int y) : base(x, y)
        {
            //Просто напоминалка, не раскомментировать
            //this.x = x;this.y = y;

        }

        public override void Draw(Graphics g)
        {
            Pen p = new Pen(clr);
            int x1 = x, y1 = y - R, y23 = (int)(y + 0.5 * R), x3 = (int)(x - 0.5 * R * Math.Sqrt(3)), x2 = (int)(x + 0.5 * R * Math.Sqrt(3));
            g.DrawLine(p, x1, y1, x2, y23);
            g.DrawLine(p, x2, y23, x3, y23);
            g.DrawLine(p, x3, y23, x1, y1);

        }

        public override bool IsInside(int x, int y)
        {
            if (y < this.y - R || y > this.y + 0.5 * R)
                return false;

            int hFromTop = y - (this.y - R);
            double halfL = (Math.Sqrt(3) / 3) * hFromTop;
            return x >= this.x - halfL && x <= this.x + halfL;
        }
    }
}
