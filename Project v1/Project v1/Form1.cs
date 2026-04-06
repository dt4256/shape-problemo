using Project_v1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization.Formatters.Binary;
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
        string path;
        bool saved;
        Color nextcol = Color.Black;
        Figures nowFigure = Figures.Circle;
        Algos Algo = Algos.Basic;
        private List<Shape> shapes = new List<Shape>();
        bool removing_flag = false;
        bool figmove = false;
        private bench _form2;//benchmarc form
        private Rad _form3;
        public Form1()
        {
            InitializeComponent();
            
            DoubleBuffered = true;
            saved = true;
            path = null;
            shapes.Add(new Circle(300, 300,nextcol));
            shapes.Add(new Circle(500, 300, nextcol));
            shapes.Add(new Circle(300, 500, nextcol));
            
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
                    shapes.Add(new Circle(e.X, e.Y,nextcol));
                    shapes[shapes.Count - 1].Flag = true;

                    Refresh();
                }
                else if (nowFigure == Figures.Square)
                {
                    shapes.Add(new Sqare(e.X, e.Y,nextcol));
                    shapes[shapes.Count - 1].Flag = true;

                    Refresh();
                }
                else if (nowFigure == Figures.Triangle)
                {
                    shapes.Add(new Triangle(e.X, e.Y,nextcol));
                    shapes[shapes.Count - 1].Flag = true;
                    Refresh();
                }
                
                if (shapes[shapes.Count - 1].Status == 0)
                {
                    figmove = true;
                    for(int i = 0; i < shapes.Count; i++)
                    {
                        shapes[i].Flag = true;
                        shapes[i].DiffX = e.X - shapes[i].X;
                        shapes[i].DiffY = e.Y - shapes[i].Y;
                    }
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

            saved = false;
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
                    
                }
                
            }
            Refresh();
            saved= false;
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
            figmove = false;
            Refresh();
            saved = false;

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
                    foreach (Shape i in shapes)
                    {
                        foreach (Shape j in shapes)
                        {
                            if (i == j) continue;

                            bool upper = false;
                            bool lower = false;
                            double[] tmp = Get_K(i.X, i.Y, j.X, j.Y);

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
                                e.Graphics.DrawLine(new Pen(Color.Black),i.X, i.Y, j.X, j.Y);
                            }
                        }
                    }



                }
                else if (Algo == Algos.Jarvis )
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
                    double mincos=4;
                    int next = 0;
                    {
                        int xtemp = -100;
                        int ytemp = shapes[p].Y;

                        for (int i = 0; i < shapes.Count; i++)
                        {
                            double cos = vectorcos(xtemp, ytemp, shapes[p].X, shapes[p].Y, shapes[i].X, shapes[i].Y);
                            
                           if(cos < mincos)
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

        private double vectorcos(int ax,int ay,int bx,int by, int cx,int cy)
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
            catch {
                return 4;//for mincos
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

        private void goToBenchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_form2 == null || _form2.IsDisposed)
            {
                _form2 = new bench(this); 
            }

            this.Hide();
            _form2.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void radiusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_form3 == null || _form3.IsDisposed)
            {
                _form3 = new Rad(this, shapes[0].Rad);
            }
            _form3.Rad_Changed += Rad_changed;
            _form3.Show();
            if (_form3.WindowState == FormWindowState.Minimized)
            {
                _form3.WindowState = FormWindowState.Normal;
            }
            _form3.Activate();
        }


        private void Rad_changed(object sender, RadiusEvent e)
        {
            foreach(var i in shapes)
            {
                i.Rad = e.R;
            }
            Refresh();
            saved=false;
        }

        private void developerdebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(var i in shapes)
            {
                Debug.WriteLine(Convert.ToString(i.Rad)+" ");
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = nextcol; 
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    nextcol = colorDialog.Color;
                    if (shapes.Count() > 0)
                    {
                        shapes[0].Clr = nextcol;
                    }
                }
            }
            saved = false;
        }

        private void Save_state(string path)
        {
            //clr,Rad
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            if (shapes.Count > 0)
            {
                bf.Serialize(fs, shapes[0].Clr);
                bf.Serialize(fs, shapes[0].Rad);
            }
            bf.Serialize(fs, shapes);
            fs.Close();
            
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Binary Files (*.bin)|*.bin|All files (*.*)|*.*";
            saveFileDialog.Title = "Save binary file";
            saveFileDialog.DefaultExt = "bin";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Save_state(saveFileDialog.FileName);
            }
            path = saveFileDialog.FileName;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (path == null)
            {
                saveAsToolStripMenuItem_Click(sender, e);
                return;
            }
            else { 
                Save_state(path);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saved == false)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Binary Files (*.bin)|*.bin|All files (*.*)|*.*";
                openFileDialog.Title = "Открыть бинарный файл";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path= openFileDialog.FileName;
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    List<string> data = new List<string>();//clr,Rad
                    Color clr = (Color)(bf.Deserialize(fs));
                    int rd = (int)(bf.Deserialize(fs));
                    shapes = (List<Shape>)bf.Deserialize(fs);
                    if (shapes.Count > 0)
                    {
                        shapes[0].Rad = rd;
                        shapes[0].Clr = clr;
                    }
                }
            }
        }
    }
}