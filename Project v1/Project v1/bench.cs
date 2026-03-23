using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_v1
{

    public partial class bench : Form
    {
        Random rnd = new Random();
        private readonly Form1 _ownerForm;
        private List<Shape> shapes = new List<Shape>();
        private Figures Fignow = Figures.Circle;
        private Algos Algo = Algos.Basic;
        bool removing_flag = false;
        List<long> basic_time = new List<long>();
        List<long> jarvis_time = new List<long>();
        bool ready_for_graphix = false;
        public bench(Form1 owner)
        {
            InitializeComponent();
            _ownerForm = owner;
        }
        
        private Shape change_figure(int type,int x,int y)
        {
            if (type == 0) {
                return new Circle(x, y,Color.Black);
            }else if (type == 1)
            {
                return new Triangle(x, y,Color.Black);
            }
            else
            {
                return new Sqare(x, y, Color.Black);
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
            
            int step = 10;
            int launches = 200 / step;
            ready_for_graphix = false;
            shapes.Clear();
            basic_time.Clear();
            jarvis_time.Clear();
            Algo = Algos.Basic;
            removing_flag = false;
            int screenWidth = this.ClientSize.Width;
            int screenHeight = this.ClientSize.Height;
            for (int i = 0; i < launches; i++)
            {
                temToolStripMenuItem.Text="B"+i.ToString();
                shapes.Clear();
                for (int j = 0; j < step * (i+1); j++)
                {
                    shapes.Add(change_figure(rnd.Next(0, 3), rnd.Next(screenWidth), rnd.Next(screenHeight)));
                }
                Stopwatch stopwatch = Stopwatch.StartNew();
                Refresh();
                stopwatch.Stop();
                basic_time.Add(stopwatch.ElapsedMilliseconds);
                
            }
            shapes.Clear();
            Refresh();
            Algo = Algos.Jarvis;
            for (int i = 0; i < launches; i++)
            {
                temToolStripMenuItem.Text = "J"+i.ToString();
                shapes.Clear();
                for (int j = 0; j < step * (i+1); j++)
                {
                    shapes.Add(change_figure(rnd.Next(0, 3), rnd.Next(screenWidth), rnd.Next(screenHeight)));
                }
                Stopwatch stopwatch = Stopwatch.StartNew();
                Refresh();
                stopwatch.Stop();
                jarvis_time.Add(stopwatch.ElapsedMilliseconds);

            }
            shapes.Clear();
            Refresh();
            ready_for_graphix = true;
            this.Invalidate();


        }

        private void startBenchToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
           


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

        private void bench_Paint(object sender, PaintEventArgs e)
        {
            //int screenWidth = this.ClientSize.Width;
            //int screenHeight = this.ClientSize.Height;
            
            if (ready_for_graphix)
            {
                int marginup = 40;
                int markx = 15;
                e.Graphics.Clear(Color.White);
                int screenWidth = this.ClientSize.Width-5;   
                int screenHeight = this.ClientSize.Height-5; 
                using (Pen pen = new Pen(Color.Black))
                {
                    pen.EndCap = LineCap.ArrowAnchor;
                    e.Graphics.DrawLine(pen, markx, screenHeight, markx, marginup);
                    e.Graphics.DrawLine(pen,markx,screenHeight,screenWidth,screenHeight); 
                }
                int workWidth = screenWidth - markx;
                int workHeight = screenHeight - marginup;
                long maxTime = Math.Max(basic_time.Max(), jarvis_time.Max());
                for (int i = 1; i < basic_time.Count; i++) {
                    int x1 = markx + (i - 1) * workWidth / (basic_time.Count - 1);
                    int y1 = screenHeight - (int)(basic_time[i - 1] * workHeight / maxTime);
                    int x2 = markx + i * workWidth / (basic_time.Count - 1);
                    int y2 = screenHeight - (int)(basic_time[i] * workHeight / maxTime);
                    e.Graphics.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
                }
                for (int i = 1; i < jarvis_time.Count; i++)
                {
                    int x1 = markx + (i - 1) * workWidth / (jarvis_time.Count - 1);
                    int y1 = screenHeight - (int)(jarvis_time[i - 1] * workHeight / maxTime);
                    int x2 = markx + i * workWidth / (jarvis_time.Count - 1);
                    int y2 = screenHeight - (int)(jarvis_time[i] * workHeight / maxTime);
                    e.Graphics.DrawLine(new Pen(Color.Red), x1, y1, x2, y2);
                }
            }
            //algos start
            for (int i = 0; i < shapes.Count; i++) shapes[i].Status = 0;
            //basic algo
            if (shapes.Count > 2 && ready_for_graphix==false)
            {
                if (Algo == Algos.Basic)
                {
                    foreach(Shape i in shapes)
                    {
                        foreach (Shape j in shapes)
                        {
                            if (i == j) continue;

                            bool upper = false;
                            bool lower = false;
                            double[] tmp = Get_K(i.X, i.Y, j.X,j.Y);

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
                                i.Status = 1;
                                j.Status = 1;
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
                    int start = p;

                    shapes[p].Status = 1;
                    double mincos = 4;
                    int next = 0;
                    {
                        int xtemp = -100;
                        int ytemp = shapes[p].Y;

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
                        a = b;
                        b = next;
                    } while (next != start);


                }

                

            }
            
        }

        private void bench_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();

        }

        private void temToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void startonlyjarvisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int step = 10;
            int launches = 2000 / step;
            ready_for_graphix = false;
            shapes.Clear();
            basic_time.Clear();
            jarvis_time.Clear();
            Algo = Algos.Basic;
            removing_flag = false;
            int screenWidth = this.ClientSize.Width;
            int screenHeight = this.ClientSize.Height;
            Refresh();
            basic_time.Add(0);
            basic_time.Add(0);
            Algo = Algos.Jarvis;
            for (int i = 0; i < launches; i++)
            {
                temToolStripMenuItem.Text = "J" + i.ToString();
                shapes.Clear();
                for (int j = 0; j < step*(i+1); j++)
                {
                    shapes.Add(change_figure(rnd.Next(0, 3), rnd.Next(screenWidth), rnd.Next(screenHeight)));
                }
                Stopwatch stopwatch = Stopwatch.StartNew();
                Refresh();
                stopwatch.Stop();
                jarvis_time.Add(stopwatch.ElapsedMilliseconds);

            }
            shapes.Clear();
            Refresh();
            ready_for_graphix = true;
            this.Invalidate();

        }
    }

}
