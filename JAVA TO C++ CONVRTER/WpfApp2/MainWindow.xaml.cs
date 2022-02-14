using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
    
namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        OpenFileDialog LoadFileDialog = new OpenFileDialog();

        public MainWindow()
        {
            InitializeComponent();

        }
        private void run_Click(object sender, RoutedEventArgs e)
        {
            if (javaArea.Text == "")
            {
                MessageBox.Show("Please Enter some code!", "Warrning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {

                var array = new List<string>();
                var Methodes = new List<string>();
                copyToClipboard.Visibility = Visibility.Visible;
                save.Visibility = Visibility.Visible;
                javaArea.TextAlignment = TextAlignment.Left;
                Ccode.Text = "";
                string[] Numofline = new string[javaArea.LineCount];
                for (int i = 0; i < javaArea.LineCount; i++)
                {
                    Numofline[i] = javaArea.GetLineText(i);
                }
                int q = 0;
                /*-----------------------------------------------------------------------------------------------------------------*/
                for (int i = 0; i < Numofline.Length; i++)
                {
                    if (Numofline[i].Contains("class") && q==0)
                    {
                        Numofline[i] = "";
                        q++;
                    }

                    if (Numofline[i].Contains("System.out.println(") && Numofline[i].Contains(");") || Numofline[i].Contains("System.out.print(") && Numofline[i].Contains(");"))
                    {
                        Numofline[i] = getPrint(Numofline[i]);
                    }
                    if ((Numofline[i].Contains("nextLong()") || (Numofline[i].Contains("nextLine()") || Numofline[i].Contains("nextInt()") || Numofline[i].Contains("next()") || Numofline[i].Contains("nextDouble()")) && Numofline[i].Contains("=")))
                    {
                        Numofline[i] = getInput(Numofline[i]);
                    }
                    if (Numofline[i].Contains("new") && Numofline[i].Contains("Scanner(System.in);"))
                    {
                        Numofline[i] = "";
                    }
                    if (Numofline[i].StartsWith("import") && Numofline[i].Contains("java"))
                    {
                        Numofline[i] = "";
                    }
                    if ( Numofline[i].Contains("[") && Numofline[i].Contains("]") && Numofline[i].Contains("{") && Numofline[i].Contains("=") && (Numofline[i].Contains("String") || Numofline[i].Contains("int") || Numofline[i].Contains("float")
                        || Numofline[i].Contains("double") || Numofline[i].Contains("boolean") || Numofline[i].Contains("char") || Numofline[i].Contains("long")))
                    {
                        int z = 0;
                        for (int c = 0; c < Numofline[i].Length; c++)
                        {
                            if (Numofline[i][c] == '[')
                            {
                                z++;
                            }
                        }
                        if (z == 1)
                        {
                            string non, varib, old, newtext;
                            non = getBetween(Numofline[i], "]", "=");
                            

                            array.Add(non.Trim());
                            varib = getBetween(Numofline[i], Numofline[i].Substring(0, 0), "[");

                            old = getBetween(Numofline[i], Numofline[i].Substring(0, 0), "=");
                            newtext = varib + " " + non + "[]";
                            Numofline[i] = Numofline[i].Replace(old, newtext);
                        }
                        else
                        {
                            string non, varib, old, newtext;
                            non = getBetween(Numofline[i], "[]", "=");
                            non = non.Substring(2);
                            varib = getBetween(Numofline[i], Numofline[i].Substring(0, 0), "[]");

                            old = getBetween(Numofline[i], Numofline[i].Substring(0, 0), "=");
                            newtext = varib + " " + non + "[][]";
                            Numofline[i] = Numofline[i].Replace(old, newtext);
                        }

                    }

                    if (Numofline[i].Contains("public") && Numofline[i].Contains("static") && Numofline[i].Contains("void") && Numofline[i].Contains("main("))
                    {
                        string x = Numofline[i].Substring(0, Numofline[i].IndexOf(")"));
                        Numofline[i] = Numofline[i].Replace(x, "int main(");
                    }
                   
                    if (Numofline[i].Contains("final"))
                    {
                        Numofline[i] = Numofline[i].Replace("final", "const");
                    }
                    if (Numofline[i].Contains("boolean"))
                    {
                        Numofline[i] = Numofline[i].Replace("boolean", "bool");
                    }
                    if (Numofline[i].Contains("static"))
                    {
                        Numofline[i] = Numofline[i].Replace("static", " ");
                    }
                    if (Numofline[i].Contains("String"))
                    {
                        Numofline[i] = Numofline[i].Replace("String", "string");
                    }
                    if (Numofline[i].Contains(".indexOf"))
                    {
                        Numofline[i] = Numofline[i].Replace(".indexOf", ".find");
                    }
                    if (Numofline[i].Contains(".concat"))
                    {
                        Numofline[i] = Numofline[i].Replace(".concat", ".append");
                    }
                    if (Numofline[i].Contains("Math.random"))
                    {
                        Numofline[i] = Numofline[i].Replace("Math.random", "rand");
                    }
                    if (Numofline[i].Contains("Math."))
                    {
                        Numofline[i] = Numofline[i].Replace("Math.", " ");
                    }
                    if (Numofline[i].Contains("substring"))
                    {
                        Numofline[i] = Numofline[i].Replace("substring", "substr");
                    }
                    if (Numofline[i].Contains("operator"))
                    {
                        Numofline[i] = Numofline[i].Replace("operator", "operator_Keyword");
                    }
                    if (Numofline[i].Contains("isEmpty"))
                    {
                        Numofline[i] = Numofline[i].Replace("isEmpty", "empty");
                    }
                    if (Numofline[i].Contains("public"))
                    {
                        Numofline[i] = Numofline[i].Replace("public", " ");
                    }
                    if (Numofline[i].Contains(".charAt"))
                    {
                        
                        string x = "["+getBetween(Numofline[i], ".charAt(", ")").Trim()+"]";
                        

                        Numofline[i] = Numofline[i].Replace(".charAt("+ getBetween(Numofline[i], ".charAt(", ")") + ")", x);
                    }
                    for (int z = 0; z < array.Count; z++)
                    {
                        if (Numofline[i].Contains(array[z] + ".length"))
                        {
                            Numofline[i] = Numofline[i].Replace(array[z] + ".length", "sizeof(" + array[z] + ")/sizeof(" + array[z] + "[0])");
                        }

                    }

                    if ((Numofline[i].Contains("boolean") || Numofline[i].Contains("bool") || Numofline[i].Contains("long") || Numofline[i].Contains("string") || Numofline[i].Contains("char") || Numofline[i].Contains("double") || Numofline[i].Contains("float") || Numofline[i].Contains("int") || Numofline[i].Contains("void")) && Numofline[i].Contains(")") && Numofline[i].Contains("(") 
                        && !Numofline[i].Contains("main") && !Numofline[i].Contains("=") && !Numofline[i].Contains(";"))
                    {
                        Methodes.Add(Numofline[i].Substring(0, Numofline[i].IndexOf(')')) + ");");
                    }

                }
                if (q == 1)
                {
                    for (int x = Numofline.Length - 1; x > 0; x--)
                    {
                        if (Numofline[x].Contains("}"))
                        {
                            Numofline[x] = Numofline[x].Replace("}", " ");
                            break;
                            q = 0;
                        }
                    }
                    //Numofline[Numofline.Length-1] = "#include <iostream>\n#include <math.h>\nusing namespace std;\n" + Numofline[0];

                }
                for (int z = 0; z < Methodes.Count; z++)
                {

                    Numofline[0] = "\n" + Methodes[z] + "\n"+ Numofline[0] ;
                }
                Numofline[0] = "#include <iostream>\n#include <math.h>\nusing namespace std;\n" + Numofline[0];
               



                //print array 
                for (int i = 0; i < Numofline.Length; i++)
                {

                    Ccode.Text = Ccode.Text + Numofline[i];
                }
            }

        }
        
        //strSource=function get text between  2 input 
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;   
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
        //function convert input 
        public static string getInput(string strSource)
        {
            strSource = " "+strSource;


            string output = "";
            string input = "";
            string[] var = { "int", "char", "Char", "double", "string", "float", "short", "long", "boolean","String" };
            string x;
            if (strSource.StartsWith("//"))
            {
                return strSource + "\n";
            }
            else
            {
               
                    foreach (char item in strSource)
                    {
                        if (char.IsWhiteSpace(item))
                        {
                            break;
                        }
                           else
                        {
                            input = input + item.ToString();
                        }

                    }
                
            
                    
                    x = getBetween(strSource,input, "=");
                int g = 0;
                for (int z = 0; z < var.Length; z++)
                {
                    if (strSource.Contains(var[z]))
                    {
                        g++;
                        break;
                    }
                }
                if (g == 1)
                {
                    output = input + " " + x + " ;";
                }
                else
                {
                    output = "";
                }


                foreach (string item in var)
                {
                    if (x.Contains(item))
                    {
                        x=x.Replace(item,"");
                    }
                }
                strSource = " cin >>" + x + ";";
                
            }


            return output + "\n" + strSource + "\n";

        }
        //function convert print 
        public static string getPrint(string strSource)
        {
            string textbetweebprint = "";
            string newtextbetweebprint = "";
            string coment = "";
            int count = 0,z=0;

            if (strSource.Contains("System.out.println(") && strSource.Contains(");")|| strSource.Contains("System.out.print(") && strSource.Contains(");"))
                {
                if (strSource.Contains("System.out.println(") && strSource.Contains(");")) { 
                textbetweebprint = getBetween(strSource, "System.out.println(", ");");
                    if (strSource.StartsWith(" "))
                    {

                        string s = strSource.Substring(0, strSource.IndexOf("S"));
                        foreach (char item in s)
                        {
                            if (char.IsWhiteSpace(item))
                            {
                                z++;
                            }
                        }

                        count = strSource.IndexOf(");"); 
                        coment = strSource.Substring(count+2);
                        Console.WriteLine(coment);
                    }
                }
                else { 
                textbetweebprint = getBetween(strSource, "System.out.print(", ");");
                    if (strSource.StartsWith(" ")){ 
                    string s = strSource.Substring(0, strSource.IndexOf('S'));
                    foreach (char item in s)
                    {
                        if (char.IsWhiteSpace(item))
                        {
                            z++;
                        }
                    }
                        try
                        {
                            count = strSource.IndexOf(");");
                            coment = strSource.Substring(count + z);
                            Console.WriteLine(coment);
                        }
                        catch {
                        }
                    }
                }
                foreach (var item in textbetweebprint)
                    {
                    if (item == ',')
                    {
                        newtextbetweebprint = newtextbetweebprint + "<<";

                    }
                    else if (item == '+')
                    { newtextbetweebprint = newtextbetweebprint + "<<"; }

                    else
                    {
                        newtextbetweebprint = newtextbetweebprint + item.ToString();

                    }
                    }
                }
            if (strSource.Contains("System.out.print("))
            {
                return "cout <<" + newtextbetweebprint + "; " + coment + "\n";
            }
            else
            {
                return "cout <<" + newtextbetweebprint +" <<\" \\n\" "+ ";" + coment + "\n";
            }
        }

        private void javaArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            javaArea.TextAlignment = TextAlignment.Left;
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            LoadFileDialog.Filter = "Text files (.txt)|*.java|All files (*.*)|*.";
            if (LoadFileDialog.ShowDialog() == true)
            {
                try
                {
                    javaArea.Clear();
                    var fi = new FileInfo(LoadFileDialog.FileName);
                    string extn = fi.Extension;
                    if (fi.Extension == ".txt"  || fi.Extension == ".java")
                    {
                        javaArea.Text = File.ReadAllText(LoadFileDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show("Error reading file, Please Enter a readable file ex:\n example.java , example.txt", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch { }
            }
        }
        private void copyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            string CppCodeCopy = Ccode.Text.ToString();
            Clipboard.SetText(CppCodeCopy);
        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
            var onlyFileName = System.IO.Path.GetFileNameWithoutExtension(LoadFileDialog.FileName);
            string fileText = Ccode.Text.ToString();
            var dialog = new SaveFileDialog()
            {
                FileName = onlyFileName,
                Filter = "cpp (.cpp)|.cpp",
                DefaultExt = "cpp",
                AddExtension = true
            };
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, fileText);
            }
        }
        private void how_Click(object sender, RoutedEventArgs e)
        {
            var h = new How_Use();
            h.Show();
            this.Close();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

    }

}
