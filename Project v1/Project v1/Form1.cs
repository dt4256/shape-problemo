using Project_v1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Project_v1
{
    public enum Figures
    {
        Circle,
        Square,
        Triangle
    }
    public enum Algos
    {
        Basic,
        Mod
    }
    public partial class Form1 : Form
    {
        Figures nowFigure = Figures.Circle;
        Algos Algo = Algos.Basic;
        List<(int,int)> dots_lines = new List<(int,int)>();
        List<Shape> shapes = new List<Shape>();
        
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            shapes.Add(new Circle(300,300));
        }


        private void RecalculateHullBasic()
        {
            dots_lines.Clear();   
            for (int i = 0; i < shapes.Count; i++)
                shapes[i].Status = 0;

            dots_lines.Clear();

            for (int i = 0; i < shapes.Count; i++)
            {
                for (int j = 0; j < shapes.Count; j++)
                {
                    if (i == j) continue;

                    bool upper = false;
                    bool lower = false;

                    double[] tmp = Get_K(shapes[i].X, shapes[i].Y, shapes[j].X, shapes[j].Y);

                    double k = tmp[0];
                    int x1 = (int)tmp[1];
                    int y1 = (int)tmp[2];

                    for (int m = 0; m < shapes.Count; m++)
                    {
                        int pos = Upper_Lower(shapes[m].X, shapes[m].Y, x1, y1, k);
                        if (pos == 1) upper = true;
                        else if (pos == -1) lower = true;
                    }

                    if ((upper && !lower) || (!upper && lower) || (!upper && !lower))
                    {
                        shapes[i].Status = 1;
                        shapes[j].Status = 1;
                        dots_lines.Add((i, j));
                    }
                }
            }
        }

        private void DeleteInsideWithRefresh() {
            List<int> temp = new List<int>();
            if (shapes.Count > 3)
            {
                for (int i = 0; i < shapes.Count; i++) {
                    if (shapes[i].Status == 0) { 
                        temp.Add(i);
                    }
                }
                temp.Sort();
                temp.Reverse();
                for (int i = temp.Count - 1; i >= 0; i--)
                {
                    shapes.RemoveAt(temp[i]);
                }
               
            }
        }

        private double[] Get_K(int x1, int y1, int x2, int y2) {
            double k;
            int tx1 = x1,ty1 = y1;
            if (x1 > x2)
            {
                (x1, x2) = (x2, x1);
                (y1,y2)=(y2, y1);
            }
            x2 -= x1;
            y2 -= y1;
            x1 = 0;y1 = 0;
            //y2=k*x2 k =y2/x2
            if (x2 == 0) { k = double.PositiveInfinity; }
            else { k = (double)(y2) / x2; }
            return new double[] { k, tx1, ty1 };
        }
        private int Upper_Lower(int x, int y, int x1,int y1, double k)
        {
            x -= x1;
            y -= y1;
            if (y > k * x)
            {
                return 1;
            }
            else if (y < k * x) {
                return -1;
            }return 0;
            //1-upper -1 lower 0 on line
        }

        //нажатие
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                if (shapes[i].IsInside(e.X, e.Y) && e.Button == MouseButtons.Left)
                {
                    shapes[i].Flag = true;
                    shapes[i].DiffX = e.X - shapes[i].X;
                    shapes[i].DiffY = e.Y - shapes[i].Y;

                }
                
            }
            bool temp = false;
            for (int i = 0; i < shapes.Count; i++)
            {
                if (shapes[i].IsInside(e.X, e.Y))
                {
                    temp = true;
                }
            }
                if (e.Button == MouseButtons.Left && temp == false)
                {
                    if (nowFigure == Figures.Circle)
                    {
                        shapes.Add(new Circle(e.X, e.Y));
                        shapes[shapes.Count - 1].Flag = true;
                        
                        Refresh();
                    }
                    else if (nowFigure == Figures.Square)
                    {
                        shapes.Add(new Sqare(e.X, e.Y));
                        shapes[shapes.Count - 1].Flag = true;
                        
                        Refresh();
                    }
                    else if (nowFigure == Figures.Triangle)
                    {
                        shapes.Add(new Triangle(e.X, e.Y));
                        shapes[shapes.Count - 1].Flag = true;
                        Refresh();
                    }


                //перерисовка оболочки
                if (shapes.Count > 2 && Algo == Algos.Basic)
                {
                    RecalculateHullBasic();
                }
                //перерисовка оболочки
                Refresh();
                
                
            }

            for (int i = shapes.Count-1; i >=0; i--)
            {
                if (e.Button == MouseButtons.Right && shapes[i].IsInside(e.X, e.Y))
                {
                    shapes.RemoveAt(i);
                    //перерисовка оболочки
                    if (shapes.Count > 2 && Algo == Algos.Basic)
                    {
                        RecalculateHullBasic();
                    }else dots_lines.Clear();
                    Refresh();
                    //перерисовка оболочки
                    break;
                }
            }

            
        }
            
        
        //движение
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            for(int i = 0;i< shapes.Count;i++)
            {
                if (shapes[i].Flag==true)
                {
                    shapes[i].X = e.X-shapes[i].DiffX;
                    shapes[i].Y = e.Y-shapes[i].DiffY;
                    Refresh();
                }
            }
            //не помню, нужно ли было по тз. Спросить в пн
            if (shapes.Count > 2 && Algo == Algos.Basic)
            {
                RecalculateHullBasic();
            }
            Refresh();
            //конец спорного
        }
        //клик
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            for(int i = 0; i<shapes.Count;i++)
            {
                shapes[i].Flag = false;
                shapes[i].DiffX = 0;
                shapes[i].DiffY = 0;
            }
            //перерисовка оболочки
            
            if (shapes.Count > 2 && Algo == Algos.Basic)
            {
               
                DeleteInsideWithRefresh();
                RecalculateHullBasic();
            }
            //перерисовка оболочки
            Refresh();
            
        }

        private void sqareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nowFigure = Figures.Square;
            sqareToolStripMenuItem.Checked = true;
            circleToolStripMenuItem.Checked = false;
            triangleToolStripMenuItem.Checked = false;

        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nowFigure = Figures.Circle;
            sqareToolStripMenuItem.Checked = false;
            circleToolStripMenuItem.Checked = true;
            triangleToolStripMenuItem.Checked = false;
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nowFigure = Figures.Triangle;
            sqareToolStripMenuItem.Checked = false;
            circleToolStripMenuItem.Checked = false;
            triangleToolStripMenuItem.Checked = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for(int i = 0; i < shapes.Count; i++)
            {
                shapes[i].Draw(e.Graphics);
            }
            foreach(var(l,m ) in dots_lines)  {
                Pen pen = new Pen(shapes[0].Clr);
                e.Graphics.DrawLine(pen, shapes[l].X, shapes[l].Y, shapes[m].X,shapes[m].Y);
            }


        }

       
    }
    
       
    }


