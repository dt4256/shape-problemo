using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_v1
{

    public partial class bench : Form
    {
        
        private readonly Form1 _ownerForm;
        private List<Shape> shapes = new List<Shape>();
        private Figures Fignow = Figures.Circle;
        private Algos Algo = Algos.Basic;
        bool removing_flag = false;
        public bench(Form1 owner)
        {
            InitializeComponent();
            _ownerForm = owner;
        }
        
        private Shape change_figure(int type,int x,int y)
        {
            if (x == 0) {
                return new Circle(x, y);
            }else if (x == 1)
            {
                return new Triangle(x, y);
            }
            else
            {
                return new Sqare(x, y);
            }
        }
        private void bench_Load(object sender, EventArgs e)
        {

        }

        private void backtomainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            _ownerForm.Show();
        }

        private void bench_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            _ownerForm.Show();
        }

        private void startBenchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shapes.Clear();
            //for(int i = 3;i<)
        }

        private void startBenchToolStripMenuItem_Paint(object sender, PaintEventArgs e)
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
                        if (shapes[i].Y < shapes[p].Y || (shapes[i].Y == shapes[p].Y && shapes[i].X < shapes[p].X))
                            p = i;

                    }
                    //e.Graphics.DrawEllipse(new Pen(Color.Red), shapes[p].X, shapes[p].Y, 5, 5);
                    int start = p;

                    shapes[p].Status = 1;
                    double mincos = 4;
                    int next = 0;
                    {
                        int xtemp = -100;
                        int ytemp = shapes[p].X;

                        for (int i = 0; i < shapes.Count; i++)
                        {
                            double cos = vectorcos(xtemp, ytemp, shapes[p].X, shapes[p].Y, shapes[i].X, shapes[i].Y);

                            if (cos < mincos)
                            {
                                mincos = cos;
                                next = i;
                            }

                        }

                    }
                    shapes[next].Status = 1;
                    e.Graphics.DrawLine(new Pen(Color.Red), shapes[start].X, shapes[start].Y, shapes[next].X, shapes[next].Y);
                    int a = start;
                    int b = next;
                    do
                    {
                        mincos = 4;
                        for (int i = 0; i < shapes.Count; i++)
                        {
                            double cos = vectorcos(shapes[a].X, shapes[a].Y, shapes[b].X, shapes[b].Y, shapes[i].X, shapes[i].Y);

                            if (cos < mincos)
                            {
                                mincos = cos;
                                next = i;
                            }
                        }
                        shapes[next].Status = 1;
                        e.Graphics.DrawLine(new Pen(Color.Red), shapes[b].X, shapes[b].Y, shapes[next].X, shapes[next].Y);
                        a = b;
                        b = next;
                    } while (next != start);


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

        private double vectorcos(int ax, int ay, int bx, int by, int cx, int cy)
        {
            int bax = ax - bx;
            int bay = ay - by;
            int bcx = cx - bx;
            int bcy = cy - by;
            double ba = Math.Sqrt(bax * bax + bay * bay);
            double bc = Math.Sqrt(bcx * bcx + bcy * bcy);
            try
            {
                return (bax * bcx + bay * bcy) / (ba * bc);
            }
            catch
            {
                return 4;//for mincos
            }


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
    }

}
