using Project_v1;
using System;
using System.Collections;
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
using static System.Windows.Forms.AxHost;




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
        Jarvis
    }
    public partial class Form1 : Form
    {
        Figures nowFigure = Figures.Circle;
        Algos Algo = Algos.Basic;
        List<Shape> shapes = new List<Shape>();
        bool removing_flag = false;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            shapes.Add(new Circle(300, 300));
        }






        private double[] Get_K(int x1, int y1, int x2, int y2)
        {
            double k;
            int tx1 = x1, ty1 = y1;
            if (x1 > x2)
            {
                (x1, x2) = (x2, x1);
                (y1, y2) = (y2, y1);
            }
            x2 -= x1;
            y2 -= y1;
            x1 = 0; y1 = 0;
            //y2=k*x2 k =y2/x2
            if (x2 == 0) { k = double.PositiveInfinity; }
            else { k = (double)(y2) / x2; }
            return new double[] { k, tx1, ty1 };
        }
        private int Upper_Lower(int x, int y, int x1, int y1, double k)
        {
            x -= x1;
            y -= y1;
            if (y > k * x)
            {
                return 1;
            }
            else if (y < k * x)
            {
                return -1;
            }
            return 0;
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


            }

            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                if (e.Button == MouseButtons.Right && shapes[i].IsInside(e.X, e.Y))
                {
                    shapes.RemoveAt(i);
                    break;
                }
            }


        }


        //движение
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                if (shapes[i].Flag == true)
                {
                    shapes[i].X = e.X - shapes[i].DiffX;
                    shapes[i].Y = e.Y - shapes[i].DiffY;
                    Refresh();
                }
            }

        }
        //клик
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                shapes[i].Flag = false;
                shapes[i].DiffX = 0;
                shapes[i].DiffY = 0;
            }
            removing_flag = true;

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
            ////
            ////sad
        }




        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //algos start
            for (int i = 0; i < shapes.Count; i++) shapes[i].Status = 0;
            //basic algo
            if (shapes.Count > 2)
            {
                if (Algo == Algos.Basic)
                {
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
                                e.Graphics.DrawLine(new Pen(Color.Black), shapes[i].X, shapes[i].Y, shapes[j].X, shapes[j].Y);
                            }
                        }
                    }



                }
                else if (Algo == Algos.Jarvis)
                {
                    int p = 0;
                    for (int i = 1; i < shapes.Count; i++)
                    {
                        if (shapes[i].X < shapes[p].X || (shapes[i].X == shapes[p].X && shapes[i].Y < shapes[p].Y))
                            p = i;
                    }

                    int start = p;
                    do
                    {
                        shapes[p].Status = 1;
                        int next = (p + 1) % shapes.Count;//выбирается след точка с защитой от out of range. 
                        for (int i = 0; i < shapes.Count; i++)
                        {
                            if (i == p) continue;
                            double D = (shapes[next].X - shapes[p].X) * (shapes[i].Y - shapes[p].Y) - (shapes[next].Y - shapes[p].Y) * (shapes[i].X - shapes[p].X);
                            if (D < 0)
                            {
                                next = i;//жадно ищем самую левую точку относительно вектора pnext учитывая что next меняется
                            }
                        }
                        e.Graphics.DrawLine(new Pen(Color.Red), shapes[p].X, shapes[p].Y, shapes[next].X, shapes[next].Y);
                        p = next;

                    } while (p != start);

                }

                    if (removing_flag)
                    {
                        //Алгоритм закончился начинается удаление.
                        //запрос в гугл оставить только определенные элементы в списке c# и мне выдало с where
                        shapes = shapes.Where(x => x.Status == 1).ToList();
                        removing_flag = false;
                    }
                
            }
                //обтяжка кончилась
                for (int i = 0; i < shapes.Count; i++)
                {
                    shapes[i].Draw(e.Graphics);
                }

            
        }

        private void jarvisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Algo = Algos.Jarvis;
            jarvisToolStripMenuItem.Checked= true;
            basicToolStripMenuItem.Checked = false;
            Refresh();  
        }

        private void basicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Algo = Algos.Basic;
            basicToolStripMenuItem.Checked= true;
            jarvisToolStripMenuItem.Checked=false;
            Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void typeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}